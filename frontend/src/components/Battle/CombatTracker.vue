<template>
  <b-container fluid>
    <h1 v-t="'battle.combatTracker.title'" />
    <template v-if="Object.keys(trainers).length">
      <h3 v-t="'trainers.title'" />
      <b-row>
        <battle-trainer-team class="col" id="playerTrainers" :pokemon="playerPokemon" :trainers="playerTrainers" @pokemonUpdated="refreshPokemon">
          <template #title><h5 v-t="'battle.players'" /></template>
        </battle-trainer-team>
        <battle-trainer-team class="col" id="opponentTrainers" :pokemon="opponentPokemon" :trainers="opponentTrainers" @pokemonUpdated="refreshPokemon">
          <template #title><h5 v-t="'battle.opponents'" /></template>
        </battle-trainer-team>
      </b-row>
    </template>
    <template v-if="Object.keys(pokemon).length">
      <h3 v-t="'pokemon.title'" />
      <b-row>
        <battle-pokemon-team class="col" :pokemon="playerPokemon" :selected="[]">
          <template #title>
            <h5>{{ $t('battle.players') }} ({{ remainingPlayerPokemon.length }}/{{ playerPokemon.length }})</h5>
          </template>
        </battle-pokemon-team>
        <battle-pokemon-team class="col" :pokemon="opponentPokemon" :selected="[]">
          <template #title>
            <h5>{{ $t('battle.opponents') }} ({{ remainingOpponentPokemon.length }}/{{ opponentPokemon.length }})</h5>
          </template>
        </battle-pokemon-team>
      </b-row>
    </template>
    <icon-button icon="chevron-left" text="battle.pokemonSelection.title" variant="warning" @click="onPrevious" />
  </b-container>
</template>

<script>
import Vue from 'vue'
import BattlePokemonTeam from './BattlePokemonTeam.vue'
import BattleTrainerTeam from './BattleTrainerTeam.vue'
import { mapState } from 'vuex'
import { getPokemonList } from '@/api/pokemon'
import { getTrainers } from '@/api/trainers'

export default {
  name: 'CombatTracker',
  components: {
    BattlePokemonTeam,
    BattleTrainerTeam
  },
  data() {
    return {
      pokemon: {},
      trainers: {}
    }
  },
  computed: {
    ...mapState(['battle']),
    opponentPokemon() {
      return this.orderBy(
        this.battle.opponents.pokemon.map(id => this.pokemon[id]),
        'speed',
        true
      )
    },
    opponentTrainers() {
      return this.battle.opponents.trainers.map(id => this.trainers[id])
    },
    playerPokemon() {
      return this.orderBy(
        this.battle.players.pokemon.map(id => this.pokemon[id]),
        'speed',
        true
      )
    },
    playerTrainers() {
      return this.battle.players.trainers.map(id => this.trainers[id])
    },
    remainingOpponentPokemon() {
      return this.opponentPokemon.filter(x => x.currentHitPoints > 0)
    },
    remainingPlayerPokemon() {
      return this.playerPokemon.filter(x => x.currentHitPoints > 0)
    }
  },
  methods: {
    onPrevious() {
      this.$store.commit('setBattleStep', 'PokemonSelection')
    },
    async refreshPokemon() {
      try {
        const { data } = await getPokemonList({ sort: 'Name', desc: false })
        for (const item of data.items) {
          Vue.set(this.pokemon, item.id, item)
        }
      } catch (e) {
        this.handleError(e)
      }
    },
    async refreshTrainers() {
      try {
        const { data } = await getTrainers({ sort: 'Name', desc: false })
        for (const trainer of data.items) {
          Vue.set(this.trainers, trainer.id, trainer)
        }
      } catch (e) {
        this.handleError(e)
      }
    }
  },
  created() {
    this.refreshTrainers()
    this.refreshPokemon()
  }
}
</script>
