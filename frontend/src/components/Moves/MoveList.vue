<template>
  <b-container>
    <h1 v-t="'moves.title'" />
    <div class="my-2">
      <icon-button class="mx-1" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" href="/create-move" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <type-select class="col" v-model="type" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="moves.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'type.label'" />
            <th scope="col" v-t="'moves.category.label'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="move in moves" :key="move.id">
            <td>
              <b-link :href="`/moves/${move.id}`">{{ move.name }}</b-link>
            </td>
            <td>{{ $t(`type.options.${move.type}`) }}</td>
            <td>{{ $t(`moves.category.options.${move.category}`) }}</td>
            <td><status-cell :actor="move.updatedBy" :date="move.updatedAt" /></td>
            <td>
              <icon-button icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${move.id}`" />
              <delete-modal
                confirm="moves.delete.confirm"
                :displayName="move.name"
                :id="`delete_${move.id}`"
                :loading="loading"
                title="moves.delete.title"
                @ok="onDelete(move, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'moves.empty'" />
  </b-container>
</template>

<script>
import { deleteMove, getMoves } from '@/api/moves'

export default {
  name: 'MoveList',
  data() {
    return {
      count: 10,
      desc: false,
      loading: false,
      moves: [],
      page: 1,
      search: null,
      sort: 'Name',
      total: 0,
      type: null
    }
  },
  computed: {
    params() {
      return {
        search: this.search,
        type: this.type,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('moves.sort.options')).map(([value, text]) => ({ text, value })),
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
          await deleteMove(id)
          refresh = true
          this.toast('success', 'moves.delete.success')
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
          const { data } = await getMoves(params ?? this.params)
          this.moves = data.items
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
        if (newValue?.index && oldValue && (newValue.search !== oldValue.search || newValue.type !== oldValue.type || newValue.count !== oldValue.count)) {
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
