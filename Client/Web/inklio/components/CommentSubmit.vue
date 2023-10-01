<template>
  <div>
    <h4><i>Submit your comment:</i></h4>
    <form class="comment-submit" @submit.prevent="submitComment">
      <div v-if="isCommentSubmittedOk" class="form-success">
        Posted!
      </div>
      <div v-if="submitError" class="form-error">
        {{ submitError.detail }}
      </div>
      <div class="comment-submit-text" ref="elCommentSubmit">
        <textarea ref="elBody" name="body" v-model="commentCreate.body" @input="autoResize" @mouseup="autoResize"
          @focus="autoResize" placeholder="What are your thoughts?"></textarea>
      </div>
      <button type="submit">Add comment</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { Ask, Delivery, Comment, CommentCreate } from "@/misc/types"
import { useAccountStore } from "~/stores/account";
const emit = defineEmits(["comment-submit"]);
const props = defineProps<{
  ask: Ask,
  delivery: Delivery|null
}>()
const account = useAccountStore();
const isCommentSubmittedOk = ref(false);
const commentCreate = ref<CommentCreate>({} as CommentCreate);
const elBody = ref(<any | null>null);
const elCommentSubmit = ref(<any | null>null);
const submitError = ref<any | null>(null);
const config = useRuntimeConfig();

const autoResize = () => {
  elCommentSubmit.value.style.height = `${elBody.value.scrollHeight + 6}px`;
};

const submitComment = async () => {
  if (account.isLoggedIn == false) {
    navigateTo("/login-register");
    return;
  }

  submitError.value = null;
  isCommentSubmittedOk.value = false;
  var url = props.delivery == null ? // If there's no delivery post the comment to the ask.
    `${config.public.apiUrl}/v1/asks/${props.ask.id}/comments`
    : `${config.public.apiUrl}/v1/asks/${props.ask.id}/deliveries/${props.delivery.id}/comments`;
  const fetchResults = await $fetch.raw(url, {
    method: "POST",
    body: commentCreate.value,
  }).catch(error => {
    submitError.value = error.data;
  });

  // Clear the form and reload the page.
  if (fetchResults?.ok) {
    isCommentSubmittedOk.value = true;
    commentCreate.value = {} as CommentCreate;
    emit("comment-submit", { id: props.ask.id });
  }
}

</script>

<style>
.comment-submit .form-success {
  margin: 0 0 10px 0;
}

.comment-submit .form-error {
  margin: 0 0 10px 0;
}

.comment-submit button {
  margin: 10px;
  display: block;
}

.comment-submit-text {
  display: flex;
  flex-direction: column;
  width: 600px;
  height: 110px;
  box-sizing: border-box;
}

.comment-submit-text input {
  width: 100%;
  flex: none;
  padding: 10px;
  box-sizing: border-box;
}

.comment-submit-text textarea {
  flex: none;
  width: 100%;
  min-height: 111px;
  box-sizing: border-box;
  resize: vertical;
}
</style>