<template>
  <b-container>
    <h1 v-if="move">{{ $t('moves.editTitle', { name: move.name }) }}</h1>
    <h1 v-else v-t="'moves.newTitle'" />
    <status-detail v-if="move" :model="move" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <template v-if="move">
            <icon-submit class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
            <icon-button class="mx-1" href="/create-move" icon="plus" text="actions.create" variant="success" />
          </template>
          <icon-submit v-else :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <b-row>
              <type-select class="col" :disabled="Boolean(move)" :required="!move" v-model="type" />
              <category-select class="col" :disabled="Boolean(move)" :required="!category" v-model="category" />
            </b-row>
            <name-field required v-model="name" />
            <b-row>
              <form-field
                class="col"
                id="powerPoints"
                label="moves.powerPoints.label"
                :minValue="5"
                :maxValue="40"
                :step="5"
                type="number"
                v-model.number="powerPoints"
              >
                <b-input-group-append is-text>{{ $t('moves.powerPoints.unit') }}</b-input-group-append>
              </form-field>
              <form-field v-if="category === 'Status'" class="col" disabled id="power" label="moves.power" type="number" :value="0" />
              <form-field v-else class="col" id="power" label="moves.power" :minValue="0" :maxValue="250" :step="5" type="number" v-model.number="power" />
              <form-field
                class="col"
                id="accuracy"
                label="moves.accuracy.label"
                :minValue="0"
                :maxValue="100"
                :step="5"
                type="number"
                v-model.number="accuracy"
              >
                <b-input-group-append is-text>{{ $t('moves.accuracy.unit') }}</b-input-group-append>
                <template #after v-if="accuracy === 0">
                  <i class="text-info" v-t="'moves.accuracy.neverMisses'" />
                </template>
              </form-field>
            </b-row>
            <description-field v-model="description" />
          </b-tab>
          <b-tab v-if="move" :title="$t('moves.stagesAndCondition')">
            <h4 v-t="'moves.statisticStages'" />
            <b-row>
              <form-field
                class="col"
                id="Attack"
                label="statistic.options.Attack"
                :minValue="-6"
                :maxValue="6"
                :step="1"
                type="number"
                v-model.number="stages.Attack"
              />
              <form-field
                class="col"
                id="Defense"
                label="statistic.options.Defense"
                :minValue="-6"
                :maxValue="6"
                :step="1"
                type="number"
                v-model.number="stages.Defense"
              />
              <form-field
                class="col"
                id="SpecialAttack"
                label="statistic.options.SpecialAttack"
                :minValue="-6"
                :maxValue="6"
                :step="1"
                type="number"
                v-model.number="stages.SpecialAttack"
              />
              <form-field
                class="col"
                id="SpecialDefense"
                label="statistic.options.SpecialDefense"
                :minValue="-6"
                :maxValue="6"
                :step="1"
                type="number"
                v-model.number="stages.SpecialDefense"
              />
              <form-field
                class="col"
                id="Speed"
                label="statistic.options.Speed"
                :minValue="-6"
                :maxValue="6"
                :step="1"
                type="number"
                v-model.number="stages.Speed"
              />
            </b-row>
            <b-row>
              <form-field
                class="col"
                id="accuracyStage"
                label="moves.accuracy.label"
                :minValue="-6"
                :maxValue="6"
                :step="1"
                type="number"
                v-model.number="accuracyStage"
              />
              <form-field
                class="col"
                id="evasionStage"
                label="moves.evasion"
                :minValue="-6"
                :maxValue="6"
                :step="1"
                type="number"
                v-model.number="evasionStage"
              />
            </b-row>
            <h4 v-t="'moves.condition'" />
            <b-row>
              <condition-select class="col" v-model="statusCondition" />
              <form-field
                class="col"
                :disabled="!statusCondition"
                id="statusChance"
                label="moves.statusChance.label"
                :minValue="0"
                :maxValue="100"
                :step="1"
                type="number"
                v-model.number="statusChance"
              >
                <b-input-group-append is-text>{{ $t('moves.statusChance.unit') }}</b-input-group-append>
              </form-field>
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
import CategorySelect from './CategorySelect.vue'
import ConditionSelect from '@/components/Pokemon/ConditionSelect.vue'
import { createMove, updateMove } from '@/api/moves'

export default {
  name: 'MoveEdit',
  components: {
    CategorySelect,
    ConditionSelect
  },
  props: {
    json: {
      type: String,
      default: ''
    },
    status: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      accuracy: 100,
      accuracyStage: 0,
      category: null,
      description: null,
      evasionStage: 0,
      loading: false,
      move: null,
      name: null,
      notes: null,
      power: 0,
      powerPoints: 0,
      reference: null,
      stages: {
        Attack: 0,
        Defense: 0,
        SpecialAttack: 0,
        SpecialDefense: 0,
        Speed: 0
      },
      statusChance: 0,
      statusCondition: null,
      type: null
    }
  },
  computed: {
    hasChanges() {
      return (
        (!this.move && (this.type || this.category)) ||
        (this.name ?? '') !== (this.move?.name ?? '') ||
        this.powerPoints !== (this.move?.powerPoints ?? 0) ||
        this.power !== (this.move?.power ?? 0) ||
        this.accuracy !== (this.move?.accuracy ?? 0) ||
        (this.description ?? '') !== (this.move?.description ?? '') ||
        this.stages.Attack !== (this.move?.statisticStages.find(({ statistic }) => statistic === 'Attack')?.value ?? 0) ||
        this.stages.Defense !== (this.move?.statisticStages.find(({ statistic }) => statistic === 'Defense')?.value ?? 0) ||
        this.stages.SpecialAttack !== (this.move?.statisticStages.find(({ statistic }) => statistic === 'SpecialAttack')?.value ?? 0) ||
        this.stages.SpecialDefense !== (this.move?.statisticStages.find(({ statistic }) => statistic === 'SpecialDefense')?.value ?? 0) ||
        this.stages.Speed !== (this.move?.statisticStages.find(({ statistic }) => statistic === 'Speed')?.value ?? 0) ||
        this.accuracyStage !== (this.move?.accuracyStage ?? 0) ||
        this.evasionStage !== (this.move?.evasionStage ?? 0) ||
        this.statusCondition !== (this.move?.statusCondition ?? null) ||
        this.statusChance !== (this.move?.statusChance ?? 0) ||
        (this.reference ?? '') !== (this.move?.reference ?? '') ||
        (this.notes ?? '') !== (this.move?.notes ?? '')
      )
    },
    payload() {
      const payload = {
        name: this.name,
        powerPoints: this.powerPoints,
        power: this.category === 'Status' ? null : this.power || null,
        accuracy: this.accuracy || null,
        description: this.description,
        statusCondition: this.statusCondition,
        statusChance: this.statusChance || null,
        statisticStages: Object.entries(this.stages).map(([statistic, value]) => ({ statistic, value })),
        accuracyStage: this.accuracyStage,
        evasionStage: this.evasionStage,
        reference: this.reference,
        notes: this.notes
      }
      if (!this.move) {
        payload.type = this.type
        payload.category = this.category
      }
      return payload
    }
  },
  methods: {
    setModel(move) {
      this.move = move
      this.accuracy = move.accuracy ?? 0
      this.accuracyStage = move.accuracyStage
      this.category = move.category
      this.description = move.description
      this.evasionStage = move.evasionStage
      this.name = move.name
      this.notes = move.notes
      this.power = move.power ?? 0
      this.powerPoints = move.powerPoints
      this.reference = move.reference
      this.stages.Attack = move.statisticStages.find(({ statistic }) => statistic === 'Attack')?.value ?? 0
      this.stages.Defense = move.statisticStages.find(({ statistic }) => statistic === 'Defense')?.value ?? 0
      this.stages.SpecialAttack = move.statisticStages.find(({ statistic }) => statistic === 'SpecialAttack')?.value ?? 0
      this.stages.SpecialDefense = move.statisticStages.find(({ statistic }) => statistic === 'SpecialDefense')?.value ?? 0
      this.stages.Speed = move.statisticStages.find(({ statistic }) => statistic === 'Speed')?.value ?? 0
      this.statusChance = move.statusChance ?? 0
      this.statusCondition = move.statusCondition
      this.type = move.type
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.move) {
              const { data } = await updateMove(this.move.id, this.payload)
              this.setModel(data)
              this.toast('success', 'moves.updated')
              this.$refs.form.reset()
            } else {
              const { data } = await createMove(this.payload)
              window.location.replace(`/moves/${data.id}?status=created`)
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
  created() {
    if (this.json) {
      this.setModel(JSON.parse(this.json))
    }
    if (this.status === 'created') {
      this.toast('success', 'moves.created')
    }
  },
  watch: {
    statusCondition(value) {
      if (value) {
        this.statusChance = this.category === 'Status' ? 100 : 10
      } else {
        this.statusChance = 0
      }
    }
  }
}
</script>
