<template>
  <b-modal id="makeMove" size="xl" :title="$t('battle.makeMove.label')">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <h6 v-t="'moves.title'" />
        <select-move-table />
        <h6 v-t="'battle.makeMove.targets'" />
        <select-target-table />
        <template v-if="doesDamage">
          <h6 v-t="'battle.makeMove.damage'" />
          <move-damage />
        </template>
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="reset(cancel)" />
      <icon-button :disabled="loading" icon="magic" :loading="loading" text="battle.makeMove.label" variant="danger" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import MoveDamage from './MoveDamage.vue'
import SelectMoveTable from './SelectMoveTable.vue'
import SelectTargetTable from './SelectTargetTable.vue'

export default {
  name: 'MakeMoveModal',
  components: {
    MoveDamage,
    SelectMoveTable,
    SelectTargetTable
  },
  data() {
    return {
      loading: false
    }
  },
  computed: {
    ...mapGetters(['selectedBattleMove']),
    doesDamage() {
      return Boolean(this.selectedBattleMove?.power)
    }
  },
  methods: {
    ...mapActions(['resetBattleMove']),
    reset(callback = null) {
      this.resetBattleMove()
      if (typeof callback === 'function') {
        callback()
      }
    },
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
