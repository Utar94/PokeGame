<template>
  <b-container fluid>
    <h1 v-t="'battle.pokemonSelection.title'" />
    <b-row>
      <selected-pokemon-team class="col" team="players" />
      <selected-pokemon-team class="col" team="opponents" />
    </b-row>
    <icon-button class="mx-1" icon="chevron-left" text="battle.trainerSelection.title" variant="primary" @click="battlePrevious" />
    <icon-button class="mx-1" :disabled="!isValid" icon="chevron-right" text="battle.title" variant="primary" @click="battleNext" />
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import SelectedPokemonTeam from './SelectedPokemonTeam.vue'

export default {
  name: 'PokemonSelection',
  components: {
    SelectedPokemonTeam
  },
  computed: {
    ...mapGetters(['battlingOpponentPokemon', 'battlingPlayerPokemon']),
    isValid() {
      return this.battlingPlayerPokemon.length > 0 && this.battlingOpponentPokemon.length > 0
    }
  },
  methods: {
    ...mapActions(['battleNext', 'battlePrevious', 'loadPokemonList'])
  },
  async created() {
    try {
      await this.loadPokemonList()
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
