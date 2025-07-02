// vite.config.js
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

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
  },
  resolve: {
    alias: [
      { find: "@", replacement: path.resolve(__dirname, "./src") }
    ]
  }
})
