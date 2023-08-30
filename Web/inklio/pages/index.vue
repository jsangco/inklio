<template>
  <div>
    pages/index
    <div v-if="user">
      <p> Hi {{ user.username }}</p>
      <button @click="logout">Log out</button>
    </div>
    <div v-else>
      <NuxtLink style="padding: 10px;" to="/login">
        <span>Login</span>
      </NuxtLink>
      <NuxtLink to="/Register">
        <span>Register</span>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
// TODO: Fix hydration mismatch that occurs
const user = useInklioUser();
const { logout } = useIdentity();
</script>