import axios from 'axios'
import { notify } from '../notify'

const context = axios.create()

context.defaults.baseURL = process.env.VUE_APP_API_URL

context.interceptors.request.use(
  config => {

    return config;
  }
)

context.interceptors.response.use(
  response => {
    var status = response.status
  
    if (status === 201) {
      notify.success(response.data.message)
    }
  
    return response;
  },
  error => {
    let status = error.response?.status

    if (status === 422) {
      notify.danger(error.response?.data.message)
    } else if (error.response?.status === 500) {
      notify.danger('非常抱歉，出现未知错误')
    }

    throw error
  }
)

export default context
