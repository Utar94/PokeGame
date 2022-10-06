<template>
  <tr :class="{ clickable }" @click="onClick">
    <template v-if="pokemon">
      <td class="small"><pokemon-icon :pokemon="pokemon" /></td>
      <td v-if="pokemon.isEgg" v-t="'game.pokemon.egg'" />
      <td v-else>
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
  name: 'PartyPokemonRow',
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
