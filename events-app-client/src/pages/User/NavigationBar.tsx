import { useEffect, useState } from "react";
import { Link, replace, useLoaderData, useNavigate } from "react-router-dom";
import { Flex, Image, Menu, Typography, Button, MenuProps } from "antd";
import { HomeOutlined, UnorderedListOutlined, UserOutlined } from "@ant-design/icons";
import React from "react";
import MenuItem from "antd/es/menu/MenuItem";
// import { useDispatch, useSelector } from "react-redux";
// import { RootState } from "../../store/store";
// import siteIcon from "../../assets/music-player.png"

type MenuItem = Required<MenuProps>['items'][number];

const { Text } = Typography;
const { SubMenu, Item } = Menu;



type NavProps = {
  appLoginState: boolean;
}

const NavigationBar: React.FC<NavProps> = ({ appLoginState }) => {
  // const authState = useSelector((state: RootState) => state.auth);
  const navigate = useNavigate();

  const [loginState, setLoginState] = useState<boolean>(appLoginState);

  const handleClickNavItem = ({ key }) => {
    // return navigate(`/${key}`)
    switch (key) {
      case "home":
        return navigate("/");

      case "my-events":
        return navigate("/my-events");

      case "my-profile":
        return navigate("/my-profile");

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
    {
      label: <Text style={{ fontSize: "20px" }}>My events</Text>,
      key: "my-events",
      icon: <UnorderedListOutlined style={{ fontSize: "18px" }} />,
      onClick: handleClickNavItem
    },
    {
      label: <Text style={{ fontSize: "20px" }}>My profile</Text>,
      key: "my-profile",
      icon: <UserOutlined style={{ fontSize: "18px" }} />,
      onClick: handleClickNavItem
    }
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

export default NavigationBar;