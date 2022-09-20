<template>
  <b-container>
    <h1>{{ $t('pokemon.editTitle', { name }) }}</h1>
    <status-detail :model="pokemon" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <icon-submit class="mx-1" :disabled="!canSubmit" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-button class="mx-1" href="/create-pokemon" icon="plus" text="actions.create" variant="success" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <b-row>
              <form-field
                class="col"
                id="currentHitPoints"
                label="statistic.options.HP"
                :minValue="0"
                :maxValue="pokemon.maximumHitPoints"
                required
                :step="1"
                type="number"
                v-model.number="currentHitPoints"
              >
                <b-input-group-append is-text>/&nbsp;{{ pokemon.maximumHitPoints }}</b-input-group-append>
              </form-field>
              <condition-select class="col" v-model="statusCondition" />
            </b-row>
            <b-row>
              <form-field
                class="col"
                id="friendship"
                label="pokemon.friendship"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="friendship"
              />
              <name-field class="col" id="surname" label="pokemon.surname.label" placeholder="pokemon.surname.placeholder" v-model="surname" />
            </b-row>
            <description-field v-model="description" />
            <h3 v-t="'pokemon.effortValues.title'" />
            <span v-if="totalEV > 510" class="text-danger" v-t="'pokemon.effortValues.exceeded'" />
            <b-row>
              <form-field
                class="col"
                id="hpIV"
                label="statistic.options.HP"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.HP"
              />
              <form-field
                class="col"
                id="attackIV"
                label="statistic.options.Attack"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.Attack"
              />
              <form-field
                class="col"
                id="defenseIV"
                label="statistic.options.Defense"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.Defense"
              />
              <form-field
                class="col"
                id="specialAttackIV"
                label="statistic.options.SpecialAttack"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.SpecialAttack"
              />
              <form-field
                class="col"
                id="specialDefenseIV"
                label="statistic.options.SpecialDefense"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.SpecialDefense"
              />
              <form-field
                class="col"
                id="speedIV"
                label="statistic.options.Speed"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.Speed"
              />
            </b-row>
            <icon-button :disabled="totalEV === 0" icon="eraser" text="actions.clear" variant="danger" @click="clearEV" />
            <p>
              <strong>{{ $t('pokemon.effortValues.totalFormat', { total: totalEV }) }}</strong>
            </p>
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
import Vue from 'vue'
import ConditionSelect from './ConditionSelect.vue'
import { updatePokemon } from '@/api/pokemon'

export default {
  name: 'PokemonEdit',
  components: {
    ConditionSelect
  },
  props: {
    json: {
      type: String,
      required: true
    },
    status: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      currentHitPoints: 0,
      description: null,
      effortValues: {
        Attack: 0,
        Defense: 0,
        HP: 0,
        SpecialAttack: 0,
        SpecialDefense: 0,
        Speed: 0
      },
      friendship: 0,
      loading: false,
      notes: null,
      pokemon: null,
      reference: null,
      statusCondition: null,
      surname: null
    }
  },
  computed: {
    canSubmit() {
      return this.totalEV <= 510 && this.hasChanges && !this.loading
    },
    hasChanges() {
      return (
        this.currentHitPoints !== this.pokemon.currentHitPoints ||
        this.statusCondition !== this.pokemon.statusCondition ||
        this.friendship !== this.pokemon.friendship ||
        (this.surname ?? '') !== (this.pokemon.surname ?? '') ||
        (this.description ?? '') !== (this.pokemon.description ?? '') ||
        (this.reference ?? '') !== (this.pokemon.reference ?? '') ||
        (this.notes ?? '') !== (this.pokemon.notes ?? '') ||
        JSON.stringify(this.payload.effortValues) !== JSON.stringify(this.pokemon.effortValues)
      )
    },
    name() {
      return `${this.pokemon.surname ?? this.pokemon.species.name} ${this.$i18n.t('pokemon.levelFormat', { level: this.pokemon.level })}`
    },
    payload() {
      const payload = {
        currentHitPoints: this.currentHitPoints,
        statusCondition: this.statusCondition,
        friendship: this.friendship,
        surname: this.surname,
        description: this.description,
        effortValues: Object.entries(this.effortValues)
          .filter(([, value]) => value !== 0)
          .map(([statistic, value]) => ({ statistic, value })),
        reference: this.reference,
        notes: this.notes
      }
      return payload
    },
    totalEV() {
      return Object.values(this.effortValues).reduce((a, b) => a + (b || 0), 0)
    }
  },
  methods: {
    clearEV() {
      for (const key of Object.keys(this.effortValues)) {
        Vue.set(this.effortValues, key, 0)
      }
    },
    setModel(pokemon) {
      this.pokemon = pokemon
      this.currentHitPoints = pokemon.currentHitPoints
      this.description = pokemon.description
      this.effortValues.Attack = pokemon.effortValues.find(({ statistic }) => statistic === 'Attack')?.value ?? 0
      this.effortValues.Defense = pokemon.effortValues.find(({ statistic }) => statistic === 'Defense')?.value ?? 0
      this.effortValues.HP = pokemon.effortValues.find(({ statistic }) => statistic === 'HP')?.value ?? 0
      this.effortValues.SpecialAttack = pokemon.effortValues.find(({ statistic }) => statistic === 'SpecialAttack')?.value ?? 0
      this.effortValues.SpecialDefense = pokemon.effortValues.find(({ statistic }) => statistic === 'SpecialDefense')?.value ?? 0
      this.effortValues.Speed = pokemon.effortValues.find(({ statistic }) => statistic === 'Speed')?.value ?? 0
      this.friendship = pokemon.friendship
      this.notes = pokemon.notes
      this.reference = pokemon.reference
      this.statusCondition = pokemon.statusCondition
      this.surname = pokemon.surname
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await updatePokemon(this.pokemon.id, this.payload)
            this.setModel(data)
            this.toast('success', 'pokemon.updated')
            this.$refs.form.reset()
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
    this.setModel(JSON.parse(this.json))
    if (this.status === 'created') {
      this.toast('success', 'pokemon.created')
    }
  }
}
</script>
