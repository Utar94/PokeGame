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
      opponents: {
        pokemon: [],
        trainers: []
      },
      players: {
        pokemon: [],
        trainers: []
      },
      round: 0,
      step: 'TrainerSelection'
    }
  },
  actions: {
    resetBattleTrainers({ commit }) {
      commit('setPlayerTrainers', [])
      commit('setOpponentTrainers', [])
      commit('setBattleStep', 'TrainerSelection')
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
    }
  },
  mutations: {
    setBattleStep(state, step) {
      state.battle.step = step ?? 'TrainerSelection'
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
