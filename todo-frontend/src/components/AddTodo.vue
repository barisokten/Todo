<template>
  <form @submit.prevent="addTodo">
    <input type="date" v-model="dueDate"/>
    <input v-model="title" placeholder="New todo..." />
    <button type="submit">Add</button>
  </form>
</template>

<script setup>
import { ref } from 'vue'

const title = ref('')
const dueDate=ref('')

// API’ye veri gönderme
async function addTodo() {
  if (!title.value.trim()) return

  const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:8080'

  await fetch(`${apiUrl}/api/todoitems`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      title: title.value,
      isCompleted: false,
      dueDate: dueDate.value || null // <- Yeni alanss
    }),
  })

  title.value = ''
  dueDate.value = '' // Form sıfırlansın diye
}
</script>
