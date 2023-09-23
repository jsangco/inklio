// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  app: {
    pageTransition: { name: 'page', mode: 'out-in' },
    layoutTransition: { name: 'layout', mode: 'out-in' }
  },
  modules: [
    '@pinia/nuxt',
    '@pinia-plugin-persistedstate/nuxt',],
  runtimeConfig: {
    public: {
      baseUrl: process.env.BASE_URL || 'http://localhost',
      apiUrl: process.env.API_URL || 'http://localhost/api',
      env: process.env.ENV,
    },
  },
  routeRules: {
    '/login': { ssr: false },
  },
  css: ['~/assets/css/main.css'],
})
