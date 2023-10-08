<template>
  <div class="askcarddelivery">
    <img v-if="delivery.images.length > 0" :src="delivery.images[0].url"/>
    <div class="askcarddelivery-overlay">
      <h1 v-if="delivery.title">{{ delivery.title }}</h1>
      <p v-if="delivery.body">{{ delivery.body }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Delivery } from "../misc/types";
import { del } from "nuxt/dist/app/compat/capi";
const props = defineProps<{
  delivery: Delivery
}>();

</script>

<style scoped>
.askcarddelivery-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: var(--askcarddelivery-overlay-background-color);
  display: flex;
  padding: 10px;
  box-sizing: border-box;
}
.askcarddelivery-overlay:hover {
  background-color: transparent;
}

.askcarddelivery h1{
  text-align: left;
  font-size: large;
}

.askcarddelivery p{
  position: absolute;
  bottom: 10px;
  left: 10px;
}

.askcarddelivery{
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  background-color: var(--askcarddelivery-background-color);
  border: solid var(--askcarddelivery-border-color) 2px;
  border-radius: 10px;
  padding: 5px;
  margin-left: 10px;
  width: 400px;
  height: 360px;
  overflow: hidden;
}
</style>