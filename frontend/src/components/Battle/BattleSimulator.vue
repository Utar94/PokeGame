<template>
  <div>
    <combat-tracker v-if="step === 'Battle'" />
    <pokemon-selection v-else-if="step === 'PokemonSelection'" />
    <trainer-selection v-else-if="step === 'TrainerSelection'" />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import CombatTracker from './CombatTracker.vue'
import PokemonSelection from './PokemonSelection.vue'
import TrainerSelection from './TrainerSelection.vue'

export default {
  name: 'BattleSimulator',
  components: {
    CombatTracker,
    PokemonSelection,
    TrainerSelection
  },
  computed: {
    ...mapState({ step: state => state.battle.step })
  }
}
</script>
