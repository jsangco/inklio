<template>
  <NuxtLayout>
    <InklioNavBar />
    <NuxtPage />
  </NuxtLayout>
</template>

<style>
.page-enter-active,
.page-leave-active {
  transition: all 0.0s;
}
.page-enter-from {
  opacity: 0;
  filter: blur(1rem);
}
.page-leave-to {
  opacity: 0;
  filter: blur(1rem);
}

</style>