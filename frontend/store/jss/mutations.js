import { LayoutServiceData } from '@sitecore-jss/sitecore-jss'

const mutations = {
  /**
   *
   * @param state
   * @param {LayoutServiceData} layout
   */
  setLayoutResponse (state, layout) {
    if (!layout) { return }
    console.log('CONTEXT: ')
    const {
      sitecore: { route, context = {} },
      eeRender
    } = layout

    // Do not replace the original state object - the store and any components that use the store
    // need to share a reference to the same object in order for mutations to be observed.
    state.statusCode = '200'
    state.route = route
    state.context = context
    state.eeRender = eeRender
  },

  setLayoutError (state, statusCode) {
    state.statusCode = statusCode
    state.route = null
    state.context = null
  }
}

export default mutations
