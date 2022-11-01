<template>
  <b-container>
    <h1 v-t="'regions.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" href="/create-region" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="regions.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="region in regions" :key="region.id">
            <td>
              <b-link :href="`/regions/${region.id}`">{{ region.name }}</b-link>
            </td>
            <td><status-cell :actor="region.updatedBy || region.createdBy" :date="region.updatedOn || region.createdOn" /></td>
            <td>
              <icon-button disabled icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${region.id}`" />
              <delete-modal
                confirm="regions.delete.confirm"
                :displayName="region.name"
                :id="`delete_${region.id}`"
                :loading="loading"
                title="regions.delete.title"
                @ok="onDelete(region, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'regions.empty'" />
  </b-container>
</template>

<script>
import { deleteRegion, getRegions } from '@/api/regions'

export default {
  name: 'RegionList',
  data() {
    return {
      count: 10,
      desc: false,
      loading: false,
      page: 1,
      regions: [],
      search: null,
      sort: 'Name',
      total: 0
    }
  },
  computed: {
    params() {
      return {
        search: this.search,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('regions.sort.options')).map(([value, text]) => ({ text, value })),
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
          await deleteRegion(id)
          refresh = true
          this.toast('success', 'regions.delete.success')
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
          const { data } = await getRegions(params ?? this.params)
          this.regions = data.items
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
        if (newValue?.index && oldValue && (newValue.search !== oldValue.search || newValue.count !== oldValue.count)) {
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
