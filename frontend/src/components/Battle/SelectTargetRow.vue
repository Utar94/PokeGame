<template>
  <tr :class="classes">
    <td>
      <strong v-if="attacker" v-t="'battle.makeMove.attacker'" />
      <b-form-checkbox :checked="Boolean(target)" size="lg" @change="toggleBattleMoveTarget(pokemon)" />
    </td>
    <td><pokemon-icon :pokemon="pokemon" /></td>
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" /> {{ pokemon.species.name }} {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
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
    <template v-if="defensiveStatistic">
      <td>
        <form-field
          :disabled="!target"
          hideLabel
          :id="`${defensiveStatistic}_${pokemon.id}`"
          :label="`statistic.options.${defensiveStatistic}`"
          :minValue="1"
          :maxValue="999"
          required
          :step="1"
          type="number"
          :value="defensiveStatisticValue"
          @input="updateBattleTargetDefense({ id: pokemon.id, value: Number($event) })"
        />
      </td>
      <td>
        <form-select
          :disabled="!target"
          hideLabel
          :id="`effectiveness_${pokemon.id}`"
          label="battle.makeMove.effectiveness.label"
          :options="effectivenessOptions"
          required
          :value="effectiveness"
          @input="updateBattleTargetEffectiveness({ id: pokemon.id, value: Number($event) })"
        />
      </td>
      <td>
        <form-field
          :disabled="!target"
          hideLabel
          :id="`otherModifiers_${pokemon.id}`"
          label="battle.makeMove.otherModifiers"
          :minValue="0"
          :maxValue="999"
          required
          :step="0.001"
          type="number"
          :value="otherModifiers"
          @input="updateBattleTargetOtherModifiers({ id: pokemon.id, value: Number($event) })"
        >
          <template #prepend>
            <b-input-group-prepend is-text>&times;</b-input-group-prepend>
          </template>
        </form-field>
      </td>
      <td v-text="target ? accuracy : '—'" />
      <td v-text="target ? damage : '—'" />
    </template>
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'
import { getAccuracyEvasionModifier } from '@/helpers/statisticUtils'

export default {
  name: 'SelectTargetRow',
  components: {
    AbilityInfo,
    ItemInfo,
    PokemonCondition
  },
  props: {
    pokemon: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      defense: 10,
      specialDefense: 10
    }
  },
  computed: {
    ...mapGetters(['battleMoveAttacker', 'battleMoveDamage', 'battleMoveTargets', 'battleStatus', 'selectedBattleMove']),
    accuracy() {
      let { accuracy } = this.selectedBattleMove
      if (accuracy === null) {
        return '—'
      }
      const accuracyStage = this.battleStatus[this.battleMoveAttacker.id]?.accuracy ?? 0
      const evasionStage = this.battleStatus[this.pokemon.id]?.evasion ?? 0
      const stage = Math.min(Math.max(accuracyStage - evasionStage, -6), 6)
      accuracy = Math.min(Math.max(accuracy * getAccuracyEvasionModifier(stage), 1), 100)
      return this.$i18n.n(accuracy / 100, 'percent')
    },
    attacker() {
      return this.pokemon.id === this.battleMoveAttacker.id
    },
    classes() {
      if (this.attacker) {
        return this.target ? ['table-warning'] : ['table-success']
      } else if (this.target) {
        return ['table-info']
      }
      return []
    },
    damage() {
      const { level } = this.battleMoveAttacker
      const { attack, burn, critical, power, random, stab } = this.battleMoveDamage
      const { effectiveness, otherModifiers } = this.target
      return effectiveness === 0
        ? 0
        : Math.max(
            1,
            Math.floor(
              ((((2 * level) / 5 + 2) * power * (attack / this.defensiveStatisticValue)) / 50 + 2) *
                this.targetsModifier *
                this.weatherModifier *
                (critical ? 1.5 : 1) *
                (random / 100) *
                stab *
                effectiveness *
                (burn ? 0.5 : 1) *
                otherModifiers
            )
          )
    },
    defensiveStatistic() {
      switch (this.selectedBattleMove.category) {
        case 'Physical':
          return 'Defense'
        case 'Special':
          return 'SpecialDefense'
        default:
          return null
      }
    },
    defensiveStatisticValue() {
      switch (this.defensiveStatistic) {
        case 'Defense':
          return this.target?.defense ?? this.pokemon.defense
        case 'SpecialDefense':
          return this.target?.specialDefense ?? this.pokemon.specialDefense
        default:
          return null
      }
    },
    effectiveness() {
      return this.target?.effectiveness ?? 1
    },
    effectivenessOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('battle.makeMove.effectiveness.options')).map(([value, text]) => ({ text, value })),
        'value'
      )
    },
    otherModifiers() {
      return this.target?.otherModifiers ?? 1
    },
    target() {
      return this.battleMoveTargets[this.pokemon.id] ?? null
    },
    targetsModifier() {
      return Object.keys(this.battleMoveTargets).length > 1 ? 0.75 : 1
    },
    trainer() {
      return this.pokemon.history?.trainer ?? null
    },
    weatherModifier() {
      const { type } = this.selectedBattleMove
      const { weather } = this.battleMoveDamage
      switch (weather) {
        case 'HarshSunlight':
          if (type === 'Fire') return 1.5
          else if (type === 'Water') return 0.5
          break
        case 'Rain':
          if (type === 'Water') return 1.5
          else if (type === 'Fire') return 0.5
          break
      }
      return 1
    }
  },
  methods: {
    ...mapActions(['toggleBattleMoveTarget', 'updateBattleTargetDefense', 'updateBattleTargetEffectiveness', 'updateBattleTargetOtherModifiers'])
  }
}
</script>
