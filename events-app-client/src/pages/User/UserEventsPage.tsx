import { Button, Card, Descriptions, Flex, Image, List, Typography } from "antd";
import React from "react";
import {  PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { useLoaderData, useRevalidator } from "react-router-dom";
import { deleteRequestData, getRequestData } from "../../services/RequestService.ts";
import { Event as EventEntity } from "../../utils/types";
import dayjs from "dayjs";
import EventPlaceholder from "../../assets/event_placeholder.png";

const { Title, Text } = Typography;

export const userEventsLoader = async () => {
  var res = await getRequestData("/Events/by-current-user");
  return res;
}

export const UserEventsPage: React.FC = () => {
  const revalidator = useRevalidator();

  const events = useLoaderData() as EventEntity[];

  const handleRefuseParticipation = async (item: EventEntity) => {
    await deleteRequestData(`/Participants/${item.id}`);
    revalidator.revalidate();
  }

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
          <List

            dataSource={events}
            renderItem={(item, index) => renderListItem(item, index)}
          >
          </List>
        </Flex>
      </Flex>
    </>
  );
};
