// utils/authUtils.js
import { auth } from '../firebase'
import { onAuthStateChanged } from 'firebase/auth'

export function getCurrentUser() {
  return new Promise((resolve, reject) => {
    const unsubscribe = onAuthStateChanged(auth, user => {
      unsubscribe()
      resolve(user)
    }, reject)
  })
}
