<template>
  <b-container fluid>
    <h1 v-t="'battle.makeMove.label'" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <select-move />
        <template v-if="selectedBattleMove">
          <select-targets />
          <move-condition />
          <move-damage v-if="dealsDamage" />
        </template>
        <div class="my-2">
          <icon-button class="mx-1" icon="ban" text="actions.cancel" @click="resetBattleMove()" />
          <icon-submit :disabled="!canSubmit" icon="magic" :loading="loading" text="battle.makeMove.label" variant="danger" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import MoveCondition from './MoveCondition.vue'
import MoveDamage from './MoveDamage.vue'
import SelectMove from './SelectMove.vue'
import SelectTargets from './SelectTargets.vue'
import { usePokemonMove } from '@/api/pokemon'

export default {
  name: 'MakeMove',
  components: {
    MoveCondition,
    MoveDamage,
    SelectMove,
    SelectTargets
  },
  data() {
    return {
      loading: false
    }
  },
  computed: {
    ...mapGetters(['battleMoveAttacker', 'battleMoveCondition', 'battleMoveDamage', 'battleMoveTargets', 'battlingOpponentPokemon', 'selectedBattleMove']),
    canSubmit() {
      return !this.loading && this.selectedBattleMove && Object.keys(this.battleMoveTargets).length > 0
    },
    dealsDamage() {
      return this.selectedBattleMove.category === 'Physical' || this.selectedBattleMove.category === 'Special'
    },
    payload() {
      const { status } = this.battleMoveCondition
      const { attack, burn, critical, power, random, stab, weather } = this.battleMoveDamage
      return {
        damage:
          this.selectedBattleMove.category === 'Status'
            ? null
            : {
                attack,
                isBurnt: burn,
                isCritical: critical,
                power,
                random: random / 100,
                stab,
                weather
              },
        statusCondition: status,
        targets: Object.values(this.battleMoveTargets).map(({ defense, effectiveness, otherModifiers, pokemon, specialDefense }) => {
          const target = {
            id: pokemon.id,
            defense: null,
            effectiveness: null,
            otherModifiers: null
          }
          if (this.selectedBattleMove.category !== 'Status') {
            target.defense = this.selectedBattleMove.category === 'Physical' ? defense : specialDefense
            target.effectiveness = effectiveness
            target.otherModifiers = otherModifiers
          }
          return target
        })
      }
    }
  },
  methods: {
    ...mapActions(['applyBattleMove', 'distributeExperience', 'resetBattleMove', 'updatePokemon']),
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await usePokemonMove(this.battleMoveAttacker.id, this.selectedBattleMove.id, this.payload)
            const defeated = []
            const opponents = Object.fromEntries(this.battlingOpponentPokemon.map(pokemon => [pokemon.id, pokemon]))
            for (const pokemon of data) {
              this.updatePokemon(pokemon)
              if (pokemon.currentHitPoints < 1 && Boolean(opponents[pokemon.id])) {
                defeated.push(pokemon)
              }
            }
            this.applyBattleMove()
            this.$refs.form.reset()
            if (defeated.length > 0) {
              this.resetBattleMove(true)
              this.distributeExperience(defeated)
            } else {
              this.resetBattleMove()
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
