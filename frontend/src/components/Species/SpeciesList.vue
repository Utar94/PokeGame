<template>
  <b-container>
    <h1 v-t="'species.title'" />
    <div class="my-2">
      <icon-button class="mx-1" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" href="/create-species" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <type-select class="col" v-model="type" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="species.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" />
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'species.number'" />
            <th scope="col" v-t="'type.label'" />
            <th scope="col" v-t="'species.category.label'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in species" :key="item.id">
            <td>
              <b-link :href="`/species/${item.id}`"><pokemon-icon :species="item" /></b-link>
            </td>
            <td>
              <b-link :href="`/species/${item.id}`">{{ item.name }}</b-link>
            </td>
            <td v-text="item.number" />
            <td>
              {{ $t(`type.options.${item.primaryType}`) }}
              <template v-if="item.secondaryType">
                <br />
                {{ $t(`type.options.${item.secondaryType}`) }}
              </template>
            </td>
            <td v-text="item.category || '—'" />
            <td><status-cell :actor="item.updatedBy || item.createdBy" :date="item.updatedAt || item.createdAt" /></td>
            <td>
              <icon-button icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${item.id}`" />
              <delete-modal
                confirm="species.delete.confirm"
                :displayName="`No. ${item.number.toString().padStart(3, '0')} ${item.name}`"
                :id="`delete_${item.id}`"
                :loading="loading"
                title="species.delete.title"
                @ok="onDelete(item, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'species.empty'" />
  </b-container>
</template>

<script>
import { deleteSpecies, getSpeciesList } from '@/api/species'

export default {
  name: 'SpeciesList',
  data() {
    return {
      count: 10,
      desc: false,
      loading: false,
      page: 1,
      search: null,
      sort: 'Number',
      species: [],
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
        Object.entries(this.$i18n.t('species.sort.options')).map(([value, text]) => ({ text, value })),
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
          await deleteSpecies(id)
          refresh = true
          this.toast('success', 'species.delete.success')
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
          const { data } = await getSpeciesList(params ?? this.params)
          this.species = data.items
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
