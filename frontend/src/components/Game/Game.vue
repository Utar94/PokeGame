<template>
  <div>
    <trainer-selection v-if="gameTrainer === null" />
    <template v-else>
      <game-menu v-if="gamePage === null" />
    </template>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import GameMenu from './GameMenu.vue'
import TrainerSelection from './TrainerSelection.vue'

export default {
  name: 'Game',
  components: {
    GameMenu,
    TrainerSelection
  },
  computed: {
    ...mapGetters(['gamePage', 'gameTrainer'])
  }
}
</script>
