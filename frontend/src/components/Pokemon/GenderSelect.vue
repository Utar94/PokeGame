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
  >
    <slot />
  </form-select>
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
      default: 'pokemon.gender.label'
    },
    placeholder: {
      type: String,
      default: 'pokemon.gender.placeholder'
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
        Object.entries(this.$i18n.t('pokemon.gender.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
