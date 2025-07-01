<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100 dark:bg-gray-900">
    <div class="bg-white dark:bg-gray-800 p-8 rounded shadow-md w-full max-w-md">
      <h2 class="text-2xl font-bold mb-4 text-center text-gray-800 dark:text-gray-100">Giriş Yap</h2>

      <form @submit.prevent="login">
        <input
          v-model="email"
          type="email"
          placeholder="E-posta"
          class="w-full p-2 mb-4 rounded border"
          required
        />
        <input
          v-model="password"
          type="password"
          placeholder="Şifre"
          class="w-full p-2 mb-4 rounded border"
          required
        />
        <button
          type="submit"
          class="w-full bg-blue-600 hover:bg-blue-700 text-white py-2 rounded"
        >
          Giriş Yap
        </button>
      </form>

      <p v-if="error" class="mt-4 text-red-500 text-sm text-center">{{ error }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { signInWithEmailAndPassword } from 'firebase/auth'
import { auth } from '../firebase'

const email = ref('')
const password = ref('')
const error = ref('')

async function login() {
  try {
    const userCredential = await signInWithEmailAndPassword(auth, email.value, password.value)
    const token = await userCredential.user.getIdToken()

    localStorage.setItem('token', token)      // ✅ Token'ı kaydettik
    window.location.href = '/Boards'           // ✅ Kullanıcıyı yönlendirdik
  } catch (err) {
    console.error('Login error:', err)
    error.value = 'Giriş başarısız. Lütfen bilgileri kontrol edin.'
  }
}
</script>
