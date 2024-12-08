import { Outlet } from "react-router-dom";
import { Layout, Flex, Typography } from "antd";
// import NavigationBar from './User/NavigationBar';
// import { BASE_URL } from "../store/constants";
// import { AuthState } from "../store/auth/authSlice";
import axios from "axios";
import { LoaderFunction, useLoaderData } from "react-router-dom";
import { showMessageStc } from "../../services/ResponseErrorHandler";
import { useEffect } from "react";
import React from "react";
import NavigationBar from "./NavigationBar.tsx";
// import { useDispatch } from "react-redux";
// import { setAuthState } from "../store/auth/authSlice";

const { Header, Footer, Content } = Layout;
const { Text } = Typography;

const backColor = "#F1F0EA";

// export const appRoleCheckLoader: LoaderFunction = async (): Promise<AuthState> => {
//   const axiosInstance = axios.create({
//     baseURL: BASE_URL,
//     withCredentials: true,
//   });

//   try {
//     const res = await axiosInstance.get(`Users/check`);
//     const data = res.data;

//     return {
//       isAuthenticated: data.isAuthenticated,
//       userId: data.id,
//       role: data.role,
//     };

//   } catch (err: any) {
//     console.error("Ошибка при загрузке состояния аутентификации: ", err);
//     showMessageStc("Ошибка при загрузке состояния аутентификации: " + err, "error");

//     return { isAuthenticated: false, userId: 0, role: 0 };
//   }
// };

export const UserApp: React.FC = () => {
  // const dispatch = useDispatch();
  // const loaderData = useLoaderData() as AuthState; // Данные, возвращённые лоадером.

  // useEffect(() => {
  //   dispatch(setAuthState(loaderData));
  // }, [loaderData, dispatch]);

  return (
    <>
      <Layout>
        <Header
          style={{
            padding: "10px 0px 0px 0px",
            height: "fit-content",
            backgroundColor: backColor,
          }}
        >
          <NavigationBar />
        </Header>
        <Content
          style={{
            padding: "10px 0px 0px 0px",
            backgroundColor: backColor,
          }}
        >
          <Outlet />
        </Content>
        <Footer
          style={{
            backgroundColor: "#F5DEBE",
          }}
        >
          <Flex justify="center">
            <Text>Music player. 2024</Text>
          </Flex>
        </Footer>
      </Layout>
    </>
  );
}

// export default UserApp;