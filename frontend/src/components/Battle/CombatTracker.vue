<template>
  <b-container fluid>
    <h1 v-t="'battle.combatTracker.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh" />
    </div>
    <b-row v-if="!hasEnded && !isTrainerBattle">
      <location-field />
      <escape-battle-info />
    </b-row>
    <h3 v-t="'trainers.title'" />
    <b-row>
      <battling-trainer-team class="col" team="players" />
      <battling-trainer-team class="col" team="opponents" />
    </b-row>
    <h3 v-t="'pokemon.title'" />
    <b-row>
      <battling-pokemon-team class="col" team="players" />
      <battling-pokemon-team class="col" team="opponents" />
    </b-row>
    <icon-button class="mx-1" icon="chevron-left" text="battle.pokemonSelection.title" variant="warning" @click="battlePrevious" />
    <end-battle v-if="hasEnded" />
    <escape-battle v-else-if="!isTrainerBattle" class="mx-1" />
    <cancel-battle class="mx-1" />
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import BattlingPokemonTeam from './BattlingPokemonTeam.vue'
import BattlingTrainerTeam from './BattlingTrainerTeam.vue'
import CancelBattle from './CancelBattle.vue'
import EndBattle from './EndBattle.vue'
import EscapeBattle from './EscapeBattle.vue'
import EscapeBattleInfo from './EscapeBattleInfo.vue'
import LocationField from './LocationField.vue'

export default {
  name: 'CombatTracker',
  components: {
    BattlingPokemonTeam,
    BattlingTrainerTeam,
    CancelBattle,
    EndBattle,
    EscapeBattle,
    EscapeBattleInfo,
    LocationField
  },
  data() {
    return {
      loading: false
    }
  },
  computed: {
    ...mapGetters(['activeBattlingPokemon', 'battlingOpponentPokemon', 'hasEnded', 'isTrainerBattle'])
  },
  methods: {
    ...mapActions(['battlePrevious', 'loadPokemonList', 'loadTrainers', 'toggleActiveBattlingPokemon']),
    async refresh() {
      if (!this.loading) {
        this.loading = true
        try {
          await this.loadTrainers()
          await this.loadPokemonList()
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  async created() {
    await this.refresh()

    if (!this.isTrainerBattle) {
      const activeBattlingPokemon = Object.fromEntries(this.activeBattlingPokemon.map(pokemon => [pokemon.id, pokemon]))
      for (const pokemon of this.battlingOpponentPokemon) {
        if (!activeBattlingPokemon[pokemon.id]) {
          this.toggleActiveBattlingPokemon(pokemon)
        }
      }
    }
  }
}
</script>
