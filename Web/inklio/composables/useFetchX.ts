export const useFetchX = (url, options = {}) => {
    console.log("useFetchX");
    const config = useRuntimeConfig();
    return useFetch(`${config.public.baseUrl}/${url}`, options)
}