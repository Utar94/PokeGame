<template>
  <b-modal id="addExperience" :title="$t('pokemon.experience.title')" @hidden="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <form-field id="experience" label="experience" :minValue="1" :maxValue="999999" required :step="1" type="number" v-model.number="experience" />
        <p v-if="experience >= pokemon.experienceToNextLevel"><i class="text-warning" v-t="'pokemon.experience.warning'" /></p>
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button :disabled="loading" icon="plus" :loading="loading" text="pokemon.experience.add" variant="success" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import { gainExperience } from '@/api/pokemon'

export default {
  name: 'ExperienceModal',
  props: {
    pokemon: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      experience: 1,
      loading: false
    }
  },
  computed: {
    payload() {
      return {
        experience: this.experience
      }
    }
  },
  methods: {
    reset() {
      this.experience = 1
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await gainExperience(this.pokemon.id, this.payload)
            this.$emit('updated', data)
            if (data.level > this.pokemon.level) {
              this.toast('warning', 'pokemon.experience.leveledUp', 'warning')
            } else {
              this.toast('success', 'pokemon.experience.gained')
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
