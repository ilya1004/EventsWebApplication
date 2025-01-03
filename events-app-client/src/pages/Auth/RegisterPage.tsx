import { Form, Button, Input, Card, Flex, Typography, DatePicker } from "antd";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { BASE_IDENTITY_URL } from "../../store/constants.ts";
import { GithubOutlined } from "@ant-design/icons";
import { Dayjs } from "dayjs";
import dayjs from "dayjs";
import { showMessageStc } from "../../services/ResponseErrorHandler.ts";

const { Title, Text } = Typography;

export const RegisterPage: React.FC = () => {

  const dateFormat = 'YYYY-MM-DD';

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [surname, setSurname] = useState<string>("");
  const [birthday, setBirthday] = useState<Dayjs | null>(dayjs(dateFormat));

  const navigate = useNavigate();

  const handleChangeEmail = (e: any) => {
    setEmail(e.target.value);
  };

  const handleChangePassword = (e: any) => {
    setPassword(e.target.value);
  };

  const handleChangeName = (e: any) => {
    setName(e.target.value);
  };

  const handleChangeSurname = (e: any) => {
    setSurname(e.target.value);
  };

  const handleChangeBirthday = (val: Dayjs | null) => {
    setBirthday(val);
  };

  const handleBack = () => navigate(-1);

  const registerUser = async (): Promise<any> => {
    try {
      const data = {
        email: email,
        password: password,
        name: name,
        surname: surname,
        birthday: birthday?.format("YYYY-MM-DD"),
      };

      const response = await axios.post(
        `${BASE_IDENTITY_URL}/api/Users/register`,
        JSON.stringify(data),
        {
          headers: { "Content-Type": "application/json", Accept: "*/*" },
        }
      );

      navigate("/login");
    } catch (err: any) {
      console.error("Error during user registration:", err.response.data);
      showMessageStc(err.response.data.detail, "error");
    }
  };


  const handleSubmit = (e: React.FormEvent) => {
    registerUser();
  };

  const disabledDate = (current: Dayjs) => {
    const today = dayjs();
    const minDate = today.subtract(18, "year");
    return current && (current.isAfter(today, "day") || current.isAfter(minDate, "day"));
  };

  return (
    <>
      <Flex
        justify="center"
        align="flex-start"
        style={{
          minHeight: "80vh",
          height: "fit-content",
        }}
      ><Flex style={{ width: "500px" }} vertical gap={20}>
          <Flex style={{ marginTop: "80px", width: "500px" }}>
            <Button onClick={() => handleBack()}>Back</Button>
          </Flex>
          <Card
            style={{ width: "500px" }}
            title={
              <Title style={{ margin: "0px" }} level={4}>
                Enter your data
              </Title>
            }
          >
            <Form
              labelCol={{ span: 10, offset: 0 }}
              wrapperCol={{ offset: 0 }}
              layout="horizontal"
              onFinish={handleSubmit}
            >
              <Form.Item
                label="Email:"
                name="email"
                rules={[{ required: true, message: "Enter your email!", }]}  >
                <Input type="email" name="email" onChange={handleChangeEmail} value={email} />
              </Form.Item>

              <Form.Item
                label="Password:"
                name="password"
                rules={[
                  { required: true, message: "Enter your password!" },
                  { min: 6, message: "Password must be at least 6 characters long." },
                  {
                    pattern: /[A-Z]/,
                    message: "Password must contain at least one uppercase letter.",
                  },
                  {
                    pattern: /[a-z]/,
                    message: "Password must contain at least one lowercase letter.",
                  },
                  {
                    pattern: /[0-9]/,
                    message: "Password must contain at least one digit.",
                  },
                  {
                    pattern: /[^a-zA-Z0-9]/,
                    message: "Password must contain at least one special character.",
                  },
                ]}>
                <Input.Password type="password" name="password" onChange={handleChangePassword} value={password} />
              </Form.Item>

              <Form.Item
                label="Name:"
                name="name"
                rules={[{ required: true, message: "Enter your name!", },]}
              >
                <Input name="name" onChange={handleChangeName} value={name} />
              </Form.Item>

              <Form.Item
                label="Surname:"
                name="surname"
                rules={[{ required: true, message: "Enter your surname!", },]}
              >
                <Input name="surname" onChange={handleChangeSurname} value={surname} />
              </Form.Item>

              <Form.Item
                label="Birthday date:"
                name="birthday"
                rules={[{ required: true, message: "Enter your birthday date!", },]}
              >
                <DatePicker name="birthday" onChange={handleChangeBirthday} value={birthday} disabledDate={disabledDate} />
              </Form.Item>

              <Form.Item>
                <Flex justify="center" align="center">
                  <Button
                    type="primary"
                    htmlType="submit"
                    style={{
                      margin: "5px 0px 0px 0px",
                      width: "75px",
                    }}
                  >
                    Sign Out
                  </Button>
                </Flex>
              </Form.Item>
            </Form>
          </Card>
        </Flex>
      </Flex>
      <div
        style={{
          backgroundColor: "#F5DEBE",
          padding: "15px 0px"
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
      </div>
    </>
  );
}
