<script setup lang="ts">
console.log("script setup");
const todoStore = useTodoStore();

const runtimeConfig = useRuntimeConfig();
const baseurl = runtimeConfig.public.baseUrl;
// const ext = await todoStore.getExt();
// const auth = await useFetch(`${runtimeConfig.public.baseUrl}/auth/`);
// const authX = await useFetchX('/auth/');
// const asks = await useFetchX('api/v1/asks/');
await todoStore.setAuth();

const login = ref();
const claims = ref();
const authBasic = ref();
onBeforeMount(async () => {
    console.log('onBeforemounted');
    // authBasic.value = await todoStore.getAuthBasic();
    // authBasic.value = todoStore.auth;
    // login.value = await todoStore.login();
    // claims.value = await todoStore.claims();
    // authBasic.value = await todoStore.getAuthBasic();
});
</script>

<template>
    <div>
        <h1>index</h1>
        <p>baseurl: {{baseurl}}</p>
        <p><b>javascript-fetch store GET {{baseurl}}/auth:</b> {{todoStore.auth}}</p>
        <!-- <p><b>ext api $fetch:</b> {{ ext }}</p>
        <p><b>useFetch {{baseurl}}/auth:</b> {{auth}}</p>
        <p><b>useFetchX {{baseurl}}/auth:</b> {{authX}}</p>
        <p><b>useFetchX {{baseurl}}/api/v1/asks:</b> {{asks}}</p> -->
        <!-- <p><b>store fetch {{baseurl}}/auth/accounts/login:</b> {{login}}</p> -->
        <!-- <p><b>store claims {{baseurl}}/auth/claims:</b> {{claims}}</p> -->
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