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
      return species ? `${this.$i18n.t('number')} ${species.number.toString().padStart(3, '0')} ${species.name}` : null
    },
    src() {
      if ((this.pokemon?.remainingHatchSteps ?? 0) > 0) {
        return '/img/egg.webp'
      }
      const species = this.species ?? this.pokemon?.species ?? null
      return species?.picture ?? null
    }
  }
}
</script>
