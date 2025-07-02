// src/router/index.js
import { createRouter, createWebHistory } from 'vue-router'
import BoardView from '../components/BoardView.vue'
import Home from '../views/Home.vue'
import Login from '../views/Login.vue'
import List from '../components/ListColumn.vue'
import ToDo from '../components/TodoList.vue'
import card from '../components/CardItem.vue'
import Dashboard from '../views/Dashboard.vue'




const routes = [
  { path: '/', name: 'Home', component: Home },                 // Ana giriş sayfası
  { path: '/board', name: 'Board', component: BoardView },      // Giriş sonrası gösterilen board sayfası
  { path: '/login', name: 'Login', component: Login },
  { path: '/list', name: 'List', component: List },
  { path: '/todo', name: 'ToDo', component: ToDo },
  { path: '/dashboard', name: 'Dashboard', component: Dashboard }
]



export const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router