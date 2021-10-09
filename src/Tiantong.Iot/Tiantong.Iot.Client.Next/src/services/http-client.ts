import axios from "axios";
import { notify } from "../shared/Notify";

const baseUrl = import.meta.env.VITE_API_URL as string;

axios.defaults.baseURL = baseUrl;

axios.interceptors.response.use(
  response => {
    if (response.data.message) {
      notify.success({ content: response.data.message });
    }

    return response;
  },
  error => {
    let message = "";

    if (error.response === undefined) {
      message = "访问服务器失败，请检查网络连接";
    } else if (error.response?.data?.message !== undefined) {
      message = error.response.data.message;
    } else {
      message = "系统发生错误，请联系技术人员";
    }

    notify.danger({ content: message });

    throw error;
  }
);

export default axios;
