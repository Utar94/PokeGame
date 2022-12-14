<template>
  <b-tab :title="$t('trainers.pokedex.label')">
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" icon="plus" text="trainers.pokedex.add" variant="success" v-b-modal.addEntry />
      <edit-entry-modal :exclude="speciesIds" id="addEntry" :trainerId="trainer.id" @updated="refresh()" />
    </div>
    <p>
      {{ $t('trainers.pokedex.count', { count: pokedexCount }) }}
      <b-badge v-if="nationalPokedex" class="mx-1" variant="info">{{ $t('trainers.pokedex.national') }}</b-badge>
      <b-badge v-else class="mx-1">{{ $t('trainers.pokedex.regional') }}</b-badge>
    </p>
    <b-row>
      <search-field class="col" v-model="search" />
      <region-select class="col" v-model="region" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="entries.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" />
            <th scope="col" v-t="'species.number.label'" />
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'trainers.pokedex.hasCaught'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <pokedex-entry
            v-for="entry in entries"
            :key="entry.species.number"
            :entry="entry"
            :loading="loading"
            :region="region"
            :trainerId="trainer.id"
            @removed="onRemove(entry, $event)"
            @updated="onUpdate()"
          />
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'trainers.pokedex.empty'" />
  </b-tab>
</template>

<script>
import EditEntryModal from './EditEntryModal.vue'
import RegionSelect from '@/components/Regions/RegionSelect.vue'
import PokedexEntry from './PokedexEntry.vue'
import { deleteEntry, getEntries } from '@/api/pokedex'
import { getTrainer } from '@/api/trainers'

export default {
  name: 'PokedexTab',
  components: {
    EditEntryModal,
    RegionSelect,
    PokedexEntry
  },
  props: {
    trainer: {
      type: Object,
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
      nationalPokedex: false,
      pokedexCount: 0,
      page: 1,
      region: null,
      search: null,
      sort: 'Number',
      type: null
    }
  },
  computed: {
    params() {
      return {
        hasCaught: this.hasCaught,
        regionId: this.region?.id ?? null,
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
          await deleteEntry(this.trainer.id, species.id)
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
    async onUpdate() {
      await this.refresh()
      this.toast('success', 'trainers.pokedex.updated')
    },
    async refresh(params = null) {
      if (!this.loading) {
        this.loading = true
        try {
          const pokedex = await getEntries(this.trainer.id, params ?? this.params)
          this.entries = pokedex.data.items
          this.total = pokedex.data.total
          const trainer = await getTrainer(this.trainer.id)
          this.nationalPokedex = trainer.data.nationalPokedex
          this.pokedexCount = trainer.data.pokedexCount
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
          (newValue.hasCaught !== oldValue.hasCaught ||
            (newValue.region?.id ?? null) !== (oldValue.region?.id ?? null) ||
            newValue.search !== oldValue.search ||
            newValue.type !== oldValue.type)
        ) {
          this.page = 1
          await this.refresh()
        } else {
          await this.refresh(newValue)
        }
      }
    },
    trainer: {
      deep: true,
      immediate: true,
      handler(trainer) {
        this.nationalPokedex = trainer?.nationalPokedex ?? false
        this.pokedexCount = trainer?.pokedexCount ?? 0
      }
    }
  }
}
</script>
