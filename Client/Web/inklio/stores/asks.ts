import { defineStore } from "pinia";
import { camelizeKeys, decamelizeKeys } from "humps";
import { AsyncData } from "nuxt/app";

export type ODataResponse<T> = {
  context: string | null;
  nextLink: string | null;
  count: number | null;
  value: T[];
  error: any;
}

function ToODataResponse<T>(response: AsyncData<any, Error | null>): ODataResponse<T> {
  if (response.error.value) {
    return ({
      error: response.error.value
    }) as ODataResponse<T>;
  }
  const obj = response.data.value;
  return ({
    context: "@odata.context" in obj ? obj["@odata.context"] : null,
    nextLink: "@odata.nextLink" in obj ? obj["@odata.nextLink"] : null,
    count: "@odata.count" in obj ? obj["@odata.count"] : null,
    value: "value" in obj ? obj.value : obj.value,
  }) as ODataResponse<T>;
}

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
    async downloadAsks(url:string) {
      const askFetch = url ? await useFetchX(url) : await useFetchX("api/v1/asks") ;
      const odataResponse = ToODataResponse<Ask>(askFetch as AsyncData<any, Error | null>);
      return odataResponse;
    }
  },
});