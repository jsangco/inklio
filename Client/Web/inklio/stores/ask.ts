import { defineStore } from "pinia";
import { Ask, ODataResponse } from "~/misc/types";
import { ToODataResponse } from "~/misc/utils/odata";
import { AsyncData } from "nuxt/app";

export type AskState = {
  ask: Ask | null;
  error: any | null,
}

const emptyAskState: AskState = {
  ask: null,
  error: null,
}

export const useAskStore = defineStore({
  id: 'askStore',
  state: () => emptyAskState,
  getters: {
    getAsk: (state) => state.ask,
  },
  actions: {
    async fetchAsk(id : number) {
      const askFetch = await useFetchX(`api/v1/asks/${id}?expand=deliveries(expand=images,comments,tags),images,comments,tags`);
      if (askFetch.error) {
        this.error = askFetch.error;
      }
      else {
        this.ask = askFetch.data.value as Ask;
      }
    },
  },
});