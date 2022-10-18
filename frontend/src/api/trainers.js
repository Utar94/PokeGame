import { _delete, get, patch, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function createTrainer(payload) {
  return await post('/api/trainers', payload)
}

export async function deleteTrainer(id) {
  return await _delete(`/api/trainers/${id}`)
}

export async function getTrainer(id) {
  return await get(`/api/trainers/${id}`)
}

export async function getTrainers(params) {
  return await get('/api/trainers' + getQueryString(params))
}

export async function healTrainerParty(id) {
  return await patch(`/api/trainers/${id}/heal-party`)
}

export async function updateTrainer(id, payload) {
  return await put(`/api/trainers/${id}`, payload)
}
