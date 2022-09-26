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
import Vue from 'vue'
import { mapActions, mapGetters } from 'vuex'
import DefeatedPokemon from './DefeatedPokemon.vue'
import WinningPokemonTable from './WinningPokemonTable.vue'
import { battleGain } from '@/api/pokemon'

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
    ...mapGetters(['battleExperienceDefeatedPokemon', 'battleExperienceWinners', 'isTrainerBattle']),
    canSubmit() {
      return !this.loading && Object.keys(this.battleExperienceWinners).length > 0
    },
    payload() {
      return {
        defeatedId: this.battleExperienceDefeatedPokemon.id,
        isTrainerBattle: this.isTrainerBattle,
        winners: Object.values(this.battleExperienceWinners).map(({ pokemon, canEvolve, hasParticipated, otherModifiers }) => ({
          id: pokemon.id,
          canEvolve,
          hasParticipated,
          otherModifiers
        }))
      }
    }
  },
  methods: {
    ...mapActions(['battlePrevious', 'loadPokemonList', 'toggleBattlingPlayerPokemon', 'updatePokemon']),
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await battleGain(this.payload)
            for (const pokemon of data) {
              this.updatePokemon(pokemon)
            }
            const { box, currentHitPoints } = this.battleExperienceDefeatedPokemon
            if (currentHitPoints > 0 && box === null) {
              this.toggleBattlingPlayerPokemon(this.battleExperienceDefeatedPokemon.id)
            }
            Vue.nextTick(() => this.toast('warning', 'battle.experience.leveledUp', 'warning'))
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
  },
  mounted() {
    if (this.battleExperienceDefeatedPokemon) {
      const { box, currentHitPoints } = this.battleExperienceDefeatedPokemon
      if (currentHitPoints > 0) {
        this.toast('success', `battle.throwBall.caughtToast.${box === null ? 'party' : 'box'}`)
      }
    }
  }
}
</script>
