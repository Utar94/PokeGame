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
export default {
  name: 'ItemTypeSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'type'
    },
    label: {
      type: String,
      default: 'items.type.label'
    },
    placeholder: {
      type: String,
      default: 'items.type.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  computed: {
    options() {
      return this.orderBy(
        Object.entries(this.$i18n.t('items.type.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
