import { _delete, get, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function createSpecies(payload) {
  return await post('/api/species', payload)
}

export async function deleteSpecies(id) {
  return await _delete(`/api/species/${id}`)
}

export async function getSpecies(id) {
  return await get(`/api/species/${id}`)
}

export async function getSpeciesEvolutions(id) {
  return await get(`/api/species/${id}/evolutions`)
}

export async function getSpeciesList(params) {
  return await get('/api/species' + getQueryString(params))
}

export async function removeSpeciesEvolution(id, speciesId) {
  return await _delete(`/api/species/${id}/evolutions/${speciesId}`)
}

export async function saveSpeciesEvolution(id, speciesId, payload) {
  return await put(`/api/species/${id}/evolutions/${speciesId}`, payload)
}

export async function updateSpecies(id, payload) {
  return await put(`/api/species/${id}`, payload)
}
