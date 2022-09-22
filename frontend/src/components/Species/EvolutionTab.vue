<template>
  <b-tab :title="$t('species.evolutions.title')">
    <div class="my-2">
      <icon-button icon="plus" text="species.evolutions.add" variant="success" v-b-modal.evolutionModal />
      <evolution-modal :evolvingId="id" :exclude="exclude" @saved="onEvolutionAdded" />
    </div>
    <table v-if="evolutions.length > 0" class="table table-striped">
      <thead>
        <tr>
          <th scope="col" />
          <th scope="col" v-t="'species.title'" />
          <th scope="col" v-t="'species.evolutions.method.label'" />
          <th scope="col" v-t="'species.evolutions.conditions'" />
          <th scope="col" v-t="'species.evolutions.notes'" />
          <th scope="col" />
        </tr>
      </thead>
      <tbody>
        <evolution-row
          v-for="evolution in evolutions"
          :key="evolution.species.id"
          :evolution="evolution"
          :exclude="exclude"
          :id="id"
          @removed="onEvolutionRemoved"
          @updated="onEvolutionUpdated"
        />
      </tbody>
    </table>
    <p v-else v-t="'species.evolutions.empty'" />
  </b-tab>
</template>

<script>
import Vue from 'vue'
import EvolutionModal from './EvolutionModal.vue'
import EvolutionRow from './EvolutionRow.vue'
import { getSpeciesEvolutions, removeSpeciesEvolution } from '@/api/species'

export default {
  name: 'EvolutionTab',
  components: {
    EvolutionModal,
    EvolutionRow
  },
  props: {
    id: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      evolutions: []
    }
  },
  computed: {
    exclude() {
      return this.evolutions.map(({ species }) => species.id).concat(this.id)
    }
  },
  methods: {
    onEvolutionAdded(evolution) {
      this.evolutions.push(evolution)
    },
    async onEvolutionRemoved({ species }) {
      if (!this.loading) {
        this.loading = true
        try {
          await removeSpeciesEvolution(this.id, species.id)
          const index = this.evolutions.findIndex(evolution => evolution.species.id === species.id)
          this.toast('success', 'species.evolutions.delete.success')
          if (index >= 0) {
            Vue.delete(this.evolutions, index)
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    },
    onEvolutionUpdated(evolution) {
      const index = this.evolutions.findIndex(({ species }) => species.id === evolution.species.id)
      if (index >= 0) {
        Vue.set(this.evolutions, index, evolution)
      }
    }
  },
  watch: {
    id: {
      immediate: true,
      async handler(id) {
        try {
          const { data } = await getSpeciesEvolutions(id)
          this.evolutions = data
        } catch (e) {
          this.handleError(e)
        }
      }
    }
  }
}
</script>
