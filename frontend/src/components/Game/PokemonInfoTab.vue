<template>
  <b-tab :title="$t('game.pokemon.info')">
    <table class="table">
      <tbody>
        <tr>
          <th scope="row" v-t="'game.pokedex.number'" />
          <td v-text="number" />
        </tr>
        <tr>
          <th scope="row" v-t="'name.label'" />
          <td v-text="pokemon.isEgg ? '—' : pokemon.name" />
        </tr>
        <tr>
          <th scope="row" v-t="'type.label'" />
          <td v-text="types" />
        </tr>
        <tr>
          <th scope="row" v-t="'pokemon.trainer.original'" />
          <td v-text="originalTrainer ? originalTrainer.name : '—'" />
        </tr>
        <tr>
          <th scope="row" v-t="'game.trainerNumber'" />
          <td v-text="originalTrainer ? originalTrainer.number : '—'" />
        </tr>
        <tr>
          <th scope="row" v-t="'game.pokemon.currentExperience'" />
          <td v-text="pokemon.currentExperience || '—'" />
        </tr>
        <tr>
          <th scope="row" v-t="'game.pokemon.pointsNeededToLevelUp'" />
          <td v-text="pokemon.experienceToNextLevel || '—'" />
        </tr>
        <template v-if="heldItem">
          <tr class="held-item">
            <th scope="row" v-t="'pokemon.heldItem.label'" />
            <td><item-icon :item="heldItem" /> {{ heldItem.name }}</td>
          </tr>
          <tr v-if="heldItem.description" class="held-item">
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
      const types = []
      if (this.pokemon.primaryType) {
        types.push(this.$i18n.t(`type.options.${this.pokemon.primaryType}`))
      }
      if (this.pokemon.secondaryType) {
        types.push(this.$i18n.t(`type.options.${this.pokemon.secondaryType}`))
      }
      return types.join(', ') || '—'
    }
  }
}
</script>

<style scoped>
.held-item {
  background-color: rgba(0, 0, 0, 0.05);
}
</style>
