<template>
  <div>
    <h1>Login</h1>
    <div v-if="loginError" class="form-error" @click="loginError = null">
      <p>{{ loginError }}</p>
    </div>
    <form @submit.prevent="loginAccount">
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
    <div style="font-size: small;padding: 10px;">
      Forget your <NuxtLink to="/password-forget">password</NuxtLink>?
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAccountStore } from '@/stores/account';

const username = ref('')
const password = ref('')
const loginError = ref(<string | null>null);
const account = useAccountStore();

watchEffect(async () => {
  if (account.isLoggedIn) {
    await navigateTo('/');
  }
});

const loginAccount = async () => {
  const { isSuccess, error } = await account.login(username.value, password.value, true);
  loginError.value = error?.detail;
}
</script>

<style>
label {
  display: inline-block;
  width: 150px;
}
</style>