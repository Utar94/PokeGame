<template>
  <b-tab :title="$t('game.pokemon.info')">
    <table class="table table-striped">
      <tbody>
        <tr>
          <th scope="row" v-t="'game.pokedex.number'" />
          <td v-text="number" />
        </tr>
        <tr>
          <th scope="row" v-t="'name.label'" />
          <td v-text="pokemon.name" />
        </tr>
        <tr>
          <th scope="row" v-t="'type.label'" />
          <td v-text="types" />
        </tr>
        <tr>
          <th scope="row" v-t="'pokemon.trainer.original'" />
          <td v-text="originalTrainer.name" />
        </tr>
        <tr>
          <th scope="row" v-t="'game.trainerNumber'" />
          <td v-text="originalTrainer.number" />
        </tr>
        <tr>
          <th scope="row" v-t="'game.pokemon.currentExperience'" />
          <td v-text="pokemon.currentExperience" />
        </tr>
        <tr>
          <th scope="row" v-t="'game.pokemon.pointsNeededToLevelUp'" />
          <td v-text="pokemon.experienceToNextLevel || '—'" />
        </tr>
        <template v-if="heldItem">
          <tr>
            <th scope="row" v-t="'pokemon.heldItem.label'" />
            <td><item-icon :item="heldItem" /> {{ heldItem.name }}</td>
          </tr>
          <tr v-if="heldItem.description">
            <td colspan="2" v-text="heldItem.description"></td>
          </tr>
        </template>
      </tbody>
    </table>
  </b-tab>
</template>

<script>
import ItemIcon from '@/components/Items/ItemIcon.vue'

export default {
  name: 'PokemonInfoTab',
  components: {
    ItemIcon
  },
  props: {
    number: {
      type: String,
      required: true
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  computed: {
    heldItem() {
      return this.pokemon.heldItem
    },
    originalTrainer() {
      return this.pokemon.originalTrainer
    },
    types() {
      const types = [this.$i18n.t(`type.options.${this.pokemon.primaryType}`)]
      if (this.pokemon.secondaryType) {
        types.push(this.$i18n.t(`type.options.${this.pokemon.secondaryType}`))
      }
      return types.join(', ')
    }
  }
}
</script>
