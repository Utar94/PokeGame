<template>
  <tr :class="{ 'table-info': active }">
    <td v-text="pokemon.speed" />
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" /> {{ pokemon.species.name }} {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
      <br />
      <template v-if="trainer"><gender-icon :gender="trainer.gender" /> {{ trainer.name }} ({{ pokemon.position + 1 }})</template>
      <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
    </td>
    <td>
      <pokemon-condition :pokemon="pokemon" />
      <!-- TODO(fpion): Volatile Conditions -->
      <!-- TODO(fpion): Stat Changes -->
    </td>
    <td>
      <ability-info :ability="pokemon.ability" />
      <template v-if="pokemon.heldItem">
        <br />
        <item-info :item="pokemon.heldItem" />
      </template>
    </td>
    <td>
      <icon-button
        v-if="active"
        class="mx-1"
        :disabled="hasEnded || pokemon.currentHitPoints === 0"
        icon="magic"
        text="battle.makeMove.label"
        variant="danger"
        @click="makeMove"
      />
      <switch-pokemon v-if="isTrainerBattle || team === 'players'" :active="active" :pokemon="pokemon" />
    </td>
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'
import SwitchPokemon from './SwitchPokemon.vue'

export default {
  name: 'BattlingPokemonRow',
  components: {
    AbilityInfo,
    ItemInfo,
    PokemonCondition,
    SwitchPokemon
  },
  props: {
    active: {
      type: Boolean,
      required: true
    },
    pokemon: {
      type: Object,
      required: true
    },
    team: {
      type: String,
      required: true
    }
  },
  computed: {
    ...mapGetters(['hasEnded', 'isTrainerBattle']),
    trainer() {
      return this.pokemon.history?.trainer ?? null
    }
  },
  methods: {
    ...mapActions(['makeBattleMove']),
    makeMove() {
      this.makeBattleMove(this.pokemon)
      this.$bvModal.show('makeMove')
    }
  }
}
</script>
