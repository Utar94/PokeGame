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
    ...mapGetters(['battleEscapeAttempts']),
    escapeDC() {
      // const activePlayerPokemon = this.playerPokemon.filter(x => x && this.battle.activePokemon.includes(x.id))
      // const playerSpeed = activePlayerPokemon.reduce((sum, { speed }) => sum + speed, 0) / activePlayerPokemon.length
      // const activeOpponentPokemon = this.opponentPokemon.filter(x => x && this.battle.activePokemon.includes(x.id))
      // const opponentSpeed = activeOpponentPokemon.reduce((sum, { speed }) => sum + speed, 0) / activeOpponentPokemon.length
      // if (!playerSpeed || !opponentSpeed || playerSpeed >= opponentSpeed) {
      //   return 5
      // }
      // return Math.ceil((1 - ((Math.floor((playerSpeed * 128) / opponentSpeed) + 30 * this.battle.escapeAttempts) % 256) / 256) * 20) + 5
      return 0 // TODO(fpion): implement
    }
  },
  methods: {
    ...mapActions(['increaseEscapeAttempts'])
  }
}
</script>
