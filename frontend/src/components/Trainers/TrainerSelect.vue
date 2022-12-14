<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="required"
    :value="value ? value.id : null"
    @input="onInput"
  >
    <slot />
    <template #after>
      <slot name="after" />
    </template>
  </form-select>
</template>

<script>
import Vue from 'vue'
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
    value: {
      type: Object,
      default: null
    }
  },
  data() {
    return {
      trainers: null
    }
  },
  computed: {
    options() {
      return this.trainers === null
        ? []
        : Object.values(this.trainers)
            .filter(({ id }) => !this.exclude.includes(id))
            .map(({ id, name }) => ({
              text: name,
              value: id
            }))
    }
  },
  methods: {
    onInput($event) {
      if (this.trainers) {
        this.$emit('input', this.trainers[$event] ?? null)
      }
    }
  },
  async created() {
    try {
      const { data } = await getTrainers({ sort: 'Name', desc: false })
      this.trainers = {}
      for (const trainer of data.items) {
        Vue.set(this.trainers, trainer.id, trainer)
      }
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
