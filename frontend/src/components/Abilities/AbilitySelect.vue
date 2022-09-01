<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="required"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
import { getAbilities } from '@/api/abilities'

export default {
  name: 'AbilitySelect',
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
      default: 'abilities.select.label'
    },
    placeholder: {
      type: String,
      default: 'abilities.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  data() {
    return {
      abilities: []
    }
  },
  computed: {
    options() {
      return this.abilities
        .filter(({ id }) => !this.exclude.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    }
  },
  async created() {
    try {
      const { data } = await getAbilities({ sort: 'Name', desc: false })
      this.abilities = data.items
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
