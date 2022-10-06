<template>
  <b-container fluid>
    <h1 v-t="'trainers.title'" />
    <p v-if="gameTrainers.length === 0" v-t="'trainers.empty'" />
    <b-row v-else>
      <b-col lg="6" class="my-2" v-for="trainer in gameTrainers" :key="trainer.id">
        <trainer-card class="mx-auto" clickable :trainer="trainer" @click="setGameTrainer(trainer)" />
      </b-col>
    </b-row>
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import TrainerCard from './TrainerCard.vue'

export default {
  name: 'TrainerSelection',
  components: {
    TrainerCard
  },
  computed: {
    ...mapGetters(['gameTrainers'])
  },
  methods: {
    ...mapActions(['loadGameTrainers', 'setGameTrainer'])
  },
  async created() {
    try {
      await this.loadGameTrainers()
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
