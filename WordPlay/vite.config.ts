import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/games": {
        target: "http://localhost:5095", // backend port
        changeOrigin: true,
      }
    }
  }
})
