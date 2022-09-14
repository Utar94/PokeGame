<template>
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
        </template>
      </tr>
    </thead>
    <tbody>
      <select-target-row v-for="pokemon in activePokemon" :key="pokemon.id" :pokemon="pokemon" />
    </tbody>
  </table>
</template>

<script>
import { mapGetters } from 'vuex'
import SelectTargetRow from './SelectTargetRow.vue'

export default {
  name: 'SelectTargetTable',
  components: {
    SelectTargetRow
  },
  computed: {
    ...mapGetters(['activeBattlingOpponentPokemon', 'activeBattlingPlayerPokemon', 'selectedBattleMoveCategory']),
    activePokemon() {
      return this.orderBy(
        this.activeBattlingPlayerPokemon
          .concat(this.activeBattlingOpponentPokemon)
          .map(pokemon => ({ ...pokemon, sort: `${pokemon.history?.trainer.name ?? this.$i18n.t('pokemon.wild')}|${pokemon.speed}` })),
        'sort'
      )
    },
    defensiveStatistic() {
      switch (this.selectedBattleMoveCategory) {
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
