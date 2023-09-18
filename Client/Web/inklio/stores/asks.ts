import { defineStore } from "pinia";
import { camelizeKeys, decamelizeKeys } from "humps";


// export ODataResponse = {
// }

export type Ask = {
  title: string;
  body: string;
}

export type AskManager = {
  asks : Ask[]
}

const emptyAskManager: AskManager = {
  asks: []
}

export const useAsksStore = defineStore({
  id: 'asksStore',
  state: () => emptyAskManager,
  getters: {},
  actions: {
    async downloadAsks() {
      const odataResponse = await useFetchX("api/v1/asks");
      const asks = odataResponse.data;
      return asks.value.value as Ask[];
    }
  },
});