<template>
  <nav class="navbar">
    <span class="logo">
      <NuxtLink to="/">
        Inklio
      </NuxtLink>
    </span>
    <span class="user-info">
      <span v-if="user.isLoggedIn">
        {{ user.username }}
        <button @click="user.logout">Log out</button>
      </span>
      <span v-else>
        <NuxtLink to="/login">
          <span>Login</span>
        </NuxtLink>
        <NuxtLink to="/register">
          <span>Register</span>
        </NuxtLink>
      </span>
    </span>
  </nav>
</template>

<script setup>
import { useUserStore } from '@/stores/user';
const user = useUserStore();
</script>

<style>
.user-info {
  float: right;
  font-size: medium;
  color:#aee6e0;
}
</style>