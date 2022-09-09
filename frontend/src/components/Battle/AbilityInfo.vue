<template>
  <span v-b-tooltip.hover :title="title">
    <font-awesome-icon icon="tools" />
    {{ ability.name }}
  </span>
</template>

<script>
export default {
  name: 'AbilityInfo',
  props: {
    ability: {
      type: Object,
      required: true
    }
  },
  computed: {
    title() {
      return this.ability.notes || this.ability.description
    }
  }
}
</script>
