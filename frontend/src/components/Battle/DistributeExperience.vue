<template>
  <b-container fluid>
    <h1 v-t="'battle.experience.title'" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <defeated-pokemon />
        <winning-pokemon-table />
        <div class="my-2">
          <icon-button class="mx-1" icon="ban" text="actions.cancel" @click="battlePrevious" />
          <icon-submit :disabled="!canSubmit" icon="level-up-alt" :loading="loading" text="battle.experience.label" variant="info" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import DefeatedPokemon from './DefeatedPokemon.vue'
import WinningPokemonTable from './WinningPokemonTable.vue'

export default {
  name: 'DistributeExperience',
  components: {
    DefeatedPokemon,
    WinningPokemonTable
  },
  data() {
    return {
      loading: false
    }
  },
  computed: {
    ...mapGetters(['battleExperienceWinners']),
    canSubmit() {
      return !this.loading && Object.keys(this.battleExperienceWinners).length > 0
    }
  },
  methods: {
    ...mapActions(['battlePrevious', 'loadPokemonList']),
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            // await usePokemonMove(this.battleMoveAttacker.id, this.selectedBattleMove.id, this.payload)
            this.loadPokemonList()
            // this.toast('success', 'pokemon.updated')
            this.$refs.form.reset()
            this.battlePrevious()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  }
}
</script>
