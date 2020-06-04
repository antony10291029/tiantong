import axios from 'axios'
import localStorage from '../local-storage'

var proxy = axios.create({
  withCredentials: true
})

proxy.defaults.baseURL = process.env.VUE_APP_REVERSE_PROXY_URL

proxy.interceptors.request.use(
  config => {
    const token = localStorage.chanjet.token.get()

    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  }
)

async function getCallbackData (context: string) {
  context.split('(')[1].split(')')[0]
}

export default proxy

export async function getAssistants () {
  const params = {
    page: 1,
    size: 200,
    bookId: 621145323732998,
    ACCOUNTBOOK: 621145323732998,
    _dc: 1591183716779
  }
  const response = await proxy.get('accounting/uzzcbkthbw2o/c0x0l6ci87/custom/assistant/list', {
    params
  })

  return response.data
}
