import { get } from '.'

export async function getGameTrainers() {
  return await get('/api/game/trainers')
}
