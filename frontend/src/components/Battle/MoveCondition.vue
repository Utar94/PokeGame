<template>
  <div>
    <h3 v-t="'battle.makeMove.conditionAndStatChanges'" />
    <b-row>
      <condition-select class="col" :value="battleMoveCondition.status" @input="updateBattleMoveCondition({ status: $event })">
        <template #prepend v-if="battleMoveCondition.status && selectedBattleMove.statusChance">
          <b-input-group-prepend is-text>{{ $n(selectedBattleMove.statusChance / 100, 'percent') }}</b-input-group-prepend>
        </template>
      </condition-select>
      <form-field
        class="col"
        id="speed"
        label="statistic.options.Speed"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        :value="battleMoveCondition.speed"
        @input="updateBattleMoveCondition({ speed: Number($event) })"
      />
      <form-field
        class="col"
        id="accuracy"
        label="battle.makeMove.accuracy"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        :value="battleMoveCondition.accuracy"
        @input="updateBattleMoveCondition({ accuracy: Number($event) })"
      />
      <form-field
        class="col"
        id="evasion"
        label="battle.makeMove.evasion"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        :value="battleMoveCondition.evasion"
        @input="updateBattleMoveCondition({ evasion: Number($event) })"
      />
    </b-row>
    <b-row>
      <form-field
        class="col"
        id="attack"
        label="statistic.options.Attack"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        :value="battleMoveCondition.attack"
        @input="updateBattleMoveCondition({ attack: Number($event) })"
      />
      <form-field
        class="col"
        id="specialAttack"
        label="statistic.options.SpecialAttack"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        :value="battleMoveCondition.specialAttack"
        @input="updateBattleMoveCondition({ specialAttack: Number($event) })"
      />
      <form-field
        class="col"
        id="defense"
        label="statistic.options.Defense"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        :value="battleMoveCondition.defense"
        @input="updateBattleMoveCondition({ defense: Number($event) })"
      />
      <form-field
        class="col"
        id="specialDefense"
        label="statistic.options.SpecialDefense"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        :value="battleMoveCondition.specialDefense"
        @input="updateBattleMoveCondition({ specialDefense: Number($event) })"
      />
    </b-row>
  </div>
</template>

<script>
import ConditionSelect from '@/components/Pokemon/ConditionSelect.vue'
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'MoveCondition',
  components: {
    ConditionSelect
  },
  data() {
    return {
      accuracy: 0,
      attack: 0,
      condition: null,
      defense: 0,
      evasion: 0,
      specialAttack: 0,
      specialDefense: 0,
      speed: 0
    }
  },
  computed: {
    ...mapGetters(['battleMoveCondition', 'selectedBattleMove'])
  },
  methods: {
    ...mapActions(['updateBattleMoveCondition'])
  }
}
</script>
