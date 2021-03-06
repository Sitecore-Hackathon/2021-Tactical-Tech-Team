<template>
  <div class="container">
    <div>
      <ScPlaceholder v-if="route" name="ttt-main" :rendering="route" />

      <nuxt-link to="/" class="button--green">
        Home page
      </nuxt-link>
      <nuxt-link to="/about" class="button--grey">
        About page
      </nuxt-link>
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
    if (!store.state.jss.isExperienceEditor) {
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

<style>
.container {
  margin: 0 auto;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
}
</style>
