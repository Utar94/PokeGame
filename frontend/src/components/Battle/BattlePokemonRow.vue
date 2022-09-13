<template>
  <tr :class="{ 'table-info': isActive }">
    <td v-text="pokemon.speed" />
    <td v-text="pokemon.position + 1" />
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" />
      {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
      {{ pokemon.species.name }}
      <br />
      <template v-if="trainer"><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</template>
      <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
    </td>
    <td><pokemon-condition :pokemon="pokemon" /></td>
    <!-- TODO(fpion): volatile conditions -->
    <!-- TODO(fpion): stat Stage changes -->
    <td>
      <a v-if="ability && ability.reference" :href="ability.reference" target="_blank">
        <ability-info :ability="ability" />
        <font-awesome-icon icon="external-link-alt" />
      </a>
      <ability-info v-else-if="ability" :ability="ability" />
      <br v-if="ability && heldItem" />
      <a v-if="heldItem && heldItem.reference" :href="heldItem.reference" target="_blank">
        <held-item-info :item="heldItem" />
        <font-awesome-icon icon="external-link-alt" />
      </a>
      <held-item-info v-else-if="heldItem" :item="heldItem" />
    </td>
    <td v-if="canSwitch">
      <template v-if="isActive && pokemon.currentHitPoints">
        <icon-button icon="magic" text="battle.makeMove.title" variant="danger" v-b-modal="`makeMove_${pokemon.id}`" />
        <make-move-modal :id="`makeMove_${pokemon.id}`" :pokemon="pokemon" />
        <div class="my-2" />
      </template>
      <icon-button
        icon="exchange-alt"
        :text="isActive ? 'battle.combatTracker.withdraw' : 'battle.combatTracker.send'"
        variant="success"
        @click="togglePokemon(pokemon.id)"
      />
    </td>
  </tr>
</template>

<script>
import { mapActions } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import HeldItemInfo from './HeldItemInfo.vue'
import MakeMoveModal from './MakeMoveModal.vue'
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'BattlePokemonRow',
  components: {
    AbilityInfo,
    HeldItemInfo,
    MakeMoveModal,
    PokemonCondition
  },
  props: {
    canSwitch: {
      type: Boolean,
      default: false
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  computed: {
    ability() {
      return this.pokemon.ability
    },
    heldItem() {
      return this.pokemon.heldItem
    },
    isActive() {
      return this.$store.state.battle.activePokemon.includes(this.pokemon.id)
    },
    trainer() {
      return this.pokemon.history?.trainer ?? null
    }
  },
  methods: {
    ...mapActions(['togglePokemon'])
  }
}
</script>
