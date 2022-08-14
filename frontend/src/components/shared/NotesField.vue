<template>
  <form-textarea :disabled="disabled" :id="id" :label="label" :placeholder="placeholder" :rows="rows" :value="value" @input="$emit('input', $event)" />
</template>

<script>
export default {
  name: 'NotesField',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'notes'
    },
    label: {
      type: String,
      default: 'notes.label'
    },
    placeholder: {
      type: String,
      default: 'notes.placeholder'
    },
    rows: {
      type: Number,
      default: 25
    },
    value: {}
  }
}
</script>
