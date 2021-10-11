import Axios, { AxiosResponse } from "axios";
import { notify } from "../shared/components/Notify";

const axios = Axios.create({
  method: "post",
  baseURL: import.meta.env.VITE_API_URL as string,
});

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

const httpClient = {
  post<TData, TParams = any>(url: string, params?: TParams): Promise<AxiosResponse<TData>> {
    return axios.post(url, params);
  }
};

export {
  axios,
  httpClient
};
