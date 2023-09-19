<template>
  <div>
    <h1>Asks</h1>
    <div ref="scrollComponent">
      <template v-if="!askStore.error" v-for="a in askStore.getAsks">
        <AskCard v-bind:ask="a"/>
      </template>
      <template v-else>
        <div>
          {{ askStore.error }}
        </div>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAsksStore } from '@/stores/asks';
const scrollComponent = ref(<any | null>null)
onMounted(() => {
  window.addEventListener("scroll", handleScroll);
})

onUnmounted(() => {
  window.removeEventListener("scroll", handleScroll);
})

const handleScroll = async () => {
  let element = scrollComponent.value
  let currentPosition = element!.getBoundingClientRect().bottom;

  if ( currentPosition < window.innerHeight) {
    await askStore.next();
  }
}

const askStore = useAsksStore();
await askStore.initialize();

</script>