<template>
  <b-tab :title="$t('trainers.inventory.label')">
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" icon="plus" text="trainers.inventory.add" variant="success" v-b-modal.addItem />
      <add-item-modal id="addItem" :trainer="trainer" @added="onAdded" @bought="onBought" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <category-select class="col" v-model="category" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="lines.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" />
            <th scope="col" v-t="'trainers.inventory.item'" />
            <th scope="col" v-t="'trainers.inventory.category'" />
            <th scope="col" v-t="'trainers.inventory.price'" />
            <th scope="col" v-t="'trainers.inventory.quantity'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="(line, index) in lines" :key="line.item.id">
            <td>
              <b-link :href="`/items/${line.item.id}`" target="_blank"><item-icon :item="line.item" /></b-link>
            </td>
            <td>
              <b-link :href="`/items/${line.item.id}`" target="_blank">{{ line.item.name }} <font-awesome-icon icon="external-link-alt" /></b-link>
            </td>
            <td>{{ $t(`items.category.options.${line.item.category}`) }}</td>
            <td v-text="line.item.price || '—'" />
            <td>
              <icon-button :disabled="loading" icon="minus" :loading="loading" size="sm" variant="danger" @click="decreaseQuantity(index)" />
              {{ line.quantity }}
              <icon-button :disabled="loading" icon="plus" :loading="loading" size="sm" variant="success" @click="increaseQuantity(index)" />
            </td>
            <td>
              <icon-button icon="trash-alt" text="trainers.inventory.remove" variant="danger" v-b-modal="`remove_${index}`" />
              <remove-item-modal
                :id="`remove_${index}`"
                :item="line.item"
                :maxQuantity="line.quantity"
                :trainer="trainer"
                @removed="onRemoved"
                @sold="onSold"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'trainers.inventory.empty'" />
  </b-tab>
</template>

<script>
import Vue from 'vue'
import AddItemModal from './AddItemModal.vue'
import CategorySelect from '@/components/Items/CategorySelect.vue'
import ItemIcon from '@/components/Items/ItemIcon.vue'
import RemoveItemModal from './RemoveItemModal.vue'
import { addInventory, getInventory, removeInventory } from '@/api/inventory'

export default {
  name: 'InventoryTab',
  components: {
    AddItemModal,
    CategorySelect,
    ItemIcon,
    RemoveItemModal
  },
  props: {
    trainer: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      category: null,
      count: 100,
      desc: false,
      lines: [],
      loading: false,
      page: 1,
      search: null,
      sort: 'ItemName',
      total: 0
    }
  },
  computed: {
    params() {
      return {
        category: this.category,
        search: this.search,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('trainers.inventory.sort.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  },
  methods: {
    async decreaseQuantity(index) {
      const line = this.lines[index]
      if (line && !this.loading) {
        this.loading = true
        try {
          const { data } = await removeInventory(this.trainer.id, line.item.id, 1)
          if (data.quantity > 0) {
            Vue.set(this.lines, index, data)
          } else {
            Vue.delete(this.lines, index)
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    },
    async increaseQuantity(index) {
      const line = this.lines[index]
      if (line && !this.loading) {
        this.loading = true
        try {
          const { data } = await addInventory(this.trainer.id, line.item.id, 1)
          Vue.set(this.lines, index, data)
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    },
    onAdded() {
      this.toast('success', 'trainers.inventory.added')
      this.refresh()
      this.$emit('updated')
    },
    onBought() {
      this.toast('success', 'trainers.inventory.bought')
      this.refresh()
      this.$emit('updated')
    },
    onRemoved() {
      this.toast('success', 'trainers.inventory.removed')
      this.refresh()
      this.$emit('updated')
    },
    onSold() {
      this.toast('success', 'trainers.inventory.sold')
      this.refresh()
      this.$emit('updated')
    },
    async refresh(params = null) {
      if (!this.loading) {
        this.loading = true
        try {
          const { data } = await getInventory(this.trainer.id, params ?? this.params)
          this.lines = data.items
          this.total = data.total
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    params: {
      deep: true,
      immediate: true,
      async handler(newValue, oldValue) {
        if (newValue?.index && oldValue && (newValue.search !== oldValue.search || newValue.category !== oldValue.category)) {
          this.page = 1
          await this.refresh()
        } else {
          await this.refresh(newValue)
        }
      }
    }
  }
}
</script>
