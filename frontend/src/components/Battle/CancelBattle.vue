<template>
  <span>
    <icon-button icon="trash-alt" text="battle.cancel.label" variant="danger" v-b-modal.cancelBattle />
    <b-modal id="cancelBattle" :title="$t('battle.cancel.label')">
      <p>
        {{ $t('battle.cancel.confirm') }}
        <br />
        <span class="text-danger" v-t="'battle.changesLost'" />
      </p>
      <template #modal-footer="{ cancel, ok }">
        <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
        <icon-button icon="trash-alt" text="battle.cancel.label" variant="danger" @click="onOk(ok)" />
      </template>
    </b-modal>
  </span>
</template>

<script>
import { mapActions } from 'vuex'

export default {
  name: 'CancelBattle',
  methods: {
    ...mapActions(['resetBattle']),
    onOk(callback = null) {
      this.resetBattle()
      if (typeof callback === 'function') {
        callback()
      }
    }
  }
}
</script>
