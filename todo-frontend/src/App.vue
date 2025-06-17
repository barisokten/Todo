<template>
  <div class="p-4 min-h-screen bg-white dark:bg-gray-900 text-gray-900 dark:text-gray-100">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">📝 ToDo Uygulaması</h1>
      <div class="flex items-center gap-2">
        <button @click="toggleDark" class="px-3 py-1 bg-gray-200 dark:bg-gray-700 rounded">
          {{ isDark ? '☀ Light' : '🌙 Dark' }}
        </button>

        <button v-if="!user" @click="login" class="bg-blue-500 text-white px-3 py-1 rounded">Giriş Yap</button>
        <button v-if="user" @click="logout" class="bg-red-500 text-white px-3 py-1 rounded">Çıkış Yap</button>
        <span v-if="user" class="ml-2 text-sm text-gray-600 dark:text-gray-300">👤 {{ user.displayName }}</span>
      </div>
    </div>

    <router-view />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { auth, provider, signInWithPopup, signOut } from './firebase'

// 🌙 Dark mode
const isDark = ref(localStorage.getItem("dark") === "true")
onMounted(() => {
  document.documentElement.classList.toggle("dark", isDark.value)
})
function toggleDark() {
  isDark.value = !isDark.value
  localStorage.setItem("dark", isDark.value)
  document.documentElement.classList.toggle("dark", isDark.value)
}

// 🔐 Giriş kontrolü
const user = ref(null)
onMounted(() => {
  auth.onAuthStateChanged(u => {
    user.value = u
  })
})
function login() {
  signInWithPopup(auth, provider)
}
function logout() {
  signOut(auth)
}
</script>