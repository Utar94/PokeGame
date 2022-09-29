import { get } from '.'

export async function getGameInventory(trainerId) {
  return await get(`/api/game/trainers/${trainerId}/inventory`)
}

export async function getGameTrainers() {
  return await get('/api/game/trainers')
}
