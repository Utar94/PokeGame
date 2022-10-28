<template>
  <b-container>
    <h1 v-if="item"><item-icon :item="item" /> {{ $t('items.editTitle', { name: item.name }) }}</h1>
    <h1 v-else v-t="'items.newTitle'" />
    <status-detail v-if="item" :model="item" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <template v-if="item">
            <icon-submit class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
            <icon-button class="mx-1" :disabled="hasChanges" href="/create-item" icon="plus" text="actions.create" variant="success" />
          </template>
          <icon-submit v-else :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
        <b-tabs content-class="mt-3">
          <b-tab :title="$t('gameData')">
            <b-row>
              <category-select class="col" :disabled="Boolean(item)" :required="!item" v-model="category" />
              <name-field class="col" required v-model="name" />
              <form-field class="col" id="price" label="items.price" :minValue="0" :maxValue="999999" :step="1" type="number" v-model.number="price" />
            </b-row>
            <b-row>
              <ball-modifier-field v-if="category === 'PokeBall'" class="col" id="defaultModifier" v-model="defaultModifier" />
              <item-kind-select v-if="category === 'PokeBall' || category === 'OtherItem'" class="col" v-model="kind" />
            </b-row>
            <description-field v-model="description" />
          </b-tab>
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
import Vue from 'vue'
import BallModifierField from './BallModifierField.vue'
import CategorySelect from './CategorySelect.vue'
import ItemIcon from './ItemIcon.vue'
import ItemKindSelect from './ItemKindSelect.vue'
import { createItem, updateItem } from '@/api/items'

export default {
  name: 'ItemEdit',
  components: {
    BallModifierField,
    CategorySelect,
    ItemIcon,
    ItemKindSelect
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
      category: null,
      defaultModifier: 0,
      description: null,
      item: null,
      kind: null,
      kinds: {},
      loading: false,
      name: null,
      notes: null,
      picture: null,
      price: 0,
      reference: null
    }
  },
  computed: {
    hasChanges() {
      return (
        (!this.item && this.category) ||
        (this.name ?? '') !== (this.item?.name ?? '') ||
        this.price !== (this.item?.price ?? 0) ||
        (this.defaultModifier ?? 0) !== (this.item?.defaultModifier ?? 0) ||
        this.kind !== (this.item?.kind ?? null) ||
        (this.description ?? '') !== (this.item?.description ?? '') ||
        (this.reference ?? '') !== (this.item?.reference ?? '') ||
        (this.picture ?? '') !== (this.item?.picture ?? '') ||
        (this.notes ?? '') !== (this.item?.notes ?? '')
      )
    },
    payload() {
      const payload = {
        kind: this.kind,
        defaultModifier: this.defaultModifier || null,
        name: this.name,
        price: this.price || null,
        description: this.description,
        reference: this.reference || null,
        picture: this.picture || null,
        notes: this.notes
      }
      if (!this.item) {
        payload.category = this.category
      }
      return payload
    }
  },
  methods: {
    setModel(item) {
      this.item = item
      this.category = item.category
      this.defaultModifier = item.defaultModifier ?? 0
      this.description = item.description
      this.kind = item.kind
      this.name = item.name
      this.notes = item.notes
      this.picture = item.picture
      this.price = item.price ?? 0
      this.reference = item.reference
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.item) {
              const { data } = await updateItem(this.item.id, this.payload)
              this.setModel(data)
              this.toast('success', 'items.updated')
              this.$refs.form.reset()
            } else {
              const { data } = await createItem(this.payload)
              window.location.replace(`/items/${data.id}?status=created`)
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
    for (const [value, text] of Object.entries(this.$i18n.t('items.kind.options'))) {
      if (!this.kinds[text]) {
        Vue.set(this.kinds, text, value)
      }
    }
    if (this.json) {
      this.setModel(JSON.parse(this.json))
    }
    if (this.status === 'created') {
      this.toast('success', 'items.created')
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
