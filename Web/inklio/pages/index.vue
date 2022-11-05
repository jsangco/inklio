<script setup lang="ts">
const fetchOnServer = false;
const todoStore = useTodoStore();

// const ext = await todoStore.getExt();
const auth = await todoStore.getAuth();
const authBasic = await todoStore.getAuthBasic();
const asks = await todoStore.getAsks();
const login = await todoStore.login();
</script>

<template>
    <div>
        <h1>index</h1>
        <p>base url: {{todoStore.test}}</p>
        <!-- <p>External api: {{ ext }}</p> -->
        <p>BASIC localhost/auth: {{authBasic}}</p>
        <p>localhost/auth: {{auth}}</p>
        <p>localhost/api/v1/asks: {{asks}}</p>
        <!-- <p>localhost/auth/accounts/login: {{login}}</p> -->
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