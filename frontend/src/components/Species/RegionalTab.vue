<template>
  <b-tab :title="$t('species.regional.title')">
    <b-alert dismissible variant="warning" :show="conflict" @input="$emit('dismissed', $event)">
      <strong v-t="'species.regional.numbersAlreadyUsed'" />
    </b-alert>
    <b-row>
      <form-field
        class="col"
        id="numberKanto"
        label="region.options.Kanto"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Kanto"
        @input="onInput('Kanto', Number($event))"
      />
      <form-field
        class="col"
        id="numberJohto"
        label="region.options.Johto"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Johto"
        @input="onInput('Johto', Number($event))"
      />
      <form-field
        class="col"
        id="numberHoenn"
        label="region.options.Hoenn"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Hoenn"
        @input="onInput('Hoenn', Number($event))"
      />
      <form-field
        class="col"
        id="numberSinnoh"
        label="region.options.Sinnoh"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Sinnoh"
        @input="onInput('Sinnoh', Number($event))"
      />
    </b-row>
    <b-row>
      <form-field
        class="col"
        id="numberUnova"
        label="region.options.Unova"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Unova"
        @input="onInput('Unova', Number($event))"
      />
      <form-field
        class="col"
        id="numberKalos"
        label="region.options.Kalos"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Kalos"
        @input="onInput('Kalos', Number($event))"
      />
      <form-field
        class="col"
        id="numberAlola"
        label="region.options.Alola"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Alola"
        @input="onInput('Alola', Number($event))"
      />
      <form-field
        class="col"
        id="numberGalar"
        label="region.options.Galar"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Galar"
        @input="onInput('Galar', Number($event))"
      />
    </b-row>
    <b-row>
      <form-field
        class="col"
        id="numberIndigo"
        label="region.options.Indigo"
        :minValue="0"
        :maxValue="999"
        :step="1"
        type="number"
        :value="value.Indigo"
        @input="onInput('Indigo', Number($event))"
      />
    </b-row>
  </b-tab>
</template>

<script>
export default {
  name: 'RegionalTab',
  props: {
    conflict: {
      type: Boolean,
      default: false
    },
    value: {
      type: Object,
      required: true
    }
  },
  methods: {
    onInput(region, number) {
      const regionalNumbers = { ...this.value }
      regionalNumbers[region] = number
      this.$emit('input', regionalNumbers)
    }
  }
}
</script>
