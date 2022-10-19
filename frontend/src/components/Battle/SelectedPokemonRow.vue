<template>
  <tr :class="{ clickable: true, 'table-info': selected }" @click.prevent="toggle">
    <td><b-form-checkbox :checked="selected" size="lg" /></td>
    <td><pokemon-icon :pokemon="pokemon" /></td>
    <td>
      <template v-if="trainer"><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</template>
      <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
    </td>
    <td v-text="pokemon.position === null ? '—' : pokemon.position" />
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" /> {{ pokemon.species.name }} {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
    </td>
    <td><pokemon-condition :pokemon="pokemon" /></td>
  </tr>
</template>

<script>
import { mapActions } from 'vuex'
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'SelectedPokemonRow',
  components: {
    PokemonCondition
  },
  props: {
    pokemon: {
      type: Object,
      required: true
    },
    selected: {
      type: Boolean,
      required: true
    },
    team: {
      type: String,
      required: true
    }
  },
  computed: {
    trainer() {
      return this.pokemon.history?.trainer ?? null
    }
  },
  methods: {
    ...mapActions(['toggleBattlingOpponentPokemon', 'toggleBattlingPlayerPokemon']),
    toggle() {
      if (this.team === 'players') {
        this.toggleBattlingPlayerPokemon(this.pokemon.id)
      } else {
        this.toggleBattlingOpponentPokemon(this.pokemon.id)
      }
    }
  }
}
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
</style>
