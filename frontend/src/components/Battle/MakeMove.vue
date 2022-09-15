<template>
  <b-container fluid>
    <h1 v-t="'battle.makeMove.label'" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <select-move />
        <template v-if="selectedBattleMove">
          <select-targets />
          <move-condition />
          <move-damage v-if="dealsDamage" />
        </template>
        <div class="my-2">
          <icon-button class="mx-1" icon="ban" text="actions.cancel" @click="resetBattleMove" />
          <!-- <icon-submit :disabled="loading" icon="magic" :loading="loading" text="battle.makeMove.label" variant="danger"  /> -->
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import MoveCondition from './MoveCondition.vue'
import MoveDamage from './MoveDamage.vue'
import SelectMove from './SelectMove.vue'
import SelectTargets from './SelectTargets.vue'

export default {
  name: 'MakeMove',
  components: {
    MoveCondition,
    MoveDamage,
    SelectMove,
    SelectTargets
  },
  data() {
    return {
      loading: false
    }
  },
  computed: {
    ...mapGetters(['selectedBattleMove']),
    dealsDamage() {
      return this.selectedBattleMove.category === 'Physical' || this.selectedBattleMove.category === 'Special'
    }
  },
  methods: {
    ...mapActions(['resetBattleMove']),
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            // TODO(fpion): implement
          }
          this.$refs.form.reset()
          if (typeof callback === 'function') {
            callback()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  }
}
</script>
