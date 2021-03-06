const parseServerData = (data, viewBag) => {
  const parsedData = data instanceof Object ? data : JSON.parse(data)
  const parsedViewBag =
    viewBag instanceof Object ? viewBag : JSON.parse(viewBag)

  return {
    viewBag: parsedViewBag,
    sitecore: parsedData && parsedData.sitecore
  }
}
const routeData = {
  placeholders: {
    'ttt-main': [
      {
        componentName: 'TitleMain',
        fields: {
          title: {
            value: 'Page Title HELLOOO'
          }
        }
      }
    ]
  }
}
const context = {
  pageEditing: 'normal'
}

export default function ({ store, route, req }) {
  return new Promise((resolve, reject) => {
    store.commit('jss/setLayoutResponse', { sitecore: { route: routeData, context } })

    // middleware called on GET and POST, sitecore use POST to render EE
    if (req.method === 'GET') { return resolve() }

    // READ the body from POST
    let body = ''
    req.on('data', (c) => {
      body += c
    })
    // end of reading, commit context to vuex store.
    req.on('end', () => {
      const resources = JSON.parse(body)
      const data = resources?.args[1]
      const viewBag = resources?.args[2]
      const sitecoreData = parseServerData(data, viewBag)
      store.commit('jss/setLayoutResponse', sitecoreData)
      resolve(body)
    })
  })
}
