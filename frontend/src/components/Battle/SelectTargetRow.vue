<template>
  <tr :class="classes">
    <td>
      <strong v-if="attacker" v-t="'battle.makeMove.attacker'" />
      <b-form-checkbox :checked="Boolean(target)" size="lg" @change="toggleBattleMoveTarget(pokemon)" />
    </td>
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" /> {{ pokemon.species.name }} {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
      <br />
      <template v-if="trainer"><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</template>
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
    <td v-text="accuracy" />
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
    </template>
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'

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
    ...mapGetters(['battleMoveAttacker', 'battleMoveTargets', 'selectedBattleMove']),
    accuracy() {
      return this.selectedBattleMove ? this.$i18n.n(this.selectedBattleMove.accuracy / 100, 'percent') : '—'
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
    trainer() {
      return this.pokemon.history?.trainer ?? null
    }
  },
  methods: {
    ...mapActions(['toggleBattleMoveTarget', 'updateBattleTargetDefense', 'updateBattleTargetEffectiveness', 'updateBattleTargetOtherModifiers'])
  }
}
</script>
