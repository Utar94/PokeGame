<template>
  <div>
    <h3>
      {{ $t(`battle.${team}`) }} <template v-if="max > 0">({{ selected.length }}/{{ max }})</template>
    </h3>
    <table v-if="pokemonList.length > 0" class="table table-hover">
      <thead>
        <th scope="col" />
        <th scope="col" v-t="'pokemon.trainer.title'" />
        <th scope="col" v-t="'pokemon.trainer.position'" />
        <th scope="col" v-t="'pokemon.identification'" />
        <th scope="col" v-t="'battle.condition'" />
      </thead>
      <tbody>
        <selected-pokemon-row v-for="pokemon in pokemonList" :key="pokemon.id" :pokemon="pokemon" :selected="selected.includes(pokemon.id)" :team="team" />
      </tbody>
    </table>
    <p v-else v-t="'pokemon.empty'" />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import SelectedPokemonRow from './SelectedPokemonRow.vue'

export default {
  name: 'SelectedPokemonTeam',
  components: {
    SelectedPokemonRow
  },
  props: {
    team: {
      type: String,
      required: true
    }
  },
  computed: {
    ...mapGetters(['battlingOpponentTrainers', 'battlingPlayerTrainers']),
    ...mapState(['battle']),
    max() {
      return Math.min(this.battlingPlayerTrainers.length, this.battlingOpponentTrainers.length) * 6
    },
    pokemonList() {
      if (this.team === 'opponents' && this.battlingOpponentTrainers.length === 0) {
        return this.$store.getters.pokemonList.filter(({ history }) => history === null)
      }
      const trainers = Object.fromEntries(
        (this.team === 'players' ? this.battlingPlayerTrainers : this.battlingOpponentTrainers).map(trainer => [trainer.id, trainer])
      )
      return this.orderBy(
        this.$store.getters.pokemonList
          .filter(({ box, history, position }) => box === null && history !== null && position !== null && Boolean(trainers[history.trainer.id]))
          .map(pokemon => ({ ...pokemon, sort: `${pokemon.history.trainer.name}|${pokemon.position}` })),
        'sort'
      )
    },
    selected() {
      return this.team === 'players' ? this.battle.players.pokemon : this.battle.opponents.pokemon
    }
  }
}
</script>
