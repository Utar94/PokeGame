import { _delete, get, patch, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function battleGain(payload) {
  return await patch('/api/pokemon/battle/gain', payload)
}

export async function catchPokemon(id, payload) {
  return await patch(`/api/pokemon/${id}/catch`, payload)
}

export async function createPokemon(payload) {
  return await post('/api/pokemon', payload)
}

export async function deletePokemon(id) {
  return await _delete(`/api/pokemon/${id}`)
}

export async function evolvePokemon(id, payload) {
  return await patch(`/api/pokemon/${id}/evolve`, payload)
}

export async function gainExperience(id, payload) {
  return await patch(`/api/pokemon/${id}/gain`, payload)
}

export async function getPokemon(id) {
  return await get(`/api/pokemon/${id}`)
}

export async function getPokemonList(params) {
  return await get('/api/pokemon' + getQueryString(params))
}

export async function healPokemon(id, payload) {
  return await patch(`/api/pokemon/${id}/heal`, payload)
}

export async function holdItem(id, itemId = null) {
  return await patch(`/api/pokemon/${id}/hold-item/${itemId ?? 'remove'}`)
}

export async function swapPokemon(id, otherId) {
  return await patch(`/api/pokemon/${id}/swap/${otherId}`)
}

export async function updatePokemon(id, payload) {
  return await put(`/api/pokemon/${id}`, payload)
}

export async function updatePokemonCondition(id, payload) {
  return await patch(`/api/pokemon/${id}/condition`, payload)
}

export async function usePokemonMove(id, moveId, payload) {
  return await patch(`/api/pokemon/${id}/use-move/${moveId}`, payload)
}
