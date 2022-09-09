<template>
  <b-container>
    <h1 v-t="'pokemon.title'" />
    <div class="my-2">
      <icon-button class="mx-1" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" href="/create-pokemon" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <gender-select class="col" v-model="gender" />
      <species-select class="col" v-model="speciesId" />
      <trainer-select class="col" v-model="trainerId" />
    </b-row>
    <b-row>
      <search-field class="col" v-model="search" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="pokemon.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" v-t="'pokemon.identification'" />
            <th scope="col" v-t="'species.select.label'" />
            <th scope="col" v-t="'trainers.select.label'" />
            <th scope="col" v-t="'pokemon.trainer.location'" />
            <th scope="col" v-t="'pokemon.heldItem'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in pokemon" :key="item.id">
            <td>
              <a :href="`/pokemon/${item.id}`">
                <gender-icon :gender="item.gender" />
                {{ item.surname || item.species.name }}
                {{ $t('pokemon.levelFormat', { level: item.level }) }}
              </a>
            </td>
            <td>
              <a :href="`/species/${item.species.id}`" target="_blank">
                {{ item.species.name }}
                <font-awesome-icon icon="external-link-alt" />
              </a>
            </td>
            <td>
              <a v-if="item.trainer" :href="`/trainers/${item.trainer.id}`" target="_blank">
                {{ item.trainer.name }}
                <font-awesome-icon icon="external-link-alt" />
              </a>
              <template v-else>&mdash;</template>
            </td>
            <td>
              <template v-if="item.position !== null">
                <template v-if="item.box !== null">{{ $t('pokemon.trainer.boxFormat', { box: item.box + 1, position: item.position + 1 }) }}</template>
                <template v-else>{{ $t('pokemon.trainer.partyFormat', { position: item.position + 1 }) }}</template>
              </template>
              <template v-else>&mdash;</template>
            </td>
            <td>
              <a v-if="item.heldItem" :href="`/items/${item.heldItem.id}`" target="_blank">
                {{ item.heldItem.name }}
                <font-awesome-icon icon="external-link-alt" />
              </a>
              <template v-else>&mdash;</template>
            </td>
            <td><status-cell :actor="item.updatedBy" :date="item.updatedAt" /></td>
            <td>
              <icon-button icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${item.id}`" />
              <delete-modal
                confirm="pokemon.delete.confirm"
                :displayName="`${item.surname || item.species.name} ${$t('pokemon.levelFormat', { level: item.level })}`"
                :id="`delete_${item.id}`"
                :loading="loading"
                title="pokemon.delete.title"
                @ok="onDelete(item, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'pokemon.empty'" />
  </b-container>
</template>

<script>
import GenderSelect from './GenderSelect.vue'
import SpeciesSelect from '@/components/Species/SpeciesSelect.vue'
import TrainerSelect from '@/components/Trainers/TrainerSelect.vue'
import { deletePokemon, getPokemonList } from '@/api/pokemon'

export default {
  name: 'PokemonList',
  components: {
    GenderSelect,
    SpeciesSelect,
    TrainerSelect
  },
  data() {
    return {
      count: 10,
      desc: false,
      gender: null,
      loading: false,
      page: 1,
      pokemon: [],
      search: null,
      sort: 'Name',
      speciesId: null,
      total: 0,
      trainerId: null
    }
  },
  computed: {
    params() {
      return {
        gender: this.gender,
        search: this.search,
        speciesId: this.speciesId,
        trainerId: this.trainerId,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('pokemon.sort.options')).map(([value, text]) => ({ text, value })),
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
          await deletePokemon(id)
          refresh = true
          this.toast('success', 'pokemon.delete.success')
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
          const { data } = await getPokemonList(params ?? this.params)
          this.pokemon = data.items
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
          (newValue.search !== oldValue.search ||
            newValue.gender !== oldValue.gender ||
            newValue.speciesId !== oldValue.speciesId ||
            newValue.trainerId !== oldValue.trainerId ||
            newValue.count !== oldValue.count)
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
