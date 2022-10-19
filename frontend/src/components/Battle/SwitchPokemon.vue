<template>
  <icon-button
    class="mx-1"
    :disabled="hasEnded"
    icon="exchange-alt"
    :text="active ? 'battle.combatTracker.withdraw' : 'battle.combatTracker.send'"
    variant="success"
    @click="toggleActiveBattlingPokemon(pokemon)"
  />
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'SwitchPokemon',
  props: {
    active: {
      type: Boolean,
      required: true
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters(['hasEnded'])
  },
  methods: {
    ...mapActions(['toggleActiveBattlingPokemon'])
  }
}
</script>
