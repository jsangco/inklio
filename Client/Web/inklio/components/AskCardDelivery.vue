<template>
  <div class="askcarddelivery askcarddelivery-wrapper">
    <img :src="delivery.images[0].url"/>
    <div class="askcarddelivery-overlay">
      <h1 v-if="delivery.title">{{ delivery.title }}</h1>
      <p v-if="delivery.body">{{ delivery.body }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Delivery } from "@/types/types"
import { del } from "nuxt/dist/app/compat/capi";
const props = defineProps<{
  delivery : Delivery,
}>();

</script>

<style>
.askcarddelivery-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .6);
  display: flex;
  padding: 10px;
  box-sizing: border-box;
}

.askcarddelivery h1{
  text-align: left;
}

.askcarddelivery p{
  position: absolute;
  bottom: 10px;
  left: 10px;
}

.askcarddelivery-wrapper{
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
  background-color: grey;
  border: none rgb(0, 0, 0) 2px;
  border-radius: 10px;
  padding: 5px;
  margin-left: 10px;
  width: 400px;
  overflow: hidden;
}
</style>