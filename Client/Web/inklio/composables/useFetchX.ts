export const useFetchX = (url: any, options: any = {}) => {

  // Add cookie to headers if it does not exist
  const headers = useRequestHeaders(['cookie']);
  var myHeaders = { headers: headers.cookie };
  if (!options.headers) {
    options.headers = headers;
  }
  else if (!options.headers['cookie']) {
    options.headers['cookie'] = headers.cookie;
  }

  const config = useRuntimeConfig();
  return useFetch(`${config.public.apiUrl}${url}`, options);
}