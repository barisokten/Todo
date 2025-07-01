<script setup>
import { ref, computed, onMounted } from 'vue'

const todos = ref([])
const newTitle = ref('')
const newDueDate = ref('')
const filter = ref('all')

const API_BASE_URL = import.meta.env.VITE_API_URL

const filteredTodos = computed(() => {
  if (filter.value === 'all') return todos.value
  if (filter.value === 'completed') return todos.value.filter(todo => todo.isCompleted)
  if (filter.value === 'pending') return todos.value.filter(todo => !todo.isCompleted)
})

function formatDate(dateStr) {
  if (!dateStr) return "Yok"
  const date = new Date(dateStr)
  return date.toLocaleDateString("tr-TR")
}

async function fetchTodos() {
  try {
    console.log("FETCH WORKING FROM: [TodoList.vue]", API_BASE_URL)


    const res = await fetch(`${API_BASE_URL}/api/todoitems`)
    if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`)
    const data = await res.json()
    todos.value = data
  } catch (error) {
    console.error("Fetch error:", error)
  }
}

async function addTodo() {
  if (!newTitle.value.trim() || !newDueDate.value) {
    alert("Lütfen başlık ve teslim tarihi giriniz.")
    return
  }

  try {
    const res = await fetch(`${API_BASE_URL}/api/todoitems`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        title: newTitle.value,
        dueDate: newDueDate.value,
        isCompleted: false  // ← Buraya dikkat
      })
    })
    if (!res.ok) throw new Error("Todo eklenemedi.")

    await res.json()
    newTitle.value = ''
    newDueDate.value = ''
    await fetchTodos()
  } catch (err) {
    console.error("Add error:", err)
  }
}

onMounted(() => {
  fetchTodos()
})
</script>

<template>
  <!-- Add New Task -->
  <form @submit.prevent="addTodo" class="mb-4 space-x-2">
    <input
      v-model="newTitle"
      placeholder="Görev başlığı..."
      class="border px-2 py-1 rounded"
    />
    <input
      v-model="newDueDate"
      type="date"
      class="border px-2 py-1 rounded"
    />
    <button type="submit" class="bg-blue-500 text-white px-3 py-1 rounded">
      Ekle
    </button>
  </form>

  <!-- Filter Buttons -->
  <div class="mb-4 space-x-2">
    <button @click="filter.value = 'all'" :class="{ 'font-bold': filter.value === 'all' }">all</button>
    <button @click="filter.value = 'completed'" :class="{ 'font-bold': filter.value === 'completed' }">completed</button>
    <button @click="filter.value = 'pending'" :class="{ 'font-bold': filter.value === 'pending' }">waiting</button>
  </div>

  <!-- Todo List -->
  <ul>
    <li v-for="todo in filteredTodos" :key="todo.id" class="mb-2">
      <div>
        <strong>{{ todo.title }}</strong> – 
        Teslim: {{ formatDate(todo.dueDate) }} – 
        <span v-if="todo.isCompleted">✅</span>
        <span v-else>❌</span>
      </div>
    </li>
  </ul>
</template>
