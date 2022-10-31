import { _delete, get, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function createRegion(payload) {
  return await post('/api/regions', payload)
}

export async function deleteRegion(id) {
  return await _delete(`/api/regions/${id}`)
}

export async function getRegion(id) {
  return await get(`/api/regions/${id}`)
}

export async function getRegions(params) {
  return await get('/api/regions' + getQueryString(params))
}

export async function updateRegion(id, payload) {
  return await put(`/api/regions/${id}`, payload)
}
