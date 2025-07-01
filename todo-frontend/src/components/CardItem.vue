<template>
  <div
    class="bg-white p-3 mb-3 rounded-md shadow-sm hover:bg-gray-100 cursor-pointer transition"
    @click="openModal = true"
  >
    <h4 class="text-sm font-medium text-gray-800">
      {{ card.title }}
    </h4>

    <CardModal
      v-if="openModal"
      :card="card"
      @close="openModal = false"
      @updated="refresh"
    />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import CardModal from './CardModal.vue'

const props = defineProps(['card'])
const emit = defineEmits(['updated'])

const openModal = ref(false)

function refresh() {
  emit('updated')
}
</script>
