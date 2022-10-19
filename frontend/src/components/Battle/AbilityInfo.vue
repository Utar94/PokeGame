<template>
  <span v-b-tooltip.hover :title="title">
    <font-awesome-icon icon="tools" />
    {{ ' ' }}
    <b-link v-if="ability.reference" :href="ability.reference" target="_blank">{{ ability.name }} <font-awesome-icon icon="external-link-alt" /></b-link>
    <template v-else>{{ ability.name }}</template>
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
