import axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from 'axios'

axios.defaults.baseURL = process.env.VUE_APP_API_URL

function beforeRequest (config: AxiosRequestConfig) {

  return config;
}

function beforeResponse (response : AxiosResponse) {

  return response;
}

function beforeError (error : AxiosError) {

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
