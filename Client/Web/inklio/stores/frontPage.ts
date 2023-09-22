import { defineStore } from "pinia";
import { AsyncData } from "nuxt/app";
import { Ask, ODataResponse } from "~/misc/types";

function ToODataResponse<T>(response: AsyncData<any, Error | null>): ODataResponse<T> {
  if (response.error.value) {
    return ({
      error: response.error.value
    }) as ODataResponse<T>;
  }
  const obj = response.data.value;
  if (obj === null) {
    return ({
      error: "Failed to fetch data",
    }) as ODataResponse<T>;
  }
  return ({
    context: "@odata.context" in obj ? obj["@odata.context"] : null,
    nextLink: "@odata.nextLink" in obj ? obj["@odata.nextLink"] : null,
    count: "@odata.count" in obj ? obj["@odata.count"] : null,
    value: "value" in obj ? obj.value : obj.value,
  }) as ODataResponse<T>;
}

export type FrontPageState = {
  asks: Ask[];
  nextLink: string | null,
  error: any | null,
}

const emptyFrontPageState: FrontPageState = {
  asks: [],
  nextLink: null,
  error: null,
}

export const useFrontPageStore = defineStore({
  id: 'frontPageStore',
  state: () => emptyFrontPageState,
  getters: {
    getAsks: (state) => state.asks,
  },
  actions: {
    async initialize() {
      this.error = null;
      const askFetch = await useFetchX("api/v1/asks?expand=deliveries(expand=images;select=images,body,title)&select=deliveries,commentCount,deliveryCount,body,title,id"); //&orderby=id%20desc
      const odataResponse = ToODataResponse<Ask>(askFetch as AsyncData<any, Error | null>);
      if (odataResponse.error) {
        this.error = odataResponse.error;
      }
      this.asks = odataResponse.value;
      this.nextLink = odataResponse.nextLink;
    },
    async next() {
      if (this.nextLink === null) {
        return;
      }

      // Prevent multiple simultaneous calls to this function from creating multiple
      // asynchronous requests
      this.error = null;
      const url = this.nextLink;
      this.nextLink = null;

      // Fetch the next collection of asks.
      const askFetch = await useFetch(url);
      const odataResponse = ToODataResponse<Ask>(askFetch as AsyncData<any, Error | null>);
      if (odataResponse.error) {
        this.error = odataResponse.error;
      }
      else {
        this.asks = this.asks.concat(odataResponse.value);
        this.nextLink = odataResponse.nextLink;
      }
    },
  },
});