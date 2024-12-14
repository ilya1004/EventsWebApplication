import axios from "axios";
import { BASE_IDENTITY_URL } from "../store/constants.ts";
import { NavigateFunction, redirect } from "react-router-dom";


export const refreshAccessToken = async () => {
  const refreshToken = localStorage.getItem("refresh_token");

  if (!refreshToken) {
    console.log("No refresh token found");
    window.location.href = "/login";
    return;
  }

  try {
    const response = await axios.post(`${BASE_IDENTITY_URL}/connect/token`, new URLSearchParams({
      grant_type: "refresh_token",
      client_id: "react_client",
      client_secret: "react_secret",
      refresh_token: refreshToken,
      scope: "openid profile offline_access",
    }), {
      headers: {
        "Content-Type": "application/x-www-form-urlencoded"
      }
    });

    if (response.status === 400) {
      window.location.href = "/login";
      return;
    }

    const { access_token } = response.data;
    localStorage.setItem("access_token", access_token);
    return null;
  } catch (error) {
    console.error("Failed to refresh token:", error);
    window.location.href = "/login";
    // return redirect("/login");
  }
};
