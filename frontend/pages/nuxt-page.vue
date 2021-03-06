<template>
  <div>
    <div class="container">
      <div>
        <div>
          <Logo />
        </div>
        <TitleMain :fields="titleField" />
        <Navigation />
        <NuxtOnly />
      </div>
    </div>
  </div>
</template>
<script>
import TitleMain from '~/components/sitecore/title-main'
export default {
  name: 'NuxtPage',
  components: {
    TitleMain
  },
  data () {
    return {
      titleField: { title: { value: 'Nuxt page' } }
    }
  }
}
</script>
<style></style>
