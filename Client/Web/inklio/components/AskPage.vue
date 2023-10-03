<template>
  <div class="askpage">
    <div class="askpage-ask">
      <h1>
        <Upvote class="askpage-ask-upvote" :isUpvoted="ask.isUpvoted" :upvoteCount="ask.upvoteCount" :askId="ask.id"
          :deliveryId="null" :commentId="null" />
        {{ ask.title }}
      </h1>
      <template v-for="ai in ask.images">
        <img :src="ai.url" />
      </template>
      <p>{{ ask.body }}</p>
      <ModActions :askId="ask.id" :comment-id="null" :delivery-id="null" />
    </div>
    <DeliverySubmit :ask="ask" @delivery-submit="reload" />
    <div class="askpage-comment-wrapper">
      <h3>{{ ask.comments.length }} Comment{{ ask.comments.length == 1 ? "" : "s" }}</h3>
      <div @click="showAskCommentSubmit">
        <CommentSubmit v-if="isShowAskCommentSubmit" :ask="ask" :delivery="null" @comment-submit="reload" />
        <a v-if="!isShowAskCommentSubmit" class="comment-add">Add a comment</a>
      </div>
      <template v-for="ac in ask.comments">
        <div class="askpage-comment">
          <div class="askpage-modactionbar">
            <Upvote :isUpvoted="ac.isUpvoted" :upvoteCount="ac.upvoteCount" :askId="ask.id" :deliveryId="null"
              :commentId="ac.id" />
            <ModActions :askId="ask.id" :comment-id="ac.id" :delivery-id="null" @post-deleted="reload" />
          </div>
          <div class="createdby">
            <img src="~/assets/img/user.png" /> <i>{{ ac.createdBy }}</i>
          </div>
          <p>{{ ac.body }}</p>
        </div>
      </template>
    </div>
    <div class="askpage-deliveries">
      <h1>{{ ask.deliveries.length }} Deliver{{ ask.deliveries.length == 1 ? "y" : "ies" }}</h1>
      <template v-for="d in ask.deliveries">
        <div class="askpage-delivery">
          <div class="askpage-modactionbar">
            <Upvote :isUpvoted="d.isUpvoted" :upvoteCount="d.upvoteCount" :askId="ask.id" :deliveryId="d.id"
              :commentId="null" />
            <ModActions :askId="ask.id" :comment-id="null" :delivery-id="d.id" @post-deleted="reload"/>
          </div>
          <div class="createdby">
            <img src="~/assets/img/user.png" /> <i>{{ d.createdBy }}</i>
          </div>
          <h2>{{ d.title }} </h2>
          <template v-for="di in d.images">
            <img :src="di.url" :class="getImageExpandState(di.url)" @click="toggleImageExpand(di.url)"
              @click.middle="openImageNewTab(di.url)" @mousedown="handleMouseDown" />
          </template>
          <p>{{ d.body }}</p>
          <div class="askpage-delivery-comment-wrapper">
            <h4>{{ d.comments.length }} Comment{{ d.comments.length == 1 ? "" : "s" }}</h4>
            <div @click="showDeliveryCommentSubmit(d.id.toString())">
              <CommentSubmit v-if="isShowDeliveryCommentSubmit[d.id.toString()]" :ask="ask" :delivery="d"
                @comment-submit="reload" />
              <a v-if="!isShowDeliveryCommentSubmit[d.id.toString()]" class="comment-add">Add a comment</a>
            </div>
            <template v-for="dc in d.comments">
              <div class="askpage-delivery-comment">
                <div class="askpage-modactionbar">
                  <Upvote :isUpvoted="dc.isUpvoted" :upvoteCount="dc.upvoteCount" :askId="ask.id" :deliveryId="d.id"
                    :commentId="dc.id" />
                  <ModActions :askId="ask.id" :comment-id="dc.id" :delivery-id="d.id" @post-deleted="reload"/>
                </div>
                <div class="createdby">
                  <img src="~/assets/img/user.png" /> <i>{{ dc.createdBy }}</i>
                </div>
                <p>{{ dc.body }}</p>
              </div>
            </template>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Ask, Delivery } from '@/misc/types';
import { del } from 'nuxt/dist/app/compat/capi';
const ask = ref<Ask>({} as Ask);
const props = defineProps<{
  id: string
}>();

// Hide/Show comment controls
const isShowAskCommentSubmit = ref(false);
const showAskCommentSubmit = () => {
  isShowAskCommentSubmit.value = true;
}
const isShowDeliveryCommentSubmit = ref<any>({});
const showDeliveryCommentSubmit = (i: string) => {
  isShowDeliveryCommentSubmit.value[i] = true;
}

// Shrink/Expand images
const expandedItems = ref<any>({});
const toggleImageExpand = (deliveryUrl: string) => {
  const isExpanded = `${deliveryUrl}` in expandedItems.value && expandedItems.value[deliveryUrl];
  if (isExpanded) {
    expandedItems.value[deliveryUrl] = false;
  }
  else {
    expandedItems.value[deliveryUrl] = true;
  }
}
const getImageExpandState = (deliveryUrl: string) => {
  const isExpanded = `${deliveryUrl}` in expandedItems.value && expandedItems.value[deliveryUrl];
  return isExpanded ? "askpage-delivery-img-expand" : "askpage-delivery-img-no-expand";
}

// Open image in new tab
const openImageNewTab = (deliveryUrl: string) => {
  event?.preventDefault();
  window.open(deliveryUrl, "_blank");
}
const handleMouseDown = (event: any) => {
  if (event.button == 1) {
    event.preventDefault();
  }
}

// (re)Loads page content when the page is initialized or when a comment/delivery is added
const reload = async () => {
  const askFetch = await useFetchX(`v1/asks/${props.id}?expand=deliveries(expand=images,comments,tags),images,comments,tags`);
  if (askFetch.error.value) {
    throw askFetch.error.value;
  }
  ask.value = askFetch.data.value as Ask;
  isShowAskCommentSubmit.value = false;
  isShowDeliveryCommentSubmit.value = {};
}
await reload(); // Initial page load

</script>

<style>
.comment-add {
  cursor: pointer;
  margin-left: 10px;
}

.createdby {
  font-size: smaller;
}

.createdby i {
  opacity: .8;
}

.createdby img {
  filter: invert();
  vertical-align: middle;
  height: 16px;
  margin-right: 10px;
}

.askpage {
  width: 800px;
}

.askpage img {
  padding-right: 5px;
}

.askpage-ask {
  margin-bottom: 10px;
}

.askpage-deliveries {
  background-color: var(--askcarddelivery-background-color);
  margin-top: 10px;
  border-top: 1px solid var(--askpage-comment-border-color);
}

.askpage-delivery-comment-wrapper {
  background-color: var(--askcard-background-color);
  margin-left: 10px;
}

.askpage-delivery {
  padding-left: 20px;
  margin-bottom: 20px;
}

.askpage-delivery-img-expand {
  max-height: 50vw;
  min-height: 333px;
  cursor: pointer;
}

.askpage-delivery-img-no-expand {
  max-width: 592px;
  max-height: 333px;
  cursor: pointer;
}

.askpage h2 {
  margin: 2px 2px 5px 5px;
}

.askpage h3 {
  margin: 2px 2px 5px 5px;
}

.askpage h4 {
  margin: 2px 2px 2px 5px;
}

.askpage p {
  margin: 11px 0px 11px 5px;
}

.askpage-delivery-comment {
  border-bottom: 1px solid var(--askpage-comment-border-color);
  padding: 20px 0 0 20px;
}

.askpage-comment-wrapper {
  background-color: var(--askcard-background-color);
  border-top: 1px solid var(--askpage-comment-border-color);
}

.askpage-comment {
  border-bottom: 1px solid var(--askpage-comment-border-color);
  padding: 20px 0 0 20px;
}

.askpage-ask-upvote {
  display: inline;
}

.askpage-modactionbar {
  display: flex;
  justify-content: space-between;
  padding-right: 10px;
}

.askpage-modactionbar-misc>* {
  padding-right: 10px;
}</style>