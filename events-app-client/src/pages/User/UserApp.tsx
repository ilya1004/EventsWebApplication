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
import { AppleFilled, GithubOutlined } from "@ant-design/icons";
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

export const userAppLoader = () => {
  if (localStorage.getItem("refresh_token")) {
    return true;
  }
  return false;
}

export const UserApp: React.FC = () => {
  
  const appLoginState = useLoaderData() as boolean;

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
          <NavigationBar appLoginState={appLoginState} />
        </Header>
        <Content
          style={{
            padding: "10px 0px 0px 0px",
            backgroundColor: backColor,
          }}
        >
          <Flex justify="center">
            <Outlet />
          </Flex>
        </Content>
        <Footer
          style={{
            backgroundColor: "#F5DEBE",
          }}
        >
          <Flex justify="center">
            <Flex align="center" vertical>
              <div>
                <Text>Events Web Application. 2024</Text>
              </div>
              <div>
                <Text>
                  <a href="https://github.com/ilya1004/EventsWebApplication" target="_blank">Link to project code on github</a>
                  <GithubOutlined style={{ margin: "0px 0px 0px 5px", fontSize: "16px" }} />
                </Text>
              </div>
            </Flex>
          </Flex>
        </Footer>
      </Layout>
    </>
  );
}

// export default UserApp;