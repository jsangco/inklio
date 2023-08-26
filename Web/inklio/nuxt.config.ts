// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      baseUrl: process.env.BASE_URL || 'https://inklio.azurewebsites.net'
    }
  },
  css: ['~/assets/css/main.css'],
})
