import { Button, Card, Flex, List, Typography, Image, Descriptions, TableProps, Table, Space, Tag } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { BASE_IDENTITY_URL, BASE_SERVER_API_URL, PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { refreshAccessToken } from "../../services/TokenService.ts"; // Импортируем наш сервис
import { redirect, useLoaderData } from "react-router-dom";
import { Event as EventEntity } from "../../utils/types";
import dayjs from "dayjs";
import { getRequestData } from "../../services/RequestRervice.ts";

const { Title, Text } = Typography;

interface DataType {
  key: string;
  name: string;
  age: number;
  address: string;
  tags: string[];
}

export const userHomeLoader = async () => {
  let res = await getRequestData(`/Events?PageNo=${1}&PageSize=${10}`);
  return res;
}


export const UserHomePage: React.FC = () => {
  // const [isLoading, setIsLoading] = useState(false);

  const events = useLoaderData() as 

  const getData = async () => {
    try {
      let accessToken = localStorage.getItem("access_token");

      if (!accessToken) {
        console.error("No access token found");
      }

      // Пробуем сделать запрос с текущим access токеном
      const response = await axios.get(`${BASE_SERVER_API_URL}/Events/user`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      console.log("Data from protected endpoint:", response.data);

    } catch (error) {
      if (error.response?.status === 401) {
        console.log("Access token expired, attempting to refresh...");
        try {
          await refreshAccessToken();

          let newAccessToken = localStorage.getItem("access_token");

          const response = await axios.get(`${BASE_SERVER_API_URL}/Events/user`, {
            headers: {
              Authorization: `Bearer ${newAccessToken}`,
            },
          });

          console.log("Data from protected endpoint:", response.data);
        } catch (refreshError) {
          console.error("Error while refreshing token:", refreshError);
        }
      } else {
        console.error("Error fetching data:", error);
      }
    }
  };

  const getData1 = async () => {
    try {
      let accessToken = localStorage.getItem("access_token");

      if (!accessToken) {
        console.error("No access token found");
      }

      // Пробуем сделать запрос с текущим access токеном
      const response = await axios.get(`${BASE_IDENTITY_URL}/api/Users/c3415321-fc58-41d8-8ea8-220a45a17355`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      console.log(response.data);

    } catch (error) {
      if (error.response?.status === 401) {
        console.log("Access token expired, attempting to refresh...");
        try {
          await refreshAccessToken();

          let newAccessToken = localStorage.getItem("access_token");

          // Повторно отправляем запрос с новым access токеном
          const response = await axios.get(`${BASE_SERVER_API_URL}/Events/user`, {
            headers: {
              Authorization: `Bearer ${newAccessToken}`,
            },
          });

          console.log("Data from protected endpoint:", response.data);
        } catch (refreshError) {
          console.error("Error while refreshing token:", refreshError);
        }
      } else {
        console.error("Error fetching data:", error);
      }
    }
  };

  // Обработчик клика для получения данных
  const handleClick = () => {
    getData();
  };

  const handleClick1 = () => {
    getData1();
  };

  const columns: TableProps<DataType>['columns'] = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      render: (text) => <a>{text}</a>,
    },
    {
      title: 'Age',
      dataIndex: 'age',
      key: 'age',
    },
    {
      title: 'Address',
      dataIndex: 'address',
      key: 'address',
    },
    {
      title: 'Tags',
      key: 'tags',
      dataIndex: 'tags',
      render: (_, { tags }) => (
        <>
          {tags.map((tag) => {
            let color = tag.length > 5 ? 'geekblue' : 'green';
            if (tag === 'loser') {
              color = 'volcano';
            }
            return (
              <Tag color={color} key={tag}>
                {tag.toUpperCase()}
              </Tag>
            );
          })}
        </>
      ),
    },
    {
      title: 'Action',
      key: 'action',
      render: (_, record) => (
        <Space size="middle">
          <a>Invite {record.name}</a>
          <a>Delete</a>
        </Space>
      ),
    },
  ];



  const renderListItem = (item: EventEntity, index: number) => {
    let datetime = dayjs(item.eventDateTime);
    let dateString = datetime.format('HH:mm DD.MM.YYYY');

    return (
      <Card title={<Title level={3}>{item.title}</Title>} style={{ margin: "20px 0px" }}>
        <Flex gap={20}>
          {item.image == null ?
            <Image src={EventPlaceholder} style={{ maxHeight: "290px", maxWidth: "290px", borderRadius: "7px" }} alt="No image" preview={false} /> :
            <Image src={`http://127.0.0.1:10000/devstoreaccount1/images/${item.image}`} alt="No image" preview={false}
              style={{ maxHeight: "350px", maxWidth: "300px", borderRadius: "7px" }} />}
          <Flex vertical gap={20}>
            <Descriptions column={1} bordered>
              <Descriptions.Item label="Description" style={{ fontSize: "16px" }}>
                {item.description}
              </Descriptions.Item>
              <Descriptions.Item label="Event date and time" style={{ fontSize: "16px" }}>
                {dateString}
              </Descriptions.Item>
              <Descriptions.Item label="Maximum number of participants" style={{ fontSize: "16px" }}>
                {item.participantsMaxCount}
              </Descriptions.Item>
              <Descriptions.Item label="Place" style={{ fontSize: "16px" }}>
                {item.place?.name}
              </Descriptions.Item>
              <Descriptions.Item label="Category" style={{ fontSize: "16px" }}>
                {item.category?.name}
              </Descriptions.Item>
            </Descriptions>
            <Flex>
              <Button onClick={() => handleRefuseParticipation(item)} danger>Refuse participation</Button>
            </Flex>
          </Flex>
        </Flex>
      </Card>
    )
  }

  return (
    <>
      <Flex
        justify="center"
        align="flex-start"
        style={{
          margin: "10px 15px",
          minHeight: PAGE_MIN_HEIGHT,
          maxWidth: "1400px"
        }}
      >
        <Flex align="center" vertical>
          <Title level={2} style={{ marginTop: "0px" }}>My events</Title>
          {/* <List

            dataSource={events}
            renderItem={(item, index) => renderListItem(item, index)}
          >
          </List> */}
          <Table columns={columns} dataSource={data} />;
        </Flex>
      </Flex>
    </>
  );
};

{/* <Button onClick={handleClick}>
          Click to Get Data
        </Button>
        <Button onClick={handleClick1}>
          Click to Get User Info
        </Button> */}