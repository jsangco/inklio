// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      baseUrl: process.env.BASE_URL || 'http://localhost'
    },
  },
  routeRules: {
    '/login': { ssr: false },
  },
  css: ['~/assets/css/main.css'],
})
