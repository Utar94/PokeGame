<template>
  <b-container>
    <h1 v-t="move ? 'moves.editTitle' : 'moves.newTitle'" />
    <status-detail v-if="move" :model="move" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <icon-submit v-if="move" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
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
                <b-input-group-append>
                  <b-input-group-text>{{ $t('moves.powerPoints.unit') }}</b-input-group-text>
                </b-input-group-append>
              </form-field>
              <form-field class="col" id="power" label="moves.power" :minValue="0" :maxValue="250" :step="5" type="number" v-model.number="power" />
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
                <b-input-group-append>
                  <b-input-group-text>{{ $t('moves.accuracy.unit') }}</b-input-group-text>
                </b-input-group-append>
              </form-field>
            </b-row>
            <description-field v-model="description" />
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
import { createMove, updateMove } from '@/api/moves'

export default {
  name: 'MoveEdit',
  components: {
    CategorySelect
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
      accuracy: 0,
      category: null,
      description: null,
      loading: false,
      move: null,
      name: null,
      notes: null,
      power: 0,
      powerPoints: 0,
      reference: null,
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
        this.accuracy / 100 !== (this.move?.accuracy ?? 0) ||
        (this.description ?? '') !== (this.move?.description ?? '') ||
        (this.reference ?? '') !== (this.move?.reference ?? '') ||
        (this.notes ?? '') !== (this.move?.notes ?? '')
      )
    },
    payload() {
      const payload = {
        name: this.name,
        powerPoints: this.powerPoints,
        power: this.power || null,
        accuracy: this.accuracy / 100 || null,
        description: this.description,
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
      this.accuracy = (move.accuracy ?? 0) * 100
      this.category = move.category
      this.description = move.description
      this.name = move.name
      this.notes = move.notes
      this.power = move.power ?? 0
      this.powerPoints = move.powerPoints
      this.reference = move.reference
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
  }
}
</script>
