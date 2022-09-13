<template>
  <b-form-group class="col-3">
    <template #label>
      <strong>{{ $t('battle.escape.label') }} ({{ $t('dcFormat', { dc: escapeDC }) }})</strong>
    </template>
    <icon-button class="mx-1" icon="plus" variant="primary" @click="increaseEscapeAttempts" />
    {{ $t('battle.escape.attemptsFormat', { attempts: battleEscapeAttempts }) }}
  </b-form-group>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'EscapeBattle',
  computed: {
    ...mapGetters(['activeBattlingOpponentPokemon', 'activeBattlingPlayerPokemon', 'battleEscapeAttempts']),
    escapeDC() {
      const playerSpeed = this.activeBattlingPlayerPokemon.reduce((sum, { speed }) => sum + speed, 0) / this.activeBattlingPlayerPokemon.length
      const opponentSpeed = this.activeBattlingOpponentPokemon.reduce((sum, { speed }) => sum + speed, 0) / this.activeBattlingOpponentPokemon.length
      if (playerSpeed >= opponentSpeed) {
        return 5
      }
      return Math.ceil((1 - ((Math.floor((playerSpeed * 128) / opponentSpeed) + 30 * this.battleEscapeAttempts) % 256) / 256) * 20) + 5
    }
  },
  methods: {
    ...mapActions(['increaseEscapeAttempts'])
  }
}
</script>
