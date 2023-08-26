<template>
  <div>
    <tr>
      <td>
        Configuration
      </td>
      <td>
        {{ useRuntimeConfig() }}
      </td>
    </tr>
    <tr>
      <td>
       API Status
      </td>
      <td>
        <p v-if="apiPending">Fetching...</p>
        <pre v-else-if="apiError">{{ apiError }}</pre>
        <pre v-else>{{ apiResult ? apiResult.status : "unreachable" }}</pre>
      </td>
    </tr>
    <tr>
      <td>
       Auth Status
      </td>
      <td>
        <p v-if="authPending">Fetching...</p>
        <pre v-else-if="authError">{{ authError }}</pre>
        <pre v-else>{{ authResult ? authResult.status : "unreachable" }}</pre>
      </td>
    </tr>
  </div>
</template>

<script setup>
// const { data: apiResult, apiPending, apiError } = await useFetchX('/api/health')
// const { data: authResult, authPending, authError } = await useFetchX(`/auth/health`)
const config = useRuntimeConfig();
const apiResult = await $fetch(`${config.public.baseUrl}/api/health`)
const authResult = await $fetch(`${config.public.baseUrl}/auth/health`)
</script>

<style>
div {
  margin: 10px;
}
td {
  font-size: large;
}
td {
  border: 1px solid;
}
td {
  padding-left: 10px;
  padding-right: 10px;
}
</style>