<template>
  <div>
    <h3 v-t="'battle.makeMove.targets'" />
    <table class="table">
      <thead>
        <tr>
          <th scope="col" />
          <th scope="col" />
          <th scope="col" v-t="'pokemon.identification'" />
          <th scope="col" v-t="'battle.condition'" />
          <th scope="col" v-t="'battle.abilityAndHeldItem'" />
          <th scope="col" v-t="'battle.experience.options'" />
          <th scope="col" v-t="'battle.experience.modifiers'" />
          <th scope="col" v-t="'battle.experience.otherModifiers'" />
          <th scope="col" v-t="'experience'" />
        </tr>
      </thead>
      <tbody>
        <winning-pokemon-row v-for="pokemon in battlingPlayerPokemon" :key="pokemon.id" :pokemon="pokemon" />
      </tbody>
    </table>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import WinningPokemonRow from './WinningPokemonRow.vue'

export default {
  name: 'WinningPokemonTable',
  components: {
    WinningPokemonRow
  },
  computed: {
    ...mapGetters(['battlingPlayerPokemon'])
  }
}
</script>
