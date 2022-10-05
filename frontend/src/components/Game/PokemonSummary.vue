<template>
  <b-modal size="xl" :title="$t('game.pokemon.summary')" :visible="value" @change="$emit('input', $event)">
    <b-row v-if="pokemon">
      <b-col>
        <b-tabs content-class="mt-3">
          <pokemon-info-tab :number="number" :pokemon="pokemon" />
          <b-tab :title="$t('game.pokemon.trainerMemo')">
            <!-- TODO(fpion): implement -->
          </b-tab>
          <b-tab :title="$t('game.pokemon.skills')">
            <!-- TODO(fpion): implement -->
          </b-tab>
          <b-tab :title="$t('game.pokemon.battleMoves')">
            <!-- TODO(fpion): implement -->
          </b-tab>
        </b-tabs>
      </b-col>
      <b-col>
        <h6>
          <item-icon :item="pokemon.caughtBall" />
          {{ pokemon.name }}
          {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
          <gender-icon :gender="pokemon.gender" />
        </h6>
        <img :alt="alt" :src="pokemon.picture" />
      </b-col>
    </b-row>
    <template #modal-footer="{ close }">
      <icon-button icon="times" text="game.close" @click="close()" />
    </template>
  </b-modal>
</template>

<script>
import { mapGetters } from 'vuex'
import ItemIcon from '@/components/Items/ItemIcon.vue'
import PokemonInfoTab from './PokemonInfoTab.vue'
import { getGamePokemonSummary } from '@/api/game'

export default {
  name: 'PokemonSummary',
  components: {
    ItemIcon,
    PokemonInfoTab
  },
  props: {
    pokemonId: {
      type: String,
      default: ''
    },
    value: {
      type: Boolean,
      required: true
    }
  },
  data() {
    return {
      pokemon: null
    }
  },
  computed: {
    ...mapGetters(['gameTrainer']),
    alt() {
      return `${this.$i18n.t('number')} ${this.number} ${this.pokemon.name}`
    },
    number() {
      return ((this.gameTrainer.hasNationalPokedex ? this.pokemon.nationalNumber : this.pokemon.regionalNumber) ?? '???').toString().padStart(3, '0')
    }
  },
  watch: {
    pokemonId: {
      immediate: true,
      async handler(pokemonId) {
        if (pokemonId) {
          try {
            const { data } = await getGamePokemonSummary(pokemonId)
            this.pokemon = data
          } catch (e) {
            this.handleError(e)
          }
        } else {
          this.pokemon = null
        }
      }
    }
  }
}
</script>

<style scoped>
img {
  max-width: 360px;
}
</style>
