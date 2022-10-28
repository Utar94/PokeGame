import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'
import effectivenessTable from './effectiveness.json'
import { getGameInventory, getGamePokedex, getGamePokemon, getGameTrainer, getGameTrainers } from '@/api/game'
import { getPokemonList } from '@/api/pokemon'
import { getStatisticModifier } from '@/helpers/statisticUtils'
import { getTrainers } from '@/api/trainers'
import { seenSpecies } from '@/api/pokedex'

function getStageChange(value, change) {
  value += change
  if (value > 6) {
    return 6
  } else if (value < -6) {
    return -6
  }
  return value
}

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
      experience: {
        defeatedPokemon: [],
        winners: {}
      },
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
          status: null,
          volatile: null
        },
        damage: {
          attack: 0,
          burn: false,
          critical: false,
          power: 0,
          random: 0,
          stab: 0,
          weather: null
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
      seenSpecies: {},
      status: {},
      step: 'TrainerSelection'
    },
    game: {
      box: 1,
      inventory: {},
      page: null,
      pokedex: {
        entries: [],
        hasNational: false
      },
      pokemon: [],
      trainer: null,
      trainers: {}
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
    battleExperienceDefeatedPokemon({ battle }) {
      return battle.experience.defeatedPokemon[0] ?? null
    },
    battleExperienceWinners({ battle }) {
      return battle.experience.winners
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
    battleStatus({ battle }) {
      return battle.status
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
    gameBox({ game }) {
      return game.box
    },
    gameInventory({ game }) {
      return game.inventory
    },
    gamePage({ game }) {
      return game.page
    },
    gamePokedex({ game }) {
      return game.pokedex
    },
    gamePokemon({ game }) {
      return game.pokemon
    },
    gameTrainer({ game }) {
      return game.trainer
    },
    gameTrainers({ game }) {
      return Object.values(game.trainers)
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
    applyBattleMove({ commit, state }) {
      const { battle } = state
      const { accuracy, attack, defense, evasion, specialAttack, specialDefense, speed, volatile } = battle.move.condition
      const volatileConditions =
        volatile
          ?.split(',')
          .map(condition => condition.trim())
          .filter(condition => condition.length > 0) ?? []
      for (const id of Object.keys(battle.move.targets)) {
        const status = {
          ...(battle.status[id] ?? {
            accuracy: 0,
            attack: 0,
            defense: 0,
            evasion: 0,
            specialAttack: 0,
            specialDefense: 0,
            speed: 0,
            volatile: []
          })
        }
        status.accuracy = getStageChange(status.accuracy, accuracy)
        status.attack = getStageChange(status.attack, attack)
        status.defense = getStageChange(status.defense, defense)
        status.evasion = getStageChange(status.evasion, evasion)
        status.specialAttack = getStageChange(status.specialAttack, specialAttack)
        status.specialDefense = getStageChange(status.specialDefense, specialDefense)
        status.speed = getStageChange(status.speed, speed)
        for (const condition of volatileConditions) {
          if (!status.volatile.includes(condition)) {
            status.volatile.push(condition)
          }
        }
        commit('setBattleStatus', { id, status })
      }
    },
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
        case 'Experience':
          commit('resetBattleExperienceDistribution')
          commit('setBattleStep', 'Battle')
          break
        case 'PokemonSelection':
          commit('setBattleStep', 'TrainerSelection')
          break
      }
    },
    distributeExperience({ commit, dispatch, getters, state }, pokemonList) {
      if (pokemonList.length > 0) {
        commit('setExperienceDefeatedPokemon', pokemonList)
        const active = Object.fromEntries(state.battle.activePokemon.map(id => [id, true]))
        for (const pokemon of getters.battlingPlayerPokemon) {
          if (pokemon.currentHitPoints > 0 && (active[pokemon.id] || pokemon.heldItem?.kind === 'ExpShare')) {
            dispatch('toggleBattleExperienceWinner', pokemon)
          }
        }
        commit('setBattleStep', 'Experience')
      }
    },
    async endBattle({ dispatch, state }) {
      const { battle } = state
      const speciesIds = Object.keys(battle.seenSpecies)
      const trainerIds = battle.players.trainers
      if (speciesIds.length > 0 && trainerIds.length > 0) {
        await seenSpecies({ speciesIds, trainerIds })
      }
      dispatch('resetBattle')
    },
    increaseEscapeAttempts({ commit, state }) {
      commit('setEscapeAttempts', state.battle.escapeAttempts + 1)
    },
    async loadGameInventory({ commit, state }) {
      const { data } = await getGameInventory(state.game.trainer.id)
      commit('setGameInventory', data)
    },
    async loadGamePokedex({ commit, state }, national = false) {
      const { data } = await getGamePokedex(state.game.trainer.id, { national })
      commit('setGamePokedex', data)
    },
    async loadGamePokemon({ commit, state }) {
      const { data } = await getGamePokemon(state.game.trainer.id)
      commit('setGamePokemon', data)
    },
    async loadGameTrainer({ dispatch, state }) {
      if (state.game.trainer) {
        const { data } = await getGameTrainer(state.game.trainer.id)
        dispatch('setGameTrainer', data)
      } else {
        dispatch('setGameTrainer', null)
      }
    },
    async loadGameTrainers({ commit, dispatch, state }) {
      const { data } = await getGameTrainers()
      const trainers = {}
      for (const trainer of data) {
        trainers[trainer.id] = trainer
      }
      commit('setGameTrainers', trainers)
      if (state.game.trainer && !trainers[state.game.trainer.id]) {
        dispatch('setGameTrainer', null)
      }
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
    navigateGame({ commit }, page) {
      commit('setGamePage', page)
    },
    nextExperienceDistribution({ commit, dispatch, state }) {
      const defeatedPokemon = state.battle.experience.defeatedPokemon.slice(1)
      if (defeatedPokemon.length > 0) {
        commit('setExperienceDefeatedPokemon', defeatedPokemon)
      } else {
        dispatch('battlePrevious')
      }
    },
    nextGameBox({ commit, state }) {
      const { box } = state.game
      commit('setGameBox', box === 32 ? 1 : box + 1)
    },
    previousGameBox({ commit, state }) {
      const { box } = state.game
      commit('setGameBox', box == 1 ? 32 : box - 1)
    },
    resetBattle({ commit }) {
      commit('resetBattleMove')
      commit('resetBattleStatus')
      commit('resetBattleExperienceDistribution')
      commit('resetBattleSeenSpecies')
      commit('setActiveBattlingPokemon', [])
      commit('setBattleLocation', null)
      commit('setBattlingPlayerPokemon', [])
      commit('setBattlingOpponentTrainers', [])
      commit('setBattlingOpponentPokemon', [])
      commit('setEscapeAttempts', 0)
      commit('setBattleStep', 'TrainerSelection')
    },
    resetBattleMove({ commit }, keepCurrentStep = false) {
      commit('resetBattleMove')
      if (!keepCurrentStep) {
        commit('setBattleStep', 'Battle')
      }
    },
    resetGame({ commit, dispatch }) {
      dispatch('setGameTrainer', null)
      commit('setGameTrainers', null)
    },
    setGameTrainer({ commit }, trainer) {
      commit('setGameTrainer', trainer)
      commit('setGameBox', null)
      commit('setGamePage', null)
      commit('setGameInventory', null)
      commit('setGamePokedex', null)
      commit('setGamePokemon', null)
    },
    toggleActiveBattlingPokemon({ commit, state }, pokemon) {
      const { id, species } = pokemon
      let activePokemon = state.battle.activePokemon
      if (activePokemon.includes(id)) {
        activePokemon = activePokemon.filter(pokemon => pokemon !== id)
      } else {
        activePokemon = activePokemon.concat([id])
        if (species) {
          commit('seenSpeciesInBattle', species.id)
        }
      }
      commit('setActiveBattlingPokemon', activePokemon)
      commit('setBattleStatus', { id })
    },
    toggleBattleExperienceWinner({ commit, state }, pokemon) {
      const { battle } = state
      const winners = { ...battle.experience.winners }
      if (winners[pokemon.id]) {
        delete winners[pokemon.id]
      } else {
        const hasParticipated = battle.activePokemon.includes(pokemon.id)
        winners[pokemon.id] = { pokemon, canEvolve: false, hasParticipated, otherModifiers: 1 }
      }
      commit('setBattleExperienceWinners', winners)
    },
    toggleBattleMove({ commit, state }, move) {
      const { battle } = state
      const { accuracyStage, category, evasionStage, id, kind, power, statisticStages, statusCondition, type, volatileConditions } = move
      if (battle.move.selected?.id === id) {
        commit('setSelectedBattleMove', null)
        commit('setBattleMoveDamage', null)
        commit('setBattleMoveCondition', null)
      } else {
        commit('setSelectedBattleMove', move)
        if (category === 'Physical' || category === 'Special') {
          const attacker = battle.move.attacker
          const status = battle.status[attacker.id] ?? {}
          const attack = Math.floor(
            category === 'Physical'
              ? attacker.attack * getStatisticModifier(status.attack ?? 0)
              : attacker.specialAttack * getStatisticModifier(status.specialAttack ?? 0)
          )
          const stab = type === attacker.species.primaryType || type === attacker.species.secondaryType
          const damage = {
            attack,
            burn: attacker.statusCondition === 'Burn' && category === 'Physical' && kind !== 'Facade' && attacker.ability.kind !== 'Guts',
            critical: false,
            power: power ?? 0,
            random: 85 + Math.floor(Math.random() * (15 + 1)),
            stab: stab ? (attacker.ability.kind === 'Adaptability' ? 2 : 1.5) : 1,
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
          status: statusCondition,
          volatile: volatileConditions?.join(', ') ?? null
        }
        commit('setBattleMoveCondition', condition)
      }
      commit('setBattleMoveTargets', null)
    },
    toggleBattleMoveTarget({ commit, state }, pokemon) {
      const { battle } = state
      const targets = { ...battle.move.targets }
      if (targets[pokemon.id]) {
        delete targets[pokemon.id]
      } else {
        const modifiers = effectivenessTable[battle.move.selected.type] ?? {}
        const effectiveness =
          (modifiers[pokemon.species.primaryType] ?? 1) * (pokemon.species.secondaryType ? modifiers[pokemon.species.secondaryType] ?? 1 : 1)
        const status = battle.status[pokemon.id] ?? {}
        const defense = Math.floor(pokemon.defense * getStatisticModifier(status.defense ?? 0))
        const specialDefense = Math.floor(pokemon.specialDefense * getStatisticModifier(status.specialDefense ?? 0))
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
    toggleExperienceWinnerEvolution({ commit, state }, id) {
      let winner = state.battle.experience.winners[id]
      if (winner) {
        winner = { ...winner, canEvolve: !winner.canEvolve }
        commit('setBattleExperienceWinner', { id, winner })
      }
    },
    toggleExperienceWinnerParticipation({ commit, state }, id) {
      let winner = state.battle.experience.winners[id]
      if (winner) {
        winner = { ...winner, hasParticipated: !winner.hasParticipated }
        commit('setBattleExperienceWinner', { id, winner })
      }
    },
    updateBattleLocation({ commit }, location) {
      commit('setBattleLocation', location.length > 100 ? location.substr(0, 100) : location)
    },
    updateBattleMoveCondition({ commit, state }, { accuracy, attack, defense, evasion, specialAttack, specialDefense, speed, status, volatile }) {
      const condition = state.battle.move.condition
      condition.accuracy = accuracy ?? condition.accuracy
      condition.attack = attack ?? condition.attack
      condition.defense = defense ?? condition.defense
      condition.evasion = evasion ?? condition.evasion
      condition.specialAttack = specialAttack ?? condition.specialAttack
      condition.specialDefense = specialDefense ?? condition.specialDefense
      condition.speed = speed ?? condition.speed
      condition.status = typeof status === 'undefined' ? condition.status : status
      condition.volatile = volatile ?? condition.volatile
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
    updateBattlingPokemonStatus({ commit, state }, { id, accuracy, attack, defense, evasion, specialAttack, specialDefense, speed, volatile }) {
      const status = { ...(state.battle.status[id] ?? {}) }
      status.accuracy = accuracy ?? status.accuracy
      status.attack = attack ?? status.attack
      status.defense = defense ?? status.defense
      status.evasion = evasion ?? status.evasion
      status.specialAttack = specialAttack ?? status.specialAttack
      status.specialDefense = specialDefense ?? status.specialDefense
      status.speed = speed ?? status.speed
      status.volatile =
        volatile
          ?.split(',')
          .map(condition => condition.trim())
          .filter(condition => condition.length > 0) ?? status.volatile
      commit('setBattleStatus', { id, status })
    },
    updateExperienceWinnerOtherModifiers({ commit, state }, { id, value }) {
      let winner = state.battle.experience.winners[id]
      if (winner) {
        winner = { ...winner, otherModifiers: value }
        commit('setBattleExperienceWinner', { id, winner })
      }
    },
    updatePokemon({ commit, state }, pokemon) {
      const pokemonList = { ...state.pokemonList }
      pokemonList[pokemon.id] = pokemon
      commit('setPokemonList', pokemonList)
    }
  },
  mutations: {
    resetBattleExperienceDistribution(state) {
      state.battle.experience = {
        defeatedPokemon: [],
        winners: {}
      }
    },
    resetBattleMove(state) {
      state.battle.move = {
        attacker: null,
        condition: {
          accuracy: 0,
          attack: 0,
          defense: 0,
          evasion: 0,
          specialAttack: 0,
          specialDefense: 0,
          speed: 0,
          status: null,
          volatile: null
        },
        damage: {
          attack: 0,
          burn: false,
          critical: false,
          power: 0,
          random: 0,
          stab: 0,
          weather: null
        },
        selected: null,
        targets: {}
      }
    },
    resetBattleSeenSpecies(state) {
      state.battle.seenSpecies = {}
    },
    resetBattleStatus(state) {
      state.battle.status = {}
    },
    seenSpeciesInBattle(state, speciesId) {
      if (speciesId) {
        state.battle.seenSpecies[speciesId] = true
      }
    },
    setActiveBattlingPokemon(state, activePokemon) {
      state.battle.activePokemon = activePokemon ?? []
    },
    setBattleExperienceWinner(state, { id, winner }) {
      if (winner) {
        state.battle.experience.winners[id] = winner
      } else {
        delete state.battle.experience.winners[id]
      }
    },
    setBattleExperienceWinners(state, winners) {
      state.battle.experience.winners = winners ?? {}
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
        status: null,
        volatile: null
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
        weather: null
      }
    },
    setBattleMoveTarget(state, { id, target }) {
      if (target) {
        state.battle.move.targets[id] = target
      } else {
        delete state.battle.move.targets[id]
      }
    },
    setBattleMoveTargets(state, targets) {
      state.battle.move.targets = targets ?? {}
    },
    setBattleStatus(state, { id, status }) {
      if (status) {
        state.battle.status[id] = status
      } else {
        delete state.battle.status[id]
      }
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
    setExperienceDefeatedPokemon(state, defeatedPokemon) {
      state.battle.experience.defeatedPokemon = defeatedPokemon ?? []
    },
    setGameBox(state, box) {
      state.game.box = box ?? 1
    },
    setGameInventory(state, inventory) {
      state.game.inventory = inventory ?? {}
    },
    setGamePage(state, page) {
      state.game.page = page ?? null
    },
    setGamePokedex(state, pokedex) {
      state.game.pokedex = pokedex ?? {
        entries: [],
        hasNational: false
      }
    },
    setGamePokemon(state, pokemon) {
      state.game.pokemon = pokemon ?? []
    },
    setGameTrainer(state, trainer) {
      state.game.trainer = trainer ?? null
    },
    setGameTrainers(state, trainers) {
      state.game.trainers = trainers ?? {}
    },
    setPokemonList(state, pokemonList) {
      state.pokemonList = pokemonList ?? {}
    },
    setSelectedBattleMove(state, selected) {
      state.battle.move.selected = selected ?? null
    },
    setTrainers(state, trainers) {
      state.trainers = trainers ?? {}
    }
  }
})
