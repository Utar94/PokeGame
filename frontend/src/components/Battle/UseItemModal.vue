<template>
  <b-modal :id="id" :title="$t('battle.useItem')" @hidden="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <form-select id="item" label="items.select.label" :options="itemOptions" placeholder="items.select.placeholder" required v-model="itemId" />
        <form-select id="pokemon" label="pokemon.select.label" :options="pokemonOptions" placeholder="pokemon.select.placeholder" v-model="pokemonId" />
        <template v-if="pokemonId">
          <form-field id="restoreHitPoints" label="battle.restoreHitPoints" :minValue="0" :step="1" type="number" v-model.number="restoreHitPoints" />
          <b-form-checkbox v-model="removeCondition">{{ $t('battle.removeCondition') }}</b-form-checkbox>
        </template>
      </b-form>
    </validation-observer>
    <slot />
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button :disabled="!hasChanges || loading" icon="shopping-cart" :loading="loading" text="battle.useItem" variant="primary" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import { getInventory, removeInventory } from '@/api/inventory'
import { healPokemon } from '@/api/pokemon'

export default {
  name: 'UseItemModal',
  props: {
    id: {
      type: String,
      default: 'useItem'
    },
    pokemon: {
      type: Array,
      default: () => []
    },
    trainerId: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      itemId: null,
      items: [],
      loading: false,
      pokemonId: null,
      removeCondition: false,
      restoreHitPoints: 0
    }
  },
  computed: {
    hasChanges() {
      return this.itemId || this.pokemonId
    },
    itemOptions() {
      return this.items.map(({ item }) => ({ text: item.name, value: item.id }))
    },
    pokemonOptions() {
      return this.orderBy(
        this.pokemon
          .filter(x => Boolean(x))
          .map(({ id, level, species, surname }) => ({
            text: surname
              ? `${surname}, ${this.$i18n.t('pokemon.levelFormat', { level })} ${species.name}`
              : `${species.name} ${this.$i18n.t('pokemon.levelFormat', { level })}`,
            value: id
          })),
        'text'
      )
    }
  },
  methods: {
    reset() {
      this.itemId = null
      this.pokemonId = null
      this.restoreHitPoints = 0
      this.removeCondition = false
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            await removeInventory(this.trainerId, this.itemId, 1)
            if (this.restoreHitPoints > 0 || this.removeCondition) {
              const { data } = await healPokemon(this.pokemonId, { restoreHitPoints: this.restoreHitPoints, removeCondition: this.removeCondition })
              this.$emit('pokemonUpdated', data)
            }
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
    pokemonId() {
      this.restoreHitPoints = 0
      this.removeCondition = false
    },
    trainerId: {
      immediate: true,
      async handler(trainerId) {
        try {
          const { data } = await getInventory(trainerId, { sort: 'ItemName', desc: false })
          this.items = data.items
        } catch (e) {
          this.handleError(e)
        }
      }
    }
  }
}
</script>
