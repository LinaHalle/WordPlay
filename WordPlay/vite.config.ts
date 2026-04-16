import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

export default defineConfig({
  plugins: [react()],
  build: {
    outDir: path.resolve(__dirname, 'backend/wwwroot'),
    emptyOutDir: true,
    sourcemap: true,
  },
  server: {
    proxy: {
      "/games": {
        target: "http://localhost:5095", // backend port
        changeOrigin: true,
      }
    }
  }
})
