<template>
  <table class="table table-hover">
    <thead>
      <tr>
        <th scope="col" />
        <th scope="col" v-t="'name.label'" />
        <th scope="col" v-t="'type.label'" />
        <th scope="col" v-t="'moves.category.label'" />
        <th scope="col" v-t="'moves.power'" />
        <th scope="col" v-t="'moves.accuracy.label'" />
        <th scope="col" v-t="'moves.powerPoints.label'" />
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="row in moves"
        :key="row.position"
        :class="{ clickable: true, 'table-info': selected && selected.position === row.position }"
        @click.prevent="onToggle(row)"
      >
        <td>
          <b-form-checkbox :checked="selected && selected.position === row.position" :disabled="!row.remainingPowerPoints" size="lg" />
        </td>
        <td><strong v-text="row.move.name" /></td>
        <td>{{ $t(`type.options.${row.move.type}`) }}</td>
        <td>{{ $t(`moves.category.options.${row.move.category}`) }}</td>
        <td v-text="row.move.power || '—'" />
        <td>{{ row.move.accuracy === null ? '—' : $n(row.move.accuracy / 100, 'percent') }}</td>
        <td>{{ $t('battle.makeMove.ppFormat', { current: row.remainingPowerPoints, max: row.move.powerPoints }) }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script>
export default {
  name: 'MakeMoveTable',
  props: {
    moves: {
      type: Array,
      default: () => []
    },
    selected: {
      type: Object,
      default: null
    }
  },
  methods: {
    onToggle(row) {
      if (row.remainingPowerPoints > 0) {
        this.$emit('toggled', row)
      }
    }
  }
}
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
</style>
