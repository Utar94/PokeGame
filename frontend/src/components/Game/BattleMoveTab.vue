<template>
  <b-tab :disabled="pokemon.isEgg" :title="$t('game.pokemon.battleMoves')">
    <table class="table table-hover">
      <tbody>
        <tr v-for="move in sortedMoves" :key="move.position" class="clickable" @click="selectMove(move)">
          <td>{{ $t(`type.options.${move.type}`) }}</td>
          <td v-text="move.name" />
          <td>{{ move.remainingPowerPoints }}/{{ move.powerPoints }}</td>
        </tr>
      </tbody>
    </table>
    <template v-if="move">
      <h6 v-text="move.name" />
      <table class="table">
        <tbody>
          <tr>
            <th scope="row" v-t="'moves.category.label'" />
            <td v-text="$t(`moves.category.options.${move.category}`)" />
          </tr>
          <tr>
            <th scope="row" v-t="'moves.power'" />
            <td v-text="move.power || '—'" />
          </tr>
          <tr>
            <th scope="row" v-t="'moves.accuracy.label'" />
            <td v-text="move.accuracy === null ? '—' : move.accuracy" />
          </tr>
          <tr v-if="move.description">
            <td colspan="2" v-text="move.description" />
          </tr>
        </tbody>
      </table>
    </template>
  </b-tab>
</template>

<script>
export default {
  name: 'BattleMoveTab',
  props: {
    pokemon: {
      type: Object,
      default: null
    }
  },
  data() {
    return {
      move: null
    }
  },
  computed: {
    sortedMoves() {
      return this.orderBy(this.pokemon.moves ?? [], 'position')
    }
  },
  methods: {
    selectMove(move) {
      this.move = move
    }
  }
}
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
</style>
