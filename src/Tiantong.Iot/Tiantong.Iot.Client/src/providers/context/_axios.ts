import Axios from 'axios'
import { notify } from '../notify/index'

export default {
  create (baseURL: string, token: () => string) {
    const axios = Axios.create()

    axios.defaults.baseURL = baseURL

    axios.interceptors.response.use(
      response => {
        const msg = response.data?.message
  
        if (msg) {
          notify.success(msg)
        }

        return response
      },
      error => {
        const msg = error.response.data?.message

        if (error.response?.status === 500) {
          notify.danger('非常抱歉，出现未知错误')
        } else if (msg) {
          notify.danger(msg)
        }

        throw error
      }
    )

    axios.interceptors.request.use(
      request => {
        const auth = token()
        
        if (auth) {
          request.headers['Authorization'] = auth
        }

        return request
      }
    )

    return axios
  }
}
