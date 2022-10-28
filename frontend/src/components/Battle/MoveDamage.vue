<template>
  <div>
    <h3 v-t="'battle.makeMove.damage'" />
    <b-row>
      <form-field
        class="col"
        id="power"
        label="moves.power"
        :minValue="0"
        :maxValue="255"
        required
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
        required
        :step="1"
        type="number"
        :value="battleMoveDamage.attack"
        @input="updateBattleMoveDamage({ attack: Number($event) })"
      />
      <form-field
        class="col"
        id="random"
        label="battle.makeMove.random"
        :minValue="85"
        :maxValue="100"
        required
        :step="1"
        type="number"
        :value="battleMoveDamage.random"
        @input="updateBattleMoveDamage({ random: Number($event) })"
      >
        <b-input-group-append>
          <icon-button icon="dice" variant="primary" @click="randomize" />
        </b-input-group-append>
      </form-field>
    </b-row>
    <b-row>
      <form-select
        class="col"
        id="stab"
        label="battle.makeMove.stab.label"
        :options="stabOptions"
        required
        :value="battleMoveDamage.stab"
        @input="updateBattleMoveDamage({ stab: Number($event) })"
      />
      <form-select
        class="col"
        :disabled="isWeatherDisabled"
        id="weather"
        label="battle.makeMove.weather.label"
        :options="weatherOptions"
        required
        :value="battleMoveDamage.weather"
        @input="updateBattleMoveDamage({ weather: $event })"
      >
        <template #after v-if="isWeatherDisabled">
          <i class="text-warning" v-t="'battle.makeMove.weather.disabledWarning'" />
        </template>
      </form-select>
      <b-form-group class="col" :label="$t('battle.makeMove.options')">
        <b-form-checkbox :checked="battleMoveDamage.critical" @change="updateBattleMoveDamage({ critical: $event })">
          {{ $t('battle.makeMove.critical') }}
        </b-form-checkbox>
        <b-form-checkbox :checked="battleMoveDamage.burn" @change="updateBattleMoveDamage({ burn: $event })">{{ $t('battle.makeMove.burn') }}</b-form-checkbox>
      </b-form-group>
    </b-row>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'MoveDamage',
  computed: {
    ...mapGetters(['activeBattlingPokemon', 'battleMoveDamage']),
    isWeatherDisabled() {
      for (const { ability } of this.activeBattlingPokemon) {
        if (ability.kind === 'AirLock' || ability.kind === 'CloudNine') {
          return true
        }
      }
      return false
    },
    stabOptions() {
      return [
        { text: this.$i18n.t('battle.makeMove.stab.no'), value: 1 },
        { text: this.$i18n.t('battle.makeMove.stab.yes'), value: 1.5 },
        { text: this.$i18n.t('battle.makeMove.stab.adaptability'), value: 2 }
      ]
    },
    weatherOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('battle.makeMove.weather.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
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
