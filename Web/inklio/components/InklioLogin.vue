<template>
  <div>
    <h1>Login</h1>
    <div v-if="loginError" style="background-color: #461b1b;" @click="loginError = null">
      <p>{{ loginError }}</p>
    </div>
    <form @submit.prevent="loginUser">
      <div>
        <label for="username">Username</label>
        <input v-model="username" type="text" id="username" />
      </div>
      <div>
        <label for="password">Password</label>
        <input v-model="password" type="password" id="password" />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user';

const username = ref('')
const password = ref('')
const loginError = ref(<string | null>null);
const user = useUserStore();

watchEffect(async () => {
  if (user.isLoggedIn) {
    await navigateTo('/');
  }
});

const loginUser = async () => {
  const { isSuccess, error } = await user.login(username.value, password.value, true);
  loginError.value = error.detail;
}
</script>

<style>
label {
  display: inline-block;
  width: 150px;
}
</style>