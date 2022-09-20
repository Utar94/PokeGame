<template>
  <span>
    <icon-button :disabled="hasEnded" icon="shopping-cart" text="battle.useItem" variant="warning" v-b-modal="id" />
    <b-modal :id="id" :title="$t('battle.useItem')" @hidden="reset" @show="loadInventory">
      <validation-observer ref="form">
        <b-form @submit.prevent="submit">
          <form-select
            :disabled="itemOptions.length === 0"
            id="item"
            label="items.select.label"
            :options="itemOptions"
            placeholder="items.select.placeholder"
            required
            v-model="itemId"
          />
          <form-select
            v-if="Boolean(itemId)"
            :disabled="pokemonOptions.length === 0"
            id="pokemon"
            label="pokemon.select.label"
            :options="pokemonOptions"
            placeholder="pokemon.select.placeholder"
            required
            v-model="pokemonId"
          />
          <template v-if="itemId && pokemonId">
            <form-field id="restoreHitPoints" label="battle.healing.restoreHitPoints" :minValue="0" :step="1" type="number" v-model.number="restoreHitPoints" />
            <condition-select :disabled="removeAllConditions" label="battle.healing.removeStatusCondition" v-model="statusCondition">
              <template #after>
                <b-form-checkbox v-model="removeAllConditions">{{ $t('battle.healing.removeAllConditions') }}</b-form-checkbox>
              </template>
            </condition-select>
          </template>
        </b-form>
      </validation-observer>
      <template #modal-footer="{ cancel, ok }">
        <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
        <icon-button :disabled="loading" icon="shopping-cart" :loading="loading" text="battle.useItem" variant="warning" @click="submit(ok)" />
      </template>
    </b-modal>
  </span>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import ConditionSelect from '@/components/Pokemon/ConditionSelect.vue'
import { getInventory, removeInventory } from '@/api/inventory'
import { healPokemon } from '@/api/pokemon'

export default {
  name: 'UseItem',
  components: {
    ConditionSelect
  },
  props: {
    team: {
      type: String,
      required: true
    },
    trainer: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      inventory: [],
      itemId: null,
      loading: false,
      pokemonId: null,
      removeAllConditions: false,
      restoreHitPoints: 0,
      statusCondition: null
    }
  },
  computed: {
    ...mapGetters(['battlingOpponentPokemon', 'battlingPlayerPokemon', 'hasEnded']),
    applyHealing() {
      return this.restoreHitPoints > 0 || this.removeAllConditions || this.statusCondition
    },
    id() {
      return `useItem_${this.trainer.id}`
    },
    itemOptions() {
      return this.orderBy(
        this.inventory.map(({ item, quantity }) => ({
          text: `${item.name} [${this.$i18n.t(`items.category.options.${item.category}`)}] (${this.$i18n.t('battle.quantityFormat', { quantity })})`,
          value: item.id
        })),
        'text'
      )
    },
    pokemonOptions() {
      const pokemonList = this.team === 'players' ? this.battlingPlayerPokemon : this.battlingOpponentPokemon
      return this.orderBy(
        pokemonList.map(({ history, id, level, species, surname }) => ({
          text:
            (surname
              ? `${surname} ${this.$i18n.t('pokemon.levelFormat', { level })} [${species.name}]`
              : `${species.name} ${this.$i18n.t('pokemon.levelFormat', { level })}`) + ` (${history?.trainer.name ?? this.$i18n.t('pokemon.wild')})`,
          value: id
        })),
        'text'
      )
    }
  },
  methods: {
    ...mapActions(['updatePokemon']),
    async loadInventory() {
      try {
        const { data } = await getInventory(this.trainer.id)
        this.inventory = data.items.filter(({ item }) => item.category === 'BattleItem' || item.category === 'Berry' || item.category === 'Medicine')
        this.itemId = this.itemOptions[0]?.value ?? null
        this.pokemonId = this.pokemonOptions[0]?.value ?? null
      } catch (e) {
        this.handleError(e)
      }
    },
    reset() {
      this.itemId = null
      this.pokemonId = null
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            await removeInventory(this.trainer.id, this.itemId, 1)
            if (this.applyHealing) {
              const { data } = await healPokemon(this.pokemonId, {
                restoreHitPoints: this.restoreHitPoints,
                removeAllConditions: this.removeAllConditions,
                statusCondition: this.statusCondition
              })
              this.updatePokemon(data)
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
  },
  watch: {
    itemId() {
      this.removeAllConditions = false
      this.restoreHitPoints = 0
      this.statusCondition = null
    },
    removeAllConditions(value) {
      if (value) {
        this.statusCondition = null
      }
    }
  }
}
</script>
