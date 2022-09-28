<template>
  <b-container fluid>
    <h1 v-t="'trainers.title'" />
    <p v-if="gameTrainers.length === 0" v-t="'trainers.empty'" />
    <b-card-group v-else deck>
      <trainer-card v-for="trainer in gameTrainers" :key="trainer.id" :trainer="trainer" />
    </b-card-group>
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
    ...mapActions(['loadGameTrainers'])
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
