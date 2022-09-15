<template>
  <div>
    <h3 v-t="'battle.makeMove.conditionAndStatChanges'" />
    <b-row>
      <condition-select class="col" v-model="condition" />
      <form-field class="col" id="speed" label="statistic.options.Speed" :minValue="-6" :maxValue="6" required :step="1" type="number" v-model="speed" />
      <form-field class="col" id="accuracy" label="battle.makeMove.accuracy" :minValue="-6" :maxValue="6" required :step="1" type="number" v-model="accuracy" />
      <form-field class="col" id="evasion" label="battle.makeMove.evasion" :minValue="-6" :maxValue="6" required :step="1" type="number" v-model="evasion" />
    </b-row>
    <b-row>
      <form-field class="col" id="attack" label="statistic.options.Attack" :minValue="-6" :maxValue="6" required :step="1" type="number" v-model="attack" />
      <form-field
        class="col"
        id="specialAttack"
        label="statistic.options.SpecialAttack"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        v-model="specialAttack"
      />
      <form-field class="col" id="defense" label="statistic.options.Defense" :minValue="-6" :maxValue="6" required :step="1" type="number" v-model="defense" />
      <form-field
        class="col"
        id="specialDefense"
        label="statistic.options.SpecialDefense"
        :minValue="-6"
        :maxValue="6"
        required
        :step="1"
        type="number"
        v-model="specialDefense"
      />
    </b-row>
  </div>
</template>

<script>
import ConditionSelect from '@/components/Pokemon/ConditionSelect.vue'

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
  }
}
</script>
