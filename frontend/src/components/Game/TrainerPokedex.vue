<template>
  <b-container fluid>
    <h1 v-t="'trainers.pokedex.label'" />
    <div class="my-2">
      <icon-button icon="arrow-left" text="game.back" variant="danger" @click="navigateGame(null)" />
    </div>
    <b-row>
      <b-col>
        <p>
          <strong>
            <font-awesome-icon icon="eye" /> {{ $t('game.pokedex.seen', { seen }) }}
            <br />
            <img alt="Poké Ball Logo" height="20" src="@/assets/poke-ball-logo.svg" /> {{ $t('game.pokedex.caught', { caught }) }}
          </strong>
        </p>
      </b-col>
      <b-col v-if="gamePokedex.hasNational">
        <b-form-checkbox name="check-button" size="lg" switch v-model="national">{{ $t('game.pokedex.national') }}</b-form-checkbox>
      </b-col>
    </b-row>
    <table class="table table-hover">
      <tbody>
        <pokedex-row v-for="entry in sortedEntries" :key="entry.number" :entry="entry" @click="openEntry(entry)" />
      </tbody>
    </table>
    <pokedex-modal :entry="selectedEntry" v-model="showModal" />
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import PokedexModal from './PokedexModal.vue'
import PokedexRow from './PokedexRow.vue'

export default {
  name: 'TrainerPokedex',
  components: {
    PokedexModal,
    PokedexRow
  },
  data() {
    return {
      national: false,
      selectedEntry: null,
      showModal: false
    }
  },
  computed: {
    ...mapGetters(['gamePokedex']),
    caught() {
      return this.gamePokedex.entries.filter(({ hasCaught }) => hasCaught).length
    },
    seen() {
      return this.gamePokedex.entries.length
    },
    sortedEntries() {
      return this.orderBy(this.gamePokedex.entries, 'number')
    }
  },
  methods: {
    ...mapActions(['loadGamePokedex', 'navigateGame']),
    openEntry(entry) {
      this.selectedEntry = entry
      this.showModal = true
    }
  },
  async created() {
    try {
      await this.loadGamePokedex()
    } catch (e) {
      this.handleError(e)
    }
  },
  watch: {
    national: {
      immediate: true,
      async handler(national) {
        await this.loadGamePokedex(national)
      }
    }
  }
}
</script>
