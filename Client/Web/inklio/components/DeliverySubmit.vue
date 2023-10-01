<template>
  <div>
    <h3><i>Submit your delivery:</i></h3>
    <form class="delivery-submit" ref="elForm" @submit.prevent="submitDelivery">
      <div v-if="isDeliverySubmittedOk" class="form-success">
        Delivered!
      </div>
      <div v-if="submitError" class="form-error">
        {{ submitError.detail }}
      </div>
      <div class="delivery-submit-text" ref="elDeliverySubmit">
        <input type="text" ref="elTitle" name="title" v-model="deliveryCreate.title"
          placeholder="(Optional) Your delivery's title..." />
        <textarea ref="elBody" name="body" v-model="deliveryCreate.body" @input="autoResize" @mouseup="autoResize"
          @focus="autoResize" @keydown.ctrl.enter="submitDelivery" placeholder="(Optional) Describe your delivery..."></textarea>
      </div>
      <label>Images: </label>
      <input type="file" ref="elImages" name="images" accept="image/jpeg,image/png" multiple="true" />
      <input type="submit" value="Submit"/>
    </form>
  </div>
</template>

<script setup lang="ts">
import { AskCard, DeliverySubmit } from "#build/components";
import { Ask, Delivery, DeliveryCreate } from "@/misc/types"
import { useAccountStore } from "~/stores/account";
const emit = defineEmits(["delivery-submit"]);
const props = defineProps<{
  ask: Ask
}>();
const account = useAccountStore();
const isDeliverySubmittedOk = ref(false);
const isSubmitting = ref(false);
const deliveryCreate = ref<DeliveryCreate>({} as DeliveryCreate);
const elForm = ref(<any | null>null);
const elBody = ref(<any | null>null);
const elTitle = ref(<any | null>null);
const elImages = ref(<any | null>null);
const elDeliverySubmit = ref(<any | null>null);
const submitError = ref<any | null>(null);
const config = useRuntimeConfig();

const autoResize = () => {
  elDeliverySubmit.value.style.height = `${elTitle.value.scrollHeight + elBody.value.scrollHeight + 6}px`;
};

const submitDelivery = async () => {
  if (account.isLoggedIn == false) {
    navigateTo("/login-register");
    return;
  }
  if (isSubmitting.value) {
    return;
  }
  isSubmitting.value = true;

  const form = new FormData();
  form.append("delivery", JSON.stringify(deliveryCreate.value));
  const files = elImages.value.files;
  for (var i = 0; i < files.length; i++) {
    let file = files[i] as File;
    form.append("images", file, `image${i}`);
  }

  submitError.value = null;
  isDeliverySubmittedOk.value = false;
  var url = `${config.public.apiUrl}/v1/asks/${props.ask.id}/deliveries`;
  const fetchResults = await $fetch.raw(url, {
    method: "POST",
    body: form,
  }).catch(error => {
    isSubmitting.value = false;
    if (error.status == 413) {
      submitError.value = { detail: "Submission size is too large" };
    }
    else {
      submitError.value = error.data;
    }
  });

  // Clear the form and reload the page.
  if (fetchResults?.ok) {
    isDeliverySubmittedOk.value = true;
    elForm.value.reset();
    deliveryCreate.value = {} as DeliveryCreate;
    emit("delivery-submit", { id: props.ask.id });
  }
  isSubmitting.value = false;
}

</script>

<style>
.delivery-submit .form-success {
  margin: 0 0 10px 0;
}

.delivery-submit .form-error {
  margin: 0 0 10px 0;
}

.delivery-submit button {
  margin: 10px;
  display: block;
}

.delivery-submit-text {
  display: flex;
  flex-direction: column;
  width: 600px;
  height: 150px;
  box-sizing: border-box;
}

.delivery-submit-text input {
  width: 100%;
  flex: none;
  padding: 10px;
  box-sizing: border-box;
}

.delivery-submit-text textarea {
  flex: none;
  width: 100%;
  min-height: 111px;
  box-sizing: border-box;
  resize: vertical;
}
</style>