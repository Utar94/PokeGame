<template>
  <b-container fluid>
    <h1 v-t="'battle.combatTracker.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh" />
    </div>
    <b-row v-if="canEscape">
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
    <!-- <icon-button
      v-if="remainingPlayerPokemon.length > 0 && remainingOpponentPokemon.length === 0"
      class="mx-1"
      icon="crown"
      text="battle.combatTracker.victory"
      variant="primary"
      @click="resetBattle"
    />
    <icon-button
      v-if="remainingPlayerPokemon.length === 0 && remainingOpponentPokemon.length > 0"
      class="mx-1"
      icon="skull"
      text="battle.combatTracker.defeat"
      variant="primary"
      @click="resetBattle"
    />
    -->
    <escape-battle v-if="canEscape" class="mx-1" />
    <cancel-battle class="mx-1" />
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import BattlingPokemonTeam from './BattlingPokemonTeam.vue'
import BattlingTrainerTeam from './BattlingTrainerTeam.vue'
import CancelBattle from './CancelBattle.vue'
import EscapeBattle from './EscapeBattle.vue'
import EscapeBattleInfo from './EscapeBattleInfo.vue'
import LocationField from './LocationField.vue'

export default {
  name: 'CombatTracker',
  components: {
    BattlingPokemonTeam,
    BattlingTrainerTeam,
    CancelBattle,
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
    ...mapGetters([
      'activeBattlingPokemon',
      'battlingOpponentPokemon',
      'isTrainerBattle',
      'remainingBattlingOpponentPokemon',
      'remainingBattlingPlayerPokemon'
    ]),
    canEscape() {
      return !this.isTrainerBattle && this.remainingBattlingPlayerPokemon.length > 0 && this.remainingBattlingOpponentPokemon.length > 0
    }
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
          this.toggleActiveBattlingPokemon(pokemon.id)
        }
      }
    }
  }
  // methods: {
  //   onPokemonCaught({ id, box }) {
  //     this.removeBattlePokemon(id)
  //     if (box === null) {
  //       this.addBattlePlayerPokemon(id)
  //       this.toast('success', 'battle.useItem.caughtToast.party')
  //     } else {
  //       this.toast('success', 'battle.useItem.caughtToast.box')
  //     }
  //   },
  // }
}
</script>
