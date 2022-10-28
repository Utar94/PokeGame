<template>
  <b-container>
    <h1 v-t="'abilities.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" href="/create-ability" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="abilities.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="ability in abilities" :key="ability.id">
            <td>
              <b-link :href="`/abilities/${ability.id}`">{{ ability.name }}</b-link>
            </td>
            <td><status-cell :actor="ability.updatedBy || ability.createdBy" :date="ability.updatedOn || ability.createdOn" /></td>
            <td>
              <icon-button icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${ability.id}`" />
              <delete-modal
                confirm="abilities.delete.confirm"
                :displayName="ability.name"
                :id="`delete_${ability.id}`"
                :loading="loading"
                title="abilities.delete.title"
                @ok="onDelete(ability, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'abilities.empty'" />
  </b-container>
</template>

<script>
import { deleteAbility, getAbilities } from '@/api/abilities'

export default {
  name: 'AbilityList',
  data() {
    return {
      abilities: [],
      count: 10,
      desc: false,
      loading: false,
      page: 1,
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
        Object.entries(this.$i18n.t('abilities.sort.options')).map(([value, text]) => ({ text, value })),
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
          await deleteAbility(id)
          refresh = true
          this.toast('success', 'abilities.delete.success')
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
          const { data } = await getAbilities(params ?? this.params)
          this.abilities = data.items
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
