import { Button, Card, Descriptions, Flex, Image, List, Typography } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { BASE_IDENTITY_URL, BASE_SERVER_API_URL, PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { refreshAccessToken } from "../../services/TokenService.ts";
import { redirect, useLoaderData, useNavigate, useParams, useRevalidator } from "react-router-dom";
import { deleteRequestData, getRequestData, postRequestData } from "../../services/RequestRervice.ts";
import { Event as EventEntity } from "../../utils/types";
import dayjs from "dayjs";
import EventPlaceholder from "../../assets/event_placeholder.png";

const { Title, Text } = Typography;

export const userEventInfoLoader = async ({ params }) => {
  var { eventId } = params;
  var res1 = await getRequestData(`/Events/${eventId}`);
  var res2 = await getRequestData(`/Participants/check-participation-by-event/${eventId}`)
  return { res1, res2 };
}

export const UserEventInfoPage: React.FC = () => {
  const { res1: item, res2: isUserPart } = useLoaderData() as { res1: EventEntity, res2: boolean };

  const navigate = useNavigate();

  const revalidator = useRevalidator();

  const handleRefuseParticipation = async (item: EventEntity) => {
    await deleteRequestData(`/Participants/${item.id}`);
    revalidator.revalidate();
  }

  const handleAddParticipation = async (item: EventEntity) => {
    await postRequestData(`/Participants/${item.id}`, null);
    revalidator.revalidate();
  }

  const renderDateTime = () => {
    let datetime = dayjs(item.eventDateTime);
    let dateString = datetime.format('HH:mm DD.MM.YYYY');
    return dateString;
  }

  const handleBack = () => {
    return navigate(-1);
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
          <Flex gap={20}>
            <Button onClick={() => handleBack()} style={{ margin: "5px 0px 0px 0px" }}>Back</Button>
            <Title level={2} style={{ marginTop: "0px" }}>Event Info</Title>
          </Flex>
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
                    {renderDateTime()}
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
                  {isUserPart == true ? 
                  <Button onClick={() => handleRefuseParticipation(item)} danger>Refuse participation</Button> :
                  <Button onClick={() => handleAddParticipation(item)} type="primary">Add participation</Button>}
                </Flex>
              </Flex>
            </Flex>
          </Card>
        </Flex>
      </Flex>
    </>
  );
};
