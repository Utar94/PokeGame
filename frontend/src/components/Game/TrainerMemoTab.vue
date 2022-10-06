<template>
  <b-tab :title="$t('game.pokemon.trainerMemo')">
    <template v-if="pokemon.isEgg">
      <p v-html="$t('game.pokemon.metEgg', { date: $d(new Date(pokemon.metOn), 'card') })" />
      <p v-html="$t('game.pokemon.eggWatch')" />
    </template>
    <p v-else>
      <span v-html="$t('game.pokemon.natureFormat', { nature: pokemon.nature })" />
      <br />
      <span v-html="$t('game.pokemon.metFormat', { date: $d(new Date(pokemon.metOn), 'card'), location: pokemon.metLocation })" />
      <br />
      <span v-html="$t('game.pokemon.metLevel', { level: pokemon.metLevel })" />
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
  }
}
</script>
