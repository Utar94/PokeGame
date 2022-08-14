<template>
  <form-field
    :id="id"
    :label="label"
    :placeholder="placeholder"
    :ref="id"
    :required="required"
    :rules="allRules"
    type="password"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
export default {
  props: {
    id: {
      type: String,
      default: 'password'
    },
    label: {
      type: String,
      default: 'users.password.label'
    },
    placeholder: {
      type: String,
      default: 'users.password.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: null
    },
    validate: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  computed: {
    allRules() {
      const rules = { ...this.rules }
      if (this.validate) {
        rules.min = 6
        rules.require_digit = true
        rules.require_lowercase = true
        rules.require_uppercase = true
      }
      return rules
    }
  },
  methods: {
    focus() {
      this.$refs[this.id].focus()
    }
  }
}
</script>
