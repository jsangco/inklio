<template>
  <div>
    <div class="postactions">
      <Icon class="icon" name="fa6-regular:flag" size="1.2em" @click="flagPost" />
      <Icon class="icon" name="fluent-mdl2:delete" size="1.2em" @click="deletePost" v-show="account.isModerator"/>
    </div>
    <PostDeleteModal v-show="showModal" @close-modal="showModal = false"
    :ask-id="askId" :delivery-id="deliveryId" :comment-id="commentId" @post-deleted="emit('post-deleted')"/>
  </div>
</template>

<script setup lang="ts">
import { del } from 'nuxt/dist/app/compat/capi';
import { useAccountStore } from '~/stores/account';

const props = defineProps<{
  askId: number,
  deliveryId: number | null,
  commentId: number | null,
}>();
const account = useAccountStore();
const emit = defineEmits(["post-deleted"]);
const showModal = ref(false);

const flagPost = () => {

}

const deletePost = () => {
  showModal.value = true;
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