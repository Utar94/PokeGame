<template>
  <b-container fluid>
    <h1 v-t="'pokemon.title'" />
    <div class="my-2">
      <icon-button icon="arrow-left" text="game.back" variant="danger" @click="navigateGame(null)" />
    </div>
    <b-row>
      <b-col lg="4">
        <h3 v-t="'pokemon.trainer.party'" />
        <table class="table">
          <tbody>
            <pokemon-row v-for="index in 6" :key="index" :pokemon="partyPokemon[index]" @click="onClick" />
          </tbody>
        </table>
      </b-col>
      <b-col lg="8">
        <h3>
          <icon-button class="mr-1" icon="step-backward" variant="primary" @click="previous" />
          <span class="mx-1">{{ $t('game.boxFormat', { box }) }}</span>
          <icon-button class="ml-1" icon="step-forward" variant="primary" @click="next" />
        </h3>
      </b-col>
    </b-row>
    <pokemon-summary :pokemonId="selectedPokemon" v-model="showModal" />
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import PokemonRow from './PokemonRow.vue'
import PokemonSummary from './PokemonSummary.vue'

export default {
  name: 'TrainerPokemon',
  components: {
    PokemonRow,
    PokemonSummary
  },
  data() {
    return {
      box: 1,
      selectedPokemon: null,
      showModal: false
    }
  },
  computed: {
    ...mapGetters(['gamePokemon']),
    partyPokemon() {
      return Object.fromEntries(this.gamePokemon.filter(({ box }) => box === null).map(pokemon => [pokemon.position, pokemon]))
    }
  },
  methods: {
    ...mapActions(['loadGamePokemon', 'navigateGame']),
    next() {
      this.box = this.box === 32 ? 1 : this.box + 1
    },
    onClick(pokemon) {
      this.selectedPokemon = pokemon?.id ?? null
      this.showModal = this.selectedPokemon !== null
    },
    previous() {
      this.box = this.box === 1 ? 32 : this.box - 1
    }
  },
  async created() {
    try {
      await this.loadGamePokemon()
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
