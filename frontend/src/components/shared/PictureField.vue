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
  name: 'PictureField',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'picture'
    },
    label: {
      type: String,
      default: 'picture.label'
    },
    placeholder: {
      type: String,
      default: 'picture.placeholder'
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
