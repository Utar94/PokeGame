<template>
  <form-field
    :disabled="disabled"
    :id="id"
    :label="label"
    :maxLength="validate ? 256 : null"
    :placeholder="placeholder"
    :ref="id"
    :required="required"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
export default {
  name: 'UsernameField',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'username'
    },
    label: {
      type: String,
      default: 'users.username.label'
    },
    placeholder: {
      type: String,
      default: 'users.username.placeholder'
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
  },
  methods: {
    focus() {
      this.$refs[this.id].focus()
    }
  }
}
</script>
