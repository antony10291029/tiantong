import Axios from 'axios'
import Vue from 'vue'

export default {
  create (baseURL: string) {
    const axios = Axios.create()

    axios.defaults.baseURL = baseURL

    axios.interceptors.response.use(
      response => {
        const msg = response.data?.message
  
        if (msg) {
          Vue.prototype.$notify.success(msg)
        }

        return response
      },
      error => {
        const msg = error.response.data?.message
        if (msg) {
          Vue.prototype.$notify.danger(msg)
        }

        throw error
      }
    )

    axios.interceptors.request.use(
      request => {

        return request
      }
    )

    return axios
  }
}
