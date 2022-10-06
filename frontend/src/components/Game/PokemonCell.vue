<template>
  <td :class="{ clickable }" @click="onClick">
    <pokemon-icon v-if="pokemon" :pokemon="pokemon" />
    <template v-else>&mdash;</template>
  </td>
</template>

<script>
export default {
  name: 'PokemonCell',
  props: {
    pokemon: {
      type: Object,
      default: null
    }
  },
  computed: {
    clickable() {
      return Boolean(this.pokemon)
    }
  },
  methods: {
    onClick() {
      if (this.clickable) {
        this.$emit('click', this.pokemon)
      }
    }
  }
}
</script>

<style scoped>
td {
  text-align: center;
  width: 16.66%;
}

.clickable {
  cursor: pointer;
}
.clickable:hover {
  background-color: #ececec;
}
</style>
