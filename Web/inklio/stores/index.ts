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
  items: Todo[] | undefined[];
};

const state = () => ({
  count: 1234,
  test: process.env.test,
  items: [],
});

const getters = {

  login: () => async () => {
    console.log('fetching login');
    return await $fetch(
      'https://inklio.azurewebsites.net/auth/accounts/login', {
      method: 'POST',
      mode: 'no-cors',
      body: {
        username: 'jace',
        password: 'Aoeuaoeu1'
        },
    });
  },
  getAsks: () => async () => {
    console.log('fetching asks1');
    return await $fetch('https://inklio.azurewebsites.net/api/v1/asks/', {
      mode: 'no-cors',
    });
  },
  getExt: () => async () => {
    console.log('fetching testData');
    return await $fetch('http://jsonplaceholder.typicode.com/todos/1', {
      mode: 'no-cors',
    });
  },
  getAuth: () => async () => {
    console.log('fetching auth');
    return await useFetch('/auth/', {
      mode: 'no-cors',
      baseURL: 'https://inklio.azurewebsites.net/',
    });
  },
  getAuthBasic: () => async () => {
    console.log('fetching basic auth');
    return await fetch('https://inklio.azurewebsites.net/auth/').then(response => response.json());
  },
  getCount: (state: TodoState) => () => { return state.count; },
};

const actions = { };

export const useTodoStore = defineStore('todoStore', {
  state,
  getters,
  actions
});