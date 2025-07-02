import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import PrimeVue from 'primevue/config';
import './assets/tailwind.css'

createApp(App)
  .use(router)
  .use(PrimeVue, {
    unstyled: true
  })
  .mount('#app')
