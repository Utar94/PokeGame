<template>
  <tr>
    <td>
      <b-link :href="`/species/${species.id}`" target="_blank"><pokemon-icon :species="species" /></b-link>
    </td>
    <td>
      <b-link :href="`/species/${species.id}`" target="_blank">
        <template v-if="region">
          {{ regionalNumber }} <sub>({{ species.number }})</sub>
        </template>
        <template v-else>{{ species.number }}</template>
        {{ ' ' }}
        <font-awesome-icon icon="external-link-alt" />
      </b-link>
    </td>
    <td v-text="species.name" />
    <td>{{ $t(entry.hasCaught ? 'yes' : 'no') }}</td>
    <td><status-cell :date="entry.updatedOn" /></td>
    <td>
      <icon-button class="mx-1" icon="edit" text="trainers.pokedex.edit" variant="primary" v-b-modal="`edit_${species.id}`" />
      <edit-entry-modal :entry="entry" :id="`edit_${species.id}`" :trainerId="trainerId" @updated="$emit('updated')" />
      <icon-button class="mx-1" icon="trash-alt" text="trainers.pokedex.remove.text" variant="danger" v-b-modal="`remove_${species.id}`" />
      <delete-modal
        confirm="trainers.pokedex.remove.confirm"
        :displayName="species.name"
        :id="`remove_${species.id}`"
        :loading="loading"
        title="trainers.pokedex.remove.text"
        @ok="$emit('removed', $event)"
      />
    </td>
  </tr>
</template>

<script>
import EditEntryModal from './EditEntryModal.vue'

export default {
  name: 'PokedexEntry',
  components: {
    EditEntryModal
  },
  props: {
    entry: {
      type: Object,
      required: true
    },
    loading: {
      type: Boolean,
      default: false
    },
    region: {
      type: String,
      default: ''
    },
    trainerId: {
      type: String,
      required: true
    }
  },
  computed: {
    regionalNumber() {
      return (this.region ? this.species.regionalNumbers.find(({ region }) => region === this.region)?.number : null) ?? 0
    },
    species() {
      return this.entry.species
    }
  }
}
</script>
