<template>
  <form-field
    class="col-3"
    id="location"
    label="battle.combatTracker.location.label"
    :maxLength="100"
    placeholder="battle.combatTracker.location.placeholder"
    :value="battleLocation"
    @input="saveBattleLocation"
  >
    <template #after>
      <i v-if="!battleLocation" class="text-warning" v-t="'battle.locationWarning'" />
    </template>
  </form-field>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'LocationField',
  computed: {
    ...mapGetters(['battleLocation'])
  },
  methods: {
    ...mapActions(['saveBattleLocation'])
  }
}
</script>
