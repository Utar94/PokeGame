<template>
  <b-container>
    <h1 v-t="'battle.trainerSelection.title'" />
    <b-row>
      <trainer-select class="col" :exclude="exclude" v-model="trainer" />
      <b-form-group class="col" :label="$t('battle.trainerSelection.team')">
        <icon-button class="mx-1" :disabled="!trainer" icon="plus" text="battle.trainerSelection.addPlayer" variant="primary" @click="addTrainer('players')" />
        <icon-button
          class="mx-1"
          :disabled="!trainer"
          icon="plus"
          text="battle.trainerSelection.addOpponent"
          variant="danger"
          @click="addTrainer('opponents')"
        />
      </b-form-group>
    </b-row>
    <b-row>
      <select-trainer-team class="col" title="battle.players" :trainers="players" @removed="removeTrainer('players', $event)" />
      <select-trainer-team class="col" title="battle.opponents" :trainers="opponents" @removed="removeTrainer('opponents', $event)" />
    </b-row>
    <icon-button :disabled="!players.length" icon="chevron-right" text="battle.pokemonSelection.title" variant="primary" @click="onNext" />
  </b-container>
</template>

<script>
import Vue from 'vue'
import { mapActions } from 'vuex'
import SelectTrainerTeam from './SelectTrainerTeam.vue'
import TrainerSelect from '@/components/Trainers/TrainerSelect.vue'
import { getTrainers } from '@/api/trainers'

export default {
  name: 'TrainerSelection',
  components: {
    SelectTrainerTeam,
    TrainerSelect
  },
  data() {
    return {
      opponents: [],
      players: [],
      trainer: null
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
    addTrainer(team) {
      switch (team) {
        case 'players':
          this.players.push(this.trainer)
          break
        case 'opponents':
          this.opponents.push(this.trainer)
          break
      }
      this.trainer = null
    },
    onNext() {
      this.setBattleTrainers({ opponents: this.opponentIds, players: this.playerIds })
    },
    removeTrainer(team, index) {
      switch (team) {
        case 'players':
          Vue.delete(this.players, index)
          break
        case 'opponents':
          Vue.delete(this.opponents, index)
          break
      }
    }
  },
  async created() {
    try {
      const { battle } = this.$store.state
      if (battle.players.trainers.length || battle.opponents.trainers.length) {
        const { data } = await getTrainers()
        const trainers = {}
        for (const trainer of data.items) {
          trainers[trainer.id] = trainer
        }
        this.players = battle.players.trainers.map(id => trainers[id])
        this.opponents = battle.opponents.trainers.map(id => trainers[id])
      }
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
