import { Outlet, redirect } from "react-router-dom";
import { Layout, Flex, Typography } from "antd";
import { useLoaderData } from "react-router-dom";
import React from "react";
import NavigationBar from "./NavigationBar.tsx";
import { GithubOutlined } from "@ant-design/icons";
import { getUserRole } from "../../services/TokenService.ts";

const { Header, Footer, Content } = Layout;
const { Text } = Typography;

const backColor = "#F1F0EA";

export const userAppLoader = () => {
  if (localStorage.getItem("refresh_token")) {

    let role = getUserRole();

    console.log(role);

    if (!role || role !== "User") {
      // window.location.href = "/login";
      // return false;
      return redirect("/login");
    }

    return true;
  }
  return redirect("/login");
  // return false;
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