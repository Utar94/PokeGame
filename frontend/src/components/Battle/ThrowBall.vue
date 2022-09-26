<template>
  <span>
    <icon-button :disabled="!battleLocation || hasEnded" icon="shopping-cart" text="battle.throwBall.label" variant="warning" v-b-modal="id" />
    <b-modal :id="id" :title="$t('battle.throwBall.label')" @hidden="reset" @show="loadInventory">
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
          >
            <template v-if="item" #prepend>
              <b-input-group-prepend><item-icon class="mx-1" :item="item" /></b-input-group-prepend>
            </template>
          </form-select>
          <form-select
            v-if="Boolean(item)"
            :disabled="pokemonOptions.length === 0"
            id="pokemon"
            label="pokemon.select.label"
            :options="pokemonOptions"
            placeholder="pokemon.select.placeholder"
            required
            v-model="pokemonId"
          >
            <template v-if="pokemon" #prepend>
              <b-input-group-prepend><pokemon-icon class="mx-1" :pokemon="pokemon" /></b-input-group-prepend>
            </template>
          </form-select>
          <template v-if="item && pokemon">
            <ball-modifier-field v-model="ballModifier">
              <b-input-group-append>
                <b-input-group-text>{{ $t('dcFormat', { dc: catchDC }) }}</b-input-group-text>
              </b-input-group-append>
              <template #after>
                <b-form-checkbox id="catchSuccess" v-model="catchSuccess">{{ $t('battle.throwBall.catchSuccess') }}</b-form-checkbox>
              </template>
            </ball-modifier-field>
            <template v-if="catchSuccess">
              <form-field id="friendship" label="pokemon.friendship" :minValue="0" :maxValue="255" :step="1" type="number" v-model.number="friendship" />
              <name-field v-if="catchSuccess" id="surname" label="pokemon.surname.label" placeholder="pokemon.surname.placeholder" v-model="surname" />
              <form-field
                id="restoreHitPoints"
                label="battle.healing.restoreHitPoints"
                :minValue="0"
                :maxValue="999"
                :step="1"
                type="number"
                v-model.number="restoreHitPoints"
              >
                <template #after>
                  <b-form-checkbox v-model="restoreAllPowerPoints">{{ $t('battle.healing.restoreAllPowerPoints') }}</b-form-checkbox>
                </template>
              </form-field>
              <condition-select :disabled="removeAllConditions" label="battle.healing.removeStatusCondition" v-model="statusCondition">
                <template #after>
                  <b-form-checkbox v-model="removeAllConditions">{{ $t('battle.healing.removeAllConditions') }}</b-form-checkbox>
                </template>
              </condition-select>
            </template>
          </template>
        </b-form>
      </validation-observer>
      <template #modal-footer="{ cancel, ok }">
        <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
        <icon-button :disabled="loading" icon="shopping-cart" :loading="loading" text="battle.throwBall.label" variant="warning" @click="submit(ok)" />
      </template>
    </b-modal>
  </span>
</template>

<script>
import { mapActions, mapGetters, mapState } from 'vuex'
import BallModifierField from '@/components/Items/BallModifierField.vue'
import ConditionSelect from '@/components/Pokemon/ConditionSelect.vue'
import ItemIcon from '@/components/Items/ItemIcon.vue'
import { catchPokemon } from '@/api/pokemon'
import { getInventory, removeInventory } from '@/api/inventory'

export default {
  name: 'ThrowBall',
  components: {
    BallModifierField,
    ConditionSelect,
    ItemIcon
  },
  props: {
    trainer: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      ballModifier: 1,
      catchSuccess: false,
      friendship: 0,
      inventory: [],
      itemId: null,
      loading: false,
      pokemonId: null,
      removeAllConditions: false,
      restoreAllPowerPoints: false,
      restoreHitPoints: 0,
      statusCondition: null,
      surname: null
    }
  },
  computed: {
    ...mapGetters(['activeBattlingOpponentPokemon', 'activeBattlingPlayerPokemon', 'battleLocation', 'hasEnded']),
    ...mapState(['pokemonList']),
    applyHealing() {
      return this.restoreHitPoints > 0 || this.removeAllConditions || this.statusCondition
    },
    catchDC() {
      const pokemon = this.pokemonList[this.pokemonId]
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
    id() {
      return `throwBall_${this.trainer.id}`
    },
    item() {
      if (!this.itemId) {
        return null
      }
      return this.inventory.find(({ item }) => item.id === this.itemId)?.item ?? null
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
    pokemon() {
      return this.pokemonId ? this.activeBattlingOpponentPokemon.find(({ id }) => id === this.pokemonId) ?? null : null
    },
    pokemonOptions() {
      return this.orderBy(
        this.activeBattlingOpponentPokemon.map(({ history, id, level, species, surname }) => ({
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
    ...mapActions(['distributeExperience', 'toggleBattlingOpponentPokemon', 'toggleBattlingPlayerPokemon', 'updatePokemon']),
    async loadInventory() {
      try {
        const { data } = await getInventory(this.trainer.id)
        this.inventory = data.items.filter(({ item }) => item.category === 'PokeBall')
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
            if (this.catchSuccess) {
              const { data } = await catchPokemon(this.pokemonId, {
                heal: this.applyHealing
                  ? {
                      restoreAllPowerPoints: this.restoreAllPowerPoints,
                      restoreHitPoints: this.restoreHitPoints,
                      removeAllConditions: this.removeAllConditions,
                      statusCondition: this.statusCondition
                    }
                  : null,
                ballId: this.itemId,
                friendship: this.friendship || null,
                surname: this.surname,
                location: this.battleLocation,
                trainerId: this.trainer.id
              })
              this.updatePokemon(data)
              this.toggleBattlingOpponentPokemon(data.id)
              this.distributeExperience([data])
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
    catchSuccess() {
      this.friendship = this.item?.name === 'Friend Ball' ? 200 : 0
      this.surname = null
      if (this.item?.name === 'Heal Ball') {
        this.removeAllConditions = true
        this.restoreAllPowerPoints = true
        this.restoreHitPoints = 999
        this.statusCondition = null
      } else {
        this.removeAllConditions = false
        this.restoreAllPowerPoints = false
        this.restoreHitPoints = 0
        this.statusCondition = null
      }
    },
    itemId() {
      this.ballModifier = this.item?.defaultModifier || 1
      this.catchSuccess = false
    },
    removeAllConditions(value) {
      if (value) {
        this.statusCondition = null
      }
    }
  }
}
</script>
