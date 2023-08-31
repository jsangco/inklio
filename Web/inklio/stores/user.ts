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

const emptyUserInfo: User = {
  id: "",
  username: "",
  roles: [],
  isEmailVerified: false,
  isLoggedIn: false,
}

const loginOrRegister = async (url: string, body: any): Promise<any> => {
  const account = await $fetch(url, {
    method: "POST",
    body: decamelizeKeys(body),
    async onResponse({ response }) {
      if (response && response._data) {
        response._data = camelizeKeys(response._data);
      }
    }
  }).catch(a => {
    return null;
  });
  return account;
};

const setLoggedInState = (state: any, account: any): boolean => {
  if (account === null) {
    return false;
  }
  else {
    // Assign all account props to the state. This must be done for each key otherwise
    // pinia will not properly save the account info to the persisted store.
    for (const key in account) {
      if (state.$state.hasOwnProperty(key)) {
        (state as any)[key] = account[key];
      }
    }

    state.isLoggedIn = true;
    return true;
  }
}

export const useUserStore = defineStore({
  id: 'userStore',
  state: () => emptyUserInfo,
  actions: {
    async login(username: string, password: string, isRememberMe: boolean) {
      var account = await loginOrRegister(
        "api/v1/accounts/login",
        { "username": username, "password": password, "is_remember_me": isRememberMe });
      return setLoggedInState(this, account);
    },
    async logout() {
      await $fetch("api/v1/accounts/logout", {
        method: "POST",
      }).catch(a => {
        console.log(`Failed to logout ${a}`);
        return false;
      });

        for (const key in emptyUserInfo) {
          if (this.$state.hasOwnProperty(key)) {
            (this.$state as any)[key] = (emptyUserInfo as any)[key];
          }
        }
      return true;
    },
    async register(username: string, email: string, password: string, confirmPassword: string) {
      var account = await loginOrRegister(
        "api/v1/accounts/register",
        { "username": username, "password": password, "confirmPassword": confirmPassword, "email": email });
      return setLoggedInState(this, account);
    }
  },
  persist: true,
});