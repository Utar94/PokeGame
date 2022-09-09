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
    setTrainers({ commit }, { opponents, players }) {
      commit('setPlayerTrainers', players)
      commit('setOpponentTrainers', opponents)
      commit('setBattleStep', 'PokemonSelection')
    }
  },
  mutations: {
    setOpponentTrainers(state, trainers) {
      state.battle.opponents.trainers = trainers ?? []
    },
    setPlayerTrainers(state, trainers) {
      state.battle.players.trainers = trainers ?? []
    },
    setBattleStep(state, step) {
      state.battle.step = step ?? 'TrainerSelection'
    }
  }
})
