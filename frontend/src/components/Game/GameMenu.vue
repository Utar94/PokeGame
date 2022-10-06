<template>
  <b-container fluid>
    <h1>
      <trainer-icon :trainer="gameTrainer" />
      {{ ' ' }}
      <gender-icon :gender="gameTrainer.gender" />
      {{ gameTrainer.name }}
      ({{ $t(`region.options.${gameTrainer.region}`) }} #{{ gameTrainer.number }})
    </h1>
    <b-card-group deck>
      <b-card class="mb-2 menu-item" img-alt="Pokédex" :img-src="'/img/pokedex.jpg'" img-top no-body tag="article" @click="navigateGame('Pokédex')">
        <template #header>
          <h4 class="mb-0">{{ $t('trainers.pokedex.label') }}</h4>
        </template>
      </b-card>
      <b-card class="mb-2 menu-item" img-alt="Poké Ball" :img-src="'/img/poke-ball.webp'" img-top no-body tag="article" @click="navigateGame('Pokémon')">
        <template #header>
          <h4 class="mb-0">{{ $t('pokemon.title') }}</h4>
        </template>
      </b-card>
      <b-card class="mb-2 menu-item" img-alt="Trainer Bag" :img-src="'/img/game-bag.png'" img-top no-body tag="article" @click="navigateGame('Bag')">
        <template #header>
          <h4 class="mb-0">{{ $t('game.bag.title') }}</h4>
        </template>
      </b-card>
      <b-card class="mb-2 menu-item" img-alt="Trainer Card" :img-src="'/img/trainer-card.png'" img-top no-body tag="article" v-b-modal.trainerCard>
        <template #header>
          <h4 class="mb-0">{{ $t('game.trainerCard') }}</h4>
        </template>
      </b-card>
    </b-card-group>
    <b-modal id="trainerCard" size="lg" :title="$t('game.trainerCard')">
      <trainer-card class="mx-auto" :trainer="gameTrainer" />
      <template #modal-footer="{ close }">
        <icon-button icon="times" text="game.close" @click="close()" />
      </template>
    </b-modal>
    <div class="my-3">
      <icon-button icon="sign-out-alt" text="game.exit" variant="danger" @click="setGameTrainer(null)" />
    </div>
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import TrainerCard from './TrainerCard.vue'
import TrainerIcon from '@/components/Trainers/TrainerIcon.vue'

export default {
  name: 'GameMenu',
  components: {
    TrainerCard,
    TrainerIcon
  },
  computed: {
    ...mapGetters(['gameTrainer'])
  },
  methods: {
    ...mapActions(['navigateGame', 'setGameTrainer'])
  }
}
</script>

<style scoped>
h4 {
  text-align: center;
}

.menu-item {
  cursor: pointer;
  max-width: 20rem;
}
</style>
