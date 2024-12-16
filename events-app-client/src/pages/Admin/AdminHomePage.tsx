import { Button, Card, Flex, List, Typography, Image, Descriptions, TableProps, Table, Space, Tag } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { BASE_IDENTITY_URL, BASE_SERVER_API_URL, PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { refreshAccessToken } from "../../services/TokenService.ts"; // Импортируем наш сервис
import { redirect, useLoaderData, useNavigate, useRevalidator } from "react-router-dom";
import { Event as EventEntity } from "../../utils/types";
import dayjs from "dayjs";
import { deleteRequestData, getRequestData } from "../../services/RequestRervice.ts";

const { Title, Text } = Typography;


export const adminHomeLoader = async () => {
  let res = await getRequestData(`/Events?PageNo=${1}&PageSize=${10}`);
  // console.log(res);
  return res;
}

export const AdminHomePage: React.FC = () => {
  // const [isLoading, setIsLoading] = useState(false);

  const navigate = useNavigate();

  const revalidator = useRevalidator();

  const events = useLoaderData() as EventEntity[];

  const renderDateTime = (value: string, record: EventEntity, index: number) => {

    let datetime = dayjs(value);
    let dateString = datetime.format('HH:mm DD.MM.YYYY');

    return (
      <Text>{dateString}</Text>
    );
  }

  const handleMoreEventInfo = (record: EventEntity) => {
    return navigate(`event/${record.id}`)
  }

  const handleEditEvent = (record: EventEntity) => {
    return navigate(`edit-event/${record.id}`)
  }

  const handleDeleteEvent = (record: EventEntity) => {
    let res = deleteRequestData(`/Events?id=${record.id}`);
    revalidator.revalidate();
  }

  const columns: TableProps<EventEntity>['columns'] = [
    {
      title: 'Title',
      dataIndex: "title",
      key: "title",
      width: "220px",
      render: (value, _, __) => <Text style={{ fontSize: "18px" }}>{value}</Text>
    },
    {
      title: "Date and time",
      dataIndex: "eventDateTime",
      key: "eventDateTime",
      width: "220px",
      render: (value, record, index) => renderDateTime(value, record, index),
    },
    {
      title: 'Place',
      dataIndex: "place",
      key: "place",
      width: "220px",
      render: (value, _, __) => <Text>{value.name}</Text>
    },
    {
      title: 'Actions',
      key: 'actions',
      width: "150px",
      render: (_, record) => (
        <Flex gap={10}>
          <Button onClick={() => handleMoreEventInfo(record)}>More</Button>
          <Button onClick={() => handleEditEvent(record)} type="primary">Edit</Button>
          <Button onClick={() => handleDeleteEvent(record)} danger>Delete</Button>
        </Flex>
      ),
    },
  ];

  const handleCreateEvent = () => {
    return navigate("create-event");
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
        <Flex vertical>
          <Flex align="center" vertical>
            <Title level={2} style={{ marginTop: "0px" }}>All Events</Title>
            <Table columns={columns} dataSource={events} />
          </Flex>
          <Flex align="start">
            <Button style={{ margin: "0px 0px 0px 20px" }} onClick={handleCreateEvent}>Create Event</Button>
          </Flex>
        </Flex>
      </Flex>
    </>
  );
};