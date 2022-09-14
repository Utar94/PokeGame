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
        pokemon: [],
        trainers: []
      },
      players: {
        pokemon: [],
        trainers: []
      },
      step: 'TrainerSelection'
    },
    pokemonList: {},
    trainers: {}
  },
  getters: {
    activeBattlingOpponentPokemon({ battle }, { battlingOpponentPokemon }) {
      return battlingOpponentPokemon.filter(({ id }) => battle.activePokemon.includes(id))
    },
    activeBattlingPlayerPokemon({ battle }, { battlingPlayerPokemon }) {
      return battlingPlayerPokemon.filter(({ id }) => battle.activePokemon.includes(id))
    },
    activeBattlingPokemon({ battle, pokemonList }) {
      return battle.activePokemon.map(id => pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battleEscapeAttempts({ battle }) {
      return battle.escapeAttempts
    },
    battleLocation({ battle }) {
      return battle.location
    },
    battleStep({ battle }) {
      return battle.step
    },
    battlingOpponentPokemon({ battle, pokemonList }) {
      return battle.opponents.pokemon.map(id => pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battlingOpponentTrainers({ battle, trainers }) {
      return battle.opponents.trainers.map(id => trainers[id]).filter(trainer => typeof trainer === 'object' && trainer !== null)
    },
    battlingPlayerPokemon({ battle, pokemonList }) {
      return battle.players.pokemon.map(id => pokemonList[id]).filter(pokemon => typeof pokemon === 'object' && pokemon !== null)
    },
    battlingPlayerTrainers({ battle, trainers }) {
      return battle.players.trainers.map(id => trainers[id]).filter(trainer => typeof trainer === 'object' && trainer !== null)
    },
    hasEnded(_, { remainingBattlingOpponentPokemon, remainingBattlingPlayerPokemon }) {
      return remainingBattlingPlayerPokemon.length === 0 || remainingBattlingOpponentPokemon.length === 0
    },
    isTrainerBattle({ battle }) {
      return battle.opponents.trainers.length > 0
    },
    pokemonList({ pokemonList }) {
      return Object.values(pokemonList)
    },
    remainingBattlingOpponentPokemon(_, { battlingOpponentPokemon }) {
      return battlingOpponentPokemon.filter(({ currentHitPoints }) => currentHitPoints > 0)
    },
    remainingBattlingPlayerPokemon(_, { battlingPlayerPokemon }) {
      return battlingPlayerPokemon.filter(({ currentHitPoints }) => currentHitPoints > 0)
    },
    trainers({ trainers }) {
      return Object.values(trainers)
    }
  },
  actions: {
    battleNext({ commit, state }) {
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
      commit('setEscapeAttempts', state.battle.escapeAttempts + 1)
    },
    async loadPokemonList({ commit }) {
      const { data } = await getPokemonList()
      const pokemonList = {}
      for (const item of data.items) {
        pokemonList[item.id] = item
      }
      commit('setPokemonList', pokemonList)
    },
    async loadTrainers({ commit }) {
      const { data } = await getTrainers()
      const trainers = {}
      for (const item of data.items) {
        trainers[item.id] = item
      }
      commit('setTrainers', trainers)
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
      commit('setBattleLocation', location.length > 100 ? location.substr(0, 100) : location)
    },
    toggleActiveBattlingPokemon({ commit, state }, id) {
      let activePokemon = state.battle.activePokemon
      activePokemon = activePokemon.includes(id) ? activePokemon.filter(pokemon => pokemon !== id) : activePokemon.concat([id])
      commit('setActiveBattlingPokemon', activePokemon)
    },
    toggleBattlingOpponentPokemon({ commit, state }, id) {
      let pokemon = state.battle.opponents.pokemon
      pokemon = pokemon.includes(id) ? pokemon.filter(trainer => trainer !== id) : pokemon.concat([id])
      commit('setBattlingOpponentPokemon', pokemon)
      if (state.battle.activePokemon.includes(id)) {
        commit(
          'setActiveBattlingPokemon',
          state.battle.activePokemon.filter(pokemon => pokemon !== id)
        )
      }
    },
    toggleBattlingOpponentTrainer({ commit, state }, id) {
      let trainers = state.battle.opponents.trainers
      trainers = trainers.includes(id) ? trainers.filter(trainer => trainer !== id) : trainers.concat([id])
      commit('setBattlingOpponentTrainers', trainers)
    },
    toggleBattlingPlayerPokemon({ commit, state }, id) {
      let pokemon = state.battle.players.pokemon
      pokemon = pokemon.includes(id) ? pokemon.filter(trainer => trainer !== id) : pokemon.concat([id])
      commit('setBattlingPlayerPokemon', pokemon)
    },
    toggleBattlingPlayerTrainer({ commit, state }, id) {
      let trainers = state.battle.players.trainers
      trainers = trainers.includes(id) ? trainers.filter(trainer => trainer !== id) : trainers.concat([id])
      commit('setBattlingPlayerTrainers', trainers)
    },
    updatePokemon({ commit, state }, pokemon) {
      const pokemonList = { ...state.pokemonList }
      pokemonList[pokemon.id] = pokemon
      commit('setPokemonList', pokemonList)
    }
  },
  mutations: {
    setActiveBattlingPokemon(state, activePokemon) {
      state.battle.activePokemon = activePokemon ?? []
    },
    setBattleLocation(state, location) {
      state.battle.location = location ?? null
    },
    setBattleStep(state, step) {
      state.battle.step = step ?? 'TrainerSelection'
    },
    setBattlingOpponentPokemon(state, pokemon) {
      state.battle.opponents.pokemon = pokemon ?? []
    },
    setBattlingOpponentTrainers(state, trainers) {
      state.battle.opponents.trainers = trainers ?? []
    },
    setBattlingPlayerPokemon(state, pokemon) {
      state.battle.players.pokemon = pokemon ?? []
    },
    setBattlingPlayerTrainers(state, trainers) {
      state.battle.players.trainers = trainers ?? []
    },
    setEscapeAttempts(state, escapeAttempts) {
      state.battle.escapeAttempts = escapeAttempts ?? 0
    },
    setPokemonList(state, pokemonList) {
      state.pokemonList = pokemonList || {}
    },
    setTrainers(state, trainers) {
      state.trainers = trainers || {}
    }
  }
})
