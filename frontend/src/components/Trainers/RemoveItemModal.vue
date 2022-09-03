<template>
  <b-modal :id="id" :title="$t('trainers.inventory.remove')" @hidden="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <category-select disabled :value="item.category" />
        <item-select disabled :value="item.id" />
        <form-field
          id="quantity"
          label="trainers.inventory.quantity"
          :minValue="1"
          :maxValue="maxQuantity"
          required
          :step="1"
          type="number"
          v-model.number="quantity"
        />
        <b-form-checkbox v-model="sell">{{ $t('trainers.inventory.sell') }}</b-form-checkbox>
        <p v-if="itemPriceRequired" class="text-danger" v-t="'trainers.inventory.itemPriceRequired'" />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button
        :disabled="loading || itemPriceRequired"
        :icon="sell ? 'dollar-sign' : 'trash-alt'"
        :loading="loading"
        :text="`trainers.inventory.${sell ? 'sellItem' : 'remove'}`"
        :variant="sell ? 'warning' : 'danger'"
        @click="submit(ok)"
      />
    </template>
  </b-modal>
</template>

<script>
import CategorySelect from '@/components/Items/CategorySelect.vue'
import ItemSelect from '@/components/Items/ItemSelect.vue'
import { removeInventory, sellInventory } from '@/api/inventory'

export default {
  name: 'RemoveItemModal',
  components: {
    CategorySelect,
    ItemSelect
  },
  props: {
    id: {
      type: String,
      required: true
    },
    item: {
      type: Object,
      required: true
    },
    maxQuantity: {
      type: Number,
      required: true
    },
    trainer: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      loading: false,
      quantity: 1,
      sell: false
    }
  },
  computed: {
    itemPriceRequired() {
      return this.sell && typeof this.item?.price !== 'number'
    }
  },
  methods: {
    reset() {
      this.quantity = 1
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.sell) {
              const { data } = await sellInventory(this.trainer.id, this.item.id, this.quantity)
              this.$emit('sold', { item: data.item, quantity: this.quantity })
            } else {
              const { data } = await removeInventory(this.trainer.id, this.item.id, this.quantity)
              this.$emit('removed', { item: data.item, quantity: this.quantity })
            }
            this.$refs.form.reset()
            if (typeof callback === 'function') {
              callback()
            }
          }
        } catch (e) {
          this.handleError(e)
          // TODO(fpion): handle InsufficientQuantity code (400)
          // TODO(fpion): handle ItemPriceRequired code (400)
        } finally {
          this.loading = false
        }
      }
    }
  }
}
</script>
