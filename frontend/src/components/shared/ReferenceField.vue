<template>
  <form-url
    :disabled="disabled"
    :id="id"
    :label="label"
    :placeholder="placeholder"
    :required="required"
    :validate="validate"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
export default {
  name: 'ReferenceField',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'reference'
    },
    label: {
      type: String,
      default: 'reference.label'
    },
    placeholder: {
      type: String,
      default: 'reference.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    validate: {
      type: Boolean,
      default: false
    },
    value: {}
  }
}
</script>
