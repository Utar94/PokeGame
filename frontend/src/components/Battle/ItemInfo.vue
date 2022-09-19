<template>
  <span v-b-tooltip.hover :title="title">
    <font-awesome-icon icon="shopping-cart" />
    {{ ' ' }}
    <a v-if="item.reference" :href="item.reference" target="_blank">{{ item.name }} <font-awesome-icon icon="external-link-alt" /></a>
    <template v-else>{{ item.name }}</template>
  </span>
</template>

<script>
export default {
  name: 'ItemInfo',
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
