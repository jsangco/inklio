<template>
  <div>
    <h1>Register</h1>
    <div v-if="errorErrors" class="form-error" @click="errorErrors = null">
      <p>{{ errorErrors }}</p>
    </div>
    <form @submit.prevent="registerAccount">
      <div>
        <label for="username">Username</label>
        <input v-model="username" type="text" id="username" />
        <span v-if="errorUsername" class="form-error" @click="errorUsername = null">
          {{ errorUsername }}
        </span>
      </div>
      <div>
        <label for="email">Email</label>
        <input v-model="email" type="text" id="email" />
        <span v-if="errorEmail" class="form-error" @click="errorEmail = null">
          {{ errorEmail }}
        </span>
      </div>
      <div>
        <label for="password">Password</label>
        <input v-model="password" type="password" id="password" />
        <span v-if="errorPassword" class="form-error" @click="errorPassword = null">
          {{ errorPassword }}
        </span>
      </div>
      <div>
        <label for="confirmPassword">Confirm Password</label>
        <input v-model="confirmPassword" type="password" id="confirmPassword" />
        <span v-if="errorConfirmPassword" class="form-error" @click="errorConfirmPassword = null">
          {{ errorConfirmPassword }}
        </span>
      </div>
      <input type="submit" value="Register"/>
    </form>
  </div>
</template>

<script setup lang="ts">
import { useAccountStore } from '@/stores/account';

const username = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const errorUsername = ref(<string | null>null);
const errorEmail = ref(<string | null>null);
const errorPassword = ref(<string | null>null);
const errorConfirmPassword = ref(<string | null>null);
const errorErrors = ref(<string | null>null);
const account = useAccountStore();

watchEffect(async () => {
  if (account.isLoggedIn) {
    await navigateTo('/');
  }
});

const registerAccount = async () => {
  const { isSuccess, error } = await account.register(username.value, email.value, password.value, confirmPassword.value);
  errorUsername.value = error?.errors?.username?.find(() => true);
  errorEmail.value = error?.errors?.email?.find(() => true);
  errorPassword.value = error?.errors?.password?.find(() => true);
  errorConfirmPassword.value = error?.errors?.confirmPassword?.find(() => true);
  errorErrors.value = error?.errors?.errors?.find(() => true);
}
</script>

<style>
label {
  display: inline-block;
  width: 150px;
}

</style>