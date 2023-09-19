<template>
  <div>
    <div>Delivery</div>
    <div v-if="delivery.title">{{ delivery.title }}</div>
    <img :src="delivery.images[0].url.replace('azure-storage', '127.0.0.1')"/>
    <div v-if="delivery.body">{{ delivery.body }}</div>
  </div>
</template>

<script setup lang="ts">
import { Delivery } from "@/types/types"
import { del } from "nuxt/dist/app/compat/capi";
const props = defineProps<{
  delivery : Delivery,
}>();

</script>