<template>
  <span class="mx-1">
    <icon-button icon="trash-alt" text="battle.cancel.label" variant="danger" v-b-modal.resetBattle />
    <b-modal id="resetBattle" :title="$t('battle.cancel.label')">
      <p class="text-danger">
        {{ $t('battle.cancel.confirm1') }}
        <br />
        {{ $t('battle.cancel.confirm2') }}
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
  name: 'DeleteModal',
  props: {
    displayName: {
      type: String,
      default: ''
    }
  },
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
