<template>
  <div>
    <h1>Register</h1>
    <div v-if="errorErrors" style="background-color: #461b1b;" @click="errorErrors = null">
      <p>{{ errorErrors }}</p>
    </div>
    <form @submit.prevent="registerUser">
      <div>
        <label for="username">Username</label>
        <input v-model="username" type="text" id="username" />
        <span v-if="errorUsername" class="register-error" @click="errorUsername = null">
          {{ errorUsername }}
        </span>
      </div>
      <div>
        <label for="email">Email</label>
        <input v-model="email" type="text" id="email" />
        <span v-if="errorEmail" class="register-error" @click="errorEmail = null">
          {{ errorEmail }}
        </span>
      </div>
      <div>
        <label for="password">Password</label>
        <input v-model="password" type="password" id="password" />
        <span v-if="errorPassword" class="register-error" @click="errorPassword = null">
          {{ errorPassword }}
        </span>
      </div>
      <div>
        <label for="confirmPassword">Confirm Password</label>
        <input v-model="confirmPassword" type="password" id="confirmPassword" />
        <span v-if="errorConfirmPassword" class="register-error" @click="errorConfirmPassword = null">
          {{ errorConfirmPassword }}
        </span>
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
const errorErrors = ref(<string | null>null);
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
  errorErrors.value = error.errors.errors?.find(() => true);
}
</script>

<style>
label {
  display: inline-block;
  width: 150px;
}

.register-error {
  background-color: #310a0a;
  border: 1px solid #b89292;
  border-radius: 4px;
  padding: 5px;
  white-space: nowrap;
}
</style>