<template>
  <b-container>
    <trainer-selection v-if="step === 'TrainerSelection'" />
  </b-container>
</template>

<script>
import { mapState } from 'vuex'
import TrainerSelection from './TrainerSelection.vue'

export default {
  name: 'BattleSimulator',
  components: {
    TrainerSelection
  },
  computed: {
    ...mapState({ step: state => state.battle.step })
  }
}
</script>
