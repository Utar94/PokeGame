<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="required"
    :value="value"
    @input="onInput"
  />
</template>

<script>
import { getItems } from '@/api/items'

export default {
  name: 'ItemSelect',
  props: {
    category: {
      type: String,
      default: null
    },
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'item'
    },
    label: {
      type: String,
      default: 'items.select.label'
    },
    placeholder: {
      type: String,
      default: 'items.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  data() {
    return {
      items: []
    }
  },
  computed: {
    options() {
      return this.items.map(({ id, name }) => ({
        text: name,
        value: id
      }))
    }
  },
  methods: {
    onInput($event) {
      this.$emit('input', $event)
      const item = this.items.find(({ id }) => id === $event)
      if (item) {
        this.$emit('item', item)
      }
    }
  },
  watch: {
    category: {
      immediate: true,
      async handler(category) {
        const { data } = await getItems({
          category,
          sort: 'Name',
          desc: false
        })
        this.items = data.items
      }
    }
  }
}
</script>
