<template>
  <b-modal :id="id" :title="$t('pokemon.trainer.swapTitle')" @show="reset">
    <form-select
      id="otherPokemonId"
      label="pokemon.select.label"
      :options="options"
      placeholder="pokemon.select.placeholder"
      required
      v-model="otherPokemonId"
    />
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button
        :disabled="!otherPokemonId || loading"
        icon="exchange-alt"
        :loading="loading"
        text="pokemon.trainer.swap"
        variant="primary"
        @click="submit(ok)"
      />
    </template>
  </b-modal>
</template>

<script>
import { getPokemonList, swapPokemon } from '@/api/pokemon'

export default {
  name: 'SwapModal',
  props: {
    id: {
      type: String,
      default: 'swapModal'
    },
    pokemon: {
      type: Object,
      required: true
    },
    trainerId: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      loading: false,
      otherPokemonId: null,
      pokemonList: []
    }
  },
  computed: {
    options() {
      return this.orderBy(
        this.pokemonList
          .filter(({ id }) => id !== this.pokemon.id)
          .map(pokemon => ({
            value: pokemon.id,
            text:
              (pokemon.surname
                ? `${pokemon.surname} ${this.$i18n.t('pokemon.levelFormat', { level: pokemon.level })} (${pokemon.species.name})`
                : `${pokemon.species.name} ${this.$i18n.t('pokemon.levelFormat', { level: pokemon.level })}`) +
              ` [${
                pokemon.box
                  ? this.$i18n.t('pokemon.trainer.boxFormat', { box: pokemon.box, position: pokemon.position })
                  : this.$i18n.t('pokemon.trainer.partyFormat', { position: pokemon.position })
              }]`,
            sort: `${pokemon.box ?? 0}_${pokemon.position ?? 0}`
          })),
        'sort'
      )
    }
  },
  methods: {
    async reset() {
      this.otherPokemonId = null
      try {
        const { data } = await getPokemonList({ trainerId: this.trainerId })
        this.pokemonList = data.items
      } catch (e) {
        this.handleError(e)
      }
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          const { data } = await swapPokemon(this.pokemon.id, this.otherPokemonId)
          this.$emit('updated', data)
          this.toast('success', 'pokemon.trainer.swapped')
          if (typeof callback === 'function') {
            callback()
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
