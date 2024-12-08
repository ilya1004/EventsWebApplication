import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Flex, Image, Menu, Typography, Button } from "antd";
import { HomeOutlined, DownOutlined, SearchOutlined } from "@ant-design/icons";
import React from "react";
// import { useDispatch, useSelector } from "react-redux";
// import { RootState } from "../../store/store";
// import siteIcon from "../../assets/music-player.png"

const { Text } = Typography;
const { SubMenu, Item } = Menu;

const NavigationBar: React.FC = () => {
  // const authState = useSelector((state: RootState) => state.auth);
  const navigate = useNavigate();

  // console.log(authState);

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
            margin: "0px 15px",
            padding: "0px 15px",
            borderRadius: "30px",
          }}
        >
          <Menu.Item key={1} icon={<HomeOutlined />} >
            <Link to="/" style={{ fontSize: "20px" }}>
              Home
            </Link>
          </Menu.Item>
          <Menu.Item key={2} icon={<SearchOutlined />} >
            <Link to="/search" style={{ fontSize: "20px" }}>
              Search
            </Link>
          </Menu.Item>
          <Menu.Item key={3}>
            <Link to="/artists" style={{ fontSize: "20px" }}>
              Artists
            </Link>
          </Menu.Item>
          <Menu.Item key={4}>
            <Link to="/user/favourite-songs" style={{ fontSize: "20px" }}>
              My songs
            </Link>
          </Menu.Item>
          <Menu.Item key={5}>
            <Link to="/user/playlists" style={{ fontSize: "20px" }}>
              My playlists
            </Link>
          </Menu.Item>
          <Menu.Item key={6}>
            <Link to="/user/profile" style={{ fontSize: "20px" }}>
              My profile
            </Link>
          </Menu.Item>
        </Menu>
        <Flex style={{

        }}>
          <Button style={{ margin: "auto" }} onClick={() => navigate("/login")}>{"Login"}</Button>
        </Flex>
      </Flex>
    </>
  );
}

export default NavigationBar;