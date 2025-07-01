<template>
  <div class="min-h-screen bg-gradient-to-br from-purple-700 to-pink-500 p-6 overflow-x-auto">
    <h2 class="text-3xl text-white font-bold mb-6">{{ board.title }}</h2>

    <div class="flex space-x-4">
      <ListColumn
        v-for="list in board.lists"
        :key="list.id"
        :list="list"
        @cardAdded="fetchBoard"
        @listDeleted="fetchBoard"
      />

      <!-- Yeni Liste Ekleme Kutusu -->
      <div class="w-72 flex-shrink-0 bg-white bg-opacity-30 hover:bg-opacity-50 cursor-pointer rounded-xl p-4 text-white">
        <input
          v-model="newListTitle"
          placeholder="New list title..."
          class="w-full p-2 rounded text-black mb-2 bg-white placeholder-gray-500"
        />
        <button
          @click="addList"
          class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-1 rounded w-full"
        >
          Add List
        </button>
      </div>
    </div>

    <!-- Test çıktısı -->
    <pre class="text-white mt-6">{{ board }}</pre>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { authFetch } from '../utils/api'
import ListColumn from './ListColumn.vue'

const API_URL = import.meta.env.VITE_API_URL

const board = ref({ title: '', lists: [] })
const newListTitle = ref('')

// 🔐 Token'lı fetch helper

async function fetchBoard() {
  const res = await authFetch(`${API_URL}/api/Boards`)
  if (!res.ok) return
  const boards = await res.json()
  board.value = boards.find(b => b.id === boardId) || { title: '', lists: [] }
}


async function addList() {
  if (!newListTitle.value.trim()) return

  const res = await authFetch(`${API_URL}/api/list`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      title: newListTitle.value,
      boardId: board.value.id
    })
  })

  if (!res.ok) {
    console.error("Liste eklenemedi:", res.status)
    return
  }

  newListTitle.value = ''
  fetchBoard()
}

onMounted(() => {
  fetchBoard()
})
</script>
