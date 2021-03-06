<template>
  <div class="container">
    <div>
      <ScLogo /> <span class="plus">+</span> <Logo />
      <ScPlaceholder v-if="route" name="ttt-main" :rendering="route" />
      <Navigation />
      <ScPlaceholder v-if="route" name="ttt-second" :rendering="route" />
    </div>
  </div>
</template>

<script>
import { Placeholder } from '@sitecore-jss/sitecore-jss-vue'
import { createNamespacedHelpers } from 'vuex'
import { JssPlaceholderPlugin } from '~/plugins/jss-placeholder'

const {
  mapGetters: mapJssGetters,
  mapActions: mapJssActions,
  mapState: mapJssState
} = createNamespacedHelpers('jss')

export default {
  components: {
    ScPlaceholder: Placeholder
  },

  async fetch ({ app, route, error, store, req, redirect }) {
    console.log('FETCH: isEeRender', store.state.jss.eeRender)

    if (!store.state.jss.eeRender) {
      await store.dispatch('jss/fetchRouteData', route)
    }

    const { statusCode } = store.state.jss
    if ([404, 403, 500].includes(statusCode)) {
      error({ statusCode })
    }
  },

  computed: {
    ...mapJssState(['context', 'route'])
  },

  created () {
    JssPlaceholderPlugin()
  },
  methods: {
    ...mapJssActions(['fetchRouteData'])
  }
}
</script>

<style scoped>
.plus {
  font-size: 48px;
  color: #526488;
}
</style>
