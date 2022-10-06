<template>
  <tr @click="$emit('click', entry)">
    <td>
      <img v-if="entry.hasCaught" alt="Poké Ball Logo" height="20" src="@/assets/poke-ball-logo.svg" />
      <font-awesome-icon v-else icon="eye" />
    </td>
    <td><pokemon-icon :species="entry" /></td>
    <td>{{ $t('number') }} {{ entry.number.toString().padStart(3, '0') }}</td>
    <td v-text="entry.name" />
  </tr>
</template>

<script>
export default {
  name: 'PokedexRow',
  props: {
    entry: {
      type: Object,
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
