<template>
  <b-tab :title="$t('game.pokemon.trainerMemo')">
    <template v-if="pokemon.isEgg">
      <p v-html="$t('game.pokemon.metEgg', { date: $d(new Date(pokemon.metOn), 'card') })" />
      <p v-html="$t('game.pokemon.eggWatch')" />
    </template>
    <p v-else>
      <span v-html="$t('game.pokemon.natureFormat', { nature: pokemon.nature.name })" />
      <br />
      <span v-html="$t('game.pokemon.metFormat', { date: $d(new Date(pokemon.metOn), 'card'), location: pokemon.metLocation })" />
      <br />
      <span v-html="$t('game.pokemon.metLevel', { level: pokemon.metLevel })" />
      <!-- TODO(fpion): Characteristic -->
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
