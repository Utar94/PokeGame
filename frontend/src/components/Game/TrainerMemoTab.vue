<template>
  <b-tab :title="$t('game.pokemon.trainerMemo')">
    <template v-if="pokemon.isEgg">
      <p v-html="$t('game.pokemon.metEgg', { date: $d(new Date(pokemon.metOn), 'card') })" />
      <p>
        <span v-t="'game.pokemon.eggWatch'" />
        <br />
        <span v-if="pokemon.remainingEggCycles <= 5" v-html="$t('game.pokemon.remainingEggCycles.5OrFewer')" />
        <span v-else-if="pokemon.remainingEggCycles <= 10" v-html="$t('game.pokemon.remainingEggCycles.6To10')" />
        <span v-else-if="pokemon.remainingEggCycles <= 40" v-html="$t('game.pokemon.remainingEggCycles.11To40')" />
        <span v-else v-html="$t('game.pokemon.remainingEggCycles.41OrMore')" />
      </p>
    </template>
    <p v-else>
      <span v-html="$t('game.pokemon.natureFormat', { nature: pokemon.nature.name })" />
      <br />
      <span v-html="$t('game.pokemon.metFormat', { date: $d(new Date(pokemon.metOn), 'card'), location: pokemon.metLocation })" />
      <br />
      <span v-html="$t('game.pokemon.metLevel', { level: pokemon.metLevel })" />
      <template v-if="pokemon.characteristic">
        <br />
        <span v-t="`pokemon.characteristic.options.${pokemon.characteristic}`" />
      </template>
      <br />
      <span v-if="flavor" v-html="$t('game.pokemon.flavorFormat', { flavor })" />
      <span v-else v-t="'game.pokemon.eatAnything'" />
    </p>
  </b-tab>
</template>

<script>
export default {
  name: 'TrainerMemoTab',
  props: {
    pokemon: {
      type: Object,
      required: true
    }
  },
  computed: {
    flavor() {
      return this.pokemon.nature?.favoriteFlavor ? this.$i18n.t(`game.flavors.${this.pokemon.nature.favoriteFlavor}`) : null
    }
  }
}
</script>
