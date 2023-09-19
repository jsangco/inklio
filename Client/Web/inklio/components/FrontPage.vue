<template>
  <div>
    <h1>Asks</h1>
    <button type="button" @click="downloadAsks(next as string)">Next</button>
    <template v-if="!(odataResponse?.error)" v-for="ask in asks">
      <div >
        <span>{{ ask.title }}</span> - <span>{{ ask.body }}</span>
      </div>
    </template>
    <template v-else>
      <div>
        {{odataResponse.error}}
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { useAsksStore, Ask, ODataResponse } from '@/stores/asks';
const asks = ref(<Ask[] | null>null);
const next = ref(<string | null>null);
const odataResponse = ref(<ODataResponse<Ask> | null>null);

const askStore = useAsksStore();

const downloadAsks = async (url:string) =>
{
  odataResponse.value = await askStore.downloadAsks(url);
  next.value = odataResponse.value.nextLink;
  asks.value = odataResponse.value.value;
}
downloadAsks(next.value as string);
</script>