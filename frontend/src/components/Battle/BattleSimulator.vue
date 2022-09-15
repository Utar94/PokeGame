<template>
  <div>
    <combat-tracker v-if="battleStep === 'Battle'" />
    <make-move v-else-if="battleStep === 'MakeMove'" />
    <pokemon-selection v-else-if="battleStep === 'PokemonSelection'" />
    <trainer-selection v-else-if="battleStep === 'TrainerSelection'" />
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import CombatTracker from './CombatTracker.vue'
import MakeMove from './MakeMove.vue'
import PokemonSelection from './PokemonSelection.vue'
import TrainerSelection from './TrainerSelection.vue'

export default {
  name: 'BattleSimulator',
  components: {
    CombatTracker,
    MakeMove,
    PokemonSelection,
    TrainerSelection
  },
  computed: {
    ...mapGetters(['battleStep'])
  }
}
</script>
