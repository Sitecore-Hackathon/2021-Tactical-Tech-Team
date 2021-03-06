import fs from 'fs'
import path from 'path'

const componentRootPath = 'components/sitecore'
const componentFactoryPath = 'sitecore/component-factory.js'

export function writeComponentFactory () {
  const componentFactory = generate(componentRootPath)
  fs.writeFileSync(componentFactoryPath, componentFactory, { encoding: 'utf8' })
  console.info(`Written component factory to "${componentFactoryPath}"`)
}

function generate (componentRootPath) {
  const imports = []
  const components = []
  const componentFiles = extractVueFiles(componentRootPath)
  componentFiles.forEach((componentFile) => {
    if (!fs.existsSync(componentFile)) {
      return
    }
    console.debug(`Registering JSS component ${componentFile}`)
    let componentName = path.basename(componentFile, '.vue')
    let importComponentFile = componentFile

    if (componentName === 'index') {
      const _componentPath = componentFile.split(path.sep)
      componentName = toPascalCase(_componentPath[_componentPath.length - 2])
      importComponentFile = path.dirname(componentFile)
    }

    const componentNameSanitized = importComponentFile
      .replace(path.join(componentRootPath, '/'), '')
      .replace(/\\/g, '/')
      .replace('.vue', '')
    const componentRootSanitized = '~/' + componentRootPath

    const importVarName = componentName.replace(/[^\w]+/g, '')
    const importVarPath = `${componentRootSanitized}/${componentNameSanitized}`

    if (importVarName.startsWith('Lazy')) {
      components.push(
        `${importVarName}: () => import('${importVarPath}' /* webpackChunkName: "${importVarPath.replace(
          '~/',
          ''
        )}" */).then(c => c.default || c)`
      )
    } else {
      imports.push(`import ${importVarName} from '${importVarPath}'`)
      components.push(importVarName)
    }
  })

  return `// Do not edit this file, it is auto-generated at build time!
// See scripts/bootstrap.js to modify the generation of this file.
${imports.join(';')}
const c = {${components.join(',')}}
export default function componentFactory (n) {return c[n]}
`
}

function toPascalCase (string) {
  return string
    .match(/[a-z]+/gi)
    .map(word => word.charAt(0).toUpperCase() + word.substr(1).toLowerCase())
    .join('')
}

/**
 * Recursively iterates the given folderPath, creating a flat array of found .vue file paths.
 * For example, given the following folder structure and using `/components` as the root folderPath:
 * /components/component0.vue
 * /components/subfolder/component1.vue
 *
 * The output would be:
 * ['component0.vue', 'subfolder/component1.vue']
 */
export function extractVueFiles (folderPath) {
  let results = []
  fs.readdirSync(folderPath).forEach((pathName) => {
    const computedPath = path.join(folderPath, pathName)
    const stat = fs.statSync(computedPath)
    if (stat && stat.isDirectory()) {
      results = results.concat(extractVueFiles(computedPath))
    } else if (path.extname(computedPath).toLowerCase() === '.vue') {
      results.push(computedPath)
    }
  })
  return results
}
