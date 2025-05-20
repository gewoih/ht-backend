import fs from 'fs'
import path from 'path'
import { fileURLToPath } from 'url'
import { SitemapStream, streamToPromise } from 'sitemap'
import { createGzip } from 'zlib'

const BASE_URL = process.env.SITE_URL || 'https://track-me.ru'

const routes = [
  '/',
  '/login',
  '/register',
  '/journal',
  '/analytics/insights',
  '/leaderboard',
  '/profile',
  '/analytics/chart',
  '/faq',
  '/about',
  '/contact',
]

const lastmod = new Date().toISOString().split('T')[0]

const __filename = fileURLToPath(import.meta.url)
const __dirname = path.dirname(__filename)
const outputDir = path.resolve(__dirname, '../public')
const xmlPath = path.join(outputDir, 'sitemap.xml')
const gzipPath = path.join(outputDir, 'sitemap.xml.gz')

async function generateSitemap() {
  const xmlStream = new SitemapStream({ hostname: BASE_URL })
  routes.forEach((route) => {
    xmlStream.write({
      url: route,
      lastmod,
      changefreq: 'weekly',
      priority: 0.7,
    })
  })
  xmlStream.end()

  const xmlData = (await streamToPromise(xmlStream)).toString()

  fs.mkdirSync(outputDir, { recursive: true })
  fs.writeFileSync(xmlPath, xmlData)

  const gzipStream = createGzip()
  const writeStream = fs.createWriteStream(gzipPath)
  gzipStream.pipe(writeStream)
  gzipStream.end(xmlData)

  console.log('✔ Sitemap сгенерирован в', outputDir)
}

generateSitemap().catch((err) => {
  console.error('Ошибка при генерации sitemap:', err)
  process.exit(1)
})
