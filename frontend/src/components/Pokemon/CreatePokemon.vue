<template>
  <b-container>
    <h1 v-t="'pokemon.newTitle'" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <div class="my-2">
          <icon-submit :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
        </div>
        <b-row>
          <species-select class="col" required v-model="speciesId" />
          <ability-select class="col" :disabled="!speciesId" required :speciesId="speciesId" v-model="abilityId" @refresh="abilitiesRefreshed">
            <b-input-group-append>
              <icon-button :disabled="!speciesId || !this.abilities.length" icon="dice" variant="primary" @click="randomAbility()" />
            </b-input-group-append>
          </ability-select>
        </b-row>
        <!-- TODO(fpion): Level -->
        <!-- TODO(fpion): Gender -->
        <!-- TODO(fpion): Nature -->
        <name-field id="surname" label="pokemon.surname.label" placeholder="pokemon.surname.placeholder" v-model="surname" />
        <description-field v-model="description" />
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import AbilitySelect from '@/components/Abilities/AbilitySelect.vue'
import SpeciesSelect from '@/components/Species/SpeciesSelect.vue'

export default {
  name: 'CreatePokemon',
  components: {
    AbilitySelect,
    SpeciesSelect
  },
  data() {
    return {
      abilities: [],
      abilityId: null,
      description: null,
      gender: null,
      level: 1,
      loading: false,
      nature: null,
      speciesId: null,
      surname: null
      // TODO(fpion): IndividualValues
      // TODO(fpion): Moves
      // TODO(fpion): HeldItemId
      // TODO(fpion): History
      // TODO(fpion): Position
      // TODO(fpion): Box
      // TODO(fpion): Notes
      // TODO(fpion): Reference
    }
  },
  computed: {
    hasChanges() {
      return this.speciesId || this.abilityId
    }
  },
  methods: {
    abilitiesRefreshed({ items }) {
      this.abilities = items
      if (this.speciesId && this.abilities.length && !this.abilityId) {
        this.randomAbility()
      }
      console.log(this.abilities) // TODO(fpion): implement
    },
    randomAbility() {
      alert('randomAbility()') // TODO(fpion): implement
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            alert('submit()') // TODO(fpion): implement
            // if (this.trainer) {
            //   const { data } = await updateTrainer(this.trainer.id, this.payload)
            //   this.setModel(data)
            //   this.toast('success', 'trainers.updated')
            //   this.$refs.form.reset()
            // } else {
            //   const { data } = await createTrainer(this.payload)
            //   window.location.replace(`/trainers/${data.id}?status=created`)
            // }
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    speciesId() {
      this.abilityId = null
    }
  }
}
</script>
