import { defineStore } from "pinia";
import { camelizeKeys, decamelizeKeys } from "humps";

export type Role = "ADMINISTRATOR" | "USER" | "MODERATOR";
export type User = {
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

const emptyUserInfo: User = {
  id: "",
  username: "",
  roles: [],
  isEmailVerified: false,
  isLoggedIn: false,
}

const loginOrRegister = async (state: any, url: string, body: any): Promise<JsonResponse> => {
  const errorResult = ref()
  const doRequest = async () => {
    const account = await $fetch(url, {
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
    if (account) {
      return <JsonResponse>{
        isSuccess: true,
        data: account,
        error: null,
      }
    }
    return errorResult.value;
  }

  const loginResult = await doRequest();
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

export const useUserStore = defineStore({
  id: 'userStore',
  state: () => emptyUserInfo,
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
        for (const key in emptyUserInfo) {
          if (this.$state.hasOwnProperty(key)) {
            (this.$state as any)[key] = (emptyUserInfo as any)[key];
          }
        }
      }
      return logoutResult;
    },
    async register(username: string, email: string, password: string, confirmPassword: string): Promise<JsonResponse> {
      return await loginOrRegister(
        this,
        "api/v1/accounts/register",
        { "username": username, "password": password, "confirmPassword": confirmPassword, "email": email });
    }
  },
  persist: true,
});