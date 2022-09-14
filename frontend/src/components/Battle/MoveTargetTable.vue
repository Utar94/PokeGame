<template>
  <table class="table">
    <thead>
      <tr>
        <th scope="col" />
        <th scope="col" v-t="'pokemon.trainer.position'" />
        <th scope="col" v-t="'pokemon.identification'" />
        <th scope="col" v-t="'battle.makeMove.condition'" />
        <th scope="col" v-t="'battle.makeMove.abilityAndHeldItem'" />
      </tr>
    </thead>
    <tbody>
      <tr v-for="target in targets" :key="target.id" :class="{ 'table-info': selected.includes(target.id) }">
        <td><b-form-checkbox :checked="selected.includes(target.id)" size="lg" @input="$emit('toggled', target)" /></td>
        <td v-text="target.position + 1" />
        <td>
          <template v-if="target.surname">
            {{ target.surname }}
            <br />
          </template>
          <gender-icon :gender="target.gender" />
          {{ $t('pokemon.levelFormat', { level: target.level }) }}
          {{ target.species.name }}
          <br />
          <template v-if="target.history"><gender-icon :gender="target.history.trainer.gender" /> {{ target.history.trainer.name }}</template>
          <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
        </td>
        <td><pokemon-condition :pokemon="target" /></td>
        <!-- TODO(fpion): volatile conditions -->
        <!-- TODO(fpion): stat Stage changes -->
        <td>
          <a v-if="target.ability && target.ability.reference" :href="target.ability.reference" target="_blank">
            <ability-info :ability="target.ability" />
            <font-awesome-icon icon="external-link-alt" />
          </a>
          <ability-info v-else-if="target.ability" :ability="target.ability" />
          <br v-if="target.ability && target.heldItem" />
          <a v-if="target.heldItem && target.heldItem.reference" :href="target.heldItem.reference" target="_blank">
            <item-info :item="target.heldItem" />
            <font-awesome-icon icon="external-link-alt" />
          </a>
          <item-info v-else-if="target.heldItem" :item="target.heldItem" />
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script>
import AbilityInfo from './AbilityInfo.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'MoveTargetTable',
  components: {
    AbilityInfo,
    ItemInfo,
    PokemonCondition
  },
  props: {
    selected: {
      type: Array,
      default: () => []
    },
    targets: {
      type: Array,
      default: () => []
    }
  }
}
</script>
