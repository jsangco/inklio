<template>
  <div>
    <form class="delivery-submit" @submit.prevent="submitDelivery">
      <div class="delivery-submit-text" ref="elDeliverySubmit">
        <input type="text" ref="elTitle" v-model="deliveryCreate.title" placeholder="Your delivery title..." />
        <textarea ref="elBody" v-model="deliveryCreate.body" @input="autoResize" @mouseup="autoResize" @focus="autoResize"
          placeholder="Describe your delivery..."></textarea>
      </div>
      <input type="file" ref="elImages" accept="image/jpeg,image/png" multiple="true" />
      <button type="submit">Submit</button>
    </form>
    {{ submitResults }}
  </div>
</template>

<script setup lang="ts">
import { DeliverySubmit } from "#build/components";
import {Ask, Delivery, DeliveryCreate} from "@/misc/types"
const props = defineProps<{
  ask: Ask
}>();
const deliveryCreate = ref<DeliveryCreate>({} as DeliveryCreate); 
const elBody = ref(<any | null>null);
const elTitle = ref(<any | null>null);
const elImages = ref(<any | null>null);
const elDeliverySubmit = ref(<any | null>null);
const submitResults = ref<any>("pending");

const autoResize = () => {
  console.log("test");
  console.log(elTitle.value.scrollHeight);
  elDeliverySubmit.value.style.height = `${elTitle.value.scrollHeight + elBody.value.scrollHeight + 6}px`;
};

const submitDelivery = async () => {
  const form = new FormData();
  console.log(JSON.stringify(JSON.stringify(deliveryCreate.value)));
  form.append("delivery", JSON.stringify(deliveryCreate.value));
  const files = elImages.value.files;
  for (var i = 0; i < files.length; i++) {
    let file = files[i] as File;
    form.append("images", file, `image${i}`);
  }

  console.log(form);
  // var url = `http://localhost:7187/v1/asks/${props.ask.id}/deliveries`;
  var url = `http://localhost/api/v1/asks/${props.ask.id}/deliveries`;
  const fetchResults = await $fetch.raw(url, {
    method: "POST",
    body: form,
  }).catch(error => {
    submitResults.value = error.data;
  });
}

</script>

<style>
.delivery-submit button {
  margin: 10px;
  display: block;
}

.delivery-submit-text {
  display: flex;
  flex-direction: column;
  /* justify-content: space-between; */
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