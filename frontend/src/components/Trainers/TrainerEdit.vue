<template>
  <b-container>
    <h1 v-if="trainer"><trainer-icon :trainer="trainer" /> {{ $t('trainers.editTitle', { name: trainer.name }) }}</h1>
    <h1 v-else v-t="'trainers.newTitle'" />
    <status-detail v-if="trainer" :model="trainer" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <template v-if="trainer">
            <icon-submit class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
            <icon-button class="mx-1" href="/create-trainer" icon="plus" text="actions.create" variant="success" />
          </template>
          <icon-submit v-else :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <b-row>
              <region-select class="col" :disabled="Boolean(trainer)" :required="!trainer" v-model="region" />
              <gender-select class="col" :disabled="Boolean(trainer)" :required="!trainer" v-model="gender" />
              <form-field class="col" disabled id="number" label="trainers.number" :required="!trainer" type="number" v-model.number="number">
                <b-input-group-append>
                  <icon-button v-if="!trainer" :disabled="!gender" icon="dice" variant="primary" @click="generateNumber" />
                </b-input-group-append>
              </form-field>
            </b-row>
            <b-row>
              <user-select class="col" v-model="userId" />
              <form-field class="col" id="money" label="trainers.money" :minValue="0" :maxValue="999999" :step="1" type="number" v-model.number="money">
                <template #prepend>
                  <b-input-group-prepend is-text>
                    <pokemon-dollar />
                  </b-input-group-prepend>
                </template>
              </form-field>
              <form-field class="col" id="playTime" label="trainers.playTime" :minValue="0" :step="1" type="number" v-model.number="playTime">
                <b-input-group-append is-text>{{ formattedPlayTime }}</b-input-group-append>
              </form-field>
            </b-row>
            <name-field required v-model="name" />
            <description-field v-model="description" />
          </b-tab>
          <inventory-tab v-if="trainer" :trainer="trainer" @updated="onInventoryUpdated" />
          <pokedex-tab v-if="trainer" :trainerId="trainer.id" />
          <b-tab :title="$t('metadata')">
            <reference-field v-model="reference" />
            <picture-field validate v-model="picture" />
            <notes-field v-model="notes" />
          </b-tab>
        </b-tabs>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import GenderSelect from './GenderSelect.vue'
import InventoryTab from './InventoryTab.vue'
import PokedexTab from './PokedexTab.vue'
import TrainerIcon from './TrainerIcon.vue'
import UserSelect from '@/components/Users/UserSelect.vue'
import { createTrainer, getTrainer, updateTrainer } from '@/api/trainers'

export default {
  name: 'TrainerEdit',
  components: {
    GenderSelect,
    InventoryTab,
    PokedexTab,
    TrainerIcon,
    UserSelect
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
      description: null,
      gender: null,
      loading: false,
      money: 0,
      name: null,
      notes: null,
      number: 123456,
      picture: null,
      playTime: 0,
      reference: null,
      region: null,
      trainer: null,
      userId: null
    }
  },
  computed: {
    formattedPlayTime() {
      const hours = Math.floor(this.playTime / 60)
      const minutes = this.playTime % 60
      return [isNaN(hours) ? 0 : hours, (isNaN(minutes) ? 0 : minutes).toString().padStart(2, '0')].join(':')
    },
    hasChanges() {
      return (
        (!this.trainer && (this.gender || this.number || this.region)) ||
        this.money !== (this.trainer?.money ?? 0) ||
        this.playTime !== (this.trainer?.playTime ?? 0) ||
        this.userId !== (this.trainer?.userId ?? null) ||
        (this.name ?? '') !== (this.trainer?.name ?? '') ||
        (this.description ?? '') !== (this.trainer?.description ?? '') ||
        (this.reference ?? '') !== (this.trainer?.reference ?? '') ||
        (this.picture ?? '') !== (this.trainer?.picture ?? '') ||
        (this.notes ?? '') !== (this.trainer?.notes ?? '')
      )
    },
    payload() {
      const payload = {
        userId: this.userId,
        money: this.money,
        playTime: this.playTime,
        name: this.name,
        description: this.description,
        reference: this.reference || null,
        picture: this.picture || null,
        notes: this.notes
      }
      if (!this.trainer) {
        payload.region = this.region
        payload.number = this.number
        payload.gender = this.gender
      }
      return payload
    }
  },
  methods: {
    checksum(number) {
      if (number < 10) {
        return number
      }
      let sum = 0
      ;[...number.toString()].forEach(digit => (sum += Number(digit)))
      return this.checksum(sum)
    },
    generateNumber() {
      const random = (Math.floor(Math.random() * 400000) + 100000).toString()
      const timestamp = Math.floor(new Date().getTime() / 1000).toString()
      const digits = []
      switch (this.gender) {
        case 'Female':
          digits.push(Number(random[0]) * 2)
          break
        case 'Male':
          digits.push(Number(random[0]) * 2 - 1)
          break
        default:
          digits.push(9)
          break
      }
      for (let i = 1; i <= 5; i++) {
        digits.push(this.checksum(Number(random[i]) + this.checksum(Number(timestamp[i - 1]) + Number(timestamp[10 - i]))))
      }
      this.number = digits.join('')
    },
    async onInventoryUpdated() {
      try {
        const { data } = await getTrainer(this.trainer.id)
        this.trainer = data
        this.money = data.money
      } catch (e) {
        this.handleError(e)
      }
    },
    setModel(trainer) {
      this.trainer = trainer
      this.description = trainer.description
      this.gender = trainer.gender
      this.money = trainer.money
      this.name = trainer.name
      this.notes = trainer.notes
      this.number = trainer.number
      this.picture = trainer.picture
      this.playTime = trainer.playTime
      this.reference = trainer.reference
      this.region = trainer.region
      this.userId = trainer.userId
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.trainer) {
              const { data } = await updateTrainer(this.trainer.id, this.payload)
              this.setModel(data)
              this.toast('success', 'trainers.updated')
              this.$refs.form.reset()
            } else {
              const { data } = await createTrainer(this.payload)
              window.location.replace(`/trainers/${data.id}?status=created`)
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
      this.toast('success', 'trainers.created')
    }
  },
  watch: {
    gender(gender) {
      if (gender && !this.trainer) {
        this.generateNumber()
      }
    }
  }
}
</script>
