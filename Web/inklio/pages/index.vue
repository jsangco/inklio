<script setup lang="ts">
const todoStore = useTodoStore();

const ext = await todoStore.getExt();
const auth = await useFetch('/auth/', {
    baseURL: 'https://inklio.azurewebsites.net/',
});
const asks = await useFetch('https://inklio.azurewebsites.net/api/v1/asks/');

const login = await useFetch(
      'https://inklio.azurewebsites.net/auth/accounts/login', {
      method: 'POST',
      body: {
        username: 'jace',
        password: 'Aoeuaoeu1'
        },
        headers: {
          'Content-Type': 'application/json'
        }
    });

const authBasic = ref();
onBeforeMount( async () => {
    console.log('onBeforemounted');
    authBasic.value = await todoStore.getAuthBasic();
})
</script>

<template>
    <div>
        <h1>index</h1>
        <p>base url: {{todoStore.test}}</p>
        <p>localhost/auth: {{auth}}</p>
        <p>External api: {{ ext }}</p>
        <p>BASIC localhost/auth: {{authBasic}}</p>
        <p>localhost/api/v1/asks: {{asks}}</p>
        <p>localhost/auth/accounts/login: {{login}}</p>
    </div>
</template>

<style>

/* Dark mode */
@media (prefers-color-scheme: dark) {
   :root {
       --body-bg: #000000;
       --body-color: #AAAAAA;
   }
}

body {
   background: var(--body-bg);
   color: var(--body-color);
   font-family: Arial, Helvetica, sans-serif;
}
</style>