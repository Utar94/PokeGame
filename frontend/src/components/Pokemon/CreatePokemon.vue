<template>
  <b-container>
    <h1 v-t="'pokemon.newTitle'" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-row>
          <species-select class="col" required v-model="speciesId" />
          <form-select
            class="col"
            :disabled="!species"
            id="ability"
            label="abilities.select.label"
            :options="abilityOptions"
            placeholder="abilities.select.placeholder"
            required
            v-model="abilityId"
          >
            <b-input-group-append>
              <icon-button :disabled="!species || species.abilities.length < 2" icon="dice" variant="primary" @click="randomAbility" />
            </b-input-group-append>
          </form-select>
        </b-row>
        <b-row>
          <form-field class="col" id="level" label="pokemon.level" :minValue="1" :maxValue="100" required :step="1" type="number" v-model.number="level" />
          <gender-select class="col" :disabled="!species || isFixedGender" :exclude="excludedGenders" required v-model="gender">
            <b-input-group-append>
              <icon-button :disabled="!species || isFixedGender" icon="dice" variant="primary" @click="randomGender" />
            </b-input-group-append>
          </gender-select>
        </b-row>
        <b-row>
          <item-select class="col" id="heldItem" label="pokemon.heldItem.label" v-model="heldItemId" />
          <form-select
            class="col"
            id="nature"
            label="pokemon.nature.label"
            :options="natureOptions"
            placeholder="pokemon.nature.placeholder"
            required
            v-model="nature"
          >
            <b-input-group-append>
              <icon-button icon="dice" variant="primary" @click="randomNature" />
            </b-input-group-append>
          </form-select>
        </b-row>
        <b-row>
          <name-field class="col" id="surname" label="pokemon.surname.label" placeholder="pokemon.surname.placeholder" v-model="surname" />
          <form-field
            class="col"
            id="friendship"
            label="pokemon.friendship"
            :minValue="0"
            :maxValue="255"
            :step="1"
            type="number"
            v-model.number="friendship"
          />
          <form-field
            class="col"
            :disabled="level > 1 || Boolean(heldItemId)"
            id="remainingHatchSteps"
            label="pokemon.remainingHatchSteps"
            :minValue="0"
            :maxValue="65535"
            required
            :step="1"
            type="number"
            v-model.number="remainingHatchSteps"
          />
        </b-row>
        <h3 v-t="'pokemon.individualValues.title'" />
        <b-row>
          <form-field
            class="col"
            id="hpIV"
            label="statistic.options.HP"
            :minValue="0"
            :maxValue="31"
            required
            :step="1"
            type="number"
            v-model.number="individualValues.HP"
          >
            <b-input-group-append>
              <icon-button icon="dice" variant="primary" @click="randomIV('HP')" />
            </b-input-group-append>
          </form-field>
          <form-field
            class="col"
            id="attackIV"
            label="statistic.options.Attack"
            :minValue="0"
            :maxValue="31"
            required
            :step="1"
            type="number"
            v-model.number="individualValues.Attack"
          >
            <b-input-group-append> <icon-button icon="dice" variant="primary" @click="randomIV('Attack')" /> </b-input-group-append>
          </form-field>
          <form-field
            class="col"
            id="defenseIV"
            label="statistic.options.Defense"
            :minValue="0"
            :maxValue="31"
            required
            :step="1"
            type="number"
            v-model.number="individualValues.Defense"
          >
            <b-input-group-append> <icon-button icon="dice" variant="primary" @click="randomIV('Defense')" /> </b-input-group-append>
          </form-field>
          <form-field
            class="col"
            id="specialAttackIV"
            label="statistic.options.SpecialAttack"
            :minValue="0"
            :maxValue="31"
            required
            :step="1"
            type="number"
            v-model.number="individualValues.SpecialAttack"
          >
            <b-input-group-append> <icon-button icon="dice" variant="primary" @click="randomIV('SpecialAttack')" /> </b-input-group-append>
          </form-field>
          <form-field
            class="col"
            id="specialDefenseIV"
            label="statistic.options.SpecialDefense"
            :minValue="0"
            :maxValue="31"
            required
            :step="1"
            type="number"
            v-model.number="individualValues.SpecialDefense"
          >
            <b-input-group-append> <icon-button icon="dice" variant="primary" @click="randomIV('SpecialDefense')" /> </b-input-group-append>
          </form-field>
          <form-field
            class="col"
            id="speedIV"
            label="statistic.options.Speed"
            :minValue="0"
            :maxValue="31"
            required
            :step="1"
            type="number"
            v-model.number="individualValues.Speed"
          >
            <b-input-group-append> <icon-button icon="dice" variant="primary" @click="randomIV('Speed')" /> </b-input-group-append>
          </form-field>
        </b-row>
        <icon-button icon="dice" text="actions.randomize" variant="warning" @click="randomIV()" />
        <p>
          <strong>{{ $t('pokemon.individualValues.totalFormat', { total: totalIV }) }}</strong>
        </p>
        <h3 v-t="'pokemon.trainer.title'" />
        <b-alert dismissible variant="warning" v-model="positionAlreadyUsed"><strong v-t="'pokemon.trainer.positionAlreadyUsed'" /></b-alert>
        <b-row>
          <trainer-select class="col" v-model="trainer" />
          <form-field
            class="col"
            :disabled="!trainer || inParty"
            id="box"
            label="pokemon.trainer.box"
            :minValue="1"
            :maxValue="32"
            :step="1"
            type="number"
            v-model.number="box"
          >
            <template #after>
              <b-form-checkbox :disabled="!trainer" v-model="inParty">{{ $t('pokemon.trainer.party') }}</b-form-checkbox>
            </template>
          </form-field>
          <form-field
            class="col"
            :disabled="!trainer"
            id="position"
            label="pokemon.trainer.position"
            :minValue="1"
            :maxValue="inParty ? 6 : 30"
            :step="1"
            type="number"
            v-model.number="position"
          />
        </b-row>
        <h4 v-t="'pokemon.trainer.history'" />
        <b-row>
          <item-select
            category="PokeBall"
            class="col"
            :disabled="!trainer"
            id="ballId"
            label="pokemon.caughtWithBall"
            :required="Boolean(trainer)"
            v-model="ballId"
          />
          <form-field
            class="col"
            :disabled="!trainer"
            id="metLevel"
            label="pokemon.trainer.metLevel"
            :minValue="1"
            :maxValue="level"
            :required="Boolean(trainer)"
            :step="1"
            type="number"
            v-model.number="metLevel"
          />
          <form-field
            class="col"
            :disabled="!trainer"
            id="metLocation"
            label="pokemon.trainer.metLocation.label"
            :maxLength="100"
            placeholder="pokemon.trainer.metLocation.placeholder"
            :required="Boolean(trainer)"
            v-model="metLocation"
          />
          <form-datetime class="col" :disabled="!trainer" id="metOn" label="pokemon.trainer.metOn" :required="Boolean(trainer)" v-model="metOn" />
        </b-row>
        <h3 v-t="'pokemon.moves.title'" />
        <b-row>
          <move-select class="col-6" :disabled="moves.length === 4" :exclude="moveIds" v-model="move">
            <b-input-group-append>
              <icon-button :disabled="!move" icon="plus" text="pokemon.moves.add" variant="success" @click="addMove" />
            </b-input-group-append>
          </move-select>
        </b-row>
        <table v-if="moves.length" class="table table-striped">
          <thead>
            <tr>
              <th scope="col" />
              <th scope="col" v-t="'name.label'" />
              <th scope="col" v-t="'type.label'" />
              <th scope="col" v-t="'moves.category.label'" />
              <th scope="col" v-t="'moves.powerPoints.label'" />
              <th scope="col" />
            </tr>
          </thead>
          <tbody>
            <tr v-for="(move, index) in moves" :key="move.id">
              <td>
                <icon-button class="mx-1" :disabled="index === 0" icon="arrow-up" variant="primary" @click="swapMoves(index, index - 1)" />
                <icon-button class="mx-1" :disabled="index === moves.length - 1" icon="arrow-down" variant="primary" @click="swapMoves(index, index + 1)" />
              </td>
              <td v-text="move.name" />
              <td>{{ $t(`type.options.${move.type}`) }}</td>
              <td>{{ $t(`moves.category.options.${move.category}`) }}</td>
              <td v-text="move.powerPoints" />
              <td><icon-button icon="times" variant="danger" @click="removeMove(index)" /></td>
            </tr>
          </tbody>
        </table>
        <div class="my-2">
          <icon-submit :disabled="!species || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import Vue from 'vue'
import GenderSelect from './GenderSelect.vue'
import ItemSelect from '@/components/Items/ItemSelect.vue'
import MoveSelect from '@/components/Moves/MoveSelect.vue'
import SpeciesSelect from '@/components/Species/SpeciesSelect.vue'
import TrainerSelect from '@/components/Trainers/TrainerSelect.vue'
import { createPokemon } from '@/api/pokemon'
import { getSpecies } from '@/api/species'

export default {
  name: 'CreatePokemon',
  components: {
    GenderSelect,
    ItemSelect,
    MoveSelect,
    SpeciesSelect,
    TrainerSelect
  },
  data() {
    return {
      abilityId: null,
      ballId: null,
      box: 1,
      friendship: 0,
      gender: null,
      heldItemId: null,
      individualValues: {
        Attack: 0,
        Defense: 0,
        HP: 0,
        SpecialAttack: 0,
        SpecialDefense: 0,
        Speed: 0
      },
      inParty: true,
      level: 1,
      loading: false,
      metLevel: 1,
      metLocation: null,
      metOn: new Date(),
      move: null,
      moves: [],
      nature: null,
      position: 1,
      positionAlreadyUsed: false,
      remainingHatchSteps: 0,
      species: null,
      speciesId: null,
      surname: null,
      trainer: null
    }
  },
  computed: {
    abilityOptions() {
      return this.orderBy(this.species?.abilities.map(({ id, name }) => ({ text: name, value: id })) ?? [], 'text')
    },
    excludedGenders() {
      return this.species?.genderRatio === null ? ['Male', 'Female'] : ['Unknown']
    },
    isFixedGender() {
      return !this.species || !this.species.genderRatio || this.species.genderRatio === 100
    },
    moveIds() {
      return this.moves.map(({ id }) => id)
    },
    natureOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('pokemon.nature.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    },
    payload() {
      return {
        speciesId: this.speciesId,
        abilityId: this.abilityId,
        level: this.level,
        gender: this.gender,
        nature: this.nature,
        surname: this.surname,
        friendship: this.friendship || null,
        remainingHatchSteps: this.remainingHatchSteps,
        individualValues: Object.entries(this.individualValues)
          .filter(([, value]) => value !== 0)
          .map(([statistic, value]) => ({ statistic, value })),
        moves: this.moves.map(({ id, powerPoints }, position) => ({
          moveId: id,
          position,
          remainingPowerPoints: powerPoints
        })),
        heldItemId: this.heldItemId,
        history: this.trainer
          ? {
              ballId: this.ballId,
              level: this.metLevel,
              location: this.metLocation,
              metOn: this.metOn,
              trainerId: this.trainer.id
            }
          : null,
        position: this.trainer ? this.position : null,
        box: this.trainer && !this.inParty ? this.box : null
      }
    },
    totalIV() {
      return Object.values(this.individualValues).reduce((a, b) => a + (b || 0), 0)
    }
  },
  methods: {
    addMove() {
      this.moves.push(this.move)
      this.move = null
    },
    randomAbility() {
      const index = Math.floor(Math.random() * this.species.abilities.length)
      this.abilityId = this.species.abilities[index].id
    },
    randomGender() {
      if (this.species.genderRatio === null) {
        this.gender = 'Unknown'
        return
      }
      const genderRatio = this.species.genderRatio / 100
      const value = Math.random()
      this.gender = value < genderRatio ? 'Male' : 'Female'
    },
    randomIV(statistic = null) {
      if (statistic) {
        Vue.set(this.individualValues, statistic, Math.floor(Math.random() * 32))
      } else {
        this.randomIV('HP')
        this.randomIV('Attack')
        this.randomIV('Defense')
        this.randomIV('SpecialAttack')
        this.randomIV('SpecialDefense')
        this.randomIV('Speed')
      }
    },
    randomNature() {
      const value = Math.floor(Math.random() * this.natureOptions.length)
      this.nature = this.natureOptions[value].value
    },
    removeMove(index) {
      Vue.delete(this.moves, index)
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.positionAlreadyUsed = false
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await createPokemon(this.payload)
            window.location.replace(`/pokemon/${data.id}?status=created`)
          }
        } catch (e) {
          const { data, status } = e
          if (status === 409 && data?.code === 'PositionAlreadyUsed') {
            this.positionAlreadyUsed = true
          } else {
            this.handleError(e)
          }
        } finally {
          this.loading = false
        }
      }
    },
    swapMoves(a, b) {
      const temp = this.moves[a]
      Vue.set(this.moves, a, this.moves[b])
      Vue.set(this.moves, b, temp)
    }
  },
  created() {
    this.randomNature()
    this.randomIV()
  },
  watch: {
    heldItemId(id) {
      if (id !== null) {
        this.remainingHatchSteps = 0
      }
    },
    inParty() {
      this.box = 1
      this.position = 1
    },
    level(level) {
      if (level > 1) {
        this.remainingHatchSteps = 0
      }
    },
    async speciesId(speciesId) {
      this.abilityId = null
      if (speciesId) {
        try {
          const { data } = await getSpecies(speciesId)
          this.species = data
          if (this.species.abilities.length) {
            this.randomAbility()
            this.randomGender()
          }
          this.friendship = this.species.baseFriendship
        } catch (e) {
          this.handleError(e)
        }
      }
    },
    trainer(trainer, previous) {
      if (!trainer) {
        this.inParty = true
        this.ballId = null
        this.metLevel = 1
        this.metLocation = null
        this.metOn = null
        this.originalTrainer = null
      } else if (!previous) {
        this.metLevel = this.level
        this.metOn = new Date()
      }
    }
  }
}
</script>
