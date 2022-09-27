<template>
  <div>
    <strong v-t="'species.baseStatistics'" />
    <b-row>
      <form-field
        class="col"
        id="baseHP"
        label="statistic.options.HP"
        :minValue="0"
        :step="1"
        type="number"
        :value="value.HP"
        @input="onInput('HP', Number($event))"
      />
      <form-field
        class="col"
        id="baseAttack"
        label="statistic.options.Attack"
        :minValue="0"
        :step="1"
        type="number"
        :value="value.Attack"
        @input="onInput('Attack', Number($event))"
      />
      <form-field
        class="col"
        id="baseDefense"
        label="statistic.options.Defense"
        :minValue="0"
        :step="1"
        type="number"
        :value="value.Defense"
        @input="onInput('Defense', Number($event))"
      />
      <form-field
        class="col"
        id="baseSpecialAttack"
        label="statistic.options.SpecialAttack"
        :minValue="0"
        :step="1"
        type="number"
        :value="value.SpecialAttack"
        @input="onInput('SpecialAttack', Number($event))"
      />
      <form-field
        class="col"
        id="baseSpecialDefense"
        label="statistic.options.SpecialDefense"
        :minValue="0"
        :step="1"
        type="number"
        :value="value.SpecialDefense"
        @input="onInput('SpecialDefense', Number($event))"
      />
      <form-field
        class="col"
        id="baseSpeed"
        label="statistic.options.Speed"
        :minValue="0"
        :step="1"
        type="number"
        :value="value.Speed"
        @input="onInput('Speed', Number($event))"
      />
    </b-row>
    <p>{{ $t('species.totalFormat', { total }) }}</p>
  </div>
</template>

<script>
export default {
  name: 'BaseStatistics',
  props: {
    value: {
      type: Object,
      required: true
    }
  },
  computed: {
    total() {
      return Object.values(this.value).reduce((a, b) => a + b, 0) || 0
    }
  },
  methods: {
    onInput(statistic, value) {
      const baseStatistics = { ...this.value }
      baseStatistics[statistic] = value
      this.$emit('input', baseStatistics)
    }
  }
}
</script>
