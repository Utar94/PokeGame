<template>
  <b-modal :id="id" size="lg" :title="$t('battle.condition')" @show="reset">
    <validation-observer :ref="id">
      <b-form @submit.prevent="submit">
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
            id="attack"
            label="statistic.options.Attack"
            :minValue="-6"
            :maxValue="6"
            required
            :step="1"
            type="number"
            v-model.number="attack"
          />
          <form-field
            class="col"
            id="specialAttack"
            label="statistic.options.SpecialAttack"
            :minValue="-6"
            :maxValue="6"
            required
            :step="1"
            type="number"
            v-model.number="specialAttack"
          />
        </b-row>
        <b-row>
          <form-field
            class="col"
            id="defense"
            label="statistic.options.Defense"
            :minValue="-6"
            :maxValue="6"
            required
            :step="1"
            type="number"
            v-model.number="defense"
          />
          <form-field
            class="col"
            id="specialDefense"
            label="statistic.options.SpecialDefense"
            :minValue="-6"
            :maxValue="6"
            required
            :step="1"
            type="number"
            v-model.number="specialDefense"
          />
        </b-row>
        <b-row>
          <form-field
            class="col"
            id="speed"
            label="statistic.options.Speed"
            :minValue="-6"
            :maxValue="6"
            required
            :step="1"
            type="number"
            v-model.number="speed"
          />
          <form-field
            class="col"
            id="accuracy"
            label="battle.makeMove.accuracy"
            :minValue="-6"
            :maxValue="6"
            required
            :step="1"
            type="number"
            v-model.number="accuracy"
          />
          <form-field
            class="col"
            id="evasion"
            label="battle.makeMove.evasion"
            :minValue="-6"
            :maxValue="6"
            required
            :step="1"
            type="number"
            v-model.number="evasion"
          />
        </b-row>
        <form-field id="volatile" label="moves.volatile.label" placeholder="moves.volatile.placeholder" v-model="volatile" />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import ConditionSelect from '@/components/Pokemon/ConditionSelect.vue'
import { updatePokemonCondition } from '@/api/pokemon'

export default {
  name: 'ConditionModal',
  components: {
    ConditionSelect
  },
  props: {
    id: {
      type: String,
      required: true
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      accuracy: 0,
      attack: 0,
      currentHitPoints: 0,
      defense: 0,
      evasion: 0,
      loading: false,
      specialAttack: 0,
      specialDefense: 0,
      speed: 0,
      statusCondition: null,
      volatile: null
    }
  },
  computed: {
    ...mapGetters(['battleStatus']),
    hasChanges() {
      return (
        this.currentHitPoints !== this.pokemon.currentHitPoints ||
        this.statusCondition !== this.pokemon.statusCondition ||
        this.accuracy !== (this.status.accuracy ?? 0) ||
        this.attack !== (this.status.attack ?? 0) ||
        this.defense !== (this.status.defense ?? 0) ||
        this.specialAttack !== (this.status.specialAttack ?? 0) ||
        this.specialDefense !== (this.status.specialDefense ?? 0) ||
        this.speed !== (this.status.speed ?? 0) ||
        (this.volatile ?? '') !== (this.status.volatile?.join(', ') ?? '')
      )
    },
    payload() {
      return {
        currentHitPoints: this.currentHitPoints,
        statusCondition: this.statusCondition
      }
    },
    status() {
      return this.battleStatus[this.pokemon.id] ?? {}
    }
  },
  methods: {
    ...mapActions(['updateBattlingPokemonStatus', 'updatePokemon']),
    reset() {
      this.currentHitPoints = this.pokemon.currentHitPoints
      this.statusCondition = this.pokemon.statusCondition
      this.accuracy = this.status.accuracy ?? 0
      this.attack = this.status.attack ?? 0
      this.defense = this.status.defense ?? 0
      this.evasion = this.status.evasion ?? 0
      this.specialAttack = this.status.specialAttack ?? 0
      this.specialDefense = this.status.specialDefense ?? 0
      this.speed = this.status.speed ?? 0
      this.volatile = this.status.volatile?.join(', ') || null
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs[this.id].validate()) {
            this.updateBattlingPokemonStatus({
              id: this.pokemon.id,
              accuracy: this.accuracy,
              attack: this.attack,
              defense: this.defense,
              evasion: this.evasion,
              specialAttack: this.specialAttack,
              specialDefense: this.specialDefense,
              speed: this.speed,
              volatile: this.volatile
            })
            if (this.currentHitPoints !== this.pokemon.currentHitPoints || this.statusCondition !== this.pokemon.statusCondition) {
              const { data } = await updatePokemonCondition(this.pokemon.id, this.payload)
              this.updatePokemon(data)
            }
            if (typeof callback === 'function') {
              callback()
            }
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  }
}
</script>
