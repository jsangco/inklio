<template>
  <div>
    <div>Ask</div>
    <span>{{ ask.id }}. {{ ask.title }}</span> - <span>{{ ask.body }}, {{ ask.createdBy }}</span>
  </div>
</template>

<script setup lang="ts">
import { Ask } from "@/types/types"
const props = defineProps<{
  ask : Ask,
}>();

</script>