<template>
  <form-url :id="id" :label="label" :placeholder="placeholder" :required="required" :validate="validate" :value="value" @input="$emit('input', $event)" />
</template>

<script>
export default {
  name: 'PictureField',
  props: {
    id: {
      type: String,
      default: 'picture'
    },
    label: {
      type: String,
      default: 'users.picture.label'
    },
    placeholder: {
      type: String,
      default: 'users.picture.placeholder'
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
