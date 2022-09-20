<template>
  <tr :class="{ 'table-info': selected }">
    <td><b-form-checkbox :checked="selected" :disabled="data.remainingPowerPoints === 0" size="lg" @change="toggleBattleMove(move)" /></td>
    <td>
      <span v-b-tooltip.hover :title="move.notes">
        <b-link v-if="move.reference" :href="move.reference" target="_blank"> {{ move.name }} <font-awesome-icon icon="external-link-alt" /> </b-link>
        <template v-else>{{ move.name }}</template>
      </span>
    </td>
    <td>{{ $t(`type.options.${move.type}`) }}</td>
    <td>{{ $t(`moves.category.options.${move.category}`) }}</td>
    <td v-text="move.power || '—'"></td>
    <td>{{ move.accuracy === null ? '—' : $n(move.accuracy / 100, 'percent') }}</td>
    <td>{{ data.remainingPowerPoints }}/{{ move.powerPoints }}</td>
    <td v-text="move.description" />
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
export default {
  name: 'SelectMoveRow',
  props: {
    data: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters(['selectedBattleMove']),
    move() {
      return this.data.move
    },
    selected() {
      return this.selectedBattleMove?.id === this.data.move.id
    }
  },
  methods: {
    ...mapActions(['toggleBattleMove'])
  }
}
</script>
