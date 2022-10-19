<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="required"
    :rules="rules"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
import { getUsers } from '@/api/users'

export default {
  name: 'UserSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'user'
    },
    label: {
      type: String,
      default: 'users.select.label'
    },
    placeholder: {
      type: String,
      default: 'users.select.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: null
    },
    value: {}
  },
  data() {
    return {
      users: []
    }
  },
  computed: {
    options() {
      return this.orderBy(
        this.users.map(({ id, fullName, username }) => ({
          text: fullName ? `${fullName} (${username})` : username,
          value: id
        })),
        'text'
      )
    }
  },
  async created() {
    try {
      const { data } = await getUsers({ isConfirmed: true, isDisabled: false })
      this.users = data.items
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
