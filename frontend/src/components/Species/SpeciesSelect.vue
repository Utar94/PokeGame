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
import { getSpeciesList } from '@/api/species'

export default {
  name: 'SpeciesSelect',
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
      default: 'species'
    },
    label: {
      type: String,
      default: 'species.select.label'
    },
    placeholder: {
      type: String,
      default: 'species.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  data() {
    return {
      species: []
    }
  },
  computed: {
    options() {
      return this.species
        .filter(({ id }) => !this.exclude.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    }
  },
  async created() {
    try {
      const { data } = await getSpeciesList({ sort: 'Name', desc: false })
      this.species = data.items
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
