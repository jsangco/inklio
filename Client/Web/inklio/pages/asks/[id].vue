<template>
  <div>
    <AskPage :id="$route.params.id"/>
  </div>
</template>

<script setup lang="ts">
import { Ask } from '~/types/types';
</script>