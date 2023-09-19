<template>
  <div>
    <h1>Asks</h1>
    <div ref="scrollComponent">
      <template v-if="!askStore.error" v-for="ask in askStore.getAsks">
        <div>
          <span>{{ ask.id }}. {{ ask.title }}</span> - <span>{{ ask.body }}</span>
        </div>
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
import { useAsksStore, Ask, ODataResponse } from '@/stores/asks';
const scrollComponent = ref(<any | null>null)
onMounted(() => {
  handleScroll();
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