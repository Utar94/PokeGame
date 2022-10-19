<template>
  <icon-button
    :icon="isVictory ? 'crown' : 'skull'"
    :text="`battle.combatTracker.${isVictory ? 'victory' : 'defeat'}`"
    variant="primary"
    @click="onBattleEnded"
  />
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'EndBattle',
  computed: {
    ...mapGetters(['remainingBattlingOpponentPokemon']),
    isVictory() {
      return this.remainingBattlingOpponentPokemon.length === 0
    }
  },
  methods: {
    ...mapActions(['endBattle']),
    async onBattleEnded() {
      try {
        await this.endBattle()
      } catch (e) {
        this.handleError(e)
      }
    }
  }
}
</script>
