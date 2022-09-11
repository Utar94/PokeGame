<template>
  <div>
    <h3 v-t="title" />
    <table class="table table-striped">
      <tbody>
        <tr v-for="(trainer, index) in trainers" :key="trainer.id">
          <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
          <td v-text="trainer.number" />
          <td>{{ $t(`region.options.${trainer.region}`) }}</td>
          <td><icon-button icon="times" variant="danger" @click="$emit('removed', index)" /></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: 'TrainerTeam',
  props: {
    title: {
      type: String,
      required: true
    },
    trainers: {
      type: Array,
      default: () => []
    }
  }
}
</script>
