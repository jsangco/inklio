<template>
  <div>
    <template v-for="ask in myAsks">
      <div >
        <span>{{ ask.title }}</span> - <span>{{ ask.body }}</span>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { useAsksStore } from '@/stores/asks';
const myAsks = ref()

const askStore = useAsksStore();
const stuff = await askStore.downloadAsks();
myAsks.value = stuff;
</script>