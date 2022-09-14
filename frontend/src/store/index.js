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
      move: {
        attacker: null,
        damage: {
          attack: 0,
          burn: false,
          critical: false,
          power: 0,
          random: 0,
          stab: 0
        },
        selected: null,
        targets: {}
      },
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
    battleMoveAttacker({ battle }) {
      return battle.move.attacker
    },
    battleMoveDamage({ battle }) {
      return battle.move.damage
    },
    battleMoveTargets({ battle }) {
      return battle.move.targets
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
    selectedBattleMove({ battle }) {
      return battle.move.selected
    },
    selectedBattleMoveCategory(_, { selectedBattleMove }) {
      return selectedBattleMove?.category ?? null
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
    makeBattleMove({ commit, state }, pokemon) {
      if (pokemon.id !== state.battle.move.attacker?.id) {
        commit('resetBattleMove')
        commit('setBattleMoveAttacker', pokemon)
      }
    },
    resetBattle({ commit }) {
      commit('resetBattleMove')
      commit('setActiveBattlingPokemon', [])
      commit('setBattleLocation', null)
      commit('setBattlingPlayerPokemon', [])
      commit('setBattlingOpponentTrainers', [])
      commit('setBattlingOpponentPokemon', [])
      commit('setEscapeAttempts', 0)
      commit('setBattleStep', 'TrainerSelection')
    },
    resetBattleMove({ commit }) {
      commit('resetBattleMove')
    },
    toggleActiveBattlingPokemon({ commit, state }, id) {
      let activePokemon = state.battle.activePokemon
      activePokemon = activePokemon.includes(id) ? activePokemon.filter(pokemon => pokemon !== id) : activePokemon.concat([id])
      commit('setActiveBattlingPokemon', activePokemon)
    },
    toggleBattleMove({ commit, state }, move) {
      const { battle } = state
      if (battle.move.selected?.id === move.id) {
        commit('setSelectedBattleMove', null)
        commit('setBattleMoveDamage', null)
      } else {
        commit('setSelectedBattleMove', move)
        if (move.category === 'Physical' || move.category === 'Special') {
          const damage = {
            attack: move.category === 'Physical' ? battle.move.attacker.attack : battle.move.attacker.specialAttack,
            burn: false,
            critical: false,
            power: move.power ?? 0,
            random: 85 + Math.floor(Math.random() * (15 + 1)),
            stab: move.type === battle.move.attacker.species.primaryType || move.type === battle.move.attacker.species.secondaryType ? 1.5 : 1
          }
          commit('setBattleMoveDamage', damage)
        } else {
          commit('setBattleMoveDamage', null)
        }
      }
    },
    toggleBattleMoveTarget({ commit, state }, pokemon) {
      const targets = { ...state.battle.move.targets }
      if (targets[pokemon.id]) {
        delete targets[pokemon.id]
      } else {
        targets[pokemon.id] = { pokemon, defense: pokemon.defense, specialDefense: pokemon.specialDefense, effectiveness: 1, otherModifiers: 1 }
      }
      commit('setBattleMoveTargets', targets)
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
    updateBattleLocation({ commit }, location) {
      commit('setBattleLocation', location.length > 100 ? location.substr(0, 100) : location)
    },
    updateBattleMoveDamage({ commit, state }, { attack, burn, critical, power, random, stab }) {
      const damage = state.battle.move.damage
      damage.attack = attack ?? damage.attack
      damage.burn = burn ?? damage.burn
      damage.critical = critical ?? damage.critical
      damage.power = power ?? damage.power
      damage.random = random ?? damage.random
      damage.stab = stab ?? damage.stab
      commit('setBattleMoveDamage', damage)
    },
    updateBattleTargetDefense({ commit, state }, { id, value }) {
      let target = state.battle.move.targets[id]
      if (target) {
        if (state.battle.move.selected.category === 'Physical') {
          target = { ...target, defense: value }
        } else {
          target = { ...target, specialDefense: value }
        }
        commit('setBattleMoveTarget', { id, target })
      }
    },
    updateBattleTargetEffectiveness({ commit, state }, { id, value }) {
      let target = state.battle.move.targets[id]
      if (target) {
        target = { ...target, effectiveness: value }
        commit('setBattleMoveTarget', { id, target })
      }
    },
    updateBattleTargetOtherModifiers({ commit, state }, { id, value }) {
      let target = state.battle.move.targets[id]
      if (target) {
        target = { ...target, otherModifiers: value }
        commit('setBattleMoveTarget', { id, target })
      }
    },
    updatePokemon({ commit, state }, pokemon) {
      const pokemonList = { ...state.pokemonList }
      pokemonList[pokemon.id] = pokemon
      commit('setPokemonList', pokemonList)
    }
  },
  mutations: {
    resetBattleMove(state) {
      state.battle.move = {
        attacker: state.battle.move.attacker ?? null,
        damage: {
          attack: 0,
          burn: false,
          critical: false,
          power: 0,
          random: 0,
          stab: 0
        },
        selected: null,
        targets: {}
      }
    },
    setActiveBattlingPokemon(state, activePokemon) {
      state.battle.activePokemon = activePokemon ?? []
    },
    setBattleLocation(state, location) {
      state.battle.location = location ?? null
    },
    setBattleMoveAttacker(state, attacker) {
      state.battle.move.attacker = attacker ?? null
    },
    setBattleMoveDamage(state, damage) {
      state.battle.move.damage = damage ?? {
        attack: 0,
        burn: false,
        critical: false,
        power: 0,
        random: 0,
        stab: 0
      }
    },
    setBattleMoveTarget(state, { id, target }) {
      state.battle.move.targets[id] = target
    },
    setBattleMoveTargets(state, targets) {
      state.battle.move.targets = targets ?? {}
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
    setSelectedBattleMove(state, selected) {
      state.battle.move.selected = selected ?? null
    },
    setTrainers(state, trainers) {
      state.trainers = trainers || {}
    }
  }
})
