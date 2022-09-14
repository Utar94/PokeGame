<template>
  <div>
    <b-row>
      <form-field
        class="col"
        id="power"
        label="moves.power"
        :minValue="0"
        :maxValue="255"
        :step="1"
        type="number"
        :value="battleMoveDamage.power"
        @input="updateBattleMoveDamage({ power: Number($event) })"
      />
      <form-field
        class="col"
        id="attack"
        label="statistic.options.Attack"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="battleMoveDamage.attack"
        @input="updateBattleMoveDamage({ attack: Number($event) })"
      />
    </b-row>
    <b-row>
      <form-field
        class="col"
        id="random"
        label="battle.makeMove.random"
        :minValue="85"
        :maxValue="100"
        :step="1"
        type="number"
        :value="battleMoveDamage.random"
        @input="updateBattleMoveDamage({ random: Number($event) })"
      >
        <b-input-group-append>
          <icon-button icon="dice" variant="primary" @click="randomize" />
        </b-input-group-append>
      </form-field>
      <form-select
        class="col"
        id="stab"
        label="battle.makeMove.stab.label"
        :options="stabOptions"
        required
        :value="battleMoveDamage.stab"
        @input="updateBattleMoveDamage({ stab: Number($event) })"
      />
    </b-row>
    <b-form-checkbox :checked="battleMoveDamage.critical" @change="updateBattleMoveDamage({ critical: $event })">{{
      $t('battle.makeMove.critical')
    }}</b-form-checkbox>
    <b-form-checkbox :checked="battleMoveDamage.burn" @change="updateBattleMoveDamage({ burn: $event })">{{ $t('battle.makeMove.burn') }}</b-form-checkbox>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'MoveDamage',
  computed: {
    ...mapGetters(['battleMoveDamage']),
    stabOptions() {
      return [
        { text: this.$i18n.t('battle.makeMove.stab.no'), value: 1 },
        { text: this.$i18n.t('battle.makeMove.stab.yes'), value: 1.5 },
        { text: this.$i18n.t('battle.makeMove.stab.adaptability'), value: 2 }
      ]
    }
  },
  methods: {
    ...mapActions(['updateBattleMoveDamage']),
    randomize() {
      this.updateBattleMoveDamage({ random: 82 + this.roll('3d6') })
    }
  }
}
</script>
