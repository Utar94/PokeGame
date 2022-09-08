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
  name: 'GenderSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'gender'
    },
    label: {
      type: String,
      default: 'trainers.gender.label'
    },
    placeholder: {
      type: String,
      default: 'trainers.gender.placeholder'
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
        Object.entries(this.$i18n.t('trainers.gender.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
