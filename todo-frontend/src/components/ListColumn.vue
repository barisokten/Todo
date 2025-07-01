<template>
  <div class="list-column bg-white p-4 rounded-xl shadow-md w-72 flex-shrink-0">
    <!-- Liste Başlığı ve Sil Butonu -->
    <div class="flex justify-between items-center mb-4">
      <h3 class="text-lg font-semibold text-gray-800 truncate">{{ list.title }}</h3>
      <button @click="deleteList" class="text-red-500 hover:text-red-600 text-sm">×</button>
    </div>

    <!-- Kartları draggable olarak listele -->
    <draggable v-model="cards" group="cards" class="space-y-2" @end="onDragEnd">
      <template #item="{ element }">
        <CardItem :card="element" />
      </template>
    </draggable>

    <!-- Yeni Kart Ekleme -->
    <div class="mt-4">
      <input
        v-model="newCardTitle"
        placeholder="Add a card..."
        class="w-full p-2 border border-gray-300 rounded mb-2 text-sm focus:outline-none focus:ring focus:border-blue-400"
      />
      <button
        @click="addCard"
        class="w-full bg-blue-600 hover:bg-blue-700 text-white text-sm py-1 rounded"
      >
        Add
      </button>
    </div>
  </div>
</template>


<script setup>
import { ref, watch } from 'vue'
import draggable from 'vuedraggable'
import CardItem from './CardItem.vue'


const API_URL = import.meta.env.VITE_API_URL

const props = defineProps(['list'])
const emit = defineEmits(['cardAdded', 'listDeleted'])

const cards = ref([...props.list.cards])
const newCardTitle = ref('')

// Liste güncellendiğinde kartları eşitle
watch(() => props.list.cards, (newVal) => {
  cards.value = [...newVal]
})

// Kart ekle
async function addCard() {
  if (!newCardTitle.value.trim()) return

  await fetch(`${API_URL}/api/card`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      title: newCardTitle.value,
      description: '',
      listModelId: props.list.id,
      isCompleted: false,
      order: cards.value.length
    })
  })

  newCardTitle.value = ''
  emit('cardAdded')
}

// Liste sil
async function deleteList() {
  if (confirm('Delete this list?')) {
    await fetch(`${API_URL}/api/list/${props.list.id}`, {
      method: 'DELETE'
    })
    emit('listDeleted')
  }
}

async function onDragEnd(evt) {
  const movedCard = cards.value[evt.newIndex]

  const updatedData = {
    cardId: movedCard.id,
    newListId: props.list.id,     // yeni bulunduğu liste
    newOrder: evt.newIndex        // listedeki sırası
  }

  const token = localStorage.getItem('token')

  await fetch(`${API_URL}/api/card/move`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(updatedData)
  })

  emit('cardAdded') // Güncellenmiş board'ı çek
}
</script>
