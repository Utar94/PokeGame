<template>
  <icon-button
    :icon="isVictory ? 'crown' : 'skull'"
    :text="`battle.combatTracker.${isVictory ? 'victory' : 'defeat'}`"
    variant="primary"
    @click="resetBattle"
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
    ...mapActions(['resetBattle'])
  }
}
</script>
