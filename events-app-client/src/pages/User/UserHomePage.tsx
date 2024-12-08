import { Button, Flex } from "antd";
import axios from "axios";
import React from "react";
import { BASE_SERVER_API_URL } from "../../store/constants.ts";


export const UserHomePage: React.FC = () => {

  const getData = async () => {
    try {
      const accessToken = localStorage.getItem("access_token");

      if (!accessToken) {
        console.error("No access token found");
        return;
      }

      
      const response = await axios.get(`${BASE_SERVER_API_URL}/Events/user`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      // Обрабатываем полученные данные
      console.log("Data from protected endpoint:", response.data);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  const handleClick = async () => {
    await getData();
  } 

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
          Click
        </Button>
      </Flex>
    </>
  );
}