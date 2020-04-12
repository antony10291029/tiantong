import axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from 'axios';
import Token from './token'
import Router from './router'
import { notify } from './notify'

axios.defaults.baseURL = process.env.VUE_APP_API_URL;

function beforeRequest (config: AxiosRequestConfig) : object {
  const token = Token.state.token;

  if (token) {
    config.headers.Authorization = token;
  }

  return config;
}

function beforeResponse (response : AxiosResponse) : AxiosResponse {
  if (response.status === 201) {
    notify.success(response.data.message)
  }

  return response;
}

function beforeError (error : AxiosError) : void {
  let status = error.response?.status
  
  if (status === 401) {
    // Token.clear()
    Router.push('/unauthorization')
  } else if (status === 400 || status === 422) {
    notify.danger(error.response?.data.message)
  } else if (error.response?.status === 500) {
    notify.danger('非常抱歉，出现未知错误')
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

export default axios;
