<template>
  <div>
    <h3 v-if="title" v-text="title" />
    <slot name="title" />
    <trainer-select
      v-if="!readonly"
      :disabled="trainers.length === 3"
      :exclude="exclude"
      :id="id"
      :value="trainer ? trainer.id : null"
      @trainer="trainer = $event"
    >
      <b-input-group-append>
        <icon-button :disabled="!trainer" icon="plus" variant="primary" @click="onAdd" />
      </b-input-group-append>
    </trainer-select>
    <table class="table table-striped">
      <tbody>
        <tr v-for="(trainer, index) in trainers" :key="trainer.id">
          <td><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</td>
          <td v-text="trainer.number" />
          <td>{{ $t(`region.options.${trainer.region}`) }}</td>
          <td>
            <template v-if="readonly">
              <icon-button icon="shopping-cart" text="battle.useItem" variant="primary" v-b-modal="`useItem_${trainer.id}`" />
              <use-item-modal :id="`useItem_${trainer.id}`" :pokemon="pokemon" :trainerId="trainer.id" @pokemonUpdated="$emit('pokemonUpdated', $event)" />
            </template>
            <icon-button v-else icon="times" variant="danger" @click="$emit('removed', index)" />
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import TrainerSelect from '@/components/Trainers/TrainerSelect.vue'
import UseItemModal from './UseItemModal.vue'

export default {
  name: 'TrainerTeam',
  components: {
    TrainerSelect,
    UseItemModal
  },
  props: {
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      required: true
    },
    pokemon: {
      type: Array,
      default: () => []
    },
    title: {
      type: String,
      default: ''
    },
    trainers: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      trainer: null
    }
  },
  computed: {
    readonly() {
      return this.$store.state.battle.step === 'Battle'
    }
  },
  methods: {
    onAdd() {
      this.$emit('added', this.trainer)
      this.trainer = null
    }
  }
}
</script>
