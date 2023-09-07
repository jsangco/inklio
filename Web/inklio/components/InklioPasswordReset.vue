<template>
  <div>
    <h1>Password Reset</h1>
    <div v-if="errorErrors" class="form-error" @click="errorErrors = null">
      <p>{{ errorErrors }}</p>
    </div>
    <div v-if="successMessage" style="background-color: #1b4625;">
      <p>{{ successMessage }}</p>
    </div>
    <form @submit.prevent="registerUser">
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
      <button type="submit">Register</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user';

const route = useRoute();
const code = route.query.code?.toString() ?? "";
const email = ref('');
const password = ref('');
const confirmPassword = ref('');
const successMessage = ref('');
const errorEmail = ref(<string | null>null);
const errorPassword = ref(<string | null>null);
const errorConfirmPassword = ref(<string | null>null);
const errorErrors = ref(<string | null>null);
const user = useUserStore();

const registerUser = async () => {
  const { isSuccess, error } = await user.passwordReset(email.value, password.value, confirmPassword.value, code);
  errorEmail.value = error?.errors?.email?.find(() => true);
  errorPassword.value = error?.errors?.password?.find(() => true);
  errorConfirmPassword.value = error?.errors?.confirmPassword?.find(() => true);
  errorErrors.value = errorEmail.value || errorPassword.value || errorConfirmPassword.value ? null : error?.detail;

  if (isSuccess) {
    successMessage.value = "Your password has been reset."
  }
}
</script>

<style>
label {
  display: inline-block;
  width: 150px;
}

</style>