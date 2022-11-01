<template>
  <b-modal :id="id" size="lg" :title="$t(`species.evolutions.${evolution ? 'edit' : 'add'}`)">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-row>
          <species-select v-if="evolution" class="col" disabled :value="speciesId" />
          <species-select v-else class="col" :exclude="exclude" required v-model="speciesId" />
          <method-select class="col" required v-model="method" />
          <form-field
            v-if="method === 'LevelUp'"
            class="col"
            id="level"
            label="species.evolutions.atLevel"
            :minValue="0"
            :maxValue="100"
            :step="1"
            type="number"
            v-model.number="level"
          />
        </b-row>
        <b-row>
          <item-select class="col" :label="itemLabel" :required="method === 'Item'" v-model="itemId" />
          <gender-select v-if="method !== 'Trade'" class="col" v-model="gender" />
          <region-select v-if="method !== 'Trade'" class="col" v-model="region" />
        </b-row>
        <template v-if="method === 'LevelUp'">
          <b-row>
            <form-field
              class="col"
              id="location"
              label="species.evolutions.location.label"
              :maxLength="100"
              placeholder="species.evolutions.location.placeholder"
              v-model="location"
            />
            <move-select class="col" v-model="move" />
            <time-of-day-select class="col" v-model="timeOfDay" />
          </b-row>
          <b-form-group>
            <b-form-checkbox v-model="highFriendship">{{ $t('species.evolutions.highFriendship') }}</b-form-checkbox>
          </b-form-group>
        </template>
        <notes-field :rows="10" v-model="notes" />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="reset(cancel)" />
      <icon-button v-if="evolution" :disabled="!canSubmit" icon="save" :loading="loading" text="actions.save" variant="primary" @click="submit(ok)" />
      <icon-button v-else :disabled="!canSubmit" icon="plus" :loading="loading" text="species.evolutions.add" variant="success" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import Vue from 'vue'
import GenderSelect from '@/components/Pokemon/GenderSelect.vue'
import ItemSelect from '@/components/Items/ItemSelect.vue'
import MethodSelect from './MethodSelect.vue'
import MoveSelect from '@/components/Moves/MoveSelect.vue'
import RegionSelect from '@/components/Regions/RegionSelect.vue'
import SpeciesSelect from '@/components/Species/SpeciesSelect.vue'
import TimeOfDaySelect from './TimeOfDaySelect.vue'
import { saveSpeciesEvolution } from '@/api/species'

export default {
  name: 'EvolutionModal',
  components: {
    GenderSelect,
    ItemSelect,
    MethodSelect,
    MoveSelect,
    RegionSelect,
    SpeciesSelect,
    TimeOfDaySelect
  },
  props: {
    evolution: {
      type: Object,
      default: null
    },
    evolvingId: {
      type: String,
      required: true
    },
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      default: 'evolutionModal'
    }
  },
  data() {
    return {
      gender: null,
      highFriendship: false,
      itemId: null,
      level: 0,
      loading: false,
      location: null,
      method: 'LevelUp',
      move: null,
      notes: null,
      region: null,
      speciesId: null,
      timeOfDay: null
    }
  },
  computed: {
    canSubmit() {
      return this.speciesId && !this.loading
    },
    itemLabel() {
      return this.method === 'Item' ? 'items.select.label' : 'species.evolutions.holdingItem'
    },
    payload() {
      return {
        method: this.method,
        gender: this.gender,
        highFriendship: this.highFriendship,
        itemId: this.itemId,
        level: this.level,
        location: this.location,
        moveId: this.move?.id ?? null,
        regionId: this.region?.id ?? null,
        timeOfDay: this.timeOfDay,
        notes: this.notes
      }
    }
  },
  methods: {
    reset(callback = null) {
      if (this.evolution) {
        this.setModel(this.evolution)
      } else {
        this.speciesId = null
        if (this.method === 'LevelUp') {
          this.level = 0
          this.itemId = null
          this.gender = null
          this.region = null
          this.location = null
          this.move = null
          this.timeOfDay = null
          this.highFriendship = false
        } else {
          this.method = 'LevelUp'
        }
        this.notes = null
      }
      if (typeof callback === 'function') {
        callback()
      }
    },
    setModel({ species, method, level, item, gender, region, location, move, timeOfDay, highFriendship, notes }) {
      this.speciesId = species.id
      this.method = method
      this.notes = notes

      Vue.nextTick(() => {
        this.level = level
        this.itemId = item?.id ?? null
        this.gender = gender
        this.region = region
        this.location = location
        this.move = move
        this.timeOfDay = timeOfDay
        this.highFriendship = highFriendship
      })
    },
    async submit(callback = null) {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await saveSpeciesEvolution(this.evolvingId, this.speciesId, this.payload)
            this.$emit('saved', data)
            this.toast('success', 'species.evolutions.saved')
            this.$refs.form.reset()
            this.reset(callback)
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
    evolution: {
      deep: true,
      immediate: true,
      handler() {
        this.reset()
      }
    },
    method: {
      immediate: true,
      handler() {
        this.level = 0
        this.itemId = null
        this.gender = null
        this.region = null
        this.location = null
        this.move = null
        this.timeOfDay = null
        this.highFriendship = false
      }
    }
  }
}
</script>
