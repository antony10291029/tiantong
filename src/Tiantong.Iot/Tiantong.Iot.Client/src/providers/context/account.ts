import axios from './_axios'
import { account } from '../local-storage'
import router from '../vue-router'

const context = axios.create(
  'https://iaccount.als-yuchuan.com',
  () => account.token.get()
)

export function login (uri: string = '') {
  const path = btoa(uri || router.currentRoute.fullPath)
  const redirect = btoa(`${window.location.origin}/#/login/${path}`)

  window.location.assign(`https://account.als-yuchuan.com/#/login?redirect=${redirect}`)
}

export default context
