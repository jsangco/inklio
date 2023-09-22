<template>
  <div class="askpage">
    <div class="askpage-ask">
      <h1>{{ ask.title }}</h1>
      <template v-for="ai in ask.images">
        <img :src="ai.url" />
      </template>
      <p>{{ ask.body }}</p>
    </div>
    <DeliverySubmit :ask="ask"/>
    <div>
      <h1>Deliveries</h1>
      <template v-for="d in ask.deliveries">
        <h3>Delivery by {{ d.createdBy }}</h3>
        <div class="askpage-delivery">
          <h2>{{ d.title }}</h2>
          <template v-for="di in d.images">
            <img :src="di.url" />
          </template>
          <p>{{ d.body }}</p>
          <div>
            <h4>Delivery comments</h4>
            <template v-for="dc in d.comments">
              <div class="askpage-delivery-comment">
                <p>{{ dc.body }}</p>
              </div>
            </template>
          </div>
        </div>
      </template>
    </div>
    <div>
      <h1>Comments</h1>
      <template v-for="ac in ask.comments">
        <div class="askpage-delivery-comment">
          <p>{{ ac.body }}</p>
        </div>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Ask } from '@/misc/types';

const props = defineProps<{
  id: string
}>();
const askFetch = await useFetchX(`api/v1/asks/${props.id}?expand=deliveries(expand=images,comments,tags),images,comments,tags`);
if (askFetch.error.value) {
  throw askFetch.error.value;
}
const ask = ref(askFetch.data.value as Ask);

</script>

<style>
.askpage img {
  padding: 5px;
}
</style>