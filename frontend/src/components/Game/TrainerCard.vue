<template>
  <b-card class="mb-2" :img-alt="trainer.name" :img-src="src" img-top tag="article" @click="setGameTrainer(trainer)">
    <template #header>
      <h4 class="mb-0"><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</h4>
    </template>
    <table class="table">
      <tr>
        <th scope="row" v-t="'trainers.number'" />
        <td v-text="trainer.number" />
      </tr>
      <tr>
        <th scope="row" v-t="'region.label'" />
        <td>{{ $t(`region.options.${trainer.region}`) }}</td>
      </tr>
      <tr>
        <th scope="row" v-t="'trainers.money'" />
        <td><pokemon-dollar /> {{ trainer.money }}</td>
      </tr>
      <tr>
        <th scope="row" v-t="'game.adventureStarted'" />
        <td>{{ $d(new Date(trainer.createdOn), 'card') }}</td>
      </tr>
    </table>
  </b-card>
</template>

<script>
/* TODO(fpion):
 * Horizontal card with picture to the right
 * ID No.
 * Name
 * Pokédex (number of Pokémon)
 * Play time
 * Flip to Back
 * View Badges
 */

import { mapActions } from 'vuex'

export default {
  name: 'TrainerCard',
  props: {
    trainer: {
      type: Object,
      required: true
    }
  },
  computed: {
    src() {
      return this.trainer.picture ?? '/img/trainer-placeholder.png'
    }
  },
  methods: {
    ...mapActions(['setGameTrainer'])
  }
}
</script>

<style scoped>
.card {
  cursor: pointer;
  max-width: 20rem;
}
</style>
