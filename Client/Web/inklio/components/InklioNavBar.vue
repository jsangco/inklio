<template>
  <nav class="navbar">
    <span class="logo">
      <NuxtLink to="/">
        Inklio
      </NuxtLink>
    </span>
    <span class="account-info">
      <span v-if="account.isLoggedIn">
        {{ account.username }}
        <button @click="account.logout">Log out</button>
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

<script setup lang="ts">
import { useAccountStore } from '@/stores/account';
const account = useAccountStore();
</script>

<style>
.account-info {
  float: right;
  font-size: medium;
  color:#aee6e0;
}
</style>