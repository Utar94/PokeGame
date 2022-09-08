<template>
  <b-modal :id="id" :title="$t(`trainers.pokedex.${entry ? 'edit' : 'add'}`)" @hidden="reset()">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <species-select :disabled="Boolean(entry)" :exclude="exclude" :required="!entry" v-model="speciesId" />
        <b-form-checkbox v-model="hasCaught">{{ $t('trainers.pokedex.hasCaught') }}</b-form-checkbox>
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button
        :disabled="loading"
        :icon="entry ? 'edit' : 'plus'"
        :loading="loading"
        :text="`trainers.pokedex.${entry ? 'edit' : 'add'}`"
        :variant="entry ? 'primary' : 'success'"
        @click="submit(ok)"
      />
    </template>
  </b-modal>
</template>

<script>
import SpeciesSelect from '@/components/Species/SpeciesSelect.vue'
import { saveEntry } from '@/api/pokedex'

export default {
  name: 'EditEntryModal',
  components: {
    SpeciesSelect
  },
  props: {
    entry: {
      type: Object,
      default: null
    },
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      required: true
    },
    trainerId: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      hasCaught: false,
      loading: false,
      speciesId: null
    }
  },
  computed: {
    payload() {
      return {
        hasCaught: this.hasCaught
      }
    }
  },
  methods: {
    reset(entry = null) {
      entry = entry ?? this.entry
      if (entry) {
        this.speciesId = entry.species.id
        this.hasCaught = entry.hasCaught
      } else {
        this.speciesId = null
        this.hasCaught = false
      }
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await saveEntry(this.trainerId, this.speciesId, this.payload)
            this.$emit('updated', data)
            this.$refs.form.reset()
            if (typeof callback === 'function') {
              callback()
            }
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    entry: {
      deep: true,
      immediate: true,
      handler(entry) {
        if (entry) {
          this.speciesId = entry.species.id
          this.hasCaught = entry.hasCaught
        } else {
          this.reset()
        }
      }
    }
  }
}
</script>
