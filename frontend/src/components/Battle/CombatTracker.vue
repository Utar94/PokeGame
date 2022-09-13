<template>
  <b-container fluid>
    <h1 v-t="'battle.combatTracker.title'" />
    <div class="my-2">
      <icon-button class="mx-1" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
    </div>
    <b-row v-if="!isTrainerBattle && remainingOpponentPokemon.length">
      <form-field
        class="col-3"
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
      <b-form-group class="col-3">
        <template #label>
          <strong>{{ $t('battle.combatTracker.escape') }} ({{ $t('dcFormat', { dc: escapeDC }) }})</strong>
        </template>
        <icon-button class="mx-1" icon="plus" variant="primary" @click="increaseEscapeAttempts" />
        {{ $t('battle.combatTracker.attemptsFormat', { attempts: battle.escapeAttempts }) }}
      </b-form-group>
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
    <icon-button class="mx-1" icon="chevron-left" text="battle.pokemonSelection.title" variant="warning" @click="onPrevious" />
    <icon-button
      v-if="remainingPlayerPokemon.length > 0 && remainingOpponentPokemon.length === 0"
      class="mx-1"
      icon="crown"
      text="battle.combatTracker.victory"
      variant="primary"
      @click="resetBattle"
    />
    <icon-button
      v-if="remainingPlayerPokemon.length === 0 && remainingOpponentPokemon.length > 0"
      class="mx-1"
      icon="skull"
      text="battle.combatTracker.defeat"
      variant="primary"
      @click="resetBattle"
    />
    <icon-button
      v-if="!isTrainerBattle && remainingPlayerPokemon.length > 0 && remainingOpponentPokemon.length > 0"
      class="mx-1"
      icon="running"
      text="battle.combatTracker.escape"
      variant="primary"
      @click="resetBattle"
    />
    <icon-button class="mx-1" icon="ban" text="actions.cancel" variant="danger" @click="resetBattle" />
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
      loading: false,
      location: null,
      pokemon: {},
      trainers: {}
    }
  },
  computed: {
    ...mapState(['battle']),
    escapeDC() {
      const activePlayerPokemon = this.playerPokemon.filter(x => x && this.battle.activePokemon.includes(x.id))
      const playerSpeed = activePlayerPokemon.reduce((sum, { speed }) => sum + speed, 0) / activePlayerPokemon.length
      const activeOpponentPokemon = this.opponentPokemon.filter(x => x && this.battle.activePokemon.includes(x.id))
      const opponentSpeed = activeOpponentPokemon.reduce((sum, { speed }) => sum + speed, 0) / activeOpponentPokemon.length
      if (!playerSpeed || !opponentSpeed || playerSpeed >= opponentSpeed) {
        return 5
      }
      return Math.ceil((1 - ((Math.floor((playerSpeed * 128) / opponentSpeed) + 30 * this.battle.escapeAttempts) % 256) / 256) * 20) + 5
    },
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
    ...mapActions(['addBattlePlayerPokemon', 'increaseEscapeAttempts', 'removeBattlePokemon', 'resetBattle', 'saveLocation', 'togglePokemon']),
    onPokemonCaught({ id, box }) {
      this.removeBattlePokemon(id)
      if (box === null) {
        this.addBattlePlayerPokemon(id)
        this.toast('success', 'battle.useItem.caughtToast.party')
      } else {
        this.toast('success', 'battle.useItem.caughtToast.box')
      }
    },
    onPrevious() {
      this.$store.commit('setBattleStep', 'PokemonSelection')
    },
    async refresh() {
      if (!this.loading) {
        this.loading = true
        await this.refreshTrainers()
        await this.refreshPokemon()
        this.loading = false
      }
    },
    async refreshPokemon() {
      try {
        const { data } = await getPokemonList()
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
