<template>
  <div>
    <strong v-t="'species.evYield.label'" />
    <template v-if="exceeded">
      <br />
      <span class="text-danger" v-t="'species.evYield.exceeded'" />
    </template>
    <b-row>
      <form-field
        class="col"
        id="hpEvYield"
        label="statistic.options.HP"
        :minValue="0"
        :maxValue="3"
        :step="1"
        type="number"
        :value="value.HP"
        @input="onInput('HP', Number($event))"
      />
      <form-field
        class="col"
        id="attackEvYield"
        label="statistic.options.Attack"
        :minValue="0"
        :maxValue="3"
        :step="1"
        type="number"
        :value="value.Attack"
        @input="onInput('Attack', Number($event))"
      />
      <form-field
        class="col"
        id="defenseEvYield"
        label="statistic.options.Defense"
        :minValue="0"
        :maxValue="3"
        :step="1"
        type="number"
        :value="value.Defense"
        @input="onInput('Defense', Number($event))"
      />
      <form-field
        class="col"
        id="specialAttackEvYield"
        label="statistic.options.SpecialAttack"
        :minValue="0"
        :maxValue="3"
        :step="1"
        type="number"
        :value="value.SpecialAttack"
        @input="onInput('SpecialAttack', Number($event))"
      />
      <form-field
        class="col"
        id="specialDefenseEvYield"
        label="statistic.options.SpecialDefense"
        :minValue="0"
        :maxValue="3"
        :step="1"
        type="number"
        :value="value.SpecialDefense"
        @input="onInput('SpecialDefense', Number($event))"
      />
      <form-field
        class="col"
        id="speedEvYield"
        label="statistic.options.Speed"
        :minValue="0"
        :maxValue="3"
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
  name: 'EvYield',
  props: {
    exceeded: {
      type: Boolean,
      default: false
    },
    total: {
      type: Number,
      default: 0
    },
    value: {
      type: Object,
      required: true
    }
  },
  methods: {
    onInput(statistic, value) {
      const evYield = { ...this.value }
      evYield[statistic] = value
      this.$emit('input', evYield)
    }
  }
}
</script>
