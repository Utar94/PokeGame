<template>
  <span>
    <icon-button
      v-if="evolutions.length === 0"
      disabled
      icon="dna"
      text="pokemon.evolution.evolve"
      variant="warning"
      v-b-tooltip.hover
      :title="$t('pokemon.evolution.empty')"
    />
    <template v-else>
      <icon-button icon="dna" text="pokemon.evolution.evolve" variant="warning" v-b-modal.evolve />
      <b-modal id="evolve" size="lg" :title="$t('pokemon.evolution.title')" @hidden="reset" @show="loadInventory">
        <validation-observer ref="form">
          <b-form @submit.prevent="submit">
            <b-row>
              <form-select
                class="col"
                id="speciesId"
                label="species.select.label"
                :options="speciesOptions"
                placeholder="species.select.placeholder"
                required
                v-model="speciesId"
              />
              <form-select
                class="col"
                :disabled="speciesId === null || hasFixedAbility"
                id="abilityId"
                label="abilities.select.label"
                :options="abilityOptions"
                placeholder="abilities.select.placeholder"
                required
                v-model="abilityId"
              />
            </b-row>
            <template v-if="evolution">
              <b-row>
                <region-select v-if="evolution.region" class="col" required v-model="region" />
                <form-field
                  v-if="evolution.location"
                  class="col"
                  id="location"
                  label="species.evolutions.location.label"
                  :maxLength="100"
                  placeholder="species.evolutions.location.placeholder"
                  required
                  v-model="location"
                />
                <time-of-day-select v-if="evolution.timeOfDay" class="col" required v-model="timeOfDay" />
              </b-row>
              <h6 v-t="'species.evolutions.conditions'" />
              <table class="table table-striped">
                <thead>
                  <tr>
                    <th scope="col" v-t="'pokemon.evolution.condition'" />
                    <th scope="col" v-t="'pokemon.evolution.current'" />
                    <th scope="col" v-t="'pokemon.evolution.success'" />
                  </tr>
                </thead>
                <tbody>
                  <tr v-if="evolution.level">
                    <td>{{ $t('species.evolutions.levelFormat', { level: evolution.level }) }}</td>
                    <td v-text="pokemon.level" />
                    <td><evolution-condition :success="conditions.level" /></td>
                  </tr>
                  <tr v-if="evolution.item">
                    <template v-if="evolution.method === 'Item'">
                      <td>{{ $t('species.evolutions.itemFormat', { item: evolution.item.name }) }}</td>
                      <td>{{ $t('battle.quantityFormat', { quantity: inventory[evolution.item.id] || 0 }) }}</td>
                    </template>
                    <template v-else>
                      <td>{{ $t('species.evolutions.holdingItemFormat', { item: evolution.item.name }) }}</td>
                      <td>{{ pokemon.heldItem ? pokemon.heldItem.name : '—' }}</td>
                    </template>
                    <td><evolution-condition :success="conditions.item" /></td>
                  </tr>
                  <tr v-if="evolution.gender">
                    <td>{{ $t('species.evolutions.genderFormat', { gender: evolution.gender }) }}</td>
                    <td>{{ $t(`pokemon.gender.options.${pokemon.gender}`) }}</td>
                    <td><evolution-condition :success="conditions.gender" /></td>
                  </tr>
                  <tr v-if="evolution.region">
                    <td>{{ $t('species.evolutions.regionFormat', { region: evolution.region.name }) }}</td>
                    <td>{{ region ? region.name : '—' }}</td>
                    <td><evolution-condition :success="conditions.region" /></td>
                  </tr>
                  <tr v-if="evolution.location">
                    <td>{{ $t('species.evolutions.location.format', { location: evolution.location }) }}</td>
                    <td>{{ location || '—' }}</td>
                    <td><evolution-condition :success="conditions.location" /></td>
                  </tr>
                  <tr v-if="evolution.move">
                    <td>{{ $t('species.evolutions.moveFormat', { move: evolution.move.name }) }}</td>
                    <td>&mdash;</td>
                    <td><evolution-condition :success="conditions.move" /></td>
                  </tr>
                  <tr v-if="evolution.timeOfDay">
                    <td>{{ $t('species.evolutions.timeOfDay.format', { timeOfDay: $t(`species.evolutions.timeOfDay.options.${evolution.timeOfDay}`) }) }}</td>
                    <td>{{ timeOfDay ? $t(`species.evolutions.timeOfDay.options.${timeOfDay}`) : '—' }}</td>
                    <td><evolution-condition :success="conditions.timeOfDay" /></td>
                  </tr>
                  <tr v-if="evolution.highFriendship">
                    <td>{{ $t('species.evolutions.highFriendship') }}</td>
                    <td v-text="pokemon.friendship" />
                    <td><evolution-condition :success="conditions.highFriendship" /></td>
                  </tr>
                </tbody>
              </table>
              <template v-if="evolution.notes">
                <h6 v-t="'notes.label'" />
                <p v-text="evolution.notes" />
              </template>
            </template>
          </b-form>
        </validation-observer>
        <template #modal-footer="{ cancel, ok }">
          <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
          <icon-button :disabled="!canSubmit" icon="dna" :loading="loading" text="pokemon.evolution.evolve" variant="warning" @click="submit(ok)" />
        </template>
      </b-modal>
    </template>
  </span>
</template>

<script>
import EvolutionCondition from './EvolutionCondition.vue'
import RegionSelect from '@/components/Regions/RegionSelect.vue'
import TimeOfDaySelect from '@/components/Species/TimeOfDaySelect.vue'
import { evolvePokemon } from '@/api/pokemon'
import { getInventory } from '@/api/inventory'

export default {
  name: 'PokemonEvolution',
  components: {
    EvolutionCondition,
    RegionSelect,
    TimeOfDaySelect
  },
  props: {
    evolutions: {
      type: Array,
      required: true
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      abilityId: null,
      hasFixedAbility: false,
      inventory: {},
      loading: false,
      location: null,
      region: null,
      speciesId: null,
      timeOfDay: null
    }
  },
  computed: {
    abilityOptions() {
      return this.orderBy(this.evolution?.species.abilities.map(({ id, name }) => ({ value: id, text: name })) ?? [], 'text')
    },
    canSubmit() {
      return !this.loading && this.evolution && Object.values(this.conditions).every(value => value === true)
    },
    conditions() {
      return {
        gender: !this.evolution.gender || this.pokemon.gender === this.evolution.gender,
        highFriendship: !this.evolution.highFriendship || this.pokemon.friendship >= 220,
        item:
          !this.evolution.item ||
          (this.evolution.method === 'Item' ? Boolean(this.inventory[this.evolution.item.id]) : this.pokemon.heldItem?.id === this.evolution.item.id),
        level: !this.evolution.level || this.pokemon.level >= this.evolution.level,
        location: !this.evolution.location || this.location === this.evolution.location,
        move: !this.evolution.move || this.pokemon.moves.some(({ move }) => move.id === this.evolution.move.id),
        region: !this.evolution.region || this.region?.id === this.evolution.region.id,
        timeOfDay: !this.evolution.timeOfDay || this.timeOfDay === this.evolution.timeOfDay
      }
    },
    evolution() {
      return this.evolutions.find(({ species }) => species.id === this.speciesId) ?? null
    },
    payload() {
      return {
        speciesId: this.speciesId,
        abilityId: this.abilityId,
        location: this.evolution.location ? this.location : null,
        regionId: this.evolution.region ? this.region?.id ?? null : null,
        timeOfDay: this.evolution.timeOfDay ? this.timeOfDay : null
      }
    },
    speciesOptions() {
      return this.orderBy(
        this.evolutions.map(({ method, species }) => ({
          value: species.id,
          text: `${species.name} [${this.$i18n.t(`species.evolutions.method.options.${method}`)}]`
        })),
        'text'
      )
    }
  },
  methods: {
    async loadInventory() {
      try {
        const { data } = await getInventory(this.pokemon.history.trainer.id)
        this.inventory = Object.fromEntries(data.items.map(({ item, quantity }) => [item.id, quantity]))
      } catch (e) {
        this.handleError(e)
      }
    },
    reset() {
      this.location = null
      this.region = null
      this.speciesId = null
      this.timeOfDay = null
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await evolvePokemon(this.pokemon.id, this.payload)
            this.$emit('updated', data)
            this.toast('success', 'pokemon.evolution.evolved')
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
    speciesId() {
      const ability = this.evolution?.species.abilities.find(({ id }) => id === this.pokemon.ability.id)
      if (ability) {
        this.abilityId = ability.id
        this.hasFixedAbility = true
      } else {
        this.abilityId = null
        this.hasFixedAbility = false
      }
    }
  }
}
</script>
