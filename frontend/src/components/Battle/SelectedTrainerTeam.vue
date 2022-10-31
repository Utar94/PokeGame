<template>
  <div>
    <h3 v-t="`battle.${team}`" />
    <table v-if="trainers.length > 0" class="table table-striped">
      <thead>
        <tr>
          <th scope="col" />
          <th scope="col" v-t="'name.label'" />
          <th scope="col" v-t="'trainers.number'" />
          <th scope="col" v-t="'regions.select.label'" />
          <th scope="col" />
        </tr>
      </thead>
      <tbody>
        <selected-trainer-row v-for="trainer in trainers" :key="trainer.id" :team="team" :trainer="trainer" />
      </tbody>
    </table>
    <p v-else v-t="'trainers.empty'" />
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import SelectedTrainerRow from './SelectedTrainerRow.vue'

export default {
  name: 'SelectedTrainerTeam',
  components: {
    SelectedTrainerRow
  },
  props: {
    team: {
      type: String,
      required: true
    }
  },
  computed: {
    ...mapGetters(['battlingOpponentTrainers', 'battlingPlayerTrainers']),
    trainers() {
      return this.team === 'players' ? this.battlingPlayerTrainers : this.battlingOpponentTrainers
    }
  }
}
</script>
