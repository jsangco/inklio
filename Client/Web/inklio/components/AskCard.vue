<template>
  <div>
    <div>Ask</div>
    <span>{{ ask.id }}. {{ ask.title }}</span> - <span>{{ ask.body }}, {{ ask.createdBy }}</span>
    <AskCardDelivery v-if="ask.deliveries.length > 0" :delivery="ask.deliveries[0]"/>
  </div>
</template>

<script setup lang="ts">
import { Ask } from "@/types/types"
const props = defineProps<{
  ask : Ask,
}>();

</script>