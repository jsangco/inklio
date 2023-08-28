<template>
  <div>
    <p>Login page</p>
    <!-- <p>{{ loginResult }}</p> -->
    <p>{{ profile }}</p>
  </div>
</template>

<script setup>
const { login, getProfile } = useIdentity();
const loginResult = await login(null);
const profile = await getProfile();
</script>