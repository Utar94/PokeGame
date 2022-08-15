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
              <type-select class="col" :disabled="Boolean(species)" id="primaryType" label="species.primaryType" :required="!species" v-model="primaryType" />
              <type-select class="col" :disabled="Boolean(species)" id="secondaryType" label="species.secondaryType" v-model="secondaryType" />
            </b-row>
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
              <ability-select class="col" v-model="abilityId" />
            </b-row>
            <description-field v-model="description" />
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
import AbilitySelect from '@/components/Abilities/AbilitySelect.vue'
import { createSpecies, updateSpecies } from '@/api/species'

export default {
  name: 'SpeciesEdit',
  components: {
    AbilitySelect
  },
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
      abilityId: null,
      category: null,
      description: null,
      loading: false,
      name: null,
      notes: null,
      number: 0,
      primaryType: null,
      reference: null,
      secondaryType: null,
      species: null
    }
  },
  computed: {
    hasChanges() {
      return (
        (!this.species && (this.number || this.primaryType || this.secondaryType)) ||
        (this.name ?? '') !== (this.species?.name ?? '') ||
        (this.category ?? '') !== (this.species?.category ?? '') ||
        this.abilityId !== (this.species?.ability?.id ?? null) ||
        (this.description ?? '') !== (this.species?.description ?? '') ||
        (this.reference ?? '') !== (this.species?.reference ?? '') ||
        (this.notes ?? '') !== (this.species?.notes ?? '')
      )
    },
    payload() {
      const payload = {
        abilityId: this.abilityId,
        name: this.name,
        category: this.category,
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
      this.abilityId = species.ability?.id ?? null
      this.category = species.category
      this.description = species.description
      this.name = species.name
      this.notes = species.notes
      this.number = species.number
      this.primaryType = species.primaryType
      this.reference = species.reference
      this.secondaryType = species.secondaryType
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
  }
}
</script>
