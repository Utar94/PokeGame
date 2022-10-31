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
  >
    <slot />
  </form-select>
</template>

<script>
import { getRegions } from '@/api/regions'

export default {
  name: 'NewRegionSelect',
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
      default: 'region'
    },
    label: {
      type: String,
      default: 'regions.select.label'
    },
    placeholder: {
      type: String,
      default: 'regions.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  data() {
    return {
      regions: []
    }
  },
  computed: {
    options() {
      return this.regions
        .filter(({ id }) => !this.exclude.includes(id))
        .map(({ id, name }) => ({
          text: name,
          value: id
        }))
    }
  },
  async created() {
    try {
      const { data } = await getRegions({ sort: 'Name', desc: false })
      this.regions = data.items
      this.$emit('refresh', data)
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
