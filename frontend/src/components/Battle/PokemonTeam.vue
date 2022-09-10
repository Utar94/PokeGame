<template>
  <div>
    <h3 v-if="title">{{ title }} ({{ selected.length }}/{{ max }})</h3>
    <slot name="title" />
    <table :class="{ table: true, 'table-hover': !readonly }">
      <tbody>
        <tr
          v-for="item in pokemon"
          :key="item.id"
          :class="{ clickable: !readonly, 'table-info': selected.includes(item.id) }"
          @click.prevent="$emit('toggled', item)"
        >
          <td>
            <b-form-checkbox v-if="!readonly" :checked="selected.includes(item.id)" size="lg" />
            <template v-else>{{ item.speed }}</template>
          </td>
          <td>
            <template v-if="item.surname">
              {{ item.surname }}
              <br />
            </template>
            <gender-icon :gender="item.gender" />
            {{ $t('pokemon.levelFormat', { level: item.level }) }}
            {{ item.species.name }}
            <template v-if="readonly">
              <br />
              <template v-if="item.trainer"><gender-icon :gender="item.trainer.gender" /> {{ item.trainer.name }}</template>
              <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
            </template>
          </td>
          <td><pokemon-condition :pokemon="item" /></td>
          <!-- TODO(fpion): volatile conditions -->
          <!-- TODO(fpion): stat Stage changes -->
          <td v-if="readonly">
            <a v-if="item.ability && item.ability.reference" :href="item.ability.reference" target="_blank">
              <ability-info :ability="item.ability" />
              <font-awesome-icon icon="external-link-alt" />
            </a>
            <ability-info v-else-if="item.ability" :ability="item.ability" />
            <br v-if="item.ability && item.heldItem" />
            <a v-if="item.heldItem && item.heldItem.reference" :href="item.heldItem.reference" target="_blank">
              <held-item-info :item="item.heldItem" />
              <font-awesome-icon icon="external-link-alt" />
            </a>
            <held-item-info v-else-if="item.heldItem" :item="item.heldItem" />
          </td>
          <td v-if="!readonly">
            <template v-if="item.trainer"><gender-icon :gender="item.trainer.gender" /> {{ item.trainer.name }}</template>
            <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
          </td>
          <!-- TODO(fpion): Use a Move (if readonly & not fainted) -->
          <!-- TODO(fpion): Switch Pokémon (if readonly) -->
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import AbilityInfo from './AbilityInfo.vue'
import HeldItemInfo from './HeldItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'PokemonTeam',
  components: {
    AbilityInfo,
    HeldItemInfo,
    PokemonCondition
  },
  props: {
    max: {
      type: Number,
      default: 0
    },
    pokemon: {
      type: Array,
      default: () => []
    },
    selected: {
      type: Array,
      default: () => []
    },
    title: {
      type: String,
      default: ''
    }
  },
  computed: {
    readonly() {
      return this.$store.state.battle.step === 'Battle'
    }
  }
}
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
</style>
