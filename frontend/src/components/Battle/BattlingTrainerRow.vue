<template>
  <tr>
    <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
    <td v-text="trainer.number" />
    <td v-text="trainer.region ? trainer.region.name : '—'" />
    <td>
      <use-item class="mx-1" :team="team" :trainer="trainer" />
      <throw-ball v-if="!isTrainerBattle" class="mx-1" :trainer="trainer" />
    </td>
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import ThrowBall from './ThrowBall.vue'
import UseItem from './UseItem.vue'

export default {
  name: 'BattlingTrainerRow',
  components: {
    ThrowBall,
    UseItem
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
  computed: {
    ...mapGetters(['isTrainerBattle'])
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
