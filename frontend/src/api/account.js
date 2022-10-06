import { post, put } from '.'

export async function changePassword(payload) {
  return await post('/api/account/password/change', payload)
}

export async function recoverPassword(payload) {
  return await post('/api/account/password/recover', payload)
}

export async function resetPassword(payload) {
  return await post('/api/account/password/reset', payload)
}

export async function saveProfile(payload) {
  return await put('/api/account/profile', payload)
}

export async function signIn(payload) {
  return await post('/api/account/sign/in', payload)
}

export async function signUp(payload) {
  return await post('/api/account/sign/up', payload)
}
