import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'
import effectivenessTable from './effectiveness.json'
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
        condition: {
          accuracy: 0,
          attack: 0,
          defense: 0,
          evasion: 0,
          specialAttack: 0,
          specialDefense: 0,
          speed: 0,
          status: null
        },
        damage: {
          attack: 0,
          burn: false,
          critical: false,
          power: 0,
          random: 0,
          stab: 0,
          weather: ''
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
    battleMoveCondition({ battle }) {
      return battle.move.condition
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
      commit('setBattleStep', 'MakeMove')
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
      commit('setBattleStep', 'Battle')
    },
    toggleActiveBattlingPokemon({ commit, state }, id) {
      let activePokemon = state.battle.activePokemon
      activePokemon = activePokemon.includes(id) ? activePokemon.filter(pokemon => pokemon !== id) : activePokemon.concat([id])
      commit('setActiveBattlingPokemon', activePokemon)
    },
    toggleBattleMove({ commit, state }, move) {
      const { battle } = state
      const { accuracyStage, category, evasionStage, id, name, power, statisticStages, statusCondition, type } = move
      if (battle.move.selected?.id === id) {
        commit('setSelectedBattleMove', null)
        commit('setBattleMoveDamage', null)
        commit('setBattleMoveCondition', null)
      } else {
        commit('setSelectedBattleMove', move)
        if (category === 'Physical' || category === 'Special') {
          const attacker = battle.move.attacker
          const stab = type === attacker.species.primaryType || type === attacker.species.secondaryType
          const damage = {
            attack: category === 'Physical' ? attacker.attack : attacker.specialAttack,
            burn: attacker.statusCondition === 'Burn' && category === 'Physical' && name !== 'Facade' && attacker.ability.name !== 'Guts',
            critical: false,
            power: power ?? 0,
            random: 85 + Math.floor(Math.random() * (15 + 1)),
            stab: stab ? (attacker.ability.name === 'Adaptability' ? 2 : 1.5) : 1,
            weather: 'Normal'
          }
          commit('setBattleMoveDamage', damage)
        } else {
          commit('setBattleMoveDamage', null)
        }
        const condition = {
          accuracy: accuracyStage,
          attack: statisticStages.find(({ statistic }) => statistic === 'Attack')?.value ?? 0,
          defense: statisticStages.find(({ statistic }) => statistic === 'Defense')?.value ?? 0,
          evasion: evasionStage,
          specialAttack: statisticStages.find(({ statistic }) => statistic === 'SpecialAttack')?.value ?? 0,
          specialDefense: statisticStages.find(({ statistic }) => statistic === 'SpecialDefense')?.value ?? 0,
          speed: statisticStages.find(({ statistic }) => statistic === 'Speed')?.value ?? 0,
          status: statusCondition
        }
        commit('setBattleMoveCondition', condition)
      }
      commit('setBattleMoveTargets', null)
    },
    toggleBattleMoveTarget({ commit, state }, pokemon) {
      const targets = { ...state.battle.move.targets }
      if (targets[pokemon.id]) {
        delete targets[pokemon.id]
      } else {
        const modifiers = effectivenessTable[state.battle.move.selected.type] ?? {}
        const effectiveness =
          (modifiers[pokemon.species.primaryType] ?? 1) * (pokemon.species.secondaryType ? modifiers[pokemon.species.secondaryType] ?? 1 : 1)
        const { defense, specialDefense } = pokemon
        targets[pokemon.id] = { pokemon, defense, specialDefense, effectiveness, otherModifiers: 1 }
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
    updateBattleMoveCondition({ commit, state }, { accuracy, attack, defense, evasion, specialAttack, specialDefense, speed, status }) {
      const condition = state.battle.move.condition
      condition.accuracy = accuracy ?? condition.accuracy
      condition.attack = attack ?? condition.attack
      condition.defense = defense ?? condition.defense
      condition.evasion = evasion ?? condition.evasion
      condition.specialAttack = specialAttack ?? condition.specialAttack
      condition.specialDefense = specialDefense ?? condition.specialDefense
      condition.speed = speed ?? condition.speed
      condition.status = typeof status === 'undefined' ? condition.status : status
      commit('setBattleMoveCondition', condition)
    },
    updateBattleMoveDamage({ commit, state }, { attack, burn, critical, power, random, stab, weather }) {
      const damage = state.battle.move.damage
      damage.attack = attack ?? damage.attack
      damage.burn = burn ?? damage.burn
      damage.critical = critical ?? damage.critical
      damage.power = power ?? damage.power
      damage.random = random ?? damage.random
      damage.stab = stab ?? damage.stab
      damage.weather = weather ?? damage.weather
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
        condition: {
          accuracy: 0,
          attack: 0,
          defense: 0,
          evasion: 0,
          specialAttack: 0,
          specialDefense: 0,
          speed: 0,
          status: null
        },
        damage: {
          attack: 0,
          burn: false,
          critical: false,
          power: 0,
          random: 0,
          stab: 0,
          weather: ''
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
    setBattleMoveCondition(state, condition) {
      state.battle.move.condition = condition ?? {
        accuracy: 0,
        attack: 0,
        defense: 0,
        evasion: 0,
        specialAttack: 0,
        specialDefense: 0,
        speed: 0,
        status: null
      }
    },
    setBattleMoveDamage(state, damage) {
      state.battle.move.damage = damage ?? {
        attack: 0,
        burn: false,
        critical: false,
        power: 0,
        random: 0,
        stab: 0,
        weather: ''
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
