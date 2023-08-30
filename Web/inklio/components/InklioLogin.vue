<template>
  <div>
    <h1>Login</h1>
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
const username = ref('')
const password = ref('')
const inklioUser = useInklioUser();

watchEffect(async () => {
  if (inklioUser.value) {
    await navigateTo('/');
  }
});


const { login } = useIdentity();
const loginUser = async () => {
  const response = await login(username.value, password.value, true);
}
</script>