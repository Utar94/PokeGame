import { _delete, get, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function deletePokedex(trainerId, speciesId) {
  return await _delete(`/api/trainers/${trainerId}/pokedex/${speciesId}`)
}

export async function getPokedex(trainerId, params) {
  return await get(`/api/trainers/${trainerId}/pokedex` + getQueryString(params))
}

export async function savePokedex(trainerId, speciesId, payload) {
  return await put(`/api/trainers/${trainerId}/pokedex/${speciesId}`, payload)
}
