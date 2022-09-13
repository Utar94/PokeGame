<template>
  <div>
    <b-row>
      <form-field class="col" id="power" label="moves.power" :minValue="0" :maxValue="255" :step="1" type="number" v-model.number="power" />
      <form-field
        class="col"
        id="offensiveStatistic"
        :label="move.move.category === 'Physical' ? 'statistic.options.Attack' : 'statistic.options.SpecialAttack'"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        v-model.number="offensiveStatistic"
      />
    </b-row>
    <b-row>
      <form-field class="col" id="random" label="battle.makeMove.random" :minValue="85" :maxValue="100" :step="1" type="number" v-model.number="random">
        <b-input-group-append>
          <icon-button icon="dice" variant="primary" @click="randomize" />
        </b-input-group-append>
      </form-field>
      <form-select class="col" id="stab" label="battle.makeMove.stab.label" :options="stabOptions" v-model="stab" />
    </b-row>
    <b-form-group>
      <b-form-checkbox v-model="burn">{{ $t('battle.makeMove.burn') }}</b-form-checkbox>
      <b-form-checkbox v-model="critical">{{ $t('battle.makeMove.critical') }}</b-form-checkbox>
    </b-form-group>
  </div>
</template>

<script>
export default {
  name: 'MakeMoveDamage',
  props: {
    move: {
      type: Object,
      required: true
    },
    value: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      burn: false,
      critical: false,
      offensiveStatistic: 0,
      power: 0,
      random: 0,
      stab: 0
    }
  },
  computed: {
    stabOptions() {
      return [
        { value: 1.0, text: this.$t('battle.makeMove.stab.no') },
        { value: 1.5, text: this.$t('battle.makeMove.stab.yes') },
        { value: 2.0, text: this.$t('battle.makeMove.stab.adaptability') }
      ]
    }
  },
  methods: {
    randomize() {
      this.random = 82 + this.roll('3d6')
    }
  },
  watch: {
    value: {
      deep: true,
      immediate: true,
      handler({ burn, critical, offensiveStatistic, power, random, stab }) {
        this.burn = burn
        this.critical = critical
        this.offensiveStatistic = offensiveStatistic
        this.power = power
        this.random = random
        this.stab = stab
      }
    }
  }
}
</script>
