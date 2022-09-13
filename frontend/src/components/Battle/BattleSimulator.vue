<template>
  <div>
    <combat-tracker v-if="battleStep === 'Battle'" />
    <pokemon-selection v-else-if="battleStep === 'PokemonSelection'" />
    <trainer-selection v-else-if="battleStep === 'TrainerSelection'" />
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
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
    ...mapGetters(['battleStep'])
  }
}
</script>
