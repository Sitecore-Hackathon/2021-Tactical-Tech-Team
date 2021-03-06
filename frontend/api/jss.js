import url from 'url'
import path from 'path'
const fs = require('fs')

export default (req, res) => {
  const { item = '/', sc_lang = 'en' } = url.parse(req.url, true).query || {}
  const dataPath = `${path.join(
    __dirname,
    'data',
    item,
    sc_lang
  )}.json`

  let pageData
  try {
    pageData = fs.readFileSync(dataPath, 'utf8')
  } catch (e) {
    console.error(
      `Can not read specified page data for item: ${item} lang: ${sc_lang}`
    )
    return res.end('Missing page data')
  }

  res.end(pageData)
}
