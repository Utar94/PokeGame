<template>
  <span v-b-tooltip.hover :title="title">
    <font-awesome-icon icon="shopping-cart" />
    {{ item.name }}
  </span>
</template>

<script>
export default {
  name: 'HeldItemInfo',
  props: {
    item: {
      type: Object,
      required: true
    }
  },
  computed: {
    title() {
      return this.item.notes || this.item.description
    }
  }
}
</script>
