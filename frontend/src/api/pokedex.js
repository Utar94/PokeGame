import { _delete, get, patch, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function deleteEntry(trainerId, speciesId) {
  return await _delete(`/api/trainers/${trainerId}/pokedex/${speciesId}`)
}

export async function getEntries(trainerId, params) {
  return await get(`/api/trainers/${trainerId}/pokedex` + getQueryString(params))
}

export async function saveEntry(trainerId, speciesId, payload) {
  return await put(`/api/trainers/${trainerId}/pokedex/${speciesId}`, payload)
}

export async function seenSpecies(payload) {
  return await patch('/api/pokedex/seen', payload)
}
