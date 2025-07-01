  
  <template>
    <div class="board-view">
      <h2>{{ board.title }}</h2>
      <div class="lists-container">
        <ListColumn
          v-for="list in board.lists"
          :key="list.id"
          :list="list"
          @cardMoved="handleCardMoved"
        />
      </div>
    </div>
  </template>

<script setup>
import { ref, onMounted } from 'vue'

import ListColumn from './ListColumn.vue'  // ya da doğru dosya yolu


const board = ref({ title: '', lists: [] })

async function fetchBoard(boardId = 1) {
const API_URL = import.meta.env.VITE_API_URL
const res = await fetch(`${API_URL}/api/board`, { headers: { Authorization: `Bearer ...` } })

  const boards = await res.json()
  board.value = boards.find(b => b.id === boardId)
}

function handleCardMoved(event) {
  // drag-drop sonrası güncelleme işlemleri yapılabilir
}

onMounted(() => {
  fetchBoard()
})
</script>

<style scoped>
.lists-container {
  display: flex;
  gap: 16px;
}
</style>
Board view