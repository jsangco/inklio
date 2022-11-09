<template>
    <div>
        <p><b>Inklio.Api:</b> {{apiStatus}}</p>
        <p><b>Inklio.Auth:</b> {{authStatus}}</p>
    </div>
</template>
<script setup lang="ts">
const config = useRuntimeConfig();
const apiStatus = ref('checking...'); 
const authStatus = ref('checking...');

onBeforeMount(async () => {
    await fetch(`${config.public.baseUrl}/api/`).then(async r => {
        apiStatus.value = (await r.json()).status;
    }).catch(e => {
        apiStatus.value = "Down";
    });
    await fetch(`${config.public.baseUrl}/auth/`).then(async r => {
        authStatus.value = (await r.json()).status;
    }).catch(e => {
        authStatus.value = "Down";
    });
});
   
</script>