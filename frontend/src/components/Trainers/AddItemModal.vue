<template>
  <b-modal :id="id" :title="$t('trainers.inventory.add')" @hidden="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <category-select v-model="category" />
        <item-select :category="category" required :value="item ? item.id : null" @item="item = $event" />
        <form-field
          id="quantity"
          label="trainers.inventory.quantity"
          :minValue="1"
          :maxValue="999"
          required
          :step="1"
          type="number"
          v-model.number="quantity"
        />
        <b-form-checkbox v-model="buy">{{ $t('trainers.inventory.buy') }}</b-form-checkbox>
        <p v-if="itemPriceRequired" class="text-danger" v-t="'trainers.inventory.itemPriceRequired'" />
        <p v-else-if="insufficientMoney" class="text-danger" v-t="'trainers.inventory.insufficientMoney'" />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button
        :disabled="loading || itemPriceRequired || insufficientMoney"
        :icon="buy ? 'dollar-sign' : 'plus'"
        :loading="loading"
        :text="`trainers.inventory.${buy ? 'buyItem' : 'add'}`"
        :variant="buy ? 'warning' : 'success'"
        @click="submit(ok)"
      />
    </template>
  </b-modal>
</template>

<script>
import CategorySelect from '@/components/Items/CategorySelect.vue'
import ItemSelect from '@/components/Items/ItemSelect.vue'
import { addInventory, buyInventory } from '@/api/inventory'

export default {
  name: 'AddItemModal',
  components: {
    CategorySelect,
    ItemSelect
  },
  props: {
    id: {
      type: String,
      required: true
    },
    trainer: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      buy: false,
      category: null,
      item: null,
      loading: false,
      quantity: 1
    }
  },
  computed: {
    insufficientMoney() {
      return this.buy && this.trainer.money < this.item.price * this.quantity
    },
    itemPriceRequired() {
      return this.buy && typeof this.item?.price !== 'number'
    }
  },
  methods: {
    reset() {
      this.category = null
      this.item = null
      this.quantity = 1
      this.buy = false
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.buy) {
              const { data } = await buyInventory(this.trainer.id, this.item.id, this.quantity)
              this.$emit('bought', { item: data.item, quantity: this.quantity })
            } else {
              const { data } = await addInventory(this.trainer.id, this.item.id, this.quantity)
              this.$emit('added', { item: data.item, quantity: this.quantity })
            }
            this.$refs.form.reset()
            if (typeof callback === 'function') {
              callback()
            }
          }
        } catch (e) {
          this.handleError(e)
          // TODO(fpion): handle InsufficientMoney code (400)
          // TODO(fpion): handle ItemPriceRequired code (400)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    category() {
      this.item = null
    }
  }
}
</script>
