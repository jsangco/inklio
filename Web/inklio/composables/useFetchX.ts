export const useFetchX = (url, options = {}) => {
    console.log("useFetchX");
    const config = useRuntimeConfig();
    return $fetch(`${config.public.baseUrl}/${url}`, options)
}