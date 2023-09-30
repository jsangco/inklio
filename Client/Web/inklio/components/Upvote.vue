<template>
  <span class="upvote" @click="toggleUpvote">
    <span class="upvote-count">{{ upvoteCount }}</span>
    <svg viewBox="0 0 512 512" height="30px" :class="getUpvoted()"><polygon points="256.5,64.5 64.5,256.5 176.5,256.5 176.5,448.5 336.5,448.5 336.5,256.5 448.5,256.5 "/></svg>
  </span>
</template>
<script setup lang="ts">
import { useAccountStore } from '~/stores/account';

const props = defineProps<{
  upvoteCount: number,
  isUpvoted: boolean,
  askId: number,
  deliveryId: number | null,
  commentId: number | null,
}>();

const account = useAccountStore();

const isUpvoted = ref(props.isUpvoted);
const upvoteCount = ref(props.upvoteCount);

const getUpvoted = () => {
  var upvoteClass = isUpvoted.value ? "upvote-on" : "upvote-off";
  return upvoteClass;
}

const toggleUpvote = async () => {
  if (account.isLoggedIn == false) {
    navigateTo("/login");
    return;
  }

  isUpvoted.value = !isUpvoted.value;
  upvoteCount.value = isUpvoted.value ? upvoteCount.value + 1 : upvoteCount.value - 1;

  const url = props.deliveryId ?
    props.commentId ? `v1/asks/${props.askId}/deliveries/${props.deliveryId}/comments/${props.commentId}/upvote`
      : `v1/asks/${props.askId}/deliveries/${props.deliveryId}/upvote`
    : props.commentId ? `v1/asks/${props.askId}/comments/${props.commentId}/upvote`
      : `v1/asks/${props.askId}/upvote`;

  const method = isUpvoted.value ? 'POST' : 'DELETE';
  const fetchResult = await useFetchX(url, {
    method: method
  });
}

</script>

<style>
.upvote {
  display:flex;
  cursor: pointer;
}

.upvote-count {
  align-self: center;
  transform: translateY(2px);
}
.upvote-off {
  fill: var(--upvote-off-fill);
}

.upvote-on {
  fill: var(--upvote-on-fill);
}
</style>