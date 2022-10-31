<template>
  <tr>
    <td>
      <b-link :href="`/species/${species.id}`" target="_blank">
        <pokemon-icon :species="species" />
      </b-link>
    </td>
    <td>
      <b-link :href="`/species/${species.id}`" target="_blank"> {{ species.name }} <font-awesome-icon icon="external-link-alt" /> </b-link>
    </td>
    <td>{{ $t(`species.evolutions.method.options.${evolution.method}`) }}</td>
    <td>
      <template v-if="hasNoCondition">&mdash;</template>
      <template v-else>
        <div v-if="evolution.level">{{ $t('species.evolutions.levelFormat', { level: evolution.level }) }}</div>
        <div v-if="item">{{ $t(`species.evolutions.${evolution.method === 'Item' ? 'itemFormat' : 'holdingItemFormat'}`, { item: item.name }) }}</div>
        <div v-if="evolution.gender">{{ $t('species.evolutions.genderFormat', { gender: this.$i18n.t(`pokemon.gender.options.${evolution.gender}`) }) }}</div>
        <div v-if="evolution.region">{{ $t('species.evolutions.regionFormat', { region: evolution.region.name }) }}</div>
        <div v-if="evolution.location">{{ $t('species.evolutions.location.format', { location: evolution.location }) }}</div>
        <div v-if="move">{{ $t('species.evolutions.moveFormat', { move: move.name }) }}</div>
        <div v-if="evolution.timeOfDay">
          {{ $t('species.evolutions.timeOfDay.format', { timeOfDay: this.$i18n.t(`species.evolutions.timeOfDay.options.${evolution.timeOfDay}`) }) }}
        </div>
        <div v-if="evolution.highFriendship">{{ $t('species.evolutions.highFriendship') }}</div>
      </template>
    </td>
    <td v-text="evolution.notes || '—'"></td>
    <td>
      <icon-button class="mx-1" icon="edit" variant="primary" v-b-modal="`edit_${species.id}`" />
      <icon-button class="mx-1" icon="times" variant="danger" v-b-modal="`remove_${species.id}`" />
      <evolution-modal :evolution="evolution" :evolvingId="id" :exclude="exclude" :id="`edit_${species.id}`" @saved="$emit('updated', $event)" />
      <delete-modal
        confirm="species.evolutions.delete.confirm"
        :displayName="species.name"
        :id="`remove_${species.id}`"
        :loading="loading"
        title="species.evolutions.delete.title"
        @ok="$emit('removed', evolution)"
      />
    </td>
  </tr>
</template>

<script>
import EvolutionModal from './EvolutionModal.vue'

export default {
  name: 'EvolutionRow',
  components: {
    EvolutionModal
  },
  props: {
    evolution: {
      type: Object,
      required: true
    },
    exclude: {
      type: Array,
      required: true
    },
    id: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      loading: false
    }
  },
  computed: {
    hasNoCondition() {
      return (
        !this.evolution.level &&
        !this.item &&
        !this.evolution.gender &&
        !this.evolution.region &&
        !this.evolution.location &&
        !this.move &&
        !this.evolution.timeOfDay &&
        !this.evolution.highFriendship
      )
    },
    item() {
      return this.evolution.item
    },
    move() {
      return this.evolution.move
    },
    species() {
      return this.evolution.species
    }
  }
}
</script>
