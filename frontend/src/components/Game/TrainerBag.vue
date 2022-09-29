<template>
  <b-container>
    <h1 v-t="'game.bag'" />
    <div class="my-2">
      <icon-button icon="sign-out-alt" text="game.exit" variant="danger" @click="navigateGame(null)" />
    </div>
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
    <b-sidebar shadow v-model="showSidebar" @hidden="selectedItem = null">
      <template v-if="selectedItem" #title><item-icon :item="selectedItem" /> {{ selectedItem.name }}</template>
      <div v-if="selectedItem && selectedItem.description" class="px-3 py-2">
        <p v-text="selectedItem.description" />
      </div>
    </b-sidebar>
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
      selectedItem: null,
      showSidebar: false
    }
  },
  computed: {
    ...mapGetters(['gameInventory'])
  },
  methods: {
    ...mapActions(['loadGameInventory', 'navigateGame']),
    onItemSelected(item) {
      if (this.selectedItem?.id === item.id) {
        this.showSidebar = false
      } else {
        this.selectedItem = item
        this.showSidebar = true
      }
    }
  },
  async created() {
    try {
      await this.loadGameInventory()
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
