<!-- src/views/Home.vue -->
fetch("http://api:8080/api/todos")

<template>
  <div>
    <h1>My TODO App</h1>
    <AddTodo />
    <TodoList />
  </div>
</template>

<script>
import AddTodo from '../components/AddTodo.vue'
import TodoList from '../components/TodoList.vue'

export default {
  components: {
    AddTodo,
    TodoList
  }
}
</script>