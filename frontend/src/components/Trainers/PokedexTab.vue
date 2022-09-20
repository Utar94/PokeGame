<template>
  <b-tab :title="$t('trainers.pokedex.label')">
    <div class="my-2">
      <icon-button class="mx-1" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" icon="plus" text="trainers.pokedex.add" variant="success" v-b-modal.addEntry />
      <edit-entry-modal :exclude="speciesIds" id="addEntry" :trainerId="trainerId" @updated="refresh()" />
    </div>
    <b-row>
      <search-field class="col" v-model="search" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="entries.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" />
            <th scope="col" v-t="'species.number'" />
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'trainers.pokedex.hasCaught'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="(entry, index) in entries" :key="entry.species.number">
            <td>
              <b-link :href="`/species/${entry.species.id}`" target="_blank"><pokemon-icon :species="entry.species" /></b-link>
            </td>
            <td>
              <b-link :href="`/species/${entry.species.id}`" target="_blank">
                {{ entry.species.number }}
                <font-awesome-icon icon="external-link-alt" />
              </b-link>
            </td>
            <td v-text="entry.species.name" />
            <td>{{ $t(entry.hasCaught ? 'yes' : 'no') }}</td>
            <td><status-cell :date="entry.updatedAt" /></td>
            <td>
              <icon-button class="mx-1" icon="edit" text="trainers.pokedex.edit" variant="primary" v-b-modal="`edit_${index}`" />
              <edit-entry-modal :entry="entry" :id="`edit_${index}`" :trainerId="trainerId" @updated="refresh()" />
              <icon-button class="mx-1" icon="trash-alt" text="trainers.pokedex.remove.text" variant="danger" v-b-modal="`remove_${index}`" />
              <delete-modal
                confirm="trainers.pokedex.remove.confirm"
                :displayName="entry.species.name"
                :id="`remove_${index}`"
                :loading="loading"
                title="trainers.pokedex.remove.text"
                @ok="onRemove(entry, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'trainers.pokedex.empty'" />
  </b-tab>
</template>

<script>
import EditEntryModal from './EditEntryModal.vue'
import { deleteEntry, getEntries } from '@/api/pokedex'

export default {
  name: 'PokedexTab',
  components: {
    EditEntryModal
  },
  props: {
    trainerId: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      count: 100,
      desc: false,
      entries: [],
      hasCaught: null,
      loading: false,
      page: 1,
      search: null,
      sort: 'Number',
      type: null
    }
  },
  computed: {
    params() {
      return {
        hasCaught: this.hasCaught,
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
        Object.entries(this.$i18n.t('trainers.pokedex.sort.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    },
    speciesIds() {
      return this.entries.map(({ species }) => species.id)
    }
  },
  methods: {
    async onRemove({ species }, callback = null) {
      if (!this.loading) {
        this.loading = true
        let refresh = false
        try {
          await deleteEntry(this.trainerId, species.id)
          refresh = true
          this.toast('success', 'trainers.pokedex.remove.success')
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
          const { data } = await getEntries(this.trainerId, params ?? this.params)
          this.entries = data.items
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
          (newValue.hasCaught !== oldValue.hasCaught || newValue.search !== oldValue.search || newValue.type !== oldValue.type)
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
