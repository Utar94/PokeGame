<template>
  <tr :class="{ clickable: Boolean(pokemon) }" @click="$emit('click', pokemon)">
    <template v-if="pokemon">
      <td class="small"><pokemon-icon :pokemon="pokemon" /></td>
      <td>
        {{ pokemon.name }}
        {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
        <gender-icon :gender="pokemon.gender" />
        <b-progress :value="pokemon.currentHitPoints" :max="pokemon.maximumHitPoints" variant="success" />
        {{ $t('battle.hpFormat', { current: pokemon.currentHitPoints, max: pokemon.maximumHitPoints }) }}
      </td>
    </template>
    <template v-else>
      <td>&mdash;</td>
      <td />
    </template>
  </tr>
</template>

<script>
export default {
  name: 'PokemonRow',
  props: {
    pokemon: {
      type: Object,
      default: null
    }
  }
}
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
.clickable:hover {
  background-color: #ececec;
}

.small {
  width: 1px;
}
</style>
