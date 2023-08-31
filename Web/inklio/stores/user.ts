import { defineStore } from "pinia";
import { camelizeKeys } from "humps";

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

export const useUserStore = defineStore({
  id: 'userStore',
  state: () => emptyUserInfo,
  actions: {
    async login(username: string, password: string, isRememberMe: boolean) {
      if (process.server) {
        console.log("WARNING: login occured during SSR");
        return;
      }

      const doLogin = async () => {
        const account = await $fetch(`api/v1/accounts/login`, {
          method: "POST",
          body: { "username": username, "password": password, "is_remember_me": isRememberMe },
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

      var account = await doLogin() as any;
      if (account === null) {
        return false;
      }
      else {
        // Assign all account props to the state. This must be done for each key otherwise
        // pinia will not properly save the account info to the persisted store.
        for (const key in account) {
          if (this.$state.hasOwnProperty(key)) {
            (this.$state as any)[key] = account[key];
          }
        }

        this.isLoggedIn = true;
        return true;
      }
    },
    async logout() {
      await $fetch(`api/v1/accounts/logout`, {
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
    }
  },
  persist: true,
});