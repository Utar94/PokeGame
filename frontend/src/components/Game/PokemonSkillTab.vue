<template>
  <b-tab :disabled="pokemon.isEgg" :title="$t('game.pokemon.skills')">
    <table class="table">
      <tbody>
        <tr :class="{ 'table-danger': decreasedStatistic === 'HP', 'table-primary': increasedStatistic === 'HP' }">
          <th scope="row" v-t="'statistic.options.HP'" />
          <td>
            <font-awesome-icon v-if="decreasedStatistic === 'HP'" icon="level-down-alt" />
            <font-awesome-icon v-if="increasedStatistic === 'HP'" icon="level-up-alt" />
            {{ pokemon.currentHitPoints }}/{{ pokemon.maximumHitPoints }}
          </td>
        </tr>
        <tr :class="{ 'table-danger': decreasedStatistic === 'Attack', 'table-primary': increasedStatistic === 'Attack' }">
          <th scope="row" v-t="'statistic.options.Attack'" />
          <td>
            <font-awesome-icon v-if="decreasedStatistic === 'Attack'" icon="level-down-alt" />
            <font-awesome-icon v-if="increasedStatistic === 'Attack'" icon="level-up-alt" />
            {{ pokemon.attack }}
          </td>
        </tr>
        <tr :class="{ 'table-danger': decreasedStatistic === 'Defense', 'table-primary': increasedStatistic === 'Defense' }">
          <th scope="row" v-t="'statistic.options.Defense'" />
          <td>
            <font-awesome-icon v-if="decreasedStatistic === 'Defense'" icon="level-down-alt" />
            <font-awesome-icon v-if="increasedStatistic === 'Defense'" icon="level-up-alt" />
            {{ pokemon.defense }}
          </td>
        </tr>
        <tr :class="{ 'table-danger': decreasedStatistic === 'SpecialAttack', 'table-primary': increasedStatistic === 'SpecialAttack' }">
          <th scope="row" v-t="'statistic.options.SpecialAttack'" />
          <td>
            <font-awesome-icon v-if="decreasedStatistic === 'SpecialAttack'" icon="level-down-alt" />
            <font-awesome-icon v-if="increasedStatistic === 'SpecialAttack'" icon="level-up-alt" />
            {{ pokemon.specialAttack }}
          </td>
        </tr>
        <tr :class="{ 'table-danger': decreasedStatistic === 'SpecialDefense', 'table-primary': increasedStatistic === 'SpecialDefense' }">
          <th scope="row" v-t="'statistic.options.SpecialDefense'" />
          <td>
            <font-awesome-icon v-if="decreasedStatistic === 'SpecialDefense'" icon="level-down-alt" />
            <font-awesome-icon v-if="increasedStatistic === 'SpecialDefense'" icon="level-up-alt" />
            {{ pokemon.specialDefense }}
          </td>
        </tr>
        <tr :class="{ 'table-danger': decreasedStatistic === 'Speed', 'table-primary': increasedStatistic === 'Speed' }">
          <th scope="row" v-t="'statistic.options.Speed'" />
          <td>
            <font-awesome-icon v-if="decreasedStatistic === 'Speed'" icon="level-down-alt" />
            <font-awesome-icon v-if="increasedStatistic === 'Speed'" icon="level-up-alt" />
            {{ pokemon.speed }}
          </td>
        </tr>
        <template v-if="pokemon.ability">
          <tr class="ability">
            <th scope="row" v-t="'abilities.select.label'" />
            <td v-text="ability.name" />
          </tr>
          <tr v-if="ability.description" class="ability">
            <td colspan="2" v-text="ability.description"></td>
          </tr>
        </template>
      </tbody>
    </table>
  </b-tab>
</template>

<script>
export default {
  name: 'PokemonSkillTab',
  props: {
    pokemon: {
      type: Object,
      default: null
    }
  },
  computed: {
    ability() {
      return this.pokemon.ability
    },
    decreasedStatistic() {
      return this.pokemon.nature?.decreasedStatistic ?? null
    },
    increasedStatistic() {
      return this.pokemon.nature?.increasedStatistic ?? null
    }
  }
}
</script>

<style scoped>
.ability {
  background-color: rgba(0, 0, 0, 0.05);
}
</style>
