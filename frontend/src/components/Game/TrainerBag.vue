<template>
  <b-container>
    <h1 v-t="'game.bag.title'" />
    <div class="my-2">
      <icon-button icon="arrow-left" text="game.back" variant="danger" @click="navigateGame(null)" />
    </div>
    <table class="table">
      <tbody>
        <tr class="table-secondary">
          <th scope="row" v-t="'trainers.money'" />
          <td><pokemon-dollar /> {{ gameInventory.money }}</td>
        </tr>
      </tbody>
    </table>
    <b-tabs content-class="mt-3">
      <bag-tab :items="gameInventory.medicine || []" title="items.category.options.Medicine" @selected="onItemSelected" />
      <bag-tab :items="gameInventory.pokeBalls || []" title="items.category.options.PokeBall" @selected="onItemSelected" />
      <bag-tab :items="gameInventory.battleItems || []" title="items.category.options.BattleItem" @selected="onItemSelected" />
      <bag-tab :items="gameInventory.berries || []" title="items.category.options.Berry" @selected="onItemSelected" />
      <bag-tab :items="gameInventory.otherItems || []" title="items.category.options.OtherItem" @selected="onItemSelected" />
      <bag-tab :items="gameInventory.tms || []" title="items.category.options.TM" @selected="onItemSelected" />
      <bag-tab :items="gameInventory.treasures || []" title="items.category.options.Treasure" @selected="onItemSelected" />
      <bag-tab :items="gameInventory.keyItems || []" title="items.category.options.KeyItem" @selected="onItemSelected" />
    </b-tabs>
    <b-modal v-model="showModal" :title="$t('game.bag.itemDetail')">
      <template #modal-header="{ close }">
        <h5 class="modal-title"><item-icon :item="selectedItem" /> {{ selectedItem.name }}</h5>
        <button aria-label="close" class="close" type="button" @click="close()">&times;</button>
      </template>
      <p v-if="selectedItem && selectedItem.description" v-text="selectedItem.description" />
      <template #modal-footer="{ close }">
        <icon-button icon="times" text="game.close" @click="close()" />
      </template>
    </b-modal>
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import BagTab from './BagTab.vue'
import ItemIcon from '@/components/Items/ItemIcon.vue'

export default {
  name: 'TrainerBag',
  components: {
    BagTab,
    ItemIcon
  },
  data() {
    return {
      interval: null,
      selectedItem: null,
      showModal: false
    }
  },
  computed: {
    ...mapGetters(['gameInventory'])
  },
  methods: {
    ...mapActions(['loadGameInventory', 'navigateGame']),
    onItemSelected(item) {
      this.selectedItem = item
      this.showModal = true
    },
    async refresh() {
      try {
        await this.loadGameInventory()
      } catch (e) {
        this.handleError(e)
      }
    }
  },
  async created() {
    await this.refresh()
    this.interval = setInterval(this.refresh, Number(process.env.VUE_APP_REFRESH_RATE))
  },
  beforeDestroy() {
    if (this.interval) {
      clearInterval(this.interval)
    }
  }
}
</script>
