<template>
  <div>
    pages/index
    <div v-if="user.isLoggedIn">
      <p> Hi {{ user.username }}</p>
      <button @click="user.logout">Log out</button>
    </div>
    <div v-else>
      <NuxtLink style="padding: 10px;" to="/login">
        <span>Login</span>
      </NuxtLink>
      <NuxtLink to="/register">
        <span>Register</span>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import {useUserStore} from '@/stores/user';
const user = useUserStore();
</script>