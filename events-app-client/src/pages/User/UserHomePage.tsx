import { Button, Card, Flex, List, Typography, Image, Descriptions, TableProps, Table, Space, Tag } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { BASE_IDENTITY_URL, BASE_SERVER_API_URL, PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { refreshAccessToken } from "../../services/TokenService.ts"; // Импортируем наш сервис
import { redirect, useLoaderData, useNavigate } from "react-router-dom";
import { Event as EventEntity } from "../../utils/types";
import dayjs from "dayjs";
import { getRequestData } from "../../services/RequestRervice.ts";

const { Title, Text } = Typography;


// interface EventWithRemainingPlacesDTO {
//   id: number,
//   title: string,
//   description: string | null,
//   eventDateTime: string,
//   participantsMaxCount: number,
//   placesRemain: number,
//   placeName: string,
//   categoryName: string | null,
// }

export const userHomeLoader = async () => {
  let res = await getRequestData(`/Events?PageNo=${1}&PageSize=${10}`);
  console.log(res);
  return res;
}

export const UserHomePage: React.FC = () => {
  // const [isLoading, setIsLoading] = useState(false);

  const navigate = useNavigate();

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
        <Button onClick={() => handleMoreEventInfo(record)}>More</Button>
      ),
    },
  ];



  // const renderListItem = (item: EventEntity, index: number) => {
  //   let datetime = dayjs(item.eventDateTime);
  //   let dateString = datetime.format('HH:mm DD.MM.YYYY');

  //   return (
  //     <Card title={<Title level={3}>{item.title}</Title>} style={{ margin: "20px 0px" }}>
  //       <Flex gap={20}>
  //         <Flex vertical gap={20}>
  //           <Descriptions column={1} bordered>
  //             <Descriptions.Item label="Description" style={{ fontSize: "16px" }}>
  //               {item.description}
  //             </Descriptions.Item>
  //             <Descriptions.Item label="Event date and time" style={{ fontSize: "16px" }}>
  //               {dateString}
  //             </Descriptions.Item>
  //             <Descriptions.Item label="Maximum number of participants" style={{ fontSize: "16px" }}>
  //               {item.participantsMaxCount}
  //             </Descriptions.Item>
  //             <Descriptions.Item label="Place" style={{ fontSize: "16px" }}>
  //               {item.place?.name}
  //             </Descriptions.Item>
  //             <Descriptions.Item label="Category" style={{ fontSize: "16px" }}>
  //               {item.category?.name}
  //             </Descriptions.Item>
  //           </Descriptions>
  //           {/* <Flex> */}
  //           {/* <Button onClick={() => handleRefuseParticipation(item)} danger>Refuse participation</Button> */}
  //           {/* </Flex> */}
  //         </Flex>
  //       </Flex>
  //     </Card>
  //   )
  // }

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
          <Title level={2} style={{ marginTop: "0px" }}>All Events</Title>
          <Table columns={columns} dataSource={events} />
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