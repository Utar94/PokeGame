import { get, post, put } from '.'
import { getQueryString } from '@/helpers/queryUtils'

export async function getUsers(params) {
  return await get('/api/users' + getQueryString(params))
}

export async function inviteUser(payload) {
  return await post('/api/users/invite', payload)
}

export async function synchronizeUsers() {
  return await put('/api/users/synchronize')
}
