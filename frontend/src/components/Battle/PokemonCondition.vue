<template>
  <span>
    {{ $t('battle.hpFormat', { current: pokemon.currentHitPoints, max: pokemon.maximumHitPoints }) }}
    <template v-if="pokemon.currentHitPoints < 1">
      <br />
      <font-awesome-icon icon="heartbeat" /> <strong>{{ $t('pokemon.fainted') }}</strong>
    </template>
    <template v-else-if="battleStep !== 'PokemonSelection'">
      <template v-if="pokemon.statusCondition">
        <br />
        {{ $t(`pokemon.condition.options.${pokemon.statusCondition}`) }}
      </template>
      <template v-if="volatile.length > 0">
        <br />
        {{ volatile.join(', ') }}
      </template>
      <template v-if="statistics.length > 0">
        <br />
        {{ statistics.join(', ') }}
      </template>
    </template>
  </span>
</template>

<script>
import { mapGetters } from 'vuex'
import { getAccuracyEvasionModifier, getStatisticModifier } from '@/helpers/statisticUtils'

export default {
  name: 'PokemonCondition',
  props: {
    pokemon: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters(['battleStep', 'battleStatus']),
    statistics() {
      const status = this.battleStatus[this.pokemon.id] ?? {}
      const statistics = []
      statistics.push([this.$i18n.t('statistic.options.Attack'), getStatisticModifier(status.attack)])
      statistics.push([this.$i18n.t('statistic.options.Defense'), getStatisticModifier(status.defense)])
      statistics.push([this.$i18n.t('statistic.options.SpecialAttack'), getStatisticModifier(status.specialAttack)])
      statistics.push([this.$i18n.t('statistic.options.SpecialDefense'), getStatisticModifier(status.specialDefense)])
      statistics.push([this.$i18n.t('statistic.options.Speed'), getStatisticModifier(status.speed)])
      statistics.push([this.$i18n.t('battle.makeMove.accuracy'), getAccuracyEvasionModifier(status.accuracy)])
      statistics.push([this.$i18n.t('battle.makeMove.evasion'), getAccuracyEvasionModifier(status.evasion)])
      return statistics.filter(([, value]) => value !== 1).map(([statistic, value]) => `${statistic} ×${Math.round(value * 1000) / 1000}`)
    },
    volatile() {
      return this.battleStatus[this.pokemon.id]?.volatile ?? []
    }
  }
}
</script>
