import { dataApi } from '@sitecore-jss/sitecore-jss-vue'
import { ENV } from '../../env'

function prependSlash (url) {
  if (typeof url === 'string') {
    return url.startsWith('/') ? url : `/${url}`
  }

  return '/'
}

const actions = {
  async fetchRouteData ({ commit }, route) {
    const item = prependSlash(route.params.pathMatch)
    console.log('FETCH: route :', item)
    try {
      let host
      if (ENV.isDisconnected) {
        host = ENV.disconnectedHost
      } else if (process.server) {
        host = ENV.SSRSitecoreCDHost
      } else {
        host = ENV.sitecoreCDHost
      }

      console.log('FETCHING HOST: ', host)
      const lang = 'en' // this.i18n.locale

      // todo: use layoutService.fetchLayoutData
      const data = await dataApi.fetchRouteData(
        item,
        {
          layoutServiceConfig: { host },
          querystringParams: { sc_lang: lang, sc_apikey: ENV.jssKey },
          fetcher: (url, data) =>
            this.$axios({
              // baseURL: '',
              url,
              method: data ? 'POST' : 'GET',
              data,
              headers: {
                'Access-Control-Allow-Origin': '*',
                'Content-Type': 'application/json'
              },
              withCredentials: false
            })
        }
      )
      commit('setLayoutResponse', data)
    } catch (error) {
      console.error(error)
      // commit('setLayoutError', 404)
    }
  }
}

export default actions
