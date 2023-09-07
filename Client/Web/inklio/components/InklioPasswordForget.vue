<template>
  <div>
    <h1>Forget Password</h1>
    <div v-if="errorErrors" class="form-error" @click="errorErrors = null">
      <p>{{ errorErrors }}</p>
    </div>
    <div v-if="successMessage" style="background-color: #1b4625;">
      <p>{{ successMessage }}</p>
    </div>
    <form @submit.prevent="passwordForget">
      <div>
        <label for="email">Email</label>
        <input v-model="email" type="text" id="email" />
        <span v-if="errorEmail" class="form-error" @click="errorEmail = null">
          {{ errorEmail }}
        </span>
      </div>
      <button type="submit">Submit</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user';

const email = ref('')
const successMessage = ref('')
const errorEmail = ref(<string | null>null);
const errorErrors = ref(<string | null>null);
const user = useUserStore();

const passwordForget = async () => {
  const { isSuccess, error } = await user.passwordForget(email.value);
  errorEmail.value = error?.errors?.email?.find(() => true);
  errorErrors.value = error?.errors?.errors?.find(() => true);
  if (isSuccess) {
    successMessage.value = "Thanks! If your email is correct, you'll receive an email with a link to reset your password shortly."
  }
}
</script>

<style>
label {
  display: inline-block;
  width: 150px;
}

</style>