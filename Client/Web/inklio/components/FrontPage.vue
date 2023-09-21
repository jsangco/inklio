<template>
  <div>
    <h1>Front Page</h1>
    <div ref="scrollComponent">
      <template v-if="!frontPageStore.error" v-for="a in frontPageStore.getAsks">
        <AskCard v-bind:ask="a"/>
      </template>
      <template v-else>
        <div>
          {{ frontPageStore.error }}
        </div>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Ask } from '~/types/types';
import { useFrontPageStore } from '@/stores/frontPage';
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
    await frontPageStore.next();
  }
}

const frontPageStore = useFrontPageStore();
await frontPageStore.initialize();

</script>