import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'

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
    }
  },
  actions: {
    addBattlePlayerPokemon({ commit, state }, id) {
      if (!state.battle.players.pokemon.includes(id)) {
        commit('setPlayerPokemon', state.battle.players.pokemon.concat([id]))
      }
    },
    increaseEscapeAttempts({ commit, state }) {
      commit('setEscapeAttempts', state.battle.escapeAttempts + 1)
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
      state.battle.step = step ?? 'TrainerSelection'
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
    }
  }
})
