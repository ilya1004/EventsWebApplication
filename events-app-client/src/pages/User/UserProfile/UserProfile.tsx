import { Avatar, Button, Card, Col, DatePicker, Descriptions, Flex, Input, Row, Typography } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { Outlet, redirect, useLoaderData, useNavigate } from "react-router-dom";
import { UserInfoDTO } from "../../../utils/types";
import { getRequestData } from "../../../services/RequestRervice.ts";
import { UserOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { Dayjs } from 'dayjs';
import { PAGE_MIN_HEIGHT } from "../../../store/constants.ts";

const { Text, Title } = Typography;

const colTextWidth = "100px";
const colInputWidth = "200px";

export const userProfileLoader = async () => {

  const data = await getRequestData("/Users");
  console.log(data);
  return data;
}

export const UserProfilePage: React.FC = () => {

  const userData = useLoaderData() as UserInfoDTO;
  const [isEditing, setIsEditing] = useState<boolean>(false);

  const [nameEdit, setNameEdit] = useState<string>("");
  const [surnameEdit, setSurnameEdit] = useState<string>("");
  const [birthdayEdit, setBirthdayEdit] = useState<Dayjs>();

  const navigate = useNavigate();

  // const [isLoading, setIsLoading] = useState(false);

  // const handleEditProfile = () => {
  //   setNameEdit(userData.name);
  //   setSurnameEdit(userData.surname);
  //   setBirthdayEdit(dayjs(userData.birthday))
  //   setIsEditing(!isEditing);
  // }

  const handleNameEdit = (e: any) => {
    setNameEdit(e.target.value);
  }

  const handleSurnameEdit = (e: any) => {
    setSurnameEdit(e.target.value);
  }

  const handleBirthdayEdit = (e: any) => {
    console.log(e);
  }

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

            <Flex justify="start" style={{ marginTop: "20px" }} gap={20}>
              {/* <Button type="primary" style={{ fontSize: "14px" }} onClick={handleEditProfile}>Edit Profile</Button> */}
              <Button type="default" style={{ fontSize: "14px" }} danger>Delete Profile</Button>
            </Flex>
          </Card>
          {isEditing ?
            <Card style={{ width: "400px", marginBottom: "10px" }}>
              <Flex align="center" vertical gap={20}>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: colTextWidth }}>
                    <Text style={{ fontSize: "16px" }}>Name:</Text>
                  </Col>
                  <Col style={{ width: colInputWidth }}>
                    <Input onChange={handleNameEdit} value={nameEdit} />
                  </Col>
                </Row>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: colTextWidth }}>
                    <Text style={{ fontSize: "16px" }}>Surname:</Text>
                  </Col>
                  <Col style={{ width: colInputWidth }}>
                    <Input onChange={handleSurnameEdit} value={surnameEdit} />
                  </Col>
                </Row>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: colTextWidth }}>
                    <Text style={{ fontSize: "16px" }}>Birthday:</Text>
                  </Col>
                  <Col style={{ width: colInputWidth }}>
                    <DatePicker onChange={handleBirthdayEdit} value={birthdayEdit}/>
                  </Col>
                </Row>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: colTextWidth }}>
                    
                  </Col>
                  <Col style={{ width: colInputWidth }}>
                    <Button type="primary">Apply</Button>
                  </Col>
                </Row>
              </Flex>
            </Card>
            : null}
        </Flex>
      </Flex>
    </>
  );
};
