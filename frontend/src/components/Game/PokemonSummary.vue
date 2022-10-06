<template>
  <b-modal size="xl" :title="$t('game.pokemon.summary')" :visible="value" @change="$emit('input', $event)">
    <b-row v-if="pokemon">
      <b-col lg="6">
        <b-tabs content-class="mt-3">
          <pokemon-info-tab :number="number" :pokemon="pokemon" />
          <trainer-memo-tab :pokemon="pokemon" />
          <pokemon-skill-tab :pokemon="pokemon" />
          <battle-move-tab :pokemon="pokemon" />
        </b-tabs>
      </b-col>
      <b-col lg="6">
        <h6>
          <item-icon :item="pokemon.caughtBall" />
          {{ ' ' }}
          <template v-if="pokemon.isEgg">{{ $t('game.pokemon.egg') }}</template>
          <template v-else>
            {{ pokemon.name }}
            {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
            <gender-icon :gender="pokemon.gender" />
          </template>
        </h6>
        <b-img :alt="alt" fluid :src="src" />
      </b-col>
    </b-row>
    <template #modal-footer="{ close }">
      <icon-button icon="times" text="game.close" @click="close()" />
    </template>
  </b-modal>
</template>

<script>
import { mapGetters } from 'vuex'
import BattleMoveTab from './BattleMoveTab.vue'
import ItemIcon from '@/components/Items/ItemIcon.vue'
import PokemonInfoTab from './PokemonInfoTab.vue'
import PokemonSkillTab from './PokemonSkillTab.vue'
import TrainerMemoTab from './TrainerMemoTab.vue'
import { getGamePokemonSummary } from '@/api/game'

export default {
  name: 'PokemonSummary',
  components: {
    BattleMoveTab,
    ItemIcon,
    PokemonInfoTab,
    PokemonSkillTab,
    TrainerMemoTab
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
      return this.pokemon.isEgg
        ? '—'
        : ((this.gameTrainer.hasNationalPokedex ? this.pokemon.nationalNumber : this.pokemon.regionalNumber) ?? '???').toString().padStart(3, '0')
    },
    src() {
      return this.pokemon.isEgg ? '/img/egg.webp' : this.pokemon.picture
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
