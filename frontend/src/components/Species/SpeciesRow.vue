<template>
  <tr>
    <td>
      <b-link :href="`/species/${species.id}`"><pokemon-icon :species="species" /></b-link>
    </td>
    <td>
      <b-link :href="`/species/${species.id}`">{{ species.name }}</b-link>
    </td>
    <td>
      <template v-if="region">
        {{ regionalNumber }} <sub>({{ species.number }})</sub>
      </template>
      <template v-else>{{ species.number }}</template>
    </td>
    <td>
      {{ $t(`type.options.${species.primaryType}`) }}
      <template v-if="species.secondaryType">
        <br />
        {{ $t(`type.options.${species.secondaryType}`) }}
      </template>
    </td>
    <td v-text="species.category || '—'" />
    <td><status-cell :actor="species.updatedBy || species.createdBy" :date="species.updatedOn || species.createdOn" /></td>
    <td>
      <icon-button disabled icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${species.id}`" />
      <delete-modal
        confirm="species.delete.confirm"
        :displayName="`${this.$i18n.t('number')} ${species.number.toString().padStart(3, '0')} ${species.name}`"
        :id="`delete_${species.id}`"
        :loading="loading"
        title="species.delete.title"
        @ok="$emit('deleted', species)"
      />
    </td>
  </tr>
</template>

<script>
export default {
  name: 'SpeciesRow',
  props: {
    loading: {
      type: Boolean,
      default: false
    },
    region: {
      type: Object,
      default: null
    },
    species: {
      type: Object,
      required: true
    }
  },
  computed: {
    regionalNumber() {
      return (this.region ? this.species.regionalNumbers.find(({ region }) => region?.id === this.region.id)?.number : null) ?? 0
    }
  }
}
</script>
