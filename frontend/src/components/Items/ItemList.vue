<template>
  <b-container>
    <h1 v-t="'items.title'" />
    <div class="my-2">
      <icon-button class="mx-1" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" href="/create-item" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <category-select class="col" v-model="category" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="items.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" />
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'items.category.label'" />
            <th scope="col" v-t="'items.price'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in items" :key="item.id">
            <td>
              <b-link :href="`/items/${item.id}`"><item-icon :item="item" /></b-link>
            </td>
            <td>
              <b-link :href="`/items/${item.id}`">{{ item.name }}</b-link>
            </td>
            <td>{{ $t(`items.category.options.${item.category}`) }}</td>
            <td v-text="item.price || '—'" />
            <td><status-cell :actor="item.updatedBy || item.createdBy" :date="item.updatedOn || item.createdOn" /></td>
            <td>
              <icon-button icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${item.id}`" />
              <delete-modal
                confirm="items.delete.confirm"
                :displayName="item.name"
                :id="`delete_${item.id}`"
                :loading="loading"
                title="items.delete.title"
                @ok="onDelete(item, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'items.empty'" />
  </b-container>
</template>

<script>
import CategorySelect from './CategorySelect.vue'
import ItemIcon from './ItemIcon.vue'
import { deleteItem, getItems } from '@/api/items'

export default {
  name: 'ItemList',
  components: {
    CategorySelect,
    ItemIcon
  },
  data() {
    return {
      category: null,
      count: 10,
      desc: false,
      loading: false,
      items: [],
      page: 1,
      search: null,
      sort: 'Name',
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
        Object.entries(this.$i18n.t('items.sort.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  },
  methods: {
    async onDelete({ id }, callback = null) {
      if (!this.loading) {
        this.loading = true
        let refresh = false
        try {
          await deleteItem(id)
          refresh = true
          this.toast('success', 'items.delete.success')
          if (typeof callback === 'function') {
            callback()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
        if (refresh) {
          await this.refresh()
        }
      }
      if (callback) {
        callback()
      }
    },
    async refresh(params = null) {
      if (!this.loading) {
        this.loading = true
        try {
          const { data } = await getItems(params ?? this.params)
          this.items = data.items
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
        if (
          newValue?.index &&
          oldValue &&
          (newValue.search !== oldValue.search || newValue.category !== oldValue.category || newValue.count !== oldValue.count)
        ) {
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
