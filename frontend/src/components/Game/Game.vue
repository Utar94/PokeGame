<template>
  <div>
    <trainer-selection v-if="gameTrainer === null" />
    <template v-else>
      <game-menu v-if="gamePage === null" />
      <trainer-bag v-else-if="gamePage === 'Bag'" />
    </template>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import GameMenu from './GameMenu.vue'
import TrainerBag from './TrainerBag.vue'
import TrainerSelection from './TrainerSelection.vue'

export default {
  name: 'Game',
  components: {
    GameMenu,
    TrainerBag,
    TrainerSelection
  },
  computed: {
    ...mapGetters(['gamePage', 'gameTrainer'])
  }
}
</script>
