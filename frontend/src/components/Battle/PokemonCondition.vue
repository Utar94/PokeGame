<template>
  <span>
    {{ $t('battle.combatTracker.hpFormat', { current: pokemon.currentHitPoints, max: pokemon.maximumHitPoints }) }}
    <br v-if="pokemon.currentHitPoints < 1 || pokemon.statusCondition" />
    <template v-if="pokemon.currentHitPoints < 1">
      <font-awesome-icon icon="heartbeat" /> <strong>{{ $t('pokemon.fainted') }}</strong>
    </template>
    <template v-else-if="pokemon.statusCondition">{{ $t(`pokemon.condition.options.${pokemon.statusCondition}`) }}</template>
  </span>
</template>

<script>
export default {
  name: 'PokemonCondition',
  props: {
    pokemon: {
      type: Object,
      required: true
    }
  }
}
</script>
