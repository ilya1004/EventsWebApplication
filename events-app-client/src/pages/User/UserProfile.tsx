import { Avatar, Card, Descriptions, Flex, Typography } from "antd";
import React from "react";
import { useLoaderData } from "react-router-dom";
import { UserInfo } from "../../utils/types";
import { UserOutlined } from "@ant-design/icons";
import { PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { getIdentityRequestData } from "../../services/identityRequestService.ts";


const { Title } = Typography;

export const userProfileLoader = async () => {
  const data = await getIdentityRequestData("/connect/userinfo")
  return data;
}

export const UserProfilePage: React.FC = () => {
  const userData = useLoaderData() as UserInfo;

  return (
    <>
      <Flex
        justify="center"
        align="flex-start"
        style={{
          margin: "10px 15px",
          minHeight: PAGE_MIN_HEIGHT,
          maxWidth: "1400px",
        }}
      >
        <Flex align="center" vertical gap={20}>
          <Card
            title={<Title level={4}>My Profile</Title>}
            style={{ width: "600px" }}
          >
            <Flex justify="center" style={{ marginBottom: "20px" }}>
              <Avatar size={100} icon={<UserOutlined />} />
            </Flex>

            <Descriptions column={1} bordered>
              <Descriptions.Item label="Email" style={{ fontSize: "16px" }}>
                {userData.email}
              </Descriptions.Item>
              <Descriptions.Item label="Name" style={{ fontSize: "16px" }}>
                {userData.name}
              </Descriptions.Item>
              <Descriptions.Item label="Surname" style={{ fontSize: "16px" }}>
                {userData.surname}
              </Descriptions.Item>
              <Descriptions.Item label="Birthday" style={{ fontSize: "16px" }}>
                {userData.birthday.substring(0, 10)}
              </Descriptions.Item>
            </Descriptions>

          </Card>
        </Flex>
      </Flex>
    </>
  );
};
