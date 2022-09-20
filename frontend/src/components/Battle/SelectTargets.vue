<template>
  <div>
    <h3 v-t="'battle.makeMove.targets'" />
    <table class="table">
      <thead>
        <tr>
          <th scope="col" />
          <th scope="col" v-t="'pokemon.identification'" />
          <th scope="col" v-t="'battle.condition'" />
          <th scope="col" v-t="'battle.abilityAndHeldItem'" />
          <template v-if="defensiveStatistic">
            <th scope="col" v-t="`statistic.options.${defensiveStatistic}`" />
            <th scope="col" v-t="'battle.makeMove.effectiveness.label'" />
            <th scope="col" v-t="'battle.makeMove.otherModifiers'" />
            <th scope="col" v-t="'battle.makeMove.accuracy'" />
            <th scope="col" v-t="'battle.makeMove.damage'" />
          </template>
        </tr>
      </thead>
      <tbody>
        <select-target-row v-for="pokemon in activePokemon" :key="pokemon.id" :pokemon="pokemon" />
      </tbody>
    </table>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import SelectTargetRow from './SelectTargetRow.vue'
import { getStatisticModifier } from '@/helpers/statisticUtils'

export default {
  name: 'SelectTargets',
  components: {
    SelectTargetRow
  },
  computed: {
    ...mapGetters(['activeBattlingPokemon', 'battleStatus', 'selectedBattleMove']),
    activePokemon() {
      return this.orderBy(
        this.activeBattlingPokemon.map(pokemon => {
          const status = this.battleStatus[pokemon.id] ?? {}
          const speed = Math.floor(pokemon.speed * getStatisticModifier(status.speed))
          return { ...pokemon, sort: `${pokemon.history?.trainer.name ?? this.$i18n.t('pokemon.wild')}|${999 - speed}` }
        }),
        'sort'
      )
    },
    defensiveStatistic() {
      switch (this.selectedBattleMove.category) {
        case 'Physical':
          return 'Defense'
        case 'Special':
          return 'SpecialDefense'
        default:
          return null
      }
    }
  }
}
</script>
