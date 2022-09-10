<template>
  <div>
    <h3 v-if="title" v-text="title" />
    <slot name="title" />
    <trainer-select
      v-if="!readonly"
      :disabled="trainers.length === 3"
      :exclude="exclude"
      :id="id"
      :value="trainer ? trainer.id : null"
      @trainer="trainer = $event"
    >
      <b-input-group-append>
        <icon-button :disabled="!trainer" icon="plus" variant="primary" @click="onAdd" />
      </b-input-group-append>
    </trainer-select>
    <table class="table table-striped">
      <tbody>
        <tr v-for="(trainer, index) in trainers" :key="trainer.id">
          <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
          <td v-text="trainer.number" />
          <td>{{ $t(`region.options.${trainer.region}`) }}</td>
          <td v-if="!readonly"><icon-button icon="times" variant="danger" @click="$emit('removed', index)" /></td>
          <!-- TODO(fpion): Use an Item (if readonly) -->
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import TrainerSelect from '@/components/Trainers/TrainerSelect.vue'

export default {
  name: 'TrainerTeam',
  components: {
    TrainerSelect
  },
  props: {
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      required: true
    },
    title: {
      type: String,
      default: ''
    },
    trainers: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      trainer: null
    }
  },
  computed: {
    readonly() {
      return this.$store.state.battle.step === 'Battle'
    }
  },
  methods: {
    onAdd() {
      this.$emit('added', this.trainer)
      this.trainer = null
    }
  }
}
</script>
