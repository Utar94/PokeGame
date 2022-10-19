<template>
  <b-container>
    <h1><pokemon-icon :pokemon="pokemon" /> {{ $t('pokemon.editTitle', { name }) }}</h1>
    <status-detail :model="pokemon" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <icon-submit class="mx-1" :disabled="!canSubmit" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-button class="mx-1" :disabled="hasChanges" href="/create-pokemon" icon="plus" text="actions.create" variant="success" />
          <pokemon-evolution class="mx-1" :evolutions="evolutions" :pokemon="pokemon" @updated="onPokemonEvolved" />
          <template v-if="hasTrainer">
            <icon-button class="mx-1" icon="info-circle" text="pokemon.summary" variant="info" @click="showSummary = true" />
            <pokemon-summary is-admin :pokemonId="pokemon.id" v-model="showSummary" />
          </template>
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <b-row>
              <form-field class="col" disabled label="species.select.label" :value="pokemon.species.name">
                <b-input-group-append>
                  <icon-button icon="external-link-alt" :href="`/species/${pokemon.species.id}`" target="_blank" variant="info" />
                </b-input-group-append>
              </form-field>
              <form-field class="col" disabled label="abilities.select.label" :value="pokemon.ability.name">
                <b-input-group-append>
                  <icon-button icon="external-link-alt" :href="`/abilities/${pokemon.ability.id}`" target="_blank" variant="info" />
                </b-input-group-append>
              </form-field>
            </b-row>
            <b-row>
              <form-field class="col" disabled label="pokemon.gender.label" :value="$t(`pokemon.gender.options.${pokemon.gender}`)">
                <template #prepend>
                  <b-input-group-prepend is-text><gender-icon :gender="pokemon.gender" /></b-input-group-prepend>
                </template>
              </form-field>
              <form-field class="col" disabled label="pokemon.nature.label" :value="$t(`pokemon.nature.options.${pokemon.nature}`)" />
            </b-row>
            <b-row>
              <form-field class="col" disabled label="pokemon.level" :value="pokemon.level" />
              <form-field class="col" disabled label="experience" :value="pokemon.experience">
                <b-input-group-append v-if="pokemon.experienceThreshold" is-text>/&nbsp;{{ pokemon.experienceThreshold }}</b-input-group-append>
                <b-input-group-append v-if="pokemon.experienceToNextLevel" is-text>
                  {{ $t('pokemon.experience.toNextLevelFormat', { experience: pokemon.experienceToNextLevel }) }}
                </b-input-group-append>
                <b-input-group-append v-if="pokemon.experienceToNextLevel">
                  <icon-button icon="plus" text="pokemon.experience.add" variant="success" v-b-modal.addExperience />
                  <experience-modal :pokemon="pokemon" @updated="onExperienceUpdated" />
                </b-input-group-append>
              </form-field>
            </b-row>
            <b-row>
              <form-field
                class="col"
                id="currentHitPoints"
                label="statistic.options.HP"
                :minValue="0"
                :maxValue="pokemon.maximumHitPoints"
                required
                :step="1"
                type="number"
                v-model.number="currentHitPoints"
              >
                <b-input-group-append is-text>/&nbsp;{{ pokemon.maximumHitPoints }}</b-input-group-append>
              </form-field>
              <condition-select class="col" v-model="statusCondition" />
            </b-row>
            <b-row>
              <name-field class="col" id="surname" label="pokemon.surname.label" placeholder="pokemon.surname.placeholder" v-model="surname" />
              <form-field
                class="col"
                id="friendship"
                label="pokemon.friendship"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="friendship"
              />
              <form-field class="col" disabled label="pokemon.remainingHatchSteps" :value="pokemon.remainingHatchSteps">
                <b-input-group-append v-if="pokemon.remainingHatchSteps > 0">
                  <icon-button icon="walking" text="pokemon.walkEgg.label" variant="primary" v-b-modal.walkEgg />
                  <walk-egg-modal :pokemon="pokemon" @updated="pokemon = $event" />
                </b-input-group-append>
              </form-field>
            </b-row>
            <item-select id="heldItem" label="pokemon.heldItem.label" v-model="heldItemId">
              <b-input-group-append v-if="pokemon.history">
                <icon-button icon="shopping-cart" text="trainers.inventory.label" variant="primary" v-b-modal.heldItem />
                <held-item-modal :pokemon="pokemon" @saved="onHeldItemSaved" />
              </b-input-group-append>
            </item-select>
            <description-field v-model="description" />
          </b-tab>
          <b-tab :title="$t('pokemon.statistics')">
            <b-row>
              <form-field class="col" disabled label="statistic.options.HP" :value="pokemon.maximumHitPoints" />
              <form-field class="col" disabled label="statistic.options.Attack" :value="pokemon.attack" />
              <form-field class="col" disabled label="statistic.options.Defense" :value="pokemon.defense" />
              <form-field class="col" disabled label="statistic.options.SpecialAttack" :value="pokemon.specialAttack" />
              <form-field class="col" disabled label="statistic.options.SpecialDefense" :value="pokemon.specialDefense" />
              <form-field class="col" disabled label="statistic.options.Speed" :value="pokemon.speed" />
            </b-row>
            <h3 v-t="'pokemon.individualValues.title'" />
            <b-row>
              <form-field class="col" disabled label="statistic.options.HP" :value="hpIV" />
              <form-field class="col" disabled label="statistic.options.Attack" :value="attackIV" />
              <form-field class="col" disabled label="statistic.options.Defense" :value="defenseIV" />
              <form-field class="col" disabled label="statistic.options.SpecialAttack" :value="specialAttackIV" />
              <form-field class="col" disabled label="statistic.options.SpecialDefense" :value="specialDefenseIV" />
              <form-field class="col" disabled label="statistic.options.Speed" :value="speedIV" />
            </b-row>
            <p>
              <strong>{{ $t('pokemon.individualValues.totalFormat', { total: totalIV }) }}</strong>
            </p>
            <h3 v-t="'pokemon.effortValues.title'" />
            <span v-if="totalEV > 510" class="text-danger" v-t="'pokemon.effortValues.exceeded'" />
            <b-row>
              <form-field
                class="col"
                id="hpIV"
                label="statistic.options.HP"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.HP"
              />
              <form-field
                class="col"
                id="attackIV"
                label="statistic.options.Attack"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.Attack"
              />
              <form-field
                class="col"
                id="defenseIV"
                label="statistic.options.Defense"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.Defense"
              />
              <form-field
                class="col"
                id="specialAttackIV"
                label="statistic.options.SpecialAttack"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.SpecialAttack"
              />
              <form-field
                class="col"
                id="specialDefenseIV"
                label="statistic.options.SpecialDefense"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.SpecialDefense"
              />
              <form-field
                class="col"
                id="speedIV"
                label="statistic.options.Speed"
                :minValue="0"
                :maxValue="255"
                required
                :step="1"
                type="number"
                v-model.number="effortValues.Speed"
              />
            </b-row>
            <icon-button :disabled="totalEV === 0" icon="eraser" text="actions.clear" variant="danger" @click="clearEV" />
            <p>
              <strong>{{ $t('pokemon.effortValues.totalFormat', { total: totalEV }) }}</strong>
            </p>
          </b-tab>
          <b-tab :title="$t('pokemon.moves.title')">
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
                  <td>
                    <form-field
                      hideLabel
                      :id="`remainingPowerPoints_${index}`"
                      label="pokemon.moves.remainingPowerPoints"
                      :minValue="0"
                      :maxValue="move.powerPoints"
                      required
                      :step="1"
                      type="number"
                      :value="move.remainingPowerPoints"
                      @input="updateRemainingPowerPoints(index, Number($event))"
                    >
                      <b-input-group-append is-text>/&nbsp;{{ move.powerPoints }}</b-input-group-append>
                    </form-field>
                  </td>
                  <td><icon-button icon="times" variant="danger" @click="removeMove(index)" /></td>
                </tr>
              </tbody>
            </table>
          </b-tab>
          <b-tab :title="$t('pokemon.trainer.title')">
            <b-alert dismissible variant="warning" v-model="positionAlreadyUsed"><strong v-t="'pokemon.trainer.positionAlreadyUsed'" /></b-alert>
            <b-row>
              <trainer-select class="col" id="currentTrainer" label="pokemon.trainer.current" v-model="currentTrainer">
                <b-input-group-append v-if="currentTrainer">
                  <icon-button icon="external-link-alt" :href="`/trainers/${currentTrainer.id}`" target="_blank" variant="info" />
                </b-input-group-append>
              </trainer-select>
              <trainer-select
                class="col"
                :disabled="!currentTrainer"
                id="originalTrainer"
                label="pokemon.trainer.original"
                :required="Boolean(currentTrainer)"
                v-model="originalTrainer"
              >
                <b-input-group-append v-if="originalTrainer">
                  <icon-button icon="external-link-alt" :href="`/trainers/${originalTrainer.id}`" target="_blank" variant="info" />
                </b-input-group-append>
              </trainer-select>
            </b-row>
            <b-row>
              <form-field
                class="col"
                :disabled="!currentTrainer || inParty"
                id="box"
                label="pokemon.trainer.box"
                :minValue="1"
                :maxValue="32"
                :required="Boolean(currentTrainer)"
                :step="1"
                type="number"
                v-model.number="box"
              >
                <template #after>
                  <b-form-checkbox :disabled="!currentTrainer" v-model="inParty">{{ $t('pokemon.trainer.party') }}</b-form-checkbox>
                </template>
              </form-field>
              <form-field
                class="col"
                :disabled="!currentTrainer"
                id="position"
                label="pokemon.trainer.position"
                :minValue="1"
                :maxValue="inParty ? 6 : 30"
                :required="Boolean(currentTrainer)"
                :step="1"
                type="number"
                v-model.number="position"
              >
                <b-input-group-append v-if="currentTrainer">
                  <icon-button icon="exchange-alt" text="pokemon.trainer.swap" variant="primary" v-b-modal.swapModal />
                  <swap-modal :pokemon="pokemon" :trainerId="currentTrainer.id" @updated="setModel" />
                </b-input-group-append>
              </form-field>
            </b-row>
            <b-row>
              <item-select
                category="PokeBall"
                class="col"
                :disabled="!currentTrainer"
                id="ballId"
                label="pokemon.caughtWithBall"
                :required="Boolean(currentTrainer)"
                v-model="ballId"
              />
              <form-field
                class="col"
                :disabled="!currentTrainer"
                id="metLevel"
                label="pokemon.trainer.metLevel"
                :minValue="1"
                :maxValue="pokemon.level"
                :required="Boolean(currentTrainer)"
                :step="1"
                type="number"
                v-model.number="metLevel"
              />
              <form-field
                class="col"
                :disabled="!currentTrainer"
                id="metLocation"
                label="pokemon.trainer.metLocation.label"
                :maxLength="100"
                placeholder="pokemon.trainer.metLocation.placeholder"
                :required="Boolean(currentTrainer)"
                v-model="metLocation"
              />
              <form-datetime
                class="col"
                :disabled="!currentTrainer"
                id="metOn"
                label="pokemon.trainer.metOn"
                :required="Boolean(currentTrainer)"
                validate
                v-model="metOn"
              />
            </b-row>
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
import Vue from 'vue'
import ConditionSelect from './ConditionSelect.vue'
import ExperienceModal from './ExperienceModal.vue'
import HeldItemModal from './HeldItemModal.vue'
import ItemSelect from '@/components/Items/ItemSelect.vue'
import MoveSelect from '@/components/Moves/MoveSelect.vue'
import PokemonEvolution from './PokemonEvolution.vue'
import PokemonSummary from '@/components/Game/PokemonSummary.vue'
import SwapModal from './SwapModal.vue'
import TrainerSelect from '@/components/Trainers/TrainerSelect.vue'
import WalkEggModal from './WalkEggModal.vue'
import { updatePokemon } from '@/api/pokemon'
import { getSpeciesEvolutions } from '@/api/species'

export default {
  name: 'PokemonEdit',
  components: {
    ConditionSelect,
    ExperienceModal,
    HeldItemModal,
    ItemSelect,
    MoveSelect,
    PokemonEvolution,
    PokemonSummary,
    SwapModal,
    TrainerSelect,
    WalkEggModal
  },
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
      ballId: null,
      box: 1,
      currentHitPoints: 0,
      currentTrainer: null,
      description: null,
      effortValues: {
        Attack: 0,
        Defense: 0,
        HP: 0,
        SpecialAttack: 0,
        SpecialDefense: 0,
        Speed: 0
      },
      evolutions: [],
      friendship: 0,
      heldItemId: null,
      inParty: true,
      loading: false,
      metLevel: 1,
      metLocation: null,
      metOn: null,
      move: null,
      moves: [],
      notes: null,
      originalTrainer: null,
      pokemon: null,
      position: 1,
      positionAlreadyUsed: false,
      reference: null,
      settingModel: false,
      showSummary: false,
      statusCondition: null,
      surname: null
    }
  },
  computed: {
    attackIV() {
      return this.pokemon.individualValues.find(({ statistic }) => statistic === 'Attack')?.value ?? 0
    },
    canSubmit() {
      return this.totalEV <= 510 && this.hasChanges && !this.loading
    },
    defenseIV() {
      return this.pokemon.individualValues.find(({ statistic }) => statistic === 'Defense')?.value ?? 0
    },
    hasChanges() {
      return (
        this.currentHitPoints !== this.pokemon.currentHitPoints ||
        this.statusCondition !== this.pokemon.statusCondition ||
        this.friendship !== this.pokemon.friendship ||
        (this.surname ?? '') !== (this.pokemon.surname ?? '') ||
        this.heldItemId !== (this.pokemon.heldItem?.id ?? null) ||
        (this.description ?? '') !== (this.pokemon.description ?? '') ||
        JSON.stringify(this.payload.effortValues) !== JSON.stringify(this.pokemon.effortValues) ||
        this.hasMovesChanged ||
        (this.originalTrainer?.id ?? null) !== (this.pokemon.originalTrainer?.id ?? null) ||
        (this.currentTrainer?.id ?? null) !== (this.pokemon.history?.trainer.id ?? null) ||
        (this.inParty ? null : this.box) !== this.pokemon.box ||
        this.position !== (this.pokemon.position ?? 1) ||
        this.ballId !== (this.pokemon.history?.ball.id ?? null) ||
        this.metLevel !== (this.pokemon.history?.level ?? 1) ||
        (this.metLocation ?? '') !== (this.pokemon.history?.location ?? '') ||
        this.metOn !== (this.pokemon.history?.metOn ?? null) ||
        (this.reference ?? '') !== (this.pokemon.reference ?? '') ||
        (this.notes ?? '') !== (this.pokemon.notes ?? '')
      )
    },
    hasMovesChanged() {
      const oldMoves = this.orderBy(this.pokemon.moves, 'position')
        .map(({ move, remainingPowerPoints }) => `${move.id}:${remainingPowerPoints}`)
        .join('|')
      const newMoves = this.moves.map(({ id, remainingPowerPoints }) => `${id}:${remainingPowerPoints}`).join('|')
      return oldMoves !== newMoves
    },
    hasTrainer() {
      return Boolean(this.pokemon.history?.trainer)
    },
    hpIV() {
      return this.pokemon.individualValues.find(({ statistic }) => statistic === 'HP')?.value ?? 0
    },
    moveIds() {
      return this.moves.map(({ id }) => id)
    },
    name() {
      return `${this.pokemon.surname ?? this.pokemon.species.name} ${this.$i18n.t('pokemon.levelFormat', { level: this.pokemon.level })}`
    },
    payload() {
      const payload = {
        currentHitPoints: this.currentHitPoints,
        statusCondition: this.statusCondition,
        friendship: this.friendship,
        heldItemId: this.heldItemId,
        surname: this.surname,
        description: this.description,
        effortValues: Object.entries(this.effortValues)
          .filter(([, value]) => value !== 0)
          .map(([statistic, value]) => ({ statistic, value })),
        moves: this.moves.map(({ id, remainingPowerPoints }, position) => ({
          moveId: id,
          position,
          remainingPowerPoints
        })),
        history: this.currentTrainer
          ? {
              ballId: this.ballId,
              level: this.metLevel,
              location: this.metLocation,
              metOn: this.metOn,
              trainerId: this.currentTrainer.id
            }
          : null,
        originalTrainerId: this.originalTrainer?.id ?? null,
        position: this.currentTrainer ? this.position : null,
        box: this.currentTrainer && !this.inParty ? this.box : null,
        reference: this.reference || null,
        notes: this.notes
      }
      return payload
    },
    specialAttackIV() {
      return this.pokemon.individualValues.find(({ statistic }) => statistic === 'SpecialAttack')?.value ?? 0
    },
    specialDefenseIV() {
      return this.pokemon.individualValues.find(({ statistic }) => statistic === 'SpecialDefense')?.value ?? 0
    },
    speedIV() {
      return this.pokemon.individualValues.find(({ statistic }) => statistic === 'Speed')?.value ?? 0
    },
    totalEV() {
      return Object.values(this.effortValues).reduce((a, b) => a + (b || 0), 0)
    },
    totalIV() {
      return this.pokemon.individualValues.reduce((a, b) => a + (b?.value || 0), 0)
    }
  },
  methods: {
    addMove() {
      const move = { ...this.move }
      move.remainingPowerPoints = move.powerPoints
      this.moves.push(move)
      this.move = null
    },
    clearEV() {
      for (const key of Object.keys(this.effortValues)) {
        Vue.set(this.effortValues, key, 0)
      }
    },
    async loadEvolutions() {
      try {
        const { data } = await getSpeciesEvolutions(this.pokemon.species.id)
        this.evolutions = data
      } catch (e) {
        this.handleError(e)
      }
    },
    onExperienceUpdated(pokemon) {
      this.pokemon = pokemon
      this.currentHitPoints = pokemon.currentHitPoints
    },
    onHeldItemSaved(pokemon) {
      this.pokemon = pokemon
      this.heldItemId = pokemon.heldItem?.id ?? null
    },
    async onPokemonEvolved(pokemon) {
      this.pokemon = pokemon
      this.currentHitPoints = pokemon.currentHitPoints
      this.heldItemId = pokemon.heldItem?.id ?? null
      await this.loadEvolutions()
    },
    removeMove(index) {
      Vue.delete(this.moves, index)
    },
    setModel(pokemon) {
      this.pokemon = pokemon
      this.currentHitPoints = pokemon.currentHitPoints
      this.currentTrainer = pokemon.history?.trainer ?? null
      this.description = pokemon.description
      this.effortValues.Attack = pokemon.effortValues.find(({ statistic }) => statistic === 'Attack')?.value ?? 0
      this.effortValues.Defense = pokemon.effortValues.find(({ statistic }) => statistic === 'Defense')?.value ?? 0
      this.effortValues.HP = pokemon.effortValues.find(({ statistic }) => statistic === 'HP')?.value ?? 0
      this.effortValues.SpecialAttack = pokemon.effortValues.find(({ statistic }) => statistic === 'SpecialAttack')?.value ?? 0
      this.effortValues.SpecialDefense = pokemon.effortValues.find(({ statistic }) => statistic === 'SpecialDefense')?.value ?? 0
      this.effortValues.Speed = pokemon.effortValues.find(({ statistic }) => statistic === 'Speed')?.value ?? 0
      this.friendship = pokemon.friendship
      this.heldItemId = pokemon.heldItem?.id ?? null
      this.inParty = pokemon.box === null
      this.moves = this.orderBy(pokemon.moves, 'position').map(({ move, remainingPowerPoints }) => ({ ...move, remainingPowerPoints }))
      this.notes = pokemon.notes
      this.reference = pokemon.reference
      this.statusCondition = pokemon.statusCondition
      this.surname = pokemon.surname

      Vue.nextTick(() => {
        this.box = pokemon.box ?? 1
        this.position = pokemon.position ?? 1

        this.ballId = pokemon.history?.ball?.id ?? null
        this.metLevel = pokemon.history?.level ?? 1
        this.metLocation = pokemon.history?.location ?? null
        this.metOn = pokemon.history?.metOn ?? null
        this.originalTrainer = pokemon.originalTrainer
      })
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.positionAlreadyUsed = false
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await updatePokemon(this.pokemon.id, this.payload)
            this.setModel(data)
            this.toast('success', 'pokemon.updated')
            this.$refs.form.reset()
          }
        } catch (e) {
          const { data, status } = e
          if (status === 409 && data?.code == 'PositionAlreadyUsed') {
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
    },
    updateRemainingPowerPoints(index, remainingPowerPoints) {
      const move = { ...this.moves[index] }
      move.remainingPowerPoints = remainingPowerPoints
      Vue.set(this.moves, index, move)
    }
  },
  async created() {
    this.setModel(JSON.parse(this.json))
    if (this.status === 'created') {
      this.toast('success', 'pokemon.created')
    }
    await this.loadEvolutions()
  },
  watch: {
    currentTrainer(trainer, previous) {
      if (!trainer) {
        this.ballId = null
        this.inParty = true
        this.metLevel = 1
        this.metLocation = null
        this.metOn = null
        this.originalTrainer = null
      } else if (!previous) {
        this.metLevel = this.pokemon.level
        this.metOn = new Date()
        this.originalTrainer = trainer
      }
    },
    inParty() {
      this.box = 1
      this.position = 1
    }
  }
}
</script>
