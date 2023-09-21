<template>
  <div>
    <h1>{{ error?.statusCode }} {{ error?.statusMessage }} </h1>
    <div v-if="isDevelopment">
      {{ error }}
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps({
  error: Object
});
const config = useRuntimeConfig();
const isDevelopment = config.public.env.toUpperCase() == "DEVELOPMENT";
</script>