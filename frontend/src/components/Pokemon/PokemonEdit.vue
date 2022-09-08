<template>
  <b-container>
    <h1>{{ $t('pokemon.editTitle', { name }) }}</h1>
    <status-detail :model="pokemon" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <icon-submit :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <description-field v-model="description" />
          </b-tab>
          <b-tab :title="$t('metadata')">
            <reference-field v-model="reference" />
            <notes-field v-model="notes" />
          </b-tab>
        </b-tabs>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { updatePokemon } from '@/api/pokemon'

export default {
  name: 'PokemonEdit',
  props: {
    json: {
      type: String,
      required: true
    },
    status: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      description: null,
      loading: false,
      notes: null,
      pokemon: null,
      reference: null
    }
  },
  computed: {
    hasChanges() {
      return (
        (this.description ?? '') !== (this.pokemon.description ?? '') ||
        (this.reference ?? '') !== (this.pokemon.reference ?? '') ||
        (this.notes ?? '') !== (this.pokemon.notes ?? '')
      )
    },
    name() {
      return `${this.pokemon.surname ?? this.pokemon.species.name} ${this.$i18n.t('pokemon.levelFormat', { level: this.pokemon.level })}`
    },
    payload() {
      const payload = {
        description: this.description,
        reference: this.reference,
        notes: this.notes
      }
      return payload
    }
  },
  methods: {
    setModel(pokemon) {
      this.pokemon = pokemon
      this.description = pokemon.description
      this.notes = pokemon.notes
      this.reference = pokemon.reference
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await updatePokemon(this.pokemon.id, this.payload)
            this.setModel(data)
            this.toast('success', 'pokemon.updated')
            this.$refs.form.reset()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  created() {
    this.setModel(JSON.parse(this.json))
    if (this.status === 'created') {
      this.toast('success', 'pokemon.created')
    }
  }
}
</script>
