import { Form, Button, Input, Card, Flex, Typography } from "antd";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
// import { useSelector } from "react-redux";
import { showMessageStc } from "../../services/ResponseErrorHandler.ts";
import { BASE_IDENTITY_URL } from "../../store/constants.ts";
import { GithubOutlined } from "@ant-design/icons";
import { getUserRole } from "../../services/TokenService.ts";
// import { RootState } from "../../store/store";

const { Title, Text } = Typography;

export const LoginPage: React.FC = () => {

  // const authState = useSelector((state: RootState) => state.auth);

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const navigate = useNavigate();


  const handleChangeEmail = (e: any) => {
    setEmail(e.target.value);
  };

  const handleChangePassword = (e: any) => {
    setPassword(e.target.value);
  };

  const loginUser = async (): Promise<any> => {
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

      const { access_token, refresh_token } = response.data;

      localStorage.setItem("access_token", access_token);
      localStorage.setItem("refresh_token", refresh_token);

      let role = getUserRole()
      console.log(role);

      if (role == "User") {
        navigate("/");
      }
      else if (role == "Admin") {
        navigate("/admin");
      }

    } catch (err: any) {
      console.log(err);
      if (err.response.data.error_description == "invalid_username_or_password") {
        showMessageStc("Invalid email or password", "error");
        setEmail("");
        setPassword("");
      }
      else {
        showMessageStc(err, "error");
      }
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
              Enter your credentials
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
                    margin: "5px 0px 0px 0px",
                    width: "75px",
                  }}
                >
                  Sign In
                </Button>
              </Flex>
            </Form.Item>
          </Form>
          <Flex align="center" justify="center">
            <Text style={{ marginRight: "5px" }}>Don't have an account yet?</Text>
            <Link to="/register">Register</Link>
          </Flex>
        </Card>
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

// export default LoginPage;