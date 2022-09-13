<template>
  <tr>
    <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
    <td v-text="trainer.number" />
    <td>{{ $t(`region.options.${trainer.region}`) }}</td>
    <td><!-- TODO(fpion): Use an Item --></td>
  </tr>
</template>

<script>
import { mapActions } from 'vuex'

export default {
  name: 'BattlingTrainerRow',
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
