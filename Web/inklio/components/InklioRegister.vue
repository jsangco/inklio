<template>
  <div>
    <h1>Register</h1>
    <div v-if="errorUsername" style="background-color: #461b1b;" @click="errorUsername = null">
      <p>{{ errorUsername }}</p>
    </div>
    <div v-if="errorEmail" style="background-color: #461b1b;" @click="errorEmail = null">
      <p>{{ errorEmail }}</p>
    </div>
    <div v-if="errorPassword" style="background-color: #461b1b;" @click="errorPassword = null">
      <p>{{ errorPassword }}</p>
    </div>
    <div v-if="errorConfirmPassword" style="background-color: #461b1b;" @click="errorConfirmPassword = null">
      <p>{{ errorConfirmPassword }}</p>
    </div>
    <div v-if="errorError" style="background-color: #461b1b;" @click="errorError = null">
      <p>{{ errorError }}</p>
    </div>
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
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user';

const username = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const errorUsername = ref(<string | null>null);
const errorEmail = ref(<string | null>null);
const errorPassword = ref(<string | null>null);
const errorConfirmPassword = ref(<string | null>null);
const errorError = ref(<string | null>null);
const user = useUserStore();

watchEffect(async () => {
  if (user.isLoggedIn) {
    await navigateTo('/');
  }
});

const registerUser = async () => {
  const { isSuccess, error } = await user.register(username.value, email.value, password.value, confirmPassword.value);
  errorUsername.value = error.errors.username?.find(() => true);
  errorEmail.value = error.errors.email?.find(() => true);
  errorPassword.value = error.errors.password?.find(() => true);
  errorConfirmPassword.value = error.errors.confirmPassword?.find(() => true);
  errorError.value = error.errors.error?.find(() => true);
}
</script>