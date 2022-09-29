<template>
  <b-tab :title="$t(title)">
    <p v-if="items.length === 0" v-t="'trainers.inventory.empty'" />
    <table v-else class="table table-hover">
      <tbody>
        <tr v-for="(item, index) in items" :key="index" @click="$emit('selected', item)">
          <td><item-icon :item="item" /></td>
          <td v-text="item.name" />
          <td>&times;</td>
          <td v-text="item.quantity" />
        </tr>
      </tbody>
    </table>
  </b-tab>
</template>

<script>
import ItemIcon from '@/components/Items/ItemIcon.vue'

export default {
  name: 'BagTab',
  components: {
    ItemIcon
  },
  props: {
    items: {
      type: Array,
      required: true
    },
    title: {
      type: String,
      required: true
    }
  }
}
</script>

<style scoped>
tr {
  cursor: pointer;
}
</style>
