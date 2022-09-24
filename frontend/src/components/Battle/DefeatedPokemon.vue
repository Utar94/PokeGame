<template>
  <div>
    <h3 v-t="'battle.experience.defeatedPokemon'" />
    <table class="table">
      <thead>
        <tr>
          <th scope="col" />
          <th scope="col" v-t="'pokemon.identification'" />
          <th scope="col" v-t="'battle.condition'" />
          <th scope="col" v-t="'battle.abilityAndHeldItem'" />
          <th scope="col" v-t="'species.baseExperienceYield'" />
        </tr>
      </thead>
      <tbody>
        <tr class="table-danger">
          <td><pokemon-icon :pokemon="pokemon" /></td>
          <td>
            <template v-if="pokemon.surname">
              {{ pokemon.surname }}
              <br />
            </template>
            <gender-icon :gender="pokemon.gender" /> {{ species.name }}
            {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
            <br />
            <template v-if="trainer"><gender-icon :gender="trainer.gender" /> {{ trainer.name }} ({{ pokemon.position }})</template>
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
          <td v-text="species.baseExperienceYield" />
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'DefeatedPokemon',
  components: {
    AbilityInfo,
    ItemInfo,
    PokemonCondition
  },
  computed: {
    ...mapGetters(['battleExperienceDefeatedPokemon']),
    pokemon() {
      return this.battleExperienceDefeatedPokemon
    },
    species() {
      return this.pokemon.species
    },
    trainer() {
      return this.pokemon.trainer
    }
  }
}
</script>
