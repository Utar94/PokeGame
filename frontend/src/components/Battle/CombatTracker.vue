<template>
  <b-container fluid>
    <h1 v-t="'battle.combatTracker.title'" />
    <b-row>
      <form-field
        class="col-3"
        v-if="!isTrainerBattle"
        id="location"
        label="battle.combatTracker.location.label"
        :maxLength="100"
        placeholder="battle.combatTracker.location.placeholder"
        v-model="location"
      >
        <b-input-group-append>
          <icon-button :disabled="!location || location === battle.location" icon="save" variant="primary" @click="saveLocation(location)" />
        </b-input-group-append>
      </form-field>
    </b-row>
    <template v-if="Object.keys(trainers).length">
      <h3 v-t="'trainers.title'" />
      <b-row>
        <battle-trainer-team
          class="col"
          id="playerTrainers"
          :opponents="isTrainerBattle || !battle.location ? [] : remainingOpponentPokemon"
          :pokemon="playerPokemon"
          :trainers="playerTrainers"
          @pokemonCaught="onPokemonCaught"
          @pokemonUpdated="refreshPokemon"
        >
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
        <battle-pokemon-team class="col" canSwitch :pokemon="playerPokemon">
          <template #title>
            <h5>{{ $t('battle.players') }} ({{ remainingPlayerPokemon.length }}/{{ playerPokemon.length }})</h5>
          </template>
        </battle-pokemon-team>
        <battle-pokemon-team class="col" :canSwitch="isTrainerBattle" :pokemon="opponentPokemon">
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
import { mapActions, mapState } from 'vuex'
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
      location: null,
      pokemon: {},
      trainers: {}
    }
  },
  computed: {
    ...mapState(['battle']),
    isTrainerBattle() {
      return this.opponentTrainers.length > 0
    },
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
      return this.opponentPokemon.filter(x => x?.currentHitPoints > 0)
    },
    remainingPlayerPokemon() {
      return this.playerPokemon.filter(x => x?.currentHitPoints > 0)
    }
  },
  methods: {
    ...mapActions(['removeBattlePokemon', 'saveLocation', 'togglePokemon']),
    onPokemonCaught({ id }) {
      this.removeBattlePokemon(id)
    },
    onPrevious() {
      this.$store.commit('setBattleStep', 'PokemonSelection')
    },
    async refreshPokemon() {
      try {
        const { data } = await getPokemonList({ sort: 'Name', desc: false })
        this.pokemon = {}
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
        this.trainers = {}
        for (const trainer of data.items) {
          Vue.set(this.trainers, trainer.id, trainer)
        }
      } catch (e) {
        this.handleError(e)
      }
    }
  },
  async created() {
    this.refreshTrainers()

    await this.refreshPokemon()

    if (!this.isTrainerBattle) {
      for (const pokemon of this.battle.opponents.pokemon) {
        if (!this.battle.activePokemon.includes(pokemon)) {
          this.togglePokemon(pokemon)
        }
      }
    }

    this.location = this.battle.location ?? null
  }
}
</script>
