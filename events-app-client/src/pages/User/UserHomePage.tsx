import { Button, Flex } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { BASE_IDENTITY_URL, BASE_SERVER_API_URL } from "../../store/constants.ts";
import { refreshAccessToken } from "../../services/TokenService.ts"; // Импортируем наш сервис
import { redirect } from "react-router-dom";

export const UserHomePage: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);

  // Функция для получения данных с защищенного эндпоинта
  const getData = async () => {
    try {
      let accessToken = localStorage.getItem("access_token");

      if (!accessToken) {
        console.error("No access token found");
      }

      // Пробуем сделать запрос с текущим access токеном
      const response = await axios.get(`${BASE_SERVER_API_URL}/Events/user`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      console.log("Data from protected endpoint:", response.data);

    } catch (error) {
      if (error.response?.status === 401) {
        console.log("Access token expired, attempting to refresh...");
        try {
          await refreshAccessToken();

          let newAccessToken = localStorage.getItem("access_token");

          // Повторно отправляем запрос с новым access токеном
          const response = await axios.get(`${BASE_SERVER_API_URL}/Events/user`, {
            headers: {
              Authorization: `Bearer ${newAccessToken}`,
            },
          });

          console.log("Data from protected endpoint:", response.data);
        } catch (refreshError) {
          console.error("Error while refreshing token:", refreshError);
        }
      } else {
        console.error("Error fetching data:", error);
      }
    }
  };

  const getData1 = async () => {
    try {
      let accessToken = localStorage.getItem("access_token");

      if (!accessToken) {
        console.error("No access token found");
      }

      // Пробуем сделать запрос с текущим access токеном
      const response = await axios.get(`${BASE_IDENTITY_URL}/api/Users/c3415321-fc58-41d8-8ea8-220a45a17355`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      console.log(response.data);

    } catch (error) {
      if (error.response?.status === 401) {
        console.log("Access token expired, attempting to refresh...");
        try {
          await refreshAccessToken();

          let newAccessToken = localStorage.getItem("access_token");

          // Повторно отправляем запрос с новым access токеном
          const response = await axios.get(`${BASE_SERVER_API_URL}/Events/user`, {
            headers: {
              Authorization: `Bearer ${newAccessToken}`,
            },
          });

          console.log("Data from protected endpoint:", response.data);
        } catch (refreshError) {
          console.error("Error while refreshing token:", refreshError);
        }
      } else {
        console.error("Error fetching data:", error);
      }
    }
  };

  // Обработчик клика для получения данных
  const handleClick = () => {
    getData();
  };

  const handleClick1 = () => {
    getData1();
  };

  return (
    <>
      <Flex
        justify="center"
        align="flex-start"
        style={{
          margin: "10px 15px",
          minHeight: "80vh",
        }}
      >
        <Button onClick={handleClick}>
          Click to Get Data
        </Button>
        <Button onClick={handleClick1}>
          Click to Get User Info
        </Button>
      </Flex>
    </>
  );
};
