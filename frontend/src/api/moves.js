import { _delete, get, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function createMove(payload) {
  return await post('/api/moves', payload)
}

export async function deleteMove(id) {
  return await _delete(`/api/moves/${id}`)
}

export async function getMove(id) {
  return await get(`/api/moves/${id}`)
}

export async function getMoves(params) {
  return await get('/api/moves' + getQueryString(params))
}

export async function updateMove(id, payload) {
  return await put(`/api/moves/${id}`, payload)
}
