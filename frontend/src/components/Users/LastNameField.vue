<template>
  <form-field
    id="lastName"
    label="users.lastName.label"
    :maxLength="validate ? 128 : null"
    placeholder="users.lastName.placeholder"
    :required="required"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
export default {
  name: 'LastNameField',
  props: {
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
