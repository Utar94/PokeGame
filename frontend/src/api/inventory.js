import { get, patch } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function addInventory(trainerId, itemId, quantity) {
  return await patch(`/api/trainers/${trainerId}/inventory/${itemId}/add`, { quantity })
}

export async function buyInventory(trainerId, itemId, quantity) {
  return await patch(`/api/trainers/${trainerId}/inventory/${itemId}/buy`, { quantity })
}

export async function getInventory(trainerId, params) {
  return await get(`/api/trainers/${trainerId}/inventory` + getQueryString(params))
}

export async function removeInventory(trainerId, itemId, quantity) {
  return await patch(`/api/trainers/${trainerId}/inventory/${itemId}/remove`, { quantity })
}

export async function sellInventory(trainerId, itemId, quantity) {
  return await patch(`/api/trainers/${trainerId}/inventory/${itemId}/sell`, { quantity })
}
