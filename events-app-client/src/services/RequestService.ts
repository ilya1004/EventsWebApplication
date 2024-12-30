import axios from "axios";
import { refreshAccessToken } from "./TokenService.ts";
import { BASE_SERVER_API_URL } from "../store/constants.ts";
import { redirect } from "react-router-dom";


export const apiClient = axios.create({
  baseURL: BASE_SERVER_API_URL,
});


apiClient.interceptors.request.use(
  (config) => {
    const accessToken = localStorage.getItem("access_token");
    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);


apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        await refreshAccessToken();
        return apiClient(originalRequest);
      } catch (refreshError) {
        console.error("Failed to refresh access token:", refreshError);
        return redirect("/login");
      }
    }

    return Promise.reject(error);
  }
);

export const getRequestData = async (url: string) => {
  try {
    const response = await apiClient.get(url);
    return response.data;
  } catch (error) {
    console.error("Error fetching data:", error);
    throw error;
  }
};


export const deleteRequestData = async (url: string) => {
  try {
    const response = await apiClient.delete(url);
    return response.data;
  } catch (error) {
    console.error(error);
    return;
  }
};

export const postRequestData = async (url: string, data: any) => {
  try {
    const response = await apiClient.post(url, data)
    return response.data;
  } catch (error) {
    console.error(error);
    return error;
  }
}