import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Flex, Image, Menu, Typography, Button, MenuProps } from "antd";
import { HomeOutlined } from "@ant-design/icons";
import React from "react";
import MenuItem from "antd/es/menu/MenuItem";

type MenuItem = Required<MenuProps>['items'][number];

const { Text } = Typography;
const { SubMenu, Item } = Menu;

type NavProps = {
  appLoginState: boolean;
}

export const AdminNavigationBar: React.FC<NavProps> = ({ appLoginState }) => {
  // const authState = useSelector((state: RootState) => state.auth);
  const navigate = useNavigate();

  const [loginState, setLoginState] = useState<boolean>(appLoginState);

  const handleClickNavItem = ({ key }) => {
    // return navigate(`/${key}`)
    switch (key) {
      case "home":
        return navigate("/admin");

      default:
        break;
    }
  }

  const navMenuItems: MenuItem[] = [
    {
      label: <Text style={{ fontSize: "20px" }}>Home</Text>,
      key: "home",
      icon: <HomeOutlined style={{ fontSize: "18px" }} />,
      onClick: handleClickNavItem,
    },
  ];

  const handleLogout = () => {
    localStorage.removeItem("access_token");
    localStorage.removeItem("refresh_token");
    return navigate("/login", { replace: true });
  }

  return (
    <>
      <Flex justify="center">
        <Image
          preview={false}
          // src={siteIcon}
          style={{
            height: "70px",
          }}
        />
        <Menu
          mode="horizontal"
          theme="light"
          style={{
            width: "fit-content",
            margin: "0px 15px 0px 35px",
            padding: "0px 15px",
            borderRadius: "30px",
          }}
          items={navMenuItems}
        >
        </Menu>
        <Flex>
          {loginState == true ?
            <Button style={{ margin: "auto" }} onClick={handleLogout}>Logout</Button> :
            <Button style={{ margin: "auto" }} onClick={() => navigate("/login")}>Login</Button>
          }
        </Flex>
      </Flex>
    </>
  );
}
