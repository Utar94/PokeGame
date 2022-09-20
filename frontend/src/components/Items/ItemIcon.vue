<template>
  <b-avatar v-if="item.picture" :alt="item.name" :src="item.picture" variant="light" />
  <b-avatar v-else :alt="item.name"><font-awesome-icon icon="shopping-cart" /></b-avatar>
</template>

<script>
export default {
  name: 'ItemIcon',
  props: {
    item: {
      type: Object,
      required: true
    }
  }
}
</script>
