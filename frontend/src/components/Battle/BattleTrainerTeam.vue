<template>
  <div>
    <slot name="title" />
    <table class="table table-striped">
      <tbody>
        <tr v-for="trainer in trainers" :key="trainer.id">
          <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
          <td v-text="trainer.number" />
          <td>{{ $t(`region.options.${trainer.region}`) }}</td>
          <td>
            <icon-button icon="shopping-cart" text="battle.useItem" variant="primary" v-b-modal="`useItem_${trainer.id}`" />
            <use-item-modal :id="`useItem_${trainer.id}`" :pokemon="pokemon" :trainerId="trainer.id" @pokemonUpdated="$emit('pokemonUpdated', $event)" />
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import UseItemModal from './UseItemModal.vue'

export default {
  name: 'TrainerTeam',
  components: {
    UseItemModal
  },
  props: {
    pokemon: {
      type: Array,
      default: () => []
    },
    trainers: {
      type: Array,
      default: () => []
    }
  }
}
</script>
