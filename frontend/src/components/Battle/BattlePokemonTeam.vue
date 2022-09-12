<template>
  <div>
    <slot name="title" />
    <table class="table">
      <tbody>
        <battle-pokemon-row v-for="item in pokemon" :key="item.id" :canSwitch="canSwitch" :pokemon="item" />
      </tbody>
    </table>
  </div>
</template>

<script>
import BattlePokemonRow from './BattlePokemonRow.vue'

export default {
  name: 'PokemonTeam',
  components: {
    BattlePokemonRow
  },
  props: {
    canSwitch: {
      type: Boolean,
      default: false
    },
    pokemon: {
      type: Array,
      default: () => []
    }
  }
}
</script>
