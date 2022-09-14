<template>
  <table class="table">
    <thead>
      <tr>
        <th scope="col" />
        <th scope="col" v-t="'pokemon.identification'" />
        <th scope="col" v-t="'battle.condition'" />
        <th scope="col" v-t="'battle.abilityAndHeldItem'" />
        <th v-if="selectedBattleMoveCategory === 'Physical'" scope="col" v-t="'statistic.options.Defense'" />
        <th v-else-if="selectedBattleMoveCategory === 'Special'" scope="col" v-t="'statistic.options.SpecialDefense'" />
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
    }
  }
}
</script>
