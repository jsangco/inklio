<template>
  <div>
    <h1>Register</h1>
    <form @submit.prevent="registerUser">
      <div>
        <label for="username">Username</label>
        <input v-model="username" type="text" id="username" />
      </div>
      <div>
        <label for="email">Email</label>
        <input v-model="email" type="text" id="email" />
      </div>
      <div>
        <label for="password">Password</label>
        <input v-model="password" type="password" id="password" />
      </div>
      <div>
        <label for="confirmPassword">Confirm Password</label>
        <input v-model="confirmPassword" type="password" id="confirmPassword" />
      </div>
      <button type="submit">Register</button>
    </form>
  </div>
  <div>
    <p>username: {{ username }}</p>
  </div>
  <div>
    <p>password: {{ password }}</p>
  </div>
  <div>
    <p>confirmPassword matches: {{ confirmPassword == password}}</p>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user';

const username = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const user = useUserStore();

watchEffect(async () => {
  if (user.isLoggedIn) {
    await navigateTo('/');
  }
});

const registerUser = async () => {
  const response = await user.register(username.value, email.value, password.value, confirmPassword.value);
}
</script>