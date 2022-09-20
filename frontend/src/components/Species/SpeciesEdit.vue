<template>
  <b-container>
    <h1 v-if="species"><pokemon-icon :species="species" /> {{ $t('species.editTitle', { name: species.name }) }}</h1>
    <h1 v-else v-t="'species.newTitle'" />
    <status-detail v-if="species" :model="species" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <template v-if="species">
            <icon-submit class="mx-1" :disabled="!canSubmit" icon="save" :loading="loading" text="actions.save" variant="primary" />
            <icon-button class="mx-1" href="/create-species" icon="plus" text="actions.create" variant="success" />
          </template>
          <icon-submit v-else :disabled="!canSubmit" icon="plus" :loading="loading" text="actions.create" variant="success" />
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
              <ability-select class="col" :exclude="ability2 ? [ability2] : []" id="ability1" v-model="ability1" />
              <ability-select class="col" :disabled="!ability1" :exclude="ability1 ? [ability1] : []" id="ability2" v-model="ability2" />
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
            <strong v-t="'species.evYield.label'" />
            <template v-if="evYieldExceeded">
              <br />
              <span class="text-danger" v-t="'species.evYield.exceeded'" />
            </template>
            <b-row>
              <form-field
                class="col"
                id="hpEvYield"
                label="statistic.options.HP"
                :minValue="0"
                :maxValue="3"
                :step="1"
                type="number"
                v-model.number="evYield.HP"
              />
              <form-field
                class="col"
                id="attackEvYield"
                label="statistic.options.Attack"
                :minValue="0"
                :maxValue="3"
                :step="1"
                type="number"
                v-model.number="evYield.Attack"
              />
              <form-field
                class="col"
                id="defenseEvYield"
                label="statistic.options.Defense"
                :minValue="0"
                :maxValue="3"
                :step="1"
                type="number"
                v-model.number="evYield.Defense"
              />
              <form-field
                class="col"
                id="specialAttackEvYield"
                label="statistic.options.SpecialAttack"
                :minValue="0"
                :maxValue="3"
                :step="1"
                type="number"
                v-model.number="evYield.SpecialAttack"
              />
              <form-field
                class="col"
                id="specialDefenseEvYield"
                label="statistic.options.SpecialDefense"
                :minValue="0"
                :maxValue="3"
                :step="1"
                type="number"
                v-model.number="evYield.SpecialDefense"
              />
              <form-field
                class="col"
                id="speedEvYield"
                label="statistic.options.Speed"
                :minValue="0"
                :maxValue="3"
                :step="1"
                type="number"
                v-model.number="evYield.Speed"
              />
            </b-row>
            <p>{{ $t('species.totalFormat', { total: totalEvYield }) }}</p>
            <description-field v-model="description" />
            <strong v-t="'species.baseStatistics'" />
            <b-row>
              <form-field class="col" id="baseHP" label="statistic.options.HP" :minValue="0" :step="1" type="number" v-model.number="baseStatistics.HP" />
              <form-field
                class="col"
                id="baseAttack"
                label="statistic.options.Attack"
                :minValue="0"
                :step="1"
                type="number"
                v-model.number="baseStatistics.Attack"
              />
              <form-field
                class="col"
                id="baseDefense"
                label="statistic.options.Defense"
                :minValue="0"
                :step="1"
                type="number"
                v-model.number="baseStatistics.Defense"
              />
              <form-field
                class="col"
                id="baseSpecialAttack"
                label="statistic.options.SpecialAttack"
                :minValue="0"
                :step="1"
                type="number"
                v-model.number="baseStatistics.SpecialAttack"
              />
              <form-field
                class="col"
                id="baseSpecialDefense"
                label="statistic.options.SpecialDefense"
                :minValue="0"
                :step="1"
                type="number"
                v-model.number="baseStatistics.SpecialDefense"
              />
              <form-field
                class="col"
                id="baseSpeed"
                label="statistic.options.Speed"
                :minValue="0"
                :step="1"
                type="number"
                v-model.number="baseStatistics.Speed"
              />
            </b-row>
            <p>{{ $t('species.totalFormat', { total: totalBaseStatistics }) }}</p>
          </b-tab>
          <b-tab :title="$t('metadata')">
            <reference-field v-model="reference" />
            <picture-field validate v-model="picture" />
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
      ability1: null,
      ability2: null,
      baseExperienceYield: 0,
      baseFriendship: 70,
      baseStatistics: {
        Attack: 0,
        Defense: 0,
        HP: 0,
        SpecialAttack: 0,
        SpecialDefense: 0,
        Speed: 0
      },
      catchRate: 0,
      category: null,
      description: null,
      evYield: {
        Attack: 0,
        Defense: 0,
        HP: 0,
        SpecialAttack: 0,
        SpecialDefense: 0,
        Speed: 0
      },
      genderRatio: 50,
      genderUnknown: false,
      height: 0,
      levelingRate: 'MediumFast',
      loading: false,
      name: null,
      notes: null,
      number: 0,
      picture: null,
      primaryType: null,
      reference: null,
      secondaryType: null,
      species: null,
      weight: 0
    }
  },
  computed: {
    canSubmit() {
      return !this.evYieldExceeded && this.hasChanges && !this.loading
    },
    hasChanges() {
      return (
        (!this.species && (this.number || this.primaryType || this.secondaryType)) ||
        (this.name ?? '') !== (this.species?.name ?? '') ||
        (this.category ?? '') !== (this.species?.category ?? '') ||
        this.ability1 !== (this.species?.abilities[0]?.id ?? null) ||
        this.ability2 !== (this.species?.abilities[1]?.id ?? null) ||
        this.payload.genderRatio !== (this.species?.genderRatio ?? 50) ||
        this.catchRate !== (this.species?.catchRate ?? 0) ||
        this.height !== (this.species?.height ?? 0) ||
        this.weight !== (this.species?.weight ?? 0) ||
        this.baseExperienceYield !== (this.species?.baseExperienceYield ?? 0) ||
        this.levelingRate !== (this.species?.levelingRate ?? 'MediumFast') ||
        this.baseFriendship !== (this.species?.baseFriendship ?? 70) ||
        (this.description ?? '') !== (this.species?.description ?? '') ||
        JSON.stringify(this.payload.evYield) !== JSON.stringify(this.species?.evYield ?? {}) ||
        JSON.stringify(this.payload.baseStatistics) !== JSON.stringify(this.species?.baseStatistics ?? {}) ||
        (this.reference ?? '') !== (this.species?.reference ?? '') ||
        (this.picture ?? '') !== (this.species?.picture ?? '') ||
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
        abilityIds: [this.ability1, this.ability2].filter(id => id),
        genderRatio: this.genderUnknown ? null : this.genderRatio,
        catchRate: this.catchRate || null,
        height: this.height || null,
        weight: this.weight || null,
        baseExperienceYield: this.baseExperienceYield || null,
        levelingRate: this.levelingRate,
        baseFriendship: this.baseFriendship,
        description: this.description,
        evYield: Object.entries(this.evYield)
          .filter(([, value]) => value !== 0)
          .map(([statistic, value]) => ({ statistic, value })),
        baseStatistics: Object.entries(this.baseStatistics)
          .filter(([, value]) => value !== 0)
          .map(([statistic, value]) => ({ statistic, value })),
        reference: this.reference,
        picture: this.picture || null,
        notes: this.notes
      }
      if (!this.species) {
        payload.number = this.number
        payload.primaryType = this.primaryType
        payload.secondaryType = this.secondaryType
      }
      return payload
    },
    evYieldExceeded() {
      return this.totalEvYield > 3
    },
    totalBaseStatistics() {
      return Object.values(this.baseStatistics).reduce((a, b) => a + b, 0) || 0
    },
    totalEvYield() {
      return Object.values(this.evYield).reduce((a, b) => a + b, 0) || 0
    }
  },
  methods: {
    setModel(species) {
      this.species = species
      this.ability1 = species.abilities.length > 0 ? species.abilities[0].id : null
      this.ability2 = species.abilities.length > 1 ? species.abilities[1].id : null
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
      this.picture = species.picture
      this.primaryType = species.primaryType
      this.reference = species.reference
      this.secondaryType = species.secondaryType
      this.weight = species.weight ?? 0

      const evYield = Object.fromEntries(species.evYield.map(({ statistic, value }) => [statistic, value]))
      this.evYield.HP = evYield.HP ?? 0
      this.evYield.Attack = evYield.Attack ?? 0
      this.evYield.Defense = evYield.Defense ?? 0
      this.evYield.SpecialAttack = evYield.SpecialAttack ?? 0
      this.evYield.SpecialDefense = evYield.SpecialDefense ?? 0
      this.evYield.Speed = evYield.Speed ?? 0

      const baseStatistics = Object.fromEntries(species.baseStatistics.map(({ statistic, value }) => [statistic, value]))
      this.baseStatistics.HP = baseStatistics.HP ?? 0
      this.baseStatistics.Attack = baseStatistics.Attack ?? 0
      this.baseStatistics.Defense = baseStatistics.Defense ?? 0
      this.baseStatistics.SpecialAttack = baseStatistics.SpecialAttack ?? 0
      this.baseStatistics.SpecialDefense = baseStatistics.SpecialDefense ?? 0
      this.baseStatistics.Speed = baseStatistics.Speed ?? 0
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
    ability1(ability1) {
      if (!ability1 && this.ability2) {
        this.ability1 = this.ability2
        this.ability2 = null
      }
    },
    genderUnknown(genderUnknown) {
      if (genderUnknown) {
        this.genderRatio = 0
      }
    }
  }
}
</script>
