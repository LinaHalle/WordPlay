import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/games': 'http://localhost:5095',
      '/categories': 'http://localhost:5095',
    },
  },
})
