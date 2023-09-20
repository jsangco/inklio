<template>
  <div class="askcard askcard-wrapper">
    <h1>{{ ask.title }}</h1>
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
  max-width: 300px;
  max-height: 300px;
}

.askcard-wrapper{
  background-color: rgb(46, 46, 46);
  border: solid rgb(0, 0, 0) 5px;
  border-radius: 10px;
  padding: 5px;
  margin-left: 40px;
  margin-right: 40px;
  width: 440px;
}

.askcard-wrapper:hover {
  background-color: rgb(57, 57, 57);

}
</style>