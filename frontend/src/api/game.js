import { get } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function getGameInventory(trainerId) {
  return await get(`/api/game/trainers/${trainerId}/inventory`)
}

export async function getGamePokedex(trainerId, params) {
  return await get(`/api/game/trainers/${trainerId}/pokedex` + getQueryString(params))
}

export async function getGameTrainers() {
  return await get('/api/game/trainers')
}
