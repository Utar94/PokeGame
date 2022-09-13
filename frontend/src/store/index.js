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
      activePokemon: [],
      escapeAttempts: 0,
      location: null,
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
    battleStep(state) {
      // NOTE(fpion): NEW!
      return state.battle.step
    },
    battlingOpponentPokemon(state) {
      // NOTE(fpion): NEW!
      return state.battle.opponents.pokemon.map(id => state.pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battlingOpponentTrainers(state) {
      // NOTE(fpion): NEW!
      return state.battle.opponents.trainers.map(id => state.trainers[id]).filter(trainer => typeof trainer === 'object' && trainer !== null)
    },
    battlingPlayerPokemon(state) {
      // NOTE(fpion): NEW!
      return state.battle.players.pokemon.map(id => state.pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battlingPlayerTrainers(state) {
      // NOTE(fpion): NEW!
      return state.battle.players.trainers.map(id => state.trainers[id]).filter(trainer => typeof trainer === 'object' && trainer !== null)
    },
    trainers(state) {
      // NOTE(fpion): NEW!
      return Object.values(state.trainers)
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
        case 'TrainerSelection':
          commit('setBattleStep', 'PokemonSelection')
          break
      }
    },
    increaseEscapeAttempts({ commit, state }) {
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
      commit('setEscapeAttempts', 0)
      commit('setLocation', null)
      commit('setActivePokemon', [])
      commit('setPlayerPokemon', [])
      commit('setOpponentTrainers', [])
      commit('setOpponentPokemon', [])
      commit('setBattleStep', 'TrainerSelection')
    },
    resetBattlePokemon({ commit }) {
      commit('setActivePokemon', [])
      commit('setPlayerPokemon', [])
      commit('setOpponentPokemon', [])
      commit('setBattleStep', 'TrainerSelection')
    },
    saveLocation({ commit }, location) {
      commit('setLocation', location)
    },
    setBattlePokemon({ commit }, { opponents, players }) {
      commit('setPlayerPokemon', players)
      commit('setOpponentPokemon', opponents)
      commit('setBattleStep', 'Battle')
    },
    setBattleTrainers({ commit }, { opponents, players }) {
      commit('setPlayerTrainers', players)
      commit('setOpponentTrainers', opponents)
      commit('setBattleStep', 'PokemonSelection')
    },
    toggleBattlingOpponentTrainer({ commit, state }, id) {
      // NOTE(fpion): NEW!
      let trainers = state.battle.opponents.trainers
      trainers = trainers.includes(id) ? trainers.filter(trainer => trainer !== id) : trainers.concat([id])
      commit('setBattlingOpponentTrainers', trainers)
    },
    toggleBattlingPlayerTrainer({ commit, state }, id) {
      // NOTE(fpion): NEW!
      let trainers = state.battle.players.trainers
      trainers = trainers.includes(id) ? trainers.filter(trainer => trainer !== id) : trainers.concat([id])
      commit('setBattlingPlayerTrainers', trainers)
    },
    togglePokemon({ commit, state }, id) {
      commit(
        'setActivePokemon',
        state.battle.activePokemon.includes(id) ? state.battle.activePokemon.filter(x => x !== id) : [...state.battle.activePokemon, id]
      )
    }
  },
  mutations: {
    setActivePokemon(state, activePokemon) {
      state.battle.activePokemon = activePokemon ?? []
    },
    setBattleStep(state, step) {
      // NOTE(fpion): NEW!
      state.battle.step = step ?? 'TrainerSelection'
    },
    setBattlingOpponentTrainers(state, trainers) {
      // NOTE(fpion): NEW!
      state.battle.opponents.trainers = trainers ?? []
    },
    setBattlingPlayerTrainers(state, trainers) {
      // NOTE(fpion): NEW!
      state.battle.players.trainers = trainers ?? []
    },
    setEscapeAttempts(state, escapeAttempts) {
      state.battle.escapeAttempts = escapeAttempts ?? 0
    },
    setLocation(state, location) {
      state.battle.location = location ?? null
    },
    setOpponentPokemon(state, pokemon) {
      state.battle.opponents.pokemon = pokemon ?? []
    },
    setOpponentTrainers(state, trainers) {
      state.battle.opponents.trainers = trainers ?? []
    },
    setPlayerPokemon(state, pokemon) {
      state.battle.players.pokemon = pokemon ?? []
    },
    setPlayerTrainers(state, trainers) {
      state.battle.players.trainers = trainers ?? []
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
