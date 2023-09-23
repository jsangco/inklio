<template>
  <div>
    <h3><i>Create your Ask:</i></h3>
    <form class="ask-submit" ref="elForm" @submit.prevent="submitAsk">
      <div v-if="isAskSubmittedOk" class="form-success">
        Posted!
      </div>
      <div v-if="submitError" class="form-error">
        {{ submitError.detail }}
      </div>
      <div class="ask-submit-text" ref="elAskSubmit">
        <input type="text" ref="elTitle" name="title" v-model="askCreate.title"
          placeholder="(Optional) Your ask's title..." />
        <textarea ref="elBody" name="body" v-model="askCreate.body" @input="autoResize" @mouseup="autoResize"
          @focus="autoResize" placeholder="(Optional) Describe your ask..."></textarea>
      </div>
      <label>Images: </label>
      <input type="file" ref="elImages" name="images" accept="image/jpeg,image/png" multiple="true" />
      <button type="submit">Submit</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { AskCreate } from "@/misc/types"
const emit = defineEmits(["ask-submit"]);
const isAskSubmittedOk = ref(false);
const askCreate = ref<AskCreate>({} as AskCreate);
const elForm = ref(<any | null>null);
const elBody = ref(<any | null>null);
const elTitle = ref(<any | null>null);
const elImages = ref(<any | null>null);
const elAskSubmit = ref(<any | null>null);
const submitError = ref<any | null>(null);
const config = useRuntimeConfig();

const autoResize = () => {
  elAskSubmit.value.style.height = `${elTitle.value.scrollHeight + elBody.value.scrollHeight + 6}px`;
};

const submitAsk = async () => {
  const form = new FormData();
  form.append("ask", JSON.stringify(askCreate.value));
  const files = elImages.value.files;
  for (var i = 0; i < files.length; i++) {
    let file = files[i] as File;
    form.append("images", file, `image${i}`);
  }

  submitError.value = null;
  isAskSubmittedOk.value = false;
  var url = `${config.public.apiUrl}/v1/asks/`;
  const fetchResults = await $fetch.raw(url, {
    method: "POST",
    body: form,
  }).catch(error => {
    if (error.status == 413) {
      submitError.value = { detail: "Submission size is too large" };
    }
    else {
      submitError.value = error.data;
    }
  });

  // Clear the form and reload the page.
  if (fetchResults?.ok) {
    isAskSubmittedOk.value = true;
    elForm.value.reset();
    askCreate.value = {} as AskCreate;
    emit("ask-submit");
  }
}

</script>

<style>
.ask-submit .form-success {
  margin: 0 0 10px 0;
}

.ask-submit .form-error {
  margin: 0 0 10px 0;
}

.ask-submit button {
  margin: 10px;
  display: block;
}

.ask-submit-text {
  display: flex;
  flex-direction: column;
  width: 600px;
  height: 150px;
  box-sizing: border-box;
}

.ask-submit-text input {
  width: 100%;
  flex: none;
  padding: 10px;
  box-sizing: border-box;
}

.ask-submit-text textarea {
  flex: none;
  width: 100%;
  min-height: 111px;
  box-sizing: border-box;
  resize: vertical;
}
</style>