<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100 dark:bg-gray-900">
    <div class="bg-white dark:bg-gray-800 p-8 rounded shadow-md w-full max-w-md">
      <h2 class="text-2xl font-bold mb-4 text-center text-gray-800 dark:text-gray-100">Giriş Yap</h2>

      <button
        @click="loginWithGoogle"
        class="w-full bg-blue-600 hover:bg-blue-700 text-white py-2 rounded"
      >
        Google ile Giriş Yap
      </button>
    </div>
  </div>
</template>

<script setup>
import { signInWithPopup } from 'firebase/auth'
import { auth, provider } from '../firebase'
import { useRouter } from 'vue-router'

const router = useRouter()

async function loginWithGoogle() {
  try {
    const result = await signInWithPopup(auth, provider)
    const token = await result.user.getIdToken()

    // ✅ Token'ı localStorage'a kaydet
    localStorage.setItem("token", token)

    // ✅ Giriş başarılı → console log (isteğe bağlı)
    console.log("Giriş Başarılı:", result.user.displayName)

    // ✅ Yönlendir
    router.push('/board')
  } catch (err) {
    console.error("Giriş hatası:", err.message)
    alert("Giriş başarısız oldu: " + err.message)
  }
}
</script>
