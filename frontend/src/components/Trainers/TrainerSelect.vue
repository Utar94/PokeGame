<template>
  <form-select :disabled="disabled" :id="id" :label="label" :options="options" :placeholder="placeholder" :required="required" :value="value" @input="onInput">
    <slot />
  </form-select>
</template>

<script>
import { getTrainers } from '@/api/trainers'

export default {
  name: 'TrainerSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      default: 'trainer'
    },
    label: {
      type: String,
      default: 'trainers.select.label'
    },
    placeholder: {
      type: String,
      default: 'trainers.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  data() {
    return {
      trainers: []
    }
  },
  computed: {
    options() {
      return this.trainers
        .filter(({ id }) => !this.exclude.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    }
  },
  methods: {
    onInput($event) {
      this.$emit('input', $event)
      const trainer = this.trainers.find(({ id }) => id === $event)
      if (trainer) {
        this.$emit('trainer', trainer)
      }
    }
  },
  async created() {
    try {
      const { data } = await getTrainers({ sort: 'Name', desc: false })
      this.trainers = data.items
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
