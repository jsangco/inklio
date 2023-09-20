<template>
  <div class="askcard">
    <h1>{{ ask.title }} ({{ ask.id }})</h1>
    <p>{{ ask.body }}</p>
    <AskCardDelivery v-if="ask.deliveries.length > 0" :delivery="ask.deliveries[0]"/>
  </div>
</template>

<script setup lang="ts">
import { Ask } from "@/types/types"
const props = defineProps<{
  ask : Ask,
}>();

</script>

<style>
.askcard h1 {
  font-size: large;
  margin: 4px;
}

.askcard p {
  font-size: small;
  margin: 4px;
}

.askcard img {
  object-fit: cover;
  max-width: 640px;
  max-height: 360px;
}

.askcard{
  background-color: var(--askcard-background-color);
  border: solid var(--askcard-border-color) 3px;
  border-radius: 10px;
  padding: 5px;
  margin-left: 40px;
  margin-right: 40px;
  margin-bottom: 5px;
  width: 440px;
}

.askcard:hover {
  background-color: var(--askcard-hover-background-color);
}
</style>