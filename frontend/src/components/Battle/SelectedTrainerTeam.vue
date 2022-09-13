<template>
  <div>
    <h3 v-t="title" />
    <table v-if="trainers.length > 0" class="table table-striped">
      <thead>
        <tr>
          <th scope="col" v-t="'name.label'" />
          <th scope="col" v-t="'trainers.number'" />
          <th scope="col" v-t="'region.label'" />
          <th scope="col" />
        </tr>
      </thead>
      <tbody>
        <tr v-for="trainer in trainers" :key="trainer.id">
          <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
          <td v-text="trainer.number" />
          <td>{{ $t(`region.options.${trainer.region}`) }}</td>
          <td><icon-button icon="times" variant="danger" @click="$emit('remove', trainer.id)" /></td>
        </tr>
      </tbody>
    </table>
    <p v-else v-t="'trainers.empty'" />
  </div>
</template>

<script>
export default {
  name: 'SelectedTrainerTeam',
  props: {
    title: {
      type: String,
      required: true
    },
    trainers: {
      type: Array,
      required: true
    }
  }
}
</script>
