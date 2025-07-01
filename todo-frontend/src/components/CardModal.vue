<template>
  <div v-if="card" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50" @click.self="close">
    <div class="bg-white rounded-xl shadow-lg p-6 w-full max-w-md">
      <h3 class="text-xl font-semibold text-gray-800 mb-4">Edit Card</h3>

      <input
        v-model="editedCard.title"
        placeholder="Title"
        class="w-full border border-gray-300 p-2 rounded mb-3 focus:outline-none focus:ring focus:border-blue-400"
      />

      <textarea
        v-model="editedCard.description"
        placeholder="Description"
        class="w-full border border-gray-300 p-2 rounded mb-3 h-24 resize-none focus:outline-none focus:ring focus:border-blue-400"
      ></textarea>

      <label class="flex items-center space-x-2 text-gray-700 mb-4">
        <input type="checkbox" v-model="editedCard.isCompleted" />
        <span>Completed</span>
      </label>

      <div class="flex justify-end space-x-2">
        <button @click="save" class="bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700">Save</button>
        <button @click="close" class="bg-gray-300 text-gray-700 px-4 py-1 rounded hover:bg-gray-400">Cancel</button>
        <button @click="deleteCard" class="bg-red-600 text-white px-4 py-1 rounded hover:bg-red-700">Delete</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, watch } from 'vue'

const API_URL = import.meta.env.VITE_API_URL

const props = defineProps(['card'])
const emit = defineEmits(['close', 'updated'])

const editedCard = reactive({ ...props.card })

watch(() => props.card, (newCard) => {
  Object.assign(editedCard, newCard)
})

function close() {
  emit('close')
}

async function deleteCard() {
  await fetch(`${API_URL}/api/card/${editedCard.id}`, {
    method: 'DELETE'
  })

  emit('updated')
  close()
}

async function save() {
  await fetch(`${API_URL}/api/card/${editedCard.id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(editedCard)
  })

  emit('updated')
  close()
}
</script>
