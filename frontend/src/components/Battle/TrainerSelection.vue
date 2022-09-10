<template>
  <b-container>
    <h1 v-t="'battle.trainerSelection.title'" />
    <b-row>
      <select-trainer-team
        class="col"
        :exclude="exclude"
        id="players"
        :title="$t('battle.players')"
        :trainers="players"
        @added="addPlayer"
        @removed="removePlayer"
      />
      <select-trainer-team
        class="col"
        :exclude="exclude"
        id="opponents"
        :title="$t('battle.opponents')"
        :trainers="opponents"
        @added="addOpponent"
        @removed="removeOpponent"
      />
    </b-row>
    <icon-button :disabled="!players.length" icon="chevron-right" text="battle.pokemonSelection.title" variant="primary" @click="onNext" />
  </b-container>
</template>

<script>
import Vue from 'vue'
import { mapActions } from 'vuex'
import SelectTrainerTeam from './SelectTrainerTeam.vue'

export default {
  name: 'TrainerSelection',
  components: {
    SelectTrainerTeam
  },
  data() {
    return {
      opponents: [],
      players: []
    }
  },
  computed: {
    exclude() {
      return this.playerIds.concat(this.opponentIds)
    },
    opponentIds() {
      return this.opponents.map(({ id }) => id)
    },
    playerIds() {
      return this.players.map(({ id }) => id)
    }
  },
  methods: {
    ...mapActions(['setBattleTrainers']),
    addOpponent(trainer) {
      this.opponents.push(trainer)
    },
    addPlayer(trainer) {
      this.players.push(trainer)
    },
    onNext() {
      this.setBattleTrainers({ opponents: this.opponentIds, players: this.playerIds })
    },
    removeOpponent(index) {
      Vue.delete(this.opponents, index)
    },
    removePlayer(index) {
      Vue.delete(this.players, index)
    }
  }
}
</script>
