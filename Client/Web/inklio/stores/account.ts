import { defineStore } from "pinia";
import { camelizeKeys, decamelizeKeys } from "humps";

export type Role = "ADMINISTRATOR" | "USER" | "MODERATOR";
export type Account = {
  id: string;
  username: string;
  roles: Role[];
  isEmailVerified: boolean;
  isLoggedIn: boolean;
};

export type JsonResponse = {
  isSuccess: boolean;
  data?: any;
  error?: any;
};

const emptyAccountState: Account = {
  id: "",
  username: "",
  roles: [],
  isEmailVerified: false,
  isLoggedIn: false,
}

const accountsRequest = async (url: string, body: any): Promise<JsonResponse> => {
  const errorResult = ref();
  const response = await $fetch(url, {
    method: "POST",
    body: decamelizeKeys(body),
    async onResponse({ response }) {
      if (response && response._data) {
        response._data = camelizeKeys(response._data);
      }
    }
  }).catch(error => {
    errorResult.value = <JsonResponse>{
      isSuccess: false,
      data: null,
      error: error.data
    };
  });
  if (response != undefined) {
    return <JsonResponse>{
      isSuccess: true,
      data: response,
      error: null,
    }
  }
  return errorResult.value;
}

const loginOrRegister = async (state: any, url: string, body: any): Promise<JsonResponse> => {
  const loginResult = await accountsRequest(url, body);
  if (loginResult.isSuccess) {
    // Assign all account props to the state. This must be done for each key otherwise
    // pinia will not properly save the account info to the persisted store.
    for (const key in loginResult.data) {
      if (state.$state.hasOwnProperty(key)) {
        state[key] = loginResult.data[key];
      }
    }

    state.isLoggedIn = true;
    return loginResult;
  }
  else {
    return loginResult;
  }
};

export const useAccountStore = defineStore({
  id: 'accountStore',
  state: () => emptyAccountState,
  actions: {
    async login(username: string, password: string, isRememberMe: boolean): Promise<JsonResponse> {
      return await loginOrRegister(
        this,
        "api/v1/accounts/login",
        { "username": username, "password": password, "is_remember_me": isRememberMe });
    },
    async logout() {
      const doLogout = async (): Promise<JsonResponse> => {
        const errorResult = ref()
        const response = await $fetch.raw("api/v1/accounts/logout", {
          method: "POST",
        }).catch(error => {
          errorResult.value = <JsonResponse>{
            isSuccess: false,
            error: error.data
          };
        });
        if (response!.ok) {
          return <JsonResponse>{
            isSuccess: true,
          };
        }
        return errorResult.value;
      };

      const logoutResult = await doLogout();
      if (logoutResult.isSuccess) {
        for (const key in emptyAccountState) {
          if (this.$state.hasOwnProperty(key)) {
            (this.$state as any)[key] = (emptyAccountState as any)[key];
          }
        }
      }
      return logoutResult;
    },
    async register(username: string, email: string, password: string, confirmPassword: string): Promise<JsonResponse> {
      return await loginOrRegister(
        this,
        "api/v1/accounts/register",
        { "username": username, "password": password, "confirm_password": confirmPassword, "email": email });
    },
    async passwordForget(email: string): Promise<JsonResponse> {
      return await accountsRequest(
        "api/v1/accounts/forget",
        { "email": email });
    },
    async passwordReset(email: string, password: string, confirmPassword: string, code: string): Promise<JsonResponse> {
      return await accountsRequest(
        "api/v1/accounts/reset",
        { "email": email, "password": password, "confirm_password": confirmPassword, "code": code });
    }
  },
  persist: true,
});