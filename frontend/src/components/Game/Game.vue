<template>
  <div>
    <trainer-selection v-if="gameTrainer === null" />
    <template v-else>
      <game-menu v-if="gamePage === null" />
      <trainer-pokemon v-else-if="gamePage === 'Pokémon'" />
      <trainer-pokedex v-else-if="gamePage === 'Pokédex'" />
      <trainer-bag v-else-if="gamePage === 'Bag'" />
    </template>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import GameMenu from './GameMenu.vue'
import TrainerBag from './TrainerBag.vue'
import TrainerPokedex from './TrainerPokedex.vue'
import TrainerPokemon from './TrainerPokemon.vue'
import TrainerSelection from './TrainerSelection.vue'

export default {
  name: 'Game',
  components: {
    GameMenu,
    TrainerBag,
    TrainerPokedex,
    TrainerPokemon,
    TrainerSelection
  },
  computed: {
    ...mapGetters(['gamePage', 'gameTrainer'])
  }
}
</script>
