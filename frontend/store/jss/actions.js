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
    console.log('FETCH', route.params.pathMatch)
    const item = prependSlash(route.params.pathMatch)
    try {
      const host = ENV.isDisconnected ? ENV.disconnectedHost : ENV.sitecoreCDHost
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
              withCredentials: true
            })
        }
      )
      console.log(data)
      // commit('setLayoutResponse', data)
    } catch (error) {
      // commit('setLayoutError', 404)
    }
  }
}

export default actions
