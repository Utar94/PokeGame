<template>
  <span>
    <icon-button icon="running" text="battle.escape.label" variant="primary" v-b-modal.escapeBattle />
    <b-modal id="escapeBattle" :title="$t('battle.escape.label')">
      <p>
        {{ $t('battle.escape.confirm') }}
        <br />
        <span class="text-danger" v-t="'battle.changesLost'" />
      </p>
      <template #modal-footer="{ cancel, ok }">
        <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
        <icon-button icon="running" text="battle.escape.label" variant="primary" @click="onBattleEscaped(ok)" />
      </template>
    </b-modal>
  </span>
</template>

<script>
import { mapActions } from 'vuex'

export default {
  name: 'EscapeBattle',
  methods: {
    ...mapActions(['endBattle']),
    async onBattleEscaped(callback = null) {
      try {
        await this.endBattle()
      } catch (e) {
        this.handleError(e)
      }
      if (typeof callback === 'function') {
        callback()
      }
    }
  }
}
</script>
