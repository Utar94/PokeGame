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
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'timeOfDay'
    },
    label: {
      type: String,
      default: 'species.evolutions.timeOfDay.label'
    },
    placeholder: {
      type: String,
      default: 'species.evolutions.timeOfDay.placeholder'
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
        Object.entries(this.$i18n.t('species.evolutions.timeOfDay.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
