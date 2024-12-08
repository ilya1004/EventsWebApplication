import { Form, Button, Input, Card, Flex, Typography } from "antd";
import React, { useState } from "react";
import { Link, useNavigate, useOutletContext } from "react-router-dom";
import axios from "axios";
// import { useSelector } from "react-redux";
import { showMessageStc } from "../../services/ResponseErrorHandler.ts";
import { BASE_IDENTITY_URL } from "../../store/constants.ts";
// import { RootState } from "../../store/store";

const { Title, Text } = Typography;

export const LoginPage: React.FC = () => {

  // const authState = useSelector((state: RootState) => state.auth);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();


  const handleChangeEmail = (e: any) => {
    setEmail(e.target.value);
  };

  const handleChangePassword = (e: any) => {
    setPassword(e.target.value);
  };

  const loginUser = async () => {
    console.log(`${email}, ${password}`);
    try {
      const response = await axios.post(`${BASE_IDENTITY_URL}/connect/token`, new URLSearchParams({
        grant_type: "password",
        client_id: "react_client",
        client_secret: "react_secret",
        username: email,
        password: password,
        scope: 'openid profile offline_access',
      }), {
        headers: {
          "Content-Type": "application/x-www-form-urlencoded"
        }
      });

      console.log(response);

      const { access_token, refresh_token } = response.data;

      localStorage.setItem("access_token", access_token);
      localStorage.setItem("refresh_token", refresh_token);

      alert("Login successful!");
    } catch (err: any) {
      console.log(err);
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    // e.preventDefault();
    loginUser();
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
      >
        <Card
          style={{
            marginTop: "100px",
            width: "450px",
          }}
          title={
            <Title style={{ margin: "0px" }} level={4}>
              Введите свои данные
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
              rules={[
                {
                  required: true,
                  message: "Enter your email!",
                },
              ]}
            >
              <Input
                // type="email"
                name="email"
                onChange={handleChangeEmail}
                value={email}
              />
            </Form.Item>

            <Form.Item
              label="Password:"
              name="password"
              rules={[
                {
                  required: true,
                  message: "Enter your password!",
                },
              ]}
            >
              <Input
                name="password"
                onChange={handleChangePassword}
                value={password}
              />
            </Form.Item>

            <Form.Item>
              <Flex justify="center" align="center">
                <Button
                  type="primary"
                  htmlType="submit"
                  style={{
                    margin: "0px",
                    width: "75px",
                  }}
                >
                  Sign In
                </Button>
              </Flex>
            </Form.Item>
          </Form>
          {/* <Flex align="center" justify="center">
            <Text style={{ marginRight: "5px" }}>Еще нет аккаунта?</Text>
            <Link to="/register">Зарегистрироваться</Link>
          </Flex> */}
        </Card>
      </Flex>
    </>
  );
}

// export default LoginPage;