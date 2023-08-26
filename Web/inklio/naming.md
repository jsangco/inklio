# Nuxt Naming Conventions

## Components

- [Documentation](https://v3.nuxtjs.org/docs/directory-structure/components)
- use PascalCase
  - ie: `BasePage.vue`
- You can use the folders as part of the naming convention
  - ie:
  ```
  /components
     base/
      Button.vue
  ```
  - can be called as
  ```
  <BaseButton />
  ```

## Pages

- [Documentation](https://v3.nuxtjs.org/docs/directory-structure/pages/)
- all of your web pages go into a `pages/` folder at the root directory

- use spinal-case naming
  - ie: `my-page.vue`
- dynamic routes use `[id].vue` syntax
- Add page level metadata or define the page layout with `definePageMeta({})` syntax
- [Meta Data Documentation](https://v3.nuxtjs.org/docs/usage/meta-tags)

### App.vue

- When first set up, there is an `app.vue` file
- This acts as the entry point to the site, and as a default layout
- **once you create a `pages/` folder, you need to make an `index.vue` file inside it**
  - this becomes the new home page
  - inject it into `app.vue` by adding `<NuxtPage />` to the `app.vue` template
  - you can also delete the `app.vue` file to set your site to use `layouts/default.vue` layout and render `index.vue` content without adding `<NuxtPage />` to it

## Layouts

- [Documentation](https://v3.nuxtjs.org/docs/directory-structure/layouts)
- use lowercase
  - ie: `default.vue` or `contactpage.vue`
- then add to a page either in the script tag or around your content
- You can cancel the default layout from being applied to a page by adding `layout: false` to the `definePageMeta({})`

### Add via script tag

```
<script setup>
definePageMeta({
  layout: "gallery"
})
</script>
```

### Add in template

```
<template>
  <NuxtLayout name="gallery">
  <!-- add your content in here -->
  </NuxtLayout>
</template>
```