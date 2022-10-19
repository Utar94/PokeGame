<template>
  <div>
    <h3 v-t="'moves.title'" />
    <table class="table">
      <thead>
        <tr>
          <th scope="col" />
          <th scope="col" v-t="'name.label'" />
          <th scope="col" v-t="'type.label'" />
          <th scope="col" v-t="'moves.category.label'" />
          <th scope="col" v-t="'moves.power'" />
          <th scope="col" v-t="'moves.accuracy.label'" />
          <th scope="col" v-t="'moves.powerPoints.unit'" />
          <th scope="col" v-t="'description.label'" />
        </tr>
      </thead>
      <tbody>
        <select-move-row v-for="pokemonMove in pokemonMoves" :key="pokemonMove.position" :data="pokemonMove" />
      </tbody>
    </table>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

import SelectMoveRow from './SelectMoveRow.vue'

export default {
  name: 'SelectMove',
  components: {
    SelectMoveRow
  },
  computed: {
    ...mapGetters(['battleMoveAttacker']),
    pokemonMoves() {
      return this.orderBy(this.battleMoveAttacker.moves, 'position')
    }
  }
}
</script>
