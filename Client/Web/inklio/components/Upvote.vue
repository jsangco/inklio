<template>
  <span class="upvote">
    <span>{{ props.upvoteCount }}</span>
    <svg viewBox="0 0 512 512" height="30px" :class="getUpvoted()"><polygon points="256.5,64.5 64.5,256.5 176.5,256.5 176.5,448.5 336.5,448.5 336.5,256.5 448.5,256.5 "/></svg>
  </span>
</template>
<script setup lang="ts">
const props = defineProps<{
  upvoteCount: 0,
  isUpvoted: false,
}>();

const getUpvoted = () => {
  var upvoteClass = props.isUpvoted ? "upvote-on" : "upvote-off";
  return upvoteClass;
}

</script>

<style>
.upvote {
  display: inline-block;
}

.upvote-off {
  fill: var(--upvote-off-fill);
}

.upvote-on {
  fill: var(--upvote-on-fill);
}
</style>