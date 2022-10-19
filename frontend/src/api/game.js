import { get } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function getGameInventory(trainerId) {
  return await get(`/api/game/trainers/${trainerId}/inventory`)
}

export async function getGamePokedex(trainerId, params) {
  return await get(`/api/game/trainers/${trainerId}/pokedex` + getQueryString(params))
}

export async function getGamePokemon(trainerId) {
  return await get(`/api/game/trainers/${trainerId}/pokemon`)
}

export async function getGamePokemonSummary(pokemonId) {
  return await get(`/api/game/trainers/pokemon/${pokemonId}`)
}

export async function getGameTrainer(id) {
  return await get(`/api/game/trainers/${id}`)
}

export async function getGameTrainers() {
  return await get('/api/game/trainers')
}
