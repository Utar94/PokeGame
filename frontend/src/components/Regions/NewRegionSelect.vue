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
      regions: null
    }
  },
  computed: {
    options() {
      return this.regions === null
        ? []
        : Object.values(this.regions)
            .filter(({ id }) => !this.exclude.includes(id))
            .map(({ id, name }) => ({
              text: name,
              value: id
            }))
    }
  },
  methods: {
    onInput($event) {
      if (this.regions) {
        this.$emit('input', this.regions[$event] ?? null)
      }
    }
  },
  async created() {
    try {
      const { data } = await getRegions({ sort: 'Name', desc: false })
      this.regions = {}
      for (const region of data.items) {
        Vue.set(this.regions, region.id, region)
      }
      this.$emit('refresh', data)
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
