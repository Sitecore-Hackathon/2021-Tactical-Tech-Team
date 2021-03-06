import Vue from 'vue'
import { SitecoreJssPlaceholderPlugin } from '@sitecore-jss/sitecore-jss-vue'

export function JssPlaceholderPlugin () {
  // DO NOT MOVE - This is here so we only load the components when visiting a `sitecore` page
  let componentFactory
  try {
    componentFactory = require('../sitecore/component-factory').default
  } catch (err) {
    console.warn('Failed to import component factory, continuing without.', err)
    componentFactory = () => null
  }

  Vue.use(SitecoreJssPlaceholderPlugin, { componentFactory })
}
