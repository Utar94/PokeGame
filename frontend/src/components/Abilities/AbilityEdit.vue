<template>
  <b-container>
    <h1 v-if="ability">{{ $t('abilities.editTitle', { name: ability.name }) }}</h1>
    <h1 v-else v-t="'abilities.newTitle'" />
    <status-detail v-if="ability" :model="ability" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <template v-if="ability">
            <icon-submit class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
            <icon-button class="mx-1" :disabled="hasChanges" href="/create-ability" icon="plus" text="actions.create" variant="success" />
          </template>
          <icon-submit v-else :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <b-row>
              <name-field class="col" required v-model="name" />
              <ability-kind-select class="col" v-model="kind" />
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
import Vue from 'vue'
import AbilityKindSelect from './AbilityKindSelect.vue'
import { createAbility, updateAbility } from '@/api/abilities'

export default {
  name: 'AbilityEdit',
  components: {
    AbilityKindSelect
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
      ability: null,
      description: null,
      kind: null,
      kinds: {},
      loading: false,
      name: null,
      notes: null,
      reference: null
    }
  },
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.ability?.name ?? '') ||
        this.kind !== (this.ability?.kind ?? null) ||
        (this.description ?? '') !== (this.ability?.description ?? '') ||
        (this.reference ?? '') !== (this.ability?.reference ?? '') ||
        (this.notes ?? '') !== (this.ability?.notes ?? '')
      )
    },
    payload() {
      const payload = {
        kind: this.kind,
        name: this.name,
        description: this.description,
        reference: this.reference || null,
        notes: this.notes
      }
      return payload
    }
  },
  methods: {
    setModel(ability) {
      this.ability = ability
      this.description = ability.description
      this.kind = ability.kind
      this.name = ability.name
      this.notes = ability.notes
      this.reference = ability.reference
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.ability) {
              const { data } = await updateAbility(this.ability.id, this.payload)
              this.setModel(data)
              this.toast('success', 'abilities.updated')
              this.$refs.form.reset()
            } else {
              const { data } = await createAbility(this.payload)
              window.location.replace(`/abilities/${data.id}?status=created`)
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
    for (const [value, text] of Object.entries(this.$i18n.t('abilities.kind.options'))) {
      if (!this.kinds[text]) {
        Vue.set(this.kinds, text, value)
      }
    }
    if (this.json) {
      this.setModel(JSON.parse(this.json))
    }
    if (this.status === 'created') {
      this.toast('success', 'abilities.created')
    }
  },
  watch: {
    name(text) {
      const kind = this.kinds[text]
      if (kind) {
        this.kind = kind
      }
    }
  }
}
</script>
