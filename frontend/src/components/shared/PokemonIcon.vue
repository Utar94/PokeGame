<template>
  <b-avatar v-if="src" :alt="alt" :src="src" variant="light" />
  <b-avatar v-else :alt="alt"><font-awesome-icon icon="paw" /></b-avatar>
</template>

<script>
export default {
  name: 'PokemonIcon',
  props: {
    pokemon: {
      type: Object,
      default: null
    },
    species: {
      type: Object,
      default: null
    }
  },
  computed: {
    alt() {
      const species = this.species ?? this.pokemon?.species ?? null
      return species ? `No. ${species.number.toFixed(3)} ${species.name}` : null
    },
    src() {
      const species = this.species ?? this.pokemon?.species ?? null
      return species?.picture ?? null
    }
  }
}
</script>
