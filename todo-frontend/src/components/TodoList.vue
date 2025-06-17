<template>
  <!-- Make sure this form only appears ONCE -->
  <form @submit.prevent="addTodo" class="mb-4 space-x-2">
    <input
      v-model="newTitle"
      placeholder="Add new task..."
      class="border px-2 py-1 rounded"
    />
    <button type="submit" class="bg-blue-500 text-white px-3 py-1 rounded">
      Add
    </button>
  </form>

  <!-- Filter buttons -->
  <div class="mb-4 space-x-2">
    <button @click="filter = 'all'" :class="{ 'font-bold': filter === 'all' }">
      All
    </button>
    <button
      @click="filter = 'completed'"
      :class="{ 'font-bold': filter === 'completed' }"
    >
      Completed
    </button>
    <button
      @click="filter = 'pending'"
      :class="{ 'font-bold': filter === 'pending' }"
    >
      Pending
    </button>
  </div>

  <!-- Todo list -->
  <ul>
    <li v-for="todo in filteredTodos" :key="todo.id">
      {{ todo.title }} -
      <span v-if="todo.completed">✅</span>
      <span v-else>❌</span>
    </li>
  </ul>
</template>

<script>
export default {
  data() {
    return {
      todos: [],
      newTitle: '',
      filter: 'all'
    }
  },
  computed: {
    filteredTodos() {
      if (this.filter === 'all') return this.todos
      if (this.filter === 'completed') return this.todos.filter(todo => todo.completed)
      if (this.filter === 'pending') return this.todos.filter(todo => !todo.completed)
    }
  },
  methods: {
    async fetchTodos() {
      try {
        const res = await fetch("http://api:8080/api/todos")
        if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`)
        const data = await res.json()
        this.todos = data
      } catch (error) {
        console.error("Fetch error:", error)
      }
    },
    async addTodo() {
      if (!this.newTitle.trim()) return
      try {
        await fetch("http://api:8080/api/todos", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            title: this.newTitle,
            isCompleted: false
          })
        })
        this.newTitle = ''
        await this.fetchTodos()
      } catch (err) {
        console.error("Add error:", err)
      }
    }
  },
  mounted() {
    this.fetchTodos()
  }
}
</script>