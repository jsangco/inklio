export const useFetchX = (url: any, options = {}) => {
  const config = useRuntimeConfig();
  return useFetch(`${config.public.baseUrl}/${url}`, options)
}