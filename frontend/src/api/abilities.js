import { _delete, get, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function createAbility(payload) {
  return await post('/api/abilities', payload)
}

export async function deleteAbility(id) {
  return await _delete(`/api/abilities/${id}`)
}

export async function getAbility(id) {
  return await get(`/api/abilities/${id}`)
}

export async function getAbilities(params) {
  return await get('/api/abilities' + getQueryString(params))
}

export async function updateAbility(id, payload) {
  return await put(`/api/abilities/${id}`, payload)
}
