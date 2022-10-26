<template>
  <b-modal :id="id" :title="$t('pokemon.heldItem.title')" @show="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <p v-if="heldItem">{{ $t('pokemon.heldItem.current') }} <strong v-text="heldItem.name" /></p>
        <form-select id="heldItem" label="pokemon.heldItem.label" :options="options" placeholder="pokemon.heldItem.placeholder" v-model="itemId" />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import { getInventory } from '@/api/inventory'
import { holdItem } from '@/api/pokemon'

export default {
  name: 'HeldItemModal',
  props: {
    id: {
      type: String,
      default: 'heldItem'
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      inventory: [],
      itemId: null,
      loading: false
    }
  },
  computed: {
    hasChanges() {
      return this.heldItem?.id !== this.itemId
    },
    heldItem() {
      return this.pokemon.heldItem
    },
    options() {
      return this.orderBy(
        this.inventory.map(({ item, quantity }) => ({
          text: `${item.name} [${this.$i18n.t(`items.category.options.${item.category}`)}] (${this.$i18n.t('battle.quantityFormat', { quantity })})`,
          value: item.id
        })),
        'text'
      )
    }
  },
  methods: {
    async refresh(trainerId = null) {
      try {
        const { data } = await getInventory(trainerId ?? this.pokemon.history.trainer.id)
        this.inventory = data.items
        this.reset()
        if (this.heldItem) {
          const item = this.inventory.find(({ item }) => item.id === this.heldItem.id)
          if (!item) {
            this.inventory.push({ item: this.heldItem, quantity: 0 })
          }
        }
      } catch (e) {
        this.handleError(e)
      }
    },
    reset() {
      this.itemId = this.heldItem?.id ?? null
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await holdItem(this.pokemon.id, this.itemId)
            await this.refresh()
            this.$emit('saved', data)
            this.toast('success', 'pokemon.heldItem.saved')
            this.$refs.form.reset()
            if (typeof callback === 'function') {
              callback()
            }
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    pokemon: {
      deep: true,
      immediate: true,
      async handler(pokemon) {
        await this.refresh(pokemon.history.trainer.id)
      }
    }
  }
}
</script>
