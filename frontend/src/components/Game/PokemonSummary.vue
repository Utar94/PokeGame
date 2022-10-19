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
import BattleMoveTab from './BattleMoveTab.vue'
import ItemIcon from '@/components/Items/ItemIcon.vue'
import PokemonInfoTab from './PokemonInfoTab.vue'
import PokemonSkillTab from './PokemonSkillTab.vue'
import TrainerMemoTab from './TrainerMemoTab.vue'

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
    pokemon: {
      type: Object,
      default: null
    },
    value: {
      type: Boolean,
      required: true
    }
  },
  computed: {
    alt() {
      return `${this.$i18n.t('number')} ${this.number} ${this.pokemon.name}`
    },
    number() {
      return this.pokemon.isEgg ? '—' : this.pokemon.number?.toString().padStart(3, '0') ?? '???'
    },
    src() {
      return this.pokemon.isEgg ? '/img/egg.webp' : this.pokemon.picture
    }
  }
}
</script>
