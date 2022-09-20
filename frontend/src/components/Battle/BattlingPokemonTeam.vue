<template>
  <div>
    <h5>{{ $t(`battle.${team}`) }} ({{ remaining }}/{{ total }})</h5>
    <table v-if="pokemonList.length > 0" class="table">
      <thead>
        <th scope="col" v-t="'statistic.options.Speed'" />
        <th scope="col" />
        <th scope="col" v-t="'pokemon.identification'" />
        <th scope="col" v-t="'battle.condition'" />
        <th scope="col" v-t="'battle.abilityAndHeldItem'" />
        <th scope="col" />
      </thead>
      <tbody>
        <battling-pokemon-row v-for="pokemon in pokemonList" :key="pokemon.id" :active="Boolean(activePokemon[pokemon.id])" :pokemon="pokemon" :team="team" />
      </tbody>
    </table>
    <p v-else v-t="'pokemon.empty'" />
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import BattlingPokemonRow from './BattlingPokemonRow.vue'
import { getStatisticModifier } from '@/helpers/statisticUtils'

export default {
  name: 'BattlingPokemonTeam',
  components: {
    BattlingPokemonRow
  },
  props: {
    team: {
      type: String,
      required: true
    }
  },
  computed: {
    ...mapGetters([
      'activeBattlingPokemon',
      'battleStatus',
      'battlingOpponentPokemon',
      'battlingPlayerPokemon',
      'remainingBattlingOpponentPokemon',
      'remainingBattlingPlayerPokemon'
    ]),
    ...mapState(['battle']),
    activePokemon() {
      return Object.fromEntries(this.activeBattlingPokemon.map(pokemon => [pokemon.id, pokemon]))
    },
    pokemonList() {
      const pokemonList = this.team === 'players' ? this.battlingPlayerPokemon : this.battlingOpponentPokemon
      const active = this.orderBy(
        pokemonList
          .filter(({ id }) => Boolean(this.activePokemon[id]))
          .map(pokemon => {
            const status = this.battleStatus[pokemon.id] ?? {}
            const sort = Math.floor(pokemon.speed * getStatisticModifier(status.speed ?? 0))
            return { ...pokemon, sort }
          }),
        'sort',
        true
      )
      const unactive = this.orderBy(
        pokemonList.filter(({ id }) => !this.activePokemon[id]).map(pokemon => ({ ...pokemon, sort: `${pokemon.history?.trainer.name}|${pokemon.position}` })),
        'sort'
      )
      return active.concat(unactive)
    },
    remaining() {
      return (this.team === 'players' ? this.remainingBattlingPlayerPokemon : this.remainingBattlingOpponentPokemon).length
    },
    total() {
      return (this.team === 'players' ? this.battlingPlayerPokemon : this.battlingOpponentPokemon).length
    }
  }
}
</script>
