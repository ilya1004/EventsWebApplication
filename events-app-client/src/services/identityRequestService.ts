import axios from "axios";
import { BASE_IDENTITY_URL } from "../store/constants.ts";
import { refreshAccessToken } from "./TokenService.ts";
import { redirect } from "react-router-dom";


export const identityClient = axios.create({
  baseURL: BASE_IDENTITY_URL,
});


identityClient.interceptors.request.use(
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


identityClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        await refreshAccessToken();
        return identityClient(originalRequest);
      } catch (refreshError) {
        console.error("Failed to refresh access token:", refreshError);
        return redirect("/login");
      }
    }

    return Promise.reject(error);
  }
);


export const getIdentityRequestData = async (url: string) => {
  try {
    const response = await identityClient.get(url);
    return response.data;
  } catch (error) {
    console.error("Error fetching data:", error);
    throw error;
  }
};