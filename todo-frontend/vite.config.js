// vite.config.js
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],  // ← Vue plugin burada aktif edildi
  server: {
    port: 3000, // ← burada belirtiyorsu
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        changeOrigin: true
      }
    }
  }
})
