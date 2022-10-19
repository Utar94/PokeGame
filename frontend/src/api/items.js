import { _delete, get, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function createItem(payload) {
  return await post('/api/items', payload)
}

export async function deleteItem(id) {
  return await _delete(`/api/items/${id}`)
}

export async function getItem(id) {
  return await get(`/api/items/${id}`)
}

export async function getItems(params) {
  return await get('/api/items' + getQueryString(params))
}

export async function updateItem(id, payload) {
  return await put(`/api/items/${id}`, payload)
}
