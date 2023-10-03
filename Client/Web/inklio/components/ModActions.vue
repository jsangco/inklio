<template>
  <div>
    <div class="postactions">
      <!-- <Icon class="icon" name="fa6-regular:flag" size="1.2em" @click="flagPost" /> -->
      <Icon class="icon" name="fluent-mdl2:delete" size="1.2em" @click="deletePost" v-show="isDeleteVisible()"/>
    </div>
    <PostDeleteModal v-show="showModal" @close-modal="showModal = false" :creator="creator"
      :ask-id="askId" :delivery-id="deliveryId" :comment-id="commentId" @post-deleted="emit('post-deleted')"/>
  </div>
</template>

<script setup lang="ts">
import { del } from 'nuxt/dist/app/compat/capi';
import { DeletionType, Deletion } from '~/misc/types';
import { useAccountStore } from '~/stores/account';

const props = defineProps<{
  askId: number,
  commentId: number | null,
  creator: string,
  deliveryId: number | null,
}>();
const account = useAccountStore();
const emit = defineEmits(["post-deleted"]);
const showModal = ref(false);
const isSubmitting = ref(false);
const isSubmittedOk = ref(false);
const submitError = ref<any | null>(null);

const flagPost = () => {

}

const deletePost = async () => {
  if (account.isModerator) {
    showModal.value = true;
  }
  else {
    if (confirm("Delete this post?")) {
      await submitDelete();
    }
  }
}

const submitDelete = async () => {
  if (isSubmitting.value) {
    return;
  }
  isSubmitting.value = true;
  const deletionCreate: Deletion = {
    deletionType: 10,
    userMessage: "",
    internalComment: ""
  };

  const url = props.deliveryId ?
    props.commentId ? `v1/asks/${props.askId}/deliveries/${props.deliveryId}/comments/${props.commentId}`
      : `v1/asks/${props.askId}/deliveries/${props.deliveryId}`
    : props.commentId ? `v1/asks/${props.askId}/comments/${props.commentId}`
      : `v1/asks/${props.askId}`;
  const fetchResult = await useFetchX(url, {
    body: deletionCreate,
    method: 'DELETE',
  }).catch(error => {
    isSubmitting.value = false;
  });

  if (fetchResult?.status.value == "success") {
    isSubmittedOk.value = true;
    emit("post-deleted");
  }
  isSubmitting.value = false;
}

const isDeleteVisible = () => {
  return account.isModerator || (props.creator == account.username);
}

</script>

<style>
.postactions>* {
  padding-right: 10px;
}

.postactions>*:hover {
  color: var(--negativeaction-color);
}

.icon {
  cursor: pointer;
}

</style>