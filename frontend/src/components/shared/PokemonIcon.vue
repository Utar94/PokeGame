<template>
  <span v-if="isEgg" class="b-avatar badge-light rounded-circle">
    <span class="b-avatar-img">
      <img :alt="alt" src="@/assets/egg.webp" />
    </span>
  </span>
  <b-avatar v-else-if="src" :alt="alt" :src="src" variant="light" />
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
    isEgg() {
      return (this.pokemon?.remainingHatchSteps ?? 0) > 0
    },
    src() {
      const species = this.species ?? this.pokemon?.species ?? null
      return species?.picture ?? null
    }
  }
}
</script>
