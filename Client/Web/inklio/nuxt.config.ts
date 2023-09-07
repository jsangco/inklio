// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: [
    '@pinia/nuxt',
    '@pinia-plugin-persistedstate/nuxt',],
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
