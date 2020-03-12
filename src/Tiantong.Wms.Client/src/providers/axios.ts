import axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from 'axios';
// import store from './store'
import Token from './token'
import Router from './router'

axios.defaults.baseURL = process.env.VUE_APP_API_URL;

function beforeRequest (config: AxiosRequestConfig) : object {
  const token = Token.state.token;

  if (token) {
    config.headers.Authorization = token;
  }

  return config;
}

function beforeResponse (response : AxiosResponse) : AxiosResponse {

  return response;
}

function beforeError (error : AxiosError) : void {
  if (error.response?.status == 401) {
    // Token.clear()
    Router.push('/unauthorization')
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
