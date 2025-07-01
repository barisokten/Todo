
// utils/api.js
import { getCurrentUser } from './authUtils'

export async function authFetch(url, options = {}) {
  const user = await getCurrentUser()
  if (!user) {
    window.location.href = '/login'
    return { ok: false, status: 401 }
  }

  const token = await user.getIdToken()
  options.headers = {
    ...(options.headers || {}),
    Authorization: `Bearer ${token}`
  }

  return fetch(url, options)
}