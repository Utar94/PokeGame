<template>
  <b-container>
    <h1 v-if="region">{{ $t('regions.editTitle', { name: region.name }) }}</h1>
    <h1 v-else v-t="'regions.newTitle'" />
    <status-detail v-if="region" :model="region" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <template v-if="region">
            <icon-submit class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
            <icon-button class="mx-1" :disabled="hasChanges" href="/create-region" icon="plus" text="actions.create" variant="success" />
          </template>
          <icon-submit v-else :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <name-field required v-model="name" />
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
import { createRegion, updateRegion } from '@/api/regions'

export default {
  name: 'RegionEdit',
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
      loading: false,
      name: null,
      notes: null,
      reference: null,
      region: null
    }
  },
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.region?.name ?? '') ||
        (this.description ?? '') !== (this.region?.description ?? '') ||
        (this.reference ?? '') !== (this.region?.reference ?? '') ||
        (this.notes ?? '') !== (this.region?.notes ?? '')
      )
    },
    payload() {
      const payload = {
        name: this.name,
        description: this.description,
        reference: this.reference || null,
        notes: this.notes
      }
      return payload
    }
  },
  methods: {
    setModel(region) {
      this.region = region
      this.description = region.description
      this.name = region.name
      this.notes = region.notes
      this.reference = region.reference
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.region) {
              const { data } = await updateRegion(this.region.id, this.payload)
              this.setModel(data)
              this.toast('success', 'regions.updated')
              this.$refs.form.reset()
            } else {
              const { data } = await createRegion(this.payload)
              window.location.replace(`/regions/${data.id}?status=created`)
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
      this.toast('success', 'regions.created')
    }
  }
}
</script>
