// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({
  ssr: true,
  imports: {
    dirs: ['stores'],
  },
  env: {
    baseUrl: process.env.BASE_URL || 'https://inklio.azurewebsites.net',
  },
  runtimeConfig: {
    public: {
      baseUrl: process.env.BASE_URL || 'https://inklio.azurewebsites.net'
    }
  },
  modules: [
    [
      '@pinia/nuxt',
      {
        autoImports: [
          // automatically imports `defineStore`
          'defineStore', // import { defineStore } from 'pinia'
          // automatically imports `defineStore` as `definePiniaStore`
          ['defineStore', 'definePiniaStore'], // import { defineStore as definePiniaStore } from 'pinia'
        ],
      },
    ],
  ],
})
