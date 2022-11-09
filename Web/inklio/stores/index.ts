export interface Todo {
  id: string;
  title: string;
  done: boolean;
  createdAt: Date;
  updatedAt: Date;
};

export interface TodoState {
  count: number,
  test: string,
  auth: string,
  items: Todo[] | undefined[];
};

const state = () => ({
  count: 1234,
  test: process.env.baseUrl,
  auth: "--",
  items: [],
});

const getters = {

  login: () => async () => {
    console.log("store login");
    const config = useRuntimeConfig();
    return await $fetch(
      `${config.public.baseUrl}/auth/accounts/login`, {
      method: 'POST',
      body: {
        username: 'jace',
        password: 'Aoeuaoeu1'
        },
        headers: {
          'Content-Type': 'application/json'
        }
    });
  },
  claims: () => async () => {
    console.log("store claims");
    const config = useRuntimeConfig();
    return await $fetch(`${config.public.baseUrl}/auth/claims`);
  },
  getAsks: () => async () => {
    console.log('fetching asks1');
    return await fetch('https://inklio.azurewebsites.net/api/v1/asks/').then(response => response.json());
  },
  getExt: () => async () => {
    console.log('fetching testData');
    return await $fetch('https://jsonplaceholder.typicode.com/todos/1', {
      parseResponse: JSON.parse,
    });
  },
  getAuth: () => async () => {
    console.log('fetching auth');
    return await useFetch('/auth/', {
      baseURL: 'https://inklio.azurewebsites.net/',
    });
  },
  getAuthBasic: () => async () => {
    console.log('fetching basic auth');
    return await fetch('https://inklio.azurewebsites.net/auth/').then(response => response.json());
  },
  getCount: (state: TodoState) => () => { return state.count; },
};

const actions = {
  async setAuth() {
    console.log("setAuth");
    const config = useRuntimeConfig();
    this.auth = await $fetch(`${config.public.baseUrl}/auth/`);
  }
};

export const useTodoStore = defineStore('todoStore', {
  state,
  getters,
  actions
});