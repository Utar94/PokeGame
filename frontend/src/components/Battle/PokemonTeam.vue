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
          <td v-if="!readonly"><b-form-checkbox :checked="selected.includes(item.id)" size="lg" /></td>
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
          <!-- TODO(fpion): Ability (if readonly) with Notes & Reference -->
          <!-- TODO(fpion): Speed (if readonly) with Stage changes -->
          <!-- TODO(fpion): Held Item (if readonly) with Notes & Reference -->
          <!-- TODO(fpion): Use a Move (if readonly) -->
          <!-- TODO(fpion): Switch Pokémon (if readonly) -->
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
