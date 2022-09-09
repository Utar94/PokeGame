<template>
  <div>
    <h3>{{ title }} ({{ selected.length }}/{{ max }})</h3>
    <table class="table table-hover">
      <tbody>
        <tr v-for="item in pokemon" :key="item.id" :class="{ 'table-info': selected.includes(item.id) }" @click.prevent="$emit('toggled', item)">
          <td><b-form-checkbox :checked="selected.includes(item.id)" size="lg" /></td>
          <td>
            <template v-if="item.surname">
              {{ item.surname }}
              <br />
            </template>
            <gender-icon :gender="item.gender" />
            {{ $t('pokemon.levelFormat', { level: item.level }) }}
            {{ item.species.name }}
          </td>
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
export default {
  name: 'PokemonTeam',
  props: {
    max: {
      type: Number,
      default: 6
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
      required: true
    }
  }
}
</script>

<style scoped>
tr {
  cursor: pointer;
}
</style>
