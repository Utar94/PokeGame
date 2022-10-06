<template>
  <b-card :class="{ clickable, 'overflow-hidden': true, 'trainer-card': true }" no-body @click="onClick">
    <b-row no-gutters>
      <b-col md="8">
        <b-card-body>
          <h4>
            <img alt="Poké Ball Logo" height="20" src="@/assets/poke-ball-logo.svg" /> {{ $t('game.trainerCard') }}
            <img alt="Poké Ball Logo" height="20" src="@/assets/poke-ball-logo.svg" />
          </h4>
          <table class="table">
            <tbody>
              <tr>
                <th scope="row" v-t="'game.trainerNumber'" />
                <td v-text="trainer.number" />
              </tr>
              <tr>
                <th scope="row" v-t="'name.label'" />
                <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
              </tr>
              <tr>
                <th scope="row" v-t="'trainers.money'" />
                <td><pokemon-dollar /> {{ trainer.money }}</td>
              </tr>
              <tr>
                <th scope="row" v-t="'trainers.pokedex.label'" />
                <td v-text="trainer.pokedex" />
              </tr>
              <tr>
                <th scope="row" v-t="'game.playTime'" />
                <td v-text="formattedPlayTime" />
              </tr>
              <tr>
                <th scope="row" v-t="'game.adventureStarted'" />
                <td>{{ $d(new Date(trainer.adventureStarted), 'card') }}</td>
              </tr>
            </tbody>
          </table>
        </b-card-body>
      </b-col>
      <b-col md="4" class="text-center">
        <b-card-img :alt="trainer.name" class="rounded-0" :src="trainer.picture || '/img/trainer-placeholder.png'" />
      </b-col>
    </b-row>
  </b-card>
</template>

<script>
/* TODO(fpion):
 * Flip to Back
 * View Badges
 */

export default {
  name: 'TrainerCard',
  props: {
    clickable: {
      type: Boolean,
      default: false
    },
    trainer: {
      type: Object,
      required: true
    }
  },
  computed: {
    formattedPlayTime() {
      const hours = Math.floor(this.trainer.playTime / 60)
      const minutes = this.trainer.playTime % 60
      return [isNaN(hours) ? 0 : hours, (isNaN(minutes) ? 0 : minutes).toString().padStart(2, '0')].join(':')
    }
  },
  methods: {
    onClick($event) {
      if (this.clickable) {
        this.$emit('click', $event)
      }
    }
  }
}
</script>

<style scoped>
.card-img {
  width: auto;
  max-height: 360px;
}

.clickable {
  cursor: pointer;
}
.clickable:hover {
  background-color: #ececec;
}

.trainer-card {
  max-width: 720px;
}
</style>
