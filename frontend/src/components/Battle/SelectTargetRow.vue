<template>
  <tr :class="classes">
    <td><b-form-checkbox :checked="selected" size="lg" @change="toggleBattleMoveTarget(pokemon)" /></td>
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" /> {{ pokemon.species.name }} {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
      <br />
      <template v-if="trainer"><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</template>
      <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
    </td>
    <td><pokemon-condition :pokemon="pokemon" /></td>
    <td>
      <ability-info :ability="pokemon.ability" />
      <template v-if="pokemon.heldItem">
        <br />
        <item-info :item="pokemon.heldItem" />
      </template>
    </td>
    <td v-if="selectedBattleMoveCategory === 'Physical'" v-text="pokemon.defense" />
    <td v-else-if="selectedBattleMoveCategory === 'Special'" v-text="pokemon.specialDefense" />
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'SelectTargetRow',
  components: {
    AbilityInfo,
    ItemInfo,
    PokemonCondition
  },
  props: {
    pokemon: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters(['battleMoveAttacker', 'battleMoveTargets', 'selectedBattleMoveCategory']),
    classes() {
      if (this.pokemon.id === this.battleMoveAttacker.id) {
        return this.selected ? ['table-warning'] : ['table-success']
      } else if (this.selected) {
        return ['table-info']
      }
      return []
    },
    selected() {
      return Boolean(this.battleMoveTargets[this.pokemon.id])
    },
    trainer() {
      return this.pokemon.history?.trainer ?? null
    }
  },
  methods: {
    ...mapActions(['toggleBattleMoveTarget'])
  }
}
</script>
