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
            <party-pokemon-row v-for="index in 6" :key="index" :pokemon="partyPokemon[index]" @click="onClick" />
          </tbody>
        </table>
      </b-col>
      <b-col lg="8">
        <h3>
          <icon-button class="mr-1" icon="step-backward" variant="primary" @click="previousGameBox" />
          <span class="mx-1">{{ $t('game.boxFormat', { box: gameBox }) }}</span>
          <icon-button class="ml-1" icon="step-forward" variant="primary" @click="nextGameBox" />
        </h3>
        <table class="table">
          <tr v-for="row in 5" :key="`row_${row}`">
            <pokemon-cell v-for="col in 6" :key="`row_${row}|col_${col}`" :pokemon="boxPokemon[(row - 1) * 6 + col]" @click="onClick" />
          </tr>
        </table>
      </b-col>
    </b-row>
    <pokemon-summary :pokemon="summary" v-model="showModal" />
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import PartyPokemonRow from './PartyPokemonRow.vue'
import PokemonCell from './PokemonCell.vue'
import PokemonSummary from './PokemonSummary.vue'
import { getGamePokemonSummary } from '@/api/game'

export default {
  name: 'TrainerPokemon',
  components: {
    PartyPokemonRow,
    PokemonCell,
    PokemonSummary
  },
  data() {
    return {
      interval: null,
      selectedPokemon: null,
      showModal: false,
      summary: null
    }
  },
  computed: {
    ...mapGetters(['gamePokemon', 'gameBox']),
    boxPokemon() {
      return Object.fromEntries(this.gamePokemon.filter(({ box }) => box === this.gameBox).map(pokemon => [pokemon.position, pokemon]))
    },
    partyPokemon() {
      return Object.fromEntries(this.gamePokemon.filter(({ box }) => box === null).map(pokemon => [pokemon.position, pokemon]))
    }
  },
  methods: {
    ...mapActions(['loadGamePokemon', 'navigateGame', 'nextGameBox', 'previousGameBox']),
    async onClick({ id }) {
      this.selectedPokemon = id
      this.showModal = true
      await this.refresh()
    },
    async refresh() {
      try {
        if (this.selectedPokemon && this.showModal) {
          const { data } = await getGamePokemonSummary(this.selectedPokemon)
          this.summary = data
        } else {
          await this.loadGamePokemon()
        }
      } catch (e) {
        this.handleError(e)
      }
    }
  },
  async created() {
    await this.refresh()
    this.interval = setInterval(this.refresh, Number(process.env.VUE_APP_REFRESH_RATE))
  },
  beforeDestroy() {
    if (this.interval) {
      clearInterval(this.interval)
    }
  }
}
</script>
