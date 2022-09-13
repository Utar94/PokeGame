import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'
import { getPokemonList } from '@/api/pokemon'
import { getTrainers } from '@/api/trainers'

Vue.use(Vuex)

const vuexLocal = new VuexPersistence({
  storage: window.localStorage
})

export default new Vuex.Store({
  plugins: [vuexLocal.plugin],
  state: {
    battle: {
      activePokemon: [], // NOTE(fpion): NEW!
      escapeAttempts: 0, // NOTE(fpion): NEW!
      location: null, // NOTE(fpion): NEW!
      opponents: {
        pokemon: [], // NOTE(fpion): NEW!
        trainers: [] // NOTE(fpion): NEW!
      },
      players: {
        pokemon: [], // NOTE(fpion): NEW!
        trainers: [] // NOTE(fpion): NEW!
      },
      step: 'TrainerSelection' // NOTE(fpion): NEW!
    },
    pokemonList: {}, // NOTE(fpion): NEW!
    trainers: {} // NOTE(fpion): NEW!
  },
  getters: {
    activeBattlingOpponentPokemon({ battle }, { battlingOpponentPokemon }) {
      // NOTE(fpion): NEW!
      return battlingOpponentPokemon.filter(({ id }) => battle.activePokemon.includes(id))
    },
    activeBattlingPlayerPokemon({ battle }, { battlingPlayerPokemon }) {
      // NOTE(fpion): NEW!
      return battlingPlayerPokemon.filter(({ id }) => battle.activePokemon.includes(id))
    },
    activeBattlingPokemon({ battle, pokemonList }) {
      // NOTE(fpion): NEW!
      return battle.activePokemon.map(id => pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battleEscapeAttempts({ battle }) {
      // NOTE(fpion): NEW!
      return battle.escapeAttempts
    },
    battleLocation({ battle }) {
      // NOTE(fpion): NEW!
      return battle.location
    },
    battleStep({ battle }) {
      // NOTE(fpion): NEW!
      return battle.step
    },
    battlingOpponentPokemon({ battle, pokemonList }) {
      // NOTE(fpion): NEW!
      return battle.opponents.pokemon.map(id => pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battlingOpponentTrainers({ battle, trainers }) {
      // NOTE(fpion): NEW!
      return battle.opponents.trainers.map(id => trainers[id]).filter(trainer => typeof trainer === 'object' && trainer !== null)
    },
    battlingPlayerPokemon({ battle, pokemonList }) {
      // NOTE(fpion): NEW!
      return battle.players.pokemon.map(id => pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battlingPlayerTrainers({ battle, trainers }) {
      // NOTE(fpion): NEW!
      return battle.players.trainers.map(id => trainers[id]).filter(trainer => typeof trainer === 'object' && trainer !== null)
    },
    isTrainerBattle({ battle }) {
      // NOTE(fpion): NEW!
      return battle.opponents.trainers.length > 0
    },
    pokemonList({ pokemonList }) {
      // NOTE(fpion): NEW!
      return Object.values(pokemonList)
    },
    remainingBattlingOpponentPokemon(_, { battlingOpponentPokemon }) {
      // NOTE(fpion): NEW!
      return battlingOpponentPokemon.filter(({ currentHitPoints }) => currentHitPoints > 0)
    },
    remainingBattlingPlayerPokemon(_, { battlingPlayerPokemon }) {
      // NOTE(fpion): NEW!
      return battlingPlayerPokemon.filter(({ currentHitPoints }) => currentHitPoints > 0)
    },
    trainers({ trainers }) {
      // NOTE(fpion): NEW!
      return Object.values(trainers)
    }
  },
  actions: {
    addBattlePlayerPokemon({ commit, state }, id) {
      if (!state.battle.players.pokemon.includes(id)) {
        commit('setPlayerPokemon', state.battle.players.pokemon.concat([id]))
      }
    },
    battleNext({ commit, state }) {
      // NOTE(fpion): NEW!
      switch (state.battle.step) {
        case 'PokemonSelection':
          commit('setBattleStep', 'Battle')
          break
        case 'TrainerSelection':
          commit('setBattleStep', 'PokemonSelection')
          break
      }
    },
    battlePrevious({ commit, state }) {
      // NOTE(fpion): NEW!
      switch (state.battle.step) {
        case 'Battle':
          commit('setActiveBattlingPokemon', [])
          commit('setBattleStep', 'PokemonSelection')
          break
        case 'PokemonSelection':
          commit('setBattleStep', 'TrainerSelection')
          break
      }
    },
    increaseEscapeAttempts({ commit, state }) {
      // NOTE(fpion): NEW!
      commit('setEscapeAttempts', state.battle.escapeAttempts + 1)
    },
    async loadPokemonList({ commit }) {
      // NOTE(fpion): NEW!
      const { data } = await getPokemonList()
      const pokemonList = {}
      for (const item of data.items) {
        pokemonList[item.id] = item
      }
      commit('setPokemonList', pokemonList)
    },
    async loadTrainers({ commit }) {
      // NOTE(fpion): NEW!
      const { data } = await getTrainers()
      const trainers = {}
      for (const item of data.items) {
        trainers[item.id] = item
      }
      commit('setTrainers', trainers)
    },
    removeBattlePokemon({ commit, state }, id) {
      if (state.battle.activePokemon.includes(id)) {
        commit(
          'setActivePokemon',
          state.battle.activePokemon.filter(x => x !== id)
        )
      }
      if (state.battle.opponents.pokemon.includes(id)) {
        commit(
          'setOpponentPokemon',
          state.battle.opponents.pokemon.filter(x => x !== id)
        )
      }
      if (state.battle.players.pokemon.includes(id)) {
        commit(
          'setOpponentPokemon',
          state.battle.players.pokemon.filter(x => x !== id)
        )
      }
    },
    resetBattle({ commit }) {
      commit('setActiveBattlingPokemon', [])
      commit('setBattleLocation', null)
      commit('setBattlingPlayerPokemon', [])
      commit('setBattlingOpponentTrainers', [])
      commit('setBattlingOpponentPokemon', [])
      commit('setEscapeAttempts', 0)
      commit('setBattleStep', 'TrainerSelection')
    },
    saveBattleLocation({ commit }, location) {
      // NOTE(fpion): NEW!
      commit('setBattleLocation', location.length > 100 ? location.substr(0, 100) : location)
    },
    toggleActiveBattlingPokemon({ commit, state }, id) {
      // NOTE(fpion): NEW!
      let activePokemon = state.battle.activePokemon
      activePokemon = activePokemon.includes(id) ? activePokemon.filter(pokemon => pokemon !== id) : activePokemon.concat([id])
      commit('setActiveBattlingPokemon', activePokemon)
    },
    toggleBattlingOpponentPokemon({ commit, state }, id) {
      // NOTE(fpion): NEW!
      let pokemon = state.battle.opponents.pokemon
      pokemon = pokemon.includes(id) ? pokemon.filter(trainer => trainer !== id) : pokemon.concat([id])
      commit('setBattlingOpponentPokemon', pokemon)
    },
    toggleBattlingOpponentTrainer({ commit, state }, id) {
      // NOTE(fpion): NEW!
      let trainers = state.battle.opponents.trainers
      trainers = trainers.includes(id) ? trainers.filter(trainer => trainer !== id) : trainers.concat([id])
      commit('setBattlingOpponentTrainers', trainers)
    },
    toggleBattlingPlayerPokemon({ commit, state }, id) {
      // NOTE(fpion): NEW!
      let pokemon = state.battle.players.pokemon
      pokemon = pokemon.includes(id) ? pokemon.filter(trainer => trainer !== id) : pokemon.concat([id])
      commit('setBattlingPlayerPokemon', pokemon)
    },
    toggleBattlingPlayerTrainer({ commit, state }, id) {
      // NOTE(fpion): NEW!
      let trainers = state.battle.players.trainers
      trainers = trainers.includes(id) ? trainers.filter(trainer => trainer !== id) : trainers.concat([id])
      commit('setBattlingPlayerTrainers', trainers)
    }
  },
  mutations: {
    setActiveBattlingPokemon(state, activePokemon) {
      // NOTE(fpion): NEW!
      state.battle.activePokemon = activePokemon ?? []
    },
    setBattleLocation(state, location) {
      // NOTE(fpion): NEW!
      state.battle.location = location ?? null
    },
    setBattleStep(state, step) {
      // NOTE(fpion): NEW!
      state.battle.step = step ?? 'TrainerSelection'
    },
    setBattlingOpponentPokemon(state, pokemon) {
      // NOTE(fpion): NEW!
      state.battle.opponents.pokemon = pokemon ?? []
    },
    setBattlingOpponentTrainers(state, trainers) {
      // NOTE(fpion): NEW!
      state.battle.opponents.trainers = trainers ?? []
    },
    setBattlingPlayerPokemon(state, pokemon) {
      // NOTE(fpion): NEW!
      state.battle.players.pokemon = pokemon ?? []
    },
    setBattlingPlayerTrainers(state, trainers) {
      // NOTE(fpion): NEW!
      state.battle.players.trainers = trainers ?? []
    },
    setEscapeAttempts(state, escapeAttempts) {
      // NOTE(fpion): NEW!
      state.battle.escapeAttempts = escapeAttempts ?? 0
    },
    setOpponentPokemon(state, pokemon) {
      state.battle.opponents.pokemon = pokemon ?? []
    },
    setPlayerPokemon(state, pokemon) {
      state.battle.players.pokemon = pokemon ?? []
    },
    setPokemonList(state, pokemonList) {
      // NOTE(fpion): NEW!
      state.pokemonList = pokemonList || {}
    },
    setTrainers(state, trainers) {
      // NOTE(fpion): NEW!
      state.trainers = trainers || {}
    }
  }
})
