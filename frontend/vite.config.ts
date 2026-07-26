import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // Proxy para redirecionar requisições /api para o backend ASP.NET Core.
    // Evita problemas de CORS em desenvolvimento e simplifica URLs no código.
    proxy: {
      '/api': {
        target: 'http://localhost:5202',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
