<template>
  <tr>
    <td><trainer-icon :trainer="trainer" /></td>
    <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
    <td v-text="trainer.number" />
    <td>{{ $t(`region.options.${trainer.legacyRegion}`) }}</td>
    <td><icon-button icon="times" variant="danger" @click="toggle" /></td>
  </tr>
</template>

<script>
import { mapActions } from 'vuex'
import TrainerIcon from '@/components/Trainers/TrainerIcon.vue'

export default {
  name: 'SelectedTrainerRow',
  components: {
    TrainerIcon
  },
  props: {
    team: {
      type: String,
      required: true
    },
    trainer: {
      type: Object,
      required: true
    }
  },
  methods: {
    ...mapActions(['toggleBattlingOpponentTrainer', 'toggleBattlingPlayerTrainer']),
    toggle() {
      if (this.team === 'players') {
        this.toggleBattlingPlayerTrainer(this.trainer.id)
      } else {
        this.toggleBattlingOpponentTrainer(this.trainer.id)
      }
    }
  }
}
</script>
