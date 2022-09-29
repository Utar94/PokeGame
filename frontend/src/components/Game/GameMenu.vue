<template>
  <b-container fluid>
    <h1 v-t="'game.menu'" />
    <b-card-group deck>
      <b-card class="mb-2" img-alt="Pokédex" :img-src="'/img/pokedex.jpg'" img-top no-body tag="article" @click="navigateGame('Pokédex')">
        <template #header>
          <h4 class="mb-0">{{ $t('trainers.pokedex.label') }}</h4>
        </template>
      </b-card>
      <b-card class="mb-2" img-alt="Poké Ball" :img-src="'/img/poke-ball.webp'" img-top no-body tag="article" @click="navigateGame('Pokémon')">
        <template #header>
          <h4 class="mb-0">{{ $t('pokemon.title') }}</h4>
        </template>
      </b-card>
      <b-card class="mb-2" img-alt="Trainer Bag" :img-src="'/img/game-bag.png'" img-top no-body tag="article" @click="navigateGame('Bag')">
        <template #header>
          <h4 class="mb-0">{{ $t('game.bag') }}</h4>
        </template>
      </b-card>
      <b-card class="mb-2" img-alt="Trainer Card" :img-src="'/img/trainer-card.png'" img-top no-body tag="article" @click="navigateGame('TrainerCard')">
        <template #header>
          <h4 class="mb-0">{{ $t('game.trainerCard') }}</h4>
        </template>
      </b-card>
    </b-card-group>
    <div class="my-3">
      <icon-button icon="sign-out-alt" text="game.exit" variant="danger" @click="setGameTrainer(null)" />
    </div>
  </b-container>
</template>

<script>
import { mapActions } from 'vuex'
export default {
  name: 'GameMenu',
  methods: {
    ...mapActions(['navigateGame', 'setGameTrainer'])
  }
}
</script>

<style scoped>
h4 {
  text-align: center;
}

.card {
  cursor: pointer;
  max-width: 20rem;
}
</style>
