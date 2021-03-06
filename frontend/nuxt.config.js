import { writeComponentFactory } from './sitecore/scripts/generate-component-factory'

export default {
  // Global page headers: https://go.nuxtjs.dev/config-head
  head: {
    title: 'ttt-frontend',
    htmlAttrs: {
      lang: 'en'
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
    ]
  },

  // Global CSS: https://go.nuxtjs.dev/config-css
  css: [
  ],

  // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
  plugins: [
  ],
  serverMiddleware: [
    { path: '/api/sitecore/api/layout/render/jss', handler: '~/api/jss.js' }
  ],

  router: {
    middleware: ['ee-renderer']
  },

  // Auto import components: https://go.nuxtjs.dev/config-components
  components: true,

  // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
  buildModules: [
    // https://go.nuxtjs.dev/eslint
    '@nuxtjs/eslint-module'
  ],

  // Modules: https://go.nuxtjs.dev/config-modules
  modules: [
    // https://go.nuxtjs.dev/axios
    '@nuxtjs/axios'
  ],

  // Axios module configuration: https://go.nuxtjs.dev/config-axios
  axios: {},

  // Content module configuration: https://go.nuxtjs.dev/config-content
  content: {},

  // Build Configuration: https://go.nuxtjs.dev/config-build
  build: {
  },

  hooks: {
    build: {
      before () {
        // fs.writeFileSync('sitecore/component-factory.js', 'as', { encoding: 'utf8' })
        writeComponentFactory()
      }
    },
    ready() {
      process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
    }
  }
}
