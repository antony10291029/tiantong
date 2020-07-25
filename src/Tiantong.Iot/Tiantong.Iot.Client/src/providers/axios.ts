import axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from 'axios'
import Vue from 'vue'

axios.defaults.baseURL = process.env.VUE_APP_API_URL

function beforeRequest (config: AxiosRequestConfig) {

  return config;
}

function beforeResponse (response : AxiosResponse) {
  var status = response.status

  if (status === 201) {
    Vue.prototype.$notify.success(response.data.message)
  }

  return response;
}

function beforeError (error : AxiosError) {
  let status = error.response?.status ?? 0

  if (status >= 400 && status < 500) {
    Vue.prototype.$notify.danger(error.response?.data.message)
  } else if (error.response?.status === 500) {
    Vue.prototype.$notify.danger('非常抱歉，出现未知错误')
  }

  throw error;
}

axios.interceptors.request.use(
  beforeRequest,
)

axios.interceptors.response.use(
  beforeResponse,
  beforeError,
)

export default axios
