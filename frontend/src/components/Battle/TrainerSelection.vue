<template>
  <b-container>
    <h1 v-t="'battle.trainerSelection.title'" />
    <b-row>
      <form-select
        class="col"
        :disabled="options.length === 0"
        id="trainer"
        label="trainers.select.label"
        :options="options"
        placeholder="trainers.select.placeholder"
        v-model="trainerId"
      />
      <b-form-group class="col" :label="$t('battle.trainerSelection.team')">
        <icon-button
          class="mx-1"
          :disabled="!trainerId"
          icon="plus"
          text="battle.trainerSelection.addPlayer"
          variant="primary"
          @click="toggleTrainer('players')"
        />
        <icon-button
          class="mx-1"
          :disabled="!trainerId"
          icon="plus"
          text="battle.trainerSelection.addOpponent"
          variant="danger"
          @click="toggleTrainer('opponents')"
        />
      </b-form-group>
    </b-row>
    <b-row>
      <selected-trainer-team class="col" title="battle.players" :trainers="battlingPlayerTrainers" @remove="toggleTrainer('players', $event)" />
      <selected-trainer-team class="col" title="battle.opponents" :trainers="battlingOpponentTrainers" @remove="toggleTrainer('opponents', $event)" />
    </b-row>
    <icon-button
      :disabled="battlingPlayerTrainers.length === 0"
      icon="chevron-right"
      text="battle.pokemonSelection.title"
      variant="primary"
      @click="battleNext"
    />
  </b-container>
</template>

<script>
import { mapActions, mapGetters, mapState } from 'vuex'
import SelectedTrainerTeam from './SelectedTrainerTeam.vue'

export default {
  name: 'TrainerSelection',
  components: {
    SelectedTrainerTeam
  },
  data() {
    return {
      trainerId: null
    }
  },
  computed: {
    ...mapGetters(['battlingOpponentTrainers', 'battlingPlayerTrainers', 'trainers']),
    ...mapState(['battle']),
    exclude() {
      return this.battle.players.trainers.concat(this.battle.opponents.trainers)
    },
    options() {
      return this.orderBy(
        this.trainers.filter(({ id }) => !this.exclude.includes(id)).map(({ id, name, number }) => ({ text: `${name} (#${number})`, value: id })),
        'text'
      )
    }
  },
  methods: {
    ...mapActions(['battleNext', 'loadTrainers', 'toggleBattlingOpponentTrainer', 'toggleBattlingPlayerTrainer']),
    toggleTrainer(team, trainerId = null) {
      if (team === 'players') {
        this.toggleBattlingPlayerTrainer(trainerId ?? this.trainerId)
      } else {
        this.toggleBattlingOpponentTrainer(trainerId ?? this.trainerId)
      }
      if (trainerId === null) {
        this.trainerId = null
      }
    }
  },
  async created() {
    try {
      await this.loadTrainers()
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
