<template>
  <b-tab :disabled="pokemon.isEgg" :title="$t('game.pokemon.skills')">
    <table class="table table-striped">
      <tbody>
        <tr>
          <th scope="row" v-t="'statistic.options.HP'" />
          <td>{{ pokemon.currentHitPoints }}/{{ pokemon.maximumHitPoints }}</td>
        </tr>
        <tr>
          <th scope="row" v-t="'statistic.options.Attack'" />
          <td v-text="pokemon.attack" />
        </tr>
        <tr>
          <th scope="row" v-t="'statistic.options.Defense'" />
          <td v-text="pokemon.defense" />
        </tr>
        <tr>
          <th scope="row" v-t="'statistic.options.SpecialAttack'" />
          <td v-text="pokemon.specialAttack" />
        </tr>
        <tr>
          <th scope="row" v-t="'statistic.options.SpecialDefense'" />
          <td v-text="pokemon.specialDefense" />
        </tr>
        <tr>
          <th scope="row" v-t="'statistic.options.Speed'" />
          <td v-text="pokemon.speed" />
        </tr>
        <template v-if="pokemon.ability">
          <tr>
            <th scope="row" v-t="'abilities.select.label'" />
            <td v-text="ability.name" />
          </tr>
          <tr v-if="ability.description">
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
    }
  }
}
</script>
