<template>
  <div>
    <trainer-selection v-if="step === 'TrainerSelection'" />
    <pokemon-selection v-else-if="step === 'PokemonSelection'" />
  </div>
</template>

<script>
import { mapState } from 'vuex'
import PokemonSelection from './PokemonSelection.vue'
import TrainerSelection from './TrainerSelection.vue'

export default {
  name: 'BattleSimulator',
  components: {
    PokemonSelection,
    TrainerSelection
  },
  computed: {
    ...mapState({ step: state => state.battle.step })
  }
}
</script>
