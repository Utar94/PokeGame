<template>
  <b-tab :title="$t('species.regional.title')">
    <b-alert dismissible variant="warning" :show="conflict" @input="$emit('dismissed', $event)">
      <strong v-t="'species.regional.numbersAlreadyUsed'" />
    </b-alert>
    <div class="my-2">
      <icon-button
        class="mx-1"
        :disabled="excludedRegions.length === regions"
        icon="plus"
        text="species.regional.addNumber"
        variant="success"
        v-b-modal.addNumber
      />
      <regional-number-modal :exclude="excludedRegions" id="addNumber" @ok="addNumber" />
    </div>
    <table v-if="value.length > 0" class="table table-striped">
      <thead>
        <tr>
          <th scope="col" v-t="'regions.select.label'" />
          <th scope="col" v-t="'species.number.label'" />
          <th scope="col" />
        </tr>
      </thead>
      <tbody>
        <tr v-for="(regionalNumber, index) in value" :key="index">
          <td>
            <b-badge v-if="regionalNumber.conflict" variant="warning">{{ $t('species.regional.conflict') }}</b-badge> {{ regionalNumber.region.name }}
          </td>
          <td v-text="regionalNumber.number" />
          <td>
            <icon-button class="mx-1" icon="edit" variant="primary" v-b-modal="`editNumber_${index}`" />
            <icon-button class="mx-1" icon="times" variant="danger" @click="onRemove(index)" />
            <regional-number-modal :exclude="excludedRegions" :id="`editNumber_${index}`" :regionalNumber="regionalNumber" @ok="editNumber(index, $event)" />
          </td>
        </tr>
      </tbody>
    </table>
    <p v-else v-t="'species.regional.empty'" />
  </b-tab>
</template>

<script>
import Vue from 'vue'
import RegionalNumberModal from './RegionalNumberModal.vue'
import { getRegions } from '@/api/regions'

export default {
  name: 'RegionalTab',
  components: {
    RegionalNumberModal
  },
  props: {
    conflict: {
      type: Boolean,
      default: false
    },
    value: {
      type: Array,
      required: true
    }
  },
  data() {
    return {
      regions: 0
    }
  },
  computed: {
    excludedRegions() {
      return this.value.filter(({ region }) => Boolean(region)).map(({ region }) => region.id)
    }
  },
  methods: {
    addNumber({ number, region }) {
      const values = [...this.value]
      values.push({ number, region })
      this.$emit('input', values)
    },
    editNumber(index, { number, region }) {
      const values = [...this.value]
      Vue.set(values, index, { number, region })
      this.$emit('input', values)
    },
    onRemove(index) {
      const values = [...this.value]
      Vue.delete(values, index)
      this.$emit('input', values)
    }
  },
  async created() {
    try {
      const { data } = await getRegions()
      this.regions = data.total
    } catch (e) {
      this.handleError(e)
    }
  }
}
</script>
