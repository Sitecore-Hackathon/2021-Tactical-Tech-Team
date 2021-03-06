<template>
  <div>
    <nuxt-link to="/" class="button--green">
      Home page
    </nuxt-link>
    <nuxt-link to="/about" class="button--grey">
      About page
    </nuxt-link>
    <nuxt-link to="/nuxt-page" class="button--grey">
      Nuxt only page
    </nuxt-link>
  </div>
</template>
<script>
export default {
  name: 'Navigation'
}
</script>

<style scoped>

</style>
