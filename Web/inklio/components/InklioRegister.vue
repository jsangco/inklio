<!-- TODO: placeholder code -->
<template>
  <div>
    <h1>Register</h1>
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
  <div>
    <p>username: {{ username }}</p>
  </div>
  <div>
    <p>password: {{ password }}</p>
  </div>
</template>

<script setup lang="ts">
import {useUserStore} from '@/stores/user';

const username = ref('')
const password = ref('')
const user = useUserStore();

watchEffect(async () => {
  if (user.isLoggedIn) {
    await navigateTo('/');
  }
});

const loginUser = async () => {
  const response = await user.login(username.value, password.value, true);
}
</script>