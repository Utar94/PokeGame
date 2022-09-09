<template>
  <b-container>
    <h1 v-t="'battle.pokemonSelection.title'" />
    <b-row>
      <pokemon-team class="col" :max="maxPokemon" :pokemon="playerPokemon" :selected="players" :title="$t('battle.players')" @toggled="togglePlayer" />
      <pokemon-team class="col" :max="maxPokemon" :pokemon="opponentPokemon" :selected="opponents" :title="$t('battle.opponents')" @toggled="toggleOpponent" />
    </b-row>
    <icon-button class="mx-1" icon="chevron-left" text="battle.trainerSelection.title" variant="danger" @click="onPrevious" />
    <icon-button class="mx-1" :disabled="!isValid" icon="chevron-right" text="battle.title" variant="primary" @click="onNext" />
  </b-container>
</template>

<script>
import Vue from 'vue'
import PokemonTeam from './PokemonTeam.vue'
import { mapActions, mapState } from 'vuex'
import { getPokemonList } from '@/api/pokemon'

export default {
  name: 'PokemonSelection',
  components: {
    PokemonTeam
  },
  data() {
    return {
      opponents: [],
      players: [],
      pokemon: {}
    }
  },
  computed: {
    ...mapState({
      opponentTrainerIds: state => state.battle.opponents.trainers,
      playerTrainerIds: state => state.battle.players.trainers
    }),
    isValid() {
      return this.players.length > 0 && this.players.length <= this.maxPokemon && this.opponents.length > 0 && this.opponents.length <= this.maxPokemon
    },
    maxPokemon() {
      if (this.opponentTrainerIds.length === 0 || this.opponentTrainerIds.length >= this.playerTrainerIds.length) {
        return this.playerTrainerIds.length * 6
      }
      return this.opponentTrainerIds.length * 6
    },
    opponentPokemon() {
      return this.opponentTrainerIds.length
        ? this.opponentTrainerIds.reduce((pokemon, trainerId) => pokemon.concat(this.pokemon[trainerId] ?? []), [])
        : this.pokemon['Wild']
    },
    playerPokemon() {
      return this.playerTrainerIds.reduce((pokemon, trainerId) => pokemon.concat(this.pokemon[trainerId] ?? []), [])
    }
  },
  methods: {
    ...mapActions(['setBattlePokemon', 'resetBattleTrainers']),
    onNext() {
      this.setBattlePokemon({ opponents: this.opponents, players: this.players })
    },
    onPrevious() {
      this.resetBattleTrainers()
    },
    toggleOpponent({ id }) {
      const index = this.opponents.findIndex(value => value === id)
      if (index >= 0) {
        Vue.delete(this.opponents, index)
      } else {
        this.opponents.push(id)
      }
    },
    togglePlayer({ id }) {
      const index = this.players.findIndex(value => value === id)
      if (index >= 0) {
        Vue.delete(this.players, index)
      } else {
        this.players.push(id)
      }
    }
  },
  async created() {
    try {
      const { data } = await getPokemonList({ sort: 'Name', desc: false })
      for (const pokemon of data.items) {
        const key = pokemon.trainer?.id ?? 'Wild'
        if (!this.pokemon[key]) {
          Vue.set(this.pokemon, key, [pokemon])
        } else {
          this.pokemon[key].push(pokemon)
        }
      }
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
