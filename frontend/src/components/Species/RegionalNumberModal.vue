<template>
  <b-modal :id="id" :title="$t(title)" @shown="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <region-select :exclude="excludedRegions" required v-model="region" />
        <form-field id="number" label="species.number.label" :minValue="1" :maxValue="999" required :step="1" type="number" v-model.number="number" />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button :icon="regionalNumber ? 'edit' : 'plus'" :text="title" :variant="regionalNumber ? 'primary' : 'success'" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import RegionSelect from '@/components/Regions/RegionSelect.vue'

export default {
  name: 'RegionalNumberModal',
  components: {
    RegionSelect
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
    regionalNumber: {
      type: Object,
      default: null
    }
  },
  data() {
    return {
      number: 0,
      region: null
    }
  },
  computed: {
    excludedRegions() {
      return this.regionalNumber?.region ? this.exclude.filter(id => id !== this.regionalNumber.region.id) : this.exclude
    },
    title() {
      return `species.regional.${this.regionalNumber ? 'editNumber' : 'addNumber'}`
    }
  },
  methods: {
    reset() {
      this.region = this.regionalNumber?.region ?? null
      this.number = this.regionalNumber?.number ?? 0
    },
    async submit(callback = null) {
      try {
        if (await this.$refs.form.validate()) {
          this.$emit('ok', { number: this.number, region: this.region })
          if (typeof callback === 'function') {
            callback()
          }
        }
      } catch (e) {
        this.handleError(e)
      }
    }
  }
}
</script>
