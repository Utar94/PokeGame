<template>
  <div>
    <h3 v-if="title">{{ title }} ({{ selected.length }}/{{ max }})</h3>
    <slot name="title" />
    <table class="table table-hover">
      <tbody>
        <tr
          v-for="item in pokemon"
          :key="item.id"
          :class="{ clickable: true, 'table-info': selected.includes(item.id) }"
          @click.prevent="$emit('toggled', item)"
        >
          <td><b-form-checkbox :checked="selected.includes(item.id)" size="lg" /></td>
          <td v-text="item.position + 1" />
          <td>
            <template v-if="item.surname">
              {{ item.surname }}
              <br />
            </template>
            <gender-icon :gender="item.gender" />
            {{ $t('pokemon.levelFormat', { level: item.level }) }}
            {{ item.species.name }}
          </td>
          <td><pokemon-condition :pokemon="item" /></td>
          <td>
            <template v-if="item.trainer"><gender-icon :gender="item.trainer.gender" /> {{ item.trainer.name }}</template>
            <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'PokemonTeam',
  components: {
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
  }
}
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
</style>
