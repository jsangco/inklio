<template>
  <div class="askcard">
    <NuxtLink :to="`/asks/${ask.id}`">
      <div>
        <h1>{{ ask.title }}</h1>
        <p>{{ ask.body }}</p>
      </div>
      <AskCardDelivery v-if="ask.deliveries.length > 0" :delivery="ask.deliveries[0]" />
    </NuxtLink>
    <div class="askcard-footer">
      <div>
        <Upvote :isUpvoted="ask.isUpvoted" :upvoteCount="ask.upvoteCount" :askId="ask.id" :deliveryId="null" :commentId="null"/>
        <span>{{ ask.commentCount + ask.deliveryCount }} replies</span>
        <ModActions :askId="ask.id" :comment-id="null" :delivery-id="null" :creator="ask.createdBy"/>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { AskCard } from "#build/components";
import { Ask } from "@/misc/types"
const props = defineProps<{
  ask : Ask,
}>();
</script>

<style scoped>
.askcard h1 {
  font-size: large;
  margin: 4px;
  align-self: flex-start;
}

.askcard p {
  font-size: small;
  margin: 4px;
}

.askcard a {
  color: var(--font-color);
}

.askcard img {
  object-fit: cover;
  max-width: 640px;
  max-height: 360px;
}

.askcard {
  display:flex;
  flex-direction: column;
  justify-content: space-between;
  background-color: var(--askcard-background-color);
  border: solid var(--askcard-border-color) 3px;
  border-radius: 10px;
  margin-left: 40px;
  margin-right: 40px;
  margin-bottom: 5px;
  width: 440px;
  overflow: hidden;
}

.askcard-footer {
  align-self: flex-end;
  width:100%;
  background-color: var(--askcard-footer-background-color);
  text-align: center;
  padding-bottom: 1px;
}

.askcard-footer div {
  display: flex;
  justify-content:space-evenly;
  align-items: center;
}

.askcard:hover {
  background-color: var(--askcard-hover-background-color);
}
</style>