export const useFetchX = (url, options = {}) => {
    const config = useRuntimeConfig();
    return $fetch(`${config.public.baseUrl}/${url}`, options)
}