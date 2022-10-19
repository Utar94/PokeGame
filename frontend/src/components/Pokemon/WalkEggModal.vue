<template>
  <b-modal :id="id" :title="$t('pokemon.walkEgg.title')" @show="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <form-field
          id="steps"
          label="pokemon.walkEgg.steps"
          :minValue="1"
          :maxValue="pokemon.remainingHatchSteps"
          required
          :step="1"
          type="number"
          v-model.number="steps"
        />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button :disabled="loading" icon="walking" :loading="loading" text="pokemon.walkEgg.label" variant="primary" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import Vue from 'vue'
import { walkPokemonEgg } from '@/api/pokemon'

export default {
  name: 'DeleteModal',
  props: {
    id: {
      type: String,
      default: 'walkEgg'
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      loading: false,
      steps: 1
    }
  },
  computed: {
    payload() {
      return {
        steps: this.steps
      }
    }
  },
  methods: {
    reset() {
      this.steps = 1
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await walkPokemonEgg(this.pokemon.id, this.payload)
            this.$emit('updated', data)
            if (data.remainingHatchSteps === 0) {
              Vue.nextTick(() => this.toast('warning', 'pokemon.walkEgg.hatched', 'warning'))
            } else {
              this.toast('success', 'pokemon.walkEgg.walked')
            }
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
  }
}
</script>
