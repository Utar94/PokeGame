<template>
  <form-select :disabled="disabled" :id="id" :label="label" :options="options" :placeholder="placeholder" :required="required" :value="value" @input="onInput">
    <slot />
  </form-select>
</template>

<script>
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
      default: 'ability'
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
      moves: []
    }
  },
  computed: {
    options() {
      return this.moves
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
      const move = this.moves.find(({ id }) => id === $event)
      if (move) {
        this.$emit('move', move)
      }
    }
  },
  async created() {
    try {
      const { data } = await getMoves({ sort: 'Name', desc: false })
      this.moves = data.items
      this.$emit('refresh', data)
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
