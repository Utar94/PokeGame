import { _delete, get, patch, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function catchPokemon(id, payload) {
  return await patch(`/api/pokemon/${id}/catch`, payload)
}

export async function createPokemon(payload) {
  return await post('/api/pokemon', payload)
}

export async function deletePokemon(id) {
  return await _delete(`/api/pokemon/${id}`)
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

export async function updatePokemon(id, payload) {
  return await put(`/api/pokemon/${id}`, payload)
}
