<template>
  <b-container>
    <h1 v-t="species ? 'species.editTitle' : 'species.newTitle'" />
    <status-detail v-if="species" :model="species" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <icon-submit v-if="species" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <b-row>
              <name-field class="col" required v-model="name" />
              <form-field
                class="col"
                id="category"
                label="species.category.label"
                :maxLength="100"
                placeholder="species.category.placeholder"
                v-model="category"
              />
              <form-field
                class="col"
                :disabled="Boolean(species)"
                id="number"
                label="species.number"
                :minValue="1"
                :maxValue="999"
                :required="!species"
                type="number"
                v-model.number="number"
              />
            </b-row>
            <b-row>
              <type-select
                class="col"
                :disabled="Boolean(species)"
                :exclude="secondaryType ? [secondaryType] : []"
                id="primaryType"
                label="species.primaryType"
                :required="!species"
                v-model="primaryType"
              />
              <type-select
                class="col"
                :disabled="Boolean(species)"
                :exclude="primaryType ? [primaryType] : []"
                id="secondaryType"
                label="species.secondaryType"
                v-model="secondaryType"
              />
            </b-row>
            <b-row>
              <form-field
                class="col"
                :disabled="genderUnknown"
                id="genderRatio"
                label="species.genderRatio.label"
                :minValue="0"
                :maxValue="100"
                :step="12.5"
                type="number"
                v-model.number="genderRatio"
              >
                <b-input-group-append>
                  <b-input-group-text>{{ $t('species.genderRatio.unit') }}</b-input-group-text>
                </b-input-group-append>
                <template #after>
                  <b-form-checkbox v-model="genderUnknown">{{ $t('species.genderRatio.genderUnknown') }}</b-form-checkbox>
                </template>
              </form-field>
              <form-field
                class="col"
                id="catchRate"
                label="species.catchRate"
                :minValue="0"
                :maxValue="255"
                :step="5"
                type="number"
                v-model.number="catchRate"
              />
              <form-field class="col" id="height" label="species.height.label" :minValue="0" :step="0.1" type="number" v-model.number="height">
                <b-input-group-append>
                  <b-input-group-text>{{ $t('species.height.unit') }}</b-input-group-text>
                </b-input-group-append>
              </form-field>
              <form-field class="col" id="weight" label="species.weight.label" :minValue="0" :step="0.1" type="number" v-model.number="weight">
                <b-input-group-append>
                  <b-input-group-text>{{ $t('species.weight.unit') }}</b-input-group-text>
                </b-input-group-append>
              </form-field>
            </b-row>
            <b-row>
              <form-field
                class="col"
                id="baseExperienceYield"
                label="species.baseExperienceYield"
                :minValue="0"
                :maxValue="999"
                :step="1"
                type="number"
                v-model.number="baseExperienceYield"
              />
              <form-select
                class="col"
                id="levelingRate"
                label="species.levelingRate.label"
                :options="levelingRates"
                placeholder="species.levelingRate.placeholder"
                required
                v-model="levelingRate"
              />
              <form-field
                class="col"
                id="baseFriendship"
                label="species.baseFriendship"
                :minValue="0"
                :maxValue="140"
                :step="5"
                type="number"
                v-model.number="baseFriendship"
              />
            </b-row>
            <!-- TODO(fpion): EV Yield -->
            <description-field v-model="description" />
            <!-- TODO(fpion): Abilities -->
            <!-- TODO(fpion): Base Stats -->
          </b-tab>
          <b-tab :title="$t('metadata')">
            <reference-field v-model="reference" />
            <notes-field v-model="notes" />
          </b-tab>
        </b-tabs>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { createSpecies, updateSpecies } from '@/api/species'

export default {
  name: 'SpeciesEdit',
  props: {
    json: {
      type: String,
      default: ''
    },
    status: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      baseExperienceYield: 0,
      baseFriendship: 70,
      catchRate: 0,
      category: null,
      description: null,
      genderRatio: 50,
      genderUnknown: false,
      height: 0,
      levelingRate: 'MediumFast',
      loading: false,
      name: null,
      notes: null,
      number: 0,
      primaryType: null,
      reference: null,
      secondaryType: null,
      species: null,
      weight: 0
    }
  },
  computed: {
    hasChanges() {
      return (
        (!this.species && (this.number || this.primaryType || this.secondaryType)) ||
        (this.name ?? '') !== (this.species?.name ?? '') ||
        (this.category ?? '') !== (this.species?.category ?? '') ||
        this.payload.genderRatio !== (this.species?.genderRatio ?? 50) ||
        this.catchRate !== (this.species?.catchRate ?? 0) ||
        this.height !== (this.species?.height ?? 0) ||
        this.weight !== (this.species?.weight ?? 0) ||
        this.baseExperienceYield !== (this.species?.baseExperienceYield ?? 0) ||
        this.levelingRate !== (this.species?.levelingRate ?? 'MediumFast') ||
        this.baseFriendship !== (this.species?.baseFriendship ?? 70) ||
        (this.description ?? '') !== (this.species?.description ?? '') ||
        (this.reference ?? '') !== (this.species?.reference ?? '') ||
        (this.notes ?? '') !== (this.species?.notes ?? '')
      )
    },
    levelingRates() {
      return this.orderBy(
        Object.entries(this.$i18n.t('species.levelingRate.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    },
    payload() {
      const payload = {
        name: this.name,
        category: this.category,
        genderRatio: this.genderUnknown ? null : this.genderRatio,
        catchRate: this.catchRate || null,
        height: this.height || null,
        weight: this.weight || null,
        baseExperienceYield: this.baseExperienceYield || null,
        levelingRate: this.levelingRate,
        baseFriendship: this.baseFriendship,
        description: this.description,
        reference: this.reference,
        notes: this.notes
      }
      if (!this.species) {
        payload.number = this.number
        payload.primaryType = this.primaryType
        payload.secondaryType = this.secondaryType
      }
      return payload
    }
  },
  methods: {
    setModel(species) {
      this.species = species
      this.baseExperienceYield = species.baseExperienceYield ?? 0
      this.baseFriendship = species.baseFriendship
      this.catchRate = species.catchRate ?? 0
      this.category = species.category
      this.description = species.description
      this.genderRatio = species.genderRatio ?? 0
      this.genderUnknown = species.genderRatio === null
      this.height = species.height ?? 0
      this.levelingRate = species.levelingRate
      this.name = species.name
      this.notes = species.notes
      this.number = species.number
      this.primaryType = species.primaryType
      this.reference = species.reference
      this.secondaryType = species.secondaryType
      this.weight = species.weight ?? 0
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.species) {
              const { data } = await updateSpecies(this.species.id, this.payload)
              this.setModel(data)
              this.toast('success', 'species.updated')
              this.$refs.form.reset()
            } else {
              const { data } = await createSpecies(this.payload)
              window.location.replace(`/species/${data.id}?status=created`)
            }
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  created() {
    if (this.json) {
      this.setModel(JSON.parse(this.json))
    }
    if (this.status === 'created') {
      this.toast('success', 'species.created')
    }
  },
  watch: {
    genderUnknown(genderUnknown) {
      if (genderUnknown) {
        this.genderRatio = 0
      }
    }
  }
}
</script>
