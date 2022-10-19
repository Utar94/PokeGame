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
  </form-select>
</template>

<script>
import Vue from 'vue'
import { getMoves } from '@/api/moves'

export default {
  name: 'MoveSelect',
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
      default: 'move'
    },
    label: {
      type: String,
      default: 'moves.select.label'
    },
    placeholder: {
      type: String,
      default: 'moves.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  data() {
    return {
      moves: null
    }
  },
  computed: {
    options() {
      return this.moves === null
        ? []
        : Object.values(this.moves)
            .filter(({ id }) => !this.exclude.includes(id))
            .map(({ id, name }) => ({
              text: name,
              value: id
            }))
    }
  },
  methods: {
    onInput($event) {
      if (this.moves) {
        this.$emit('input', this.moves[$event])
      }
    }
  },
  async created() {
    try {
      const { data } = await getMoves({ sort: 'Name', desc: false })
      this.moves = {}
      for (const move of data.items) {
        Vue.set(this.moves, move.id, move)
      }
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
