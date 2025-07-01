<template>
  <div class="min-h-screen p-6 transition-colors duration-300 bg-white dark:bg-gray-900 text-gray-900 dark:text-white">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold flex items-center gap-2">📝 ToDo Uygulaması</h1>
      <div class="flex items-center gap-2">
        <button @click="toggleDark" class="px-2 py-1 border rounded">
          {{ isDark ? '☀️ Light' : '🌙 Dark' }}
        </button>
        <button @click="logout" class="bg-red-600 text-white px-3 py-1 rounded">Çıkış Yap</button>
        <span class="ml-2 text-sm">👤 {{ userName }}</span>
      </div>
    </div>

    <!-- Add New Task -->
    <form @submit.prevent="addTodo" class="mb-4 flex gap-2">
      <input v-model="newTitle" placeholder="Görev başlığı..." class="border px-2 py-1 rounded flex-1 dark:bg-gray-800 dark:border-gray-600" />
      <input v-model="newDueDate" type="date" class="border px-2 py-1 rounded dark:bg-gray-800 dark:border-gray-600" />
      <button type="submit" class="bg-blue-500 text-white px-3 py-1 rounded">Ekle</button>
    </form>

    <!-- Filter Buttons -->
    <div class="mb-4 flex gap-2">
      <button @click="filter.value = 'all'" :class="buttonClass('all')">Tümü</button>
      <button @click="filter.value = 'completed'" :class="buttonClass('completed')">Tamamlanan</button>
      <button @click="filter.value = 'pending'" :class="buttonClass('pending')">Bekleyen</button>
    </div>

    <!-- Todo List with Drag & Drop -->
    <draggable v-model="todos" item-key="id" @end="onDragEnd" class="space-y-2">
      <template #item="{ element: todo }">
        <li class="mb-2 flex items-center justify-between bg-gray-100 dark:bg-gray-800 p-2 rounded">
          <div class="flex items-center gap-2">
            <span @click="toggleCompleted(todo)" class="cursor-pointer text-xl">
              <span v-if="todo.isCompleted">✅</span>
              <span v-else>❌</span>
            </span>
            <span><strong>{{ todo.title }}</strong> – Teslim: {{ formatDate(todo.dueDate) }}</span>
          </div>
          <div class="flex items-center gap-2">
            <button @click="editTodo(todo)" class="text-orange-500 hover:text-orange-700 text-lg">✏</button>
            <button @click="deleteTodo(todo.id)" class="text-red-600 hover:text-red-800 text-lg">🗑</button>
          </div>
        </li>
      </template>
    </draggable>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import draggable from 'vuedraggable'

// 🌙 Dark Mode
const isDark = ref(localStorage.getItem('dark') === 'true')
function toggleDark() {
  isDark.value = !isDark.value
  document.documentElement.classList.toggle('dark', isDark.value)
  localStorage.setItem('dark', isDark.value)
}
onMounted(() => {
  document.documentElement.classList.toggle('dark', isDark.value)
})

// 🔐 Kullanıcı Bilgisi
const userName = 'Barış Ökten'
function logout() {
  localStorage.removeItem('token')
  location.reload()
}

// 🗂 Todo State
const todos = ref([])
const newTitle = ref('')
const newDueDate = ref('')
const filter = ref('all')
const API_BASE_URL = import.meta.env.VITE_API_URL
const token = localStorage.getItem('token')

// 🧠 Filtre
const filteredTodos = computed(() => {
  if (filter.value === 'all') return todos.value
  if (filter.value === 'completed') return todos.value.filter(t => t.isCompleted)
  if (filter.value === 'pending') return todos.value.filter(t => !t.isCompleted)
})

// 📆 Tarih Formatı
function formatDate(dateStr) {
  if (!dateStr) return "Yok"
  const date = new Date(dateStr)
  return date.toLocaleDateString("tr-TR")
}

// 📥 Getir
async function fetchTodos() {
  try {
    const res = await fetch(`${API_BASE_URL}/api/todoitems`, {
      headers: { Authorization: `Bearer ${token}` }
    })
    if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`)
    todos.value = await res.json()
  } catch (error) {
    console.error("Fetch error:", error)
  }
}

// ➕ Ekle
async function addTodo() {
  if (!newTitle.value.trim() || !newDueDate.value) {
    alert("Lütfen başlık ve teslim tarihi giriniz.")
    return
  }

  try {
    const res = await fetch(`${API_BASE_URL}/api/todoitems`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify({
        title: newTitle.value,
        dueDate: newDueDate.value,
        isCompleted: false
      })
    })
    if (!res.ok) throw new Error("Todo eklenemedi.")
    await fetchTodos()
    newTitle.value = ''
    newDueDate.value = ''
  } catch (err) {
    console.error("Add error:", err)
  }
}

// ❌ Sil
async function deleteTodo(id) {
  try {
    const res = await fetch(`${API_BASE_URL}/api/todoitems/${id}`, {
      method: "DELETE",
      headers: { Authorization: `Bearer ${token}` }
    })
    if (!res.ok) throw new Error("Silme başarısız.")
    await fetchTodos()
  } catch (err) {
    console.error("Silme hatası:", err)
  }
}

// ✅ Durum Güncelle
async function toggleCompleted(todo) {
  try {
    const res = await fetch(`${API_BASE_URL}/api/todoitems/${todo.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify({ ...todo, isCompleted: !todo.isCompleted })
    })
    if (!res.ok) throw new Error("Güncelleme başarısız.")
    await fetchTodos()
  } catch (err) {
    console.error("Toggle error:", err)
  }
}

// 🔃 Sıralama
async function onDragEnd() {
  const updated = todos.value.map((todo, index) => ({
    ...todo,
    order: index + 1
  }))

  try {
    const res = await fetch(`${API_BASE_URL}/api/todoitems/reorder`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(updated)
    })
    if (!res.ok) throw new Error("Sıralama güncellenemedi")
  } catch (err) {
    console.error("Sıralama hatası:", err)
  }
}

// ✏ Düzenleme Örneği
function editTodo(todo) {
  alert(`Düzenlenecek görev: ${todo.title}`)
}

// 🎨 Buton Stili
function buttonClass(value) {
  return {
    'px-3 py-1 rounded': true,
    'bg-blue-500 text-white': filter.value === value,
    'bg-gray-200 dark:bg-gray-700 text-gray-900 dark:text-white': filter.value !== value
  }
}

onMounted(() => {
  fetchTodos()
})
</script>

<style>
body {
  transition: background-color 0.3s, color 0.3s;
}
</style>




