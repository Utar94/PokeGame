<template>
  <b-modal :id="id" :title="$t('battle.useItem.title')" @hidden="reset">
    <p v-if="displayLocationWarning"><i class="text-warning" v-t="'battle.useItem.locationWarning'" /></p>
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <form-select
          :disabled="Boolean(item)"
          id="item"
          label="items.select.label"
          :options="itemOptions"
          placeholder="items.select.placeholder"
          required
          :value="item ? item.id : null"
          @input="onInput"
        />
        <form-select
          v-if="item"
          id="pokemon"
          label="pokemon.select.label"
          :options="pokemonOptions"
          placeholder="pokemon.select.placeholder"
          v-model="pokemonId"
        />
        <template v-if="pokemonId">
          <form-field id="restoreHitPoints" label="battle.useItem.restoreHitPoints" :minValue="0" :step="1" type="number" v-model.number="restoreHitPoints" />
          <condition-select :disabled="removeAllConditions" label="battle.useItem.removeStatusCondition" v-model="statusCondition">
            <template #after>
              <b-form-checkbox v-model="removeAllConditions">{{ $t('battle.useItem.removeAllConditions') }}</b-form-checkbox>
            </template>
          </condition-select>
          <ball-modifier-field v-if="item.category === 'PokeBall'" v-model="ballModifier">
            <b-input-group-append>
              <b-input-group-text>{{ $t('dcFormat', { dc: catchDC }) }}</b-input-group-text>
            </b-input-group-append>
            <template #after>
              <b-form-checkbox id="catchSuccess" v-model="catchSuccess">{{ $t('battle.useItem.catchSuccess') }}</b-form-checkbox>
            </template>
          </ball-modifier-field>
          <name-field v-if="catchSuccess" id="surname" label="pokemon.surname.label" placeholder="pokemon.surname.placeholder" v-model="surname" />
        </template>
      </b-form>
    </validation-observer>
    <slot />
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button
        :disabled="!hasChanges || loading"
        icon="shopping-cart"
        :loading="loading"
        :text="item && item.category === 'PokeBall' ? 'battle.useItem.throwBall' : 'battle.useItem.title'"
        variant="warning"
        @click="submit(ok)"
      />
    </template>
  </b-modal>
</template>

<script>
import Vue from 'vue'
import { mapState } from 'vuex'
import BallModifierField from '@/components/Items/BallModifierField.vue'
import ConditionSelect from '@/components/Pokemon/ConditionSelect.vue'
import { getInventory, removeInventory } from '@/api/inventory'
import { catchPokemon, healPokemon } from '@/api/pokemon'

export default {
  name: 'UseItemModal',
  components: {
    BallModifierField,
    ConditionSelect
  },
  props: {
    id: {
      type: String,
      default: 'useItem'
    },
    opponents: {
      type: Array,
      default: () => []
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
      ballModifier: 1,
      catchSuccess: false,
      item: null,
      items: {},
      loading: false,
      pokemonId: null,
      removeAllConditions: false,
      restoreHitPoints: 0,
      statusCondition: null,
      surname: null
    }
  },
  computed: {
    ...mapState(['battle']),
    applyHealing() {
      return this.restoreHitPoints > 0 || this.removeAllConditions || this.statusCondition
    },
    categories() {
      const categories = []
      if (this.pokemon.length) {
        categories.push('BattleItem')
        categories.push('Berry')
        categories.push('Medicine')
      }
      if (this.opponents.length) {
        categories.push('PokeBall')
      }
      return categories
    },
    catchDC() {
      const pokemon = this.selectedPokemon
      if (!pokemon) {
        return 0
      }
      let status = 1.0
      switch (pokemon.statusCondition) {
        case 'Freeze':
        case 'Sleep':
          status = 2.0
          break
        case 'Burn':
        case 'Paralysis':
        case 'Poison':
          status = 1.5
          break
      }
      const a = Math.floor(
        (((3 * pokemon.maximumHitPoints - 2 * pokemon.currentHitPoints) * pokemon.species.catchRate * this.ballModifier) / (3 * pokemon.maximumHitPoints)) *
          status
      )
      const b = Math.floor(1048560 / Math.floor(Math.sqrt(Math.floor(Math.sqrt(Math.floor(16711680 / a))))))
      const DC = Math.ceil((1 - b / 65535) * 20) + 5
      return DC < 0 ? 0 : DC
    },
    displayLocationWarning() {
      return !this.battle.location && !this.battle.opponents.trainers.length
    },
    hasChanges() {
      return this.item || this.pokemonId
    },
    itemOptions() {
      return Object.values(this.items)
        .filter(({ category }) => this.categories.includes(category))
        .map(({ id, name }) => ({ text: name, value: id }))
    },
    pokemonOptions() {
      return this.orderBy(
        (this.item?.category === 'PokeBall' ? this.opponents : this.pokemon)
          .filter(x => Boolean(x))
          .map(({ id, level, species, surname }) => ({
            text: surname
              ? `${surname}, ${this.$i18n.t('pokemon.levelFormat', { level })} ${species.name}`
              : `${species.name} ${this.$i18n.t('pokemon.levelFormat', { level })}`,
            value: id
          })),
        'text'
      )
    },
    selectedPokemon() {
      return (this.item?.category === 'PokeBall' ? this.opponents : this.pokemon).find(({ id }) => id === this.pokemonId)
    }
  },
  methods: {
    onInput($event) {
      this.item = this.items[$event]
    },
    reset() {
      this.item = null
      this.pokemonId = null
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            await removeInventory(this.trainerId, this.item.id, 1)
            if (this.catchSuccess) {
              const { data } = await catchPokemon(this.pokemonId, {
                heal: this.applyHealing
                  ? {
                      restoreHitPoints: this.restoreHitPoints,
                      removeAllConditions: this.removeAllConditions,
                      statusCondition: this.statusCondition
                    }
                  : null,
                surname: this.surname,
                location: this.battle.location,
                trainerId: this.trainerId
              })
              this.$emit('pokemonCaught', data)
            } else if (this.applyHealing) {
              const { data } = await healPokemon(this.pokemonId, {
                restoreHitPoints: this.restoreHitPoints,
                removeAllConditions: this.removeAllConditions,
                statusCondition: this.statusCondition
              })
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
    catchSuccess() {
      this.surname = null
    },
    item(newItem, oldValue) {
      if (!newItem || newItem?.category !== oldValue?.category) {
        this.pokemonId = null
      }
      if (newItem?.category === 'PokeBall') {
        this.ballModifier = newItem.defaultModifier ?? 1
      }
    },
    pokemonId() {
      this.restoreHitPoints = 0
      this.statusCondition = null
      this.removeAllConditions = false
      this.ballModifier = 1
      this.surname = null
    },
    removeAllConditions(value) {
      if (value) {
        this.statusCondition = null
      }
    },
    trainerId: {
      immediate: true,
      async handler(trainerId) {
        try {
          const { data } = await getInventory(trainerId, { sort: 'ItemName', desc: false })
          this.items = {}
          for (const value of data.items) {
            Vue.set(this.items, value.item.id, value.item)
          }
        } catch (e) {
          this.handleError(e)
        }
      }
    }
  }
}
</script>
