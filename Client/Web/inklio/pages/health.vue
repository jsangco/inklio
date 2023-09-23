<template>
  <div>
    <h1>Service Health</h1>
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
  </div>
</template>

<script setup>
const config = useRuntimeConfig();
const apiResult = await $fetch(`${config.public.apiUrl}/health`)
</script>

<style>
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