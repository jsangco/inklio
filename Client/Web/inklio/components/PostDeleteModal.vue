<template>
  <div class="modal-overlay" @click="$emit('close-modal')">
    <div class="modal" @click.stop>
      <h3>Post Deletion</h3>
      <form ref="elForm" @submit.prevent="submit">
        <div v-if="isSubmittedOk" class="form-success">
          Post has been deleted.
        </div>
        <div v-if="submitError" class="form-error">
          {{ submitError.detail }}
        </div>
        <div>
          <label>User Message: </label>
        </div>
        <textarea v-model="deletionCreate.userMessage" @keydown.ctrl.enter="submit"
          placeholder="Explain to the user why their post is being deleted..." />
        <div>
          <label>Internal Comments: </label>
        </div>
        <textarea v-model="deletionCreate.internalComment" @keydown.ctrl.enter="submit"
          placeholder="Any additional comments that only moderators should see?" />
        <input type="submit" value="Submit" />
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { format } from 'path';
import { Deletion } from '~/misc/types';

const props = defineProps<{
  askId: number,
  deliveryId: number | null,
  commentId: number | null,
}>();

const deletionCreate = ref<Deletion>({
  deletionType: 0,
} as Deletion);
const isSubmitting = ref(false);
const isSubmittedOk = ref(false);
const submitError = ref<any | null>(null);
const elForm = ref(<any | null>null);
const emit = defineEmits(["post-deleted", "close-modal"]);

const submit = async () => {
  if (isSubmitting.value) {
    return;
  }
  isSubmitting.value = true;

  const url = props.deliveryId ?
    props.commentId ? `v1/asks/${props.askId}/deliveries/${props.deliveryId}/comments/${props.commentId}`
      : `v1/asks/${props.askId}/deliveries/${props.deliveryId}`
    : props.commentId ? `v1/asks/${props.askId}/comments/${props.commentId}`
      : `v1/asks/${props.askId}`;
  const fetchResult = await useFetchX(url, {
    body: deletionCreate.value,
    method: 'DELETE',
  }).catch(error => {
    isSubmitting.value = false;
  });

  if (fetchResult?.status.value == "success") {
    isSubmittedOk.value = true;
    emit("post-deleted");
  }
}

</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  display: flex;
  justify-content: center;
  background-color: #00000077;
  z-index: 10;
}

textarea {
  resize:none;
  height: 150px;
  width: 400px;
}

.modal {
  text-align: center;
  background-color: var(--background-color);
  height: 500px;
  width: 500px;
  margin-top: 10%;
  padding: 60px 0;
  border-radius: 20px;
}

.close {
  margin: 10% 0 0 16px;
  cursor: pointer;
}

.close-img {
  width: 25px;
}

.check {
  width: 150px;
}

h6 {
  font-weight: 500;
  font-size: 28px;
  margin: 20px 0;
}

p {
  font-size: 16px;
  margin: 20px 0;
}

button {
  background-color: #ac003e;
  width: 150px;
  height: 40px;
  color: white;
  font-size: 14px;
  border-radius: 16px;
  margin-top: 50px;
}
</style>