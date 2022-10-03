<template>
  <b-modal size="xl" :title="$t('game.bag.itemDetail')" :visible="value" @change="$emit('input', $event)">
    <template #modal-header="{ close }">
      <h5 class="modal-title">
        <img v-if="entry.hasCaught" alt="Poké Ball Logo" height="20" src="@/assets/poke-ball-logo.svg" />
        <font-awesome-icon v-else icon="eye" /> {{ ' ' }}
        <pokemon-icon :species="entry" />
        {{ $t('number') }} {{ entry.number.toString().padStart(3, '0') }}
        {{ entry.name }}
        <gender-icon v-if="entry.genderRatio > 0" gender="Male" />
        {{ ' ' }}
        <gender-icon v-if="entry.genderRatio < 100" gender="Female" />
      </h5>
      <button aria-label="close" class="close" type="button" @click="close()">&times;</button>
    </template>
    <b-row v-if="entry">
      <b-col><img :alt="alt" :src="src" /></b-col>
      <b-col>
        <table class="table table-striped">
          <tbody>
            <tr>
              <th scope="row" v-t="'species.category.label'" />
              <td v-if="entry.category">{{ $t('species.category.format', { category: entry.category }) }}</td>
              <td v-else>???</td>
            </tr>
            <tr>
              <th scope="row" v-t="'type.label'" />
              <td v-if="entry.types" v-text="entry.types" />
              <td v-else>???</td>
            </tr>
            <tr>
              <th scope="row" v-t="'species.height.label'" />
              <td v-if="entry.height">{{ $t('species.height.format', { height: entry.height.toFixed(1) }) }}</td>
              <td v-else>???</td>
            </tr>
            <tr>
              <th scope="row" v-t="'species.weight.label'" />
              <td v-if="entry.weight">{{ $t('species.weight.format', { weight: entry.weight.toFixed(1) }) }}</td>
              <td v-else>???</td>
            </tr>
          </tbody>
        </table>
        <template>
          <h6 v-t="'description.label'" />
          {{ entry.hasCaught && entry.description ? entry.description : '???' }}
        </template>
      </b-col>
    </b-row>
    <template #modal-footer="{ close }">
      <icon-button icon="times" text="game.close" @click="close()" />
    </template>
  </b-modal>
</template>

<script>
export default {
  name: 'PokedexModal',
  props: {
    entry: {
      type: Object,
      default: null
    },
    value: {
      type: Boolean,
      required: true
    }
  },
  computed: {
    alt() {
      return this.entry === null ? null : `${this.$i18n.t('number')} ${this.entry.number.toString().padStart(3, '0')} ${this.entry.name}`
    },
    src() {
      return this.entry.picture ?? null
    },
    type() {
      return [this.entry.primaryType, this.entry.secondaryType].filter(type => Boolean(type)).join(', ')
    }
  }
}
</script>
