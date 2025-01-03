import { Button, Card, Descriptions, Flex, Image, Table, TableProps, Typography } from "antd";
import React from "react";
import { PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { useLoaderData, useNavigate } from "react-router-dom";
import { getRequestData } from "../../services/RequestService.ts";
import dayjs from "dayjs";
import EventPlaceholder from "../../assets/event_placeholder.png";

const { Title, Text } = Typography;

export const adminEventInfoLoader = async ({ params }) => {
  var { eventId } = params;
  var res = await getRequestData(`/Events/${eventId}/with-participants`);
  return res;
}

export const AdminEventInfoPage: React.FC = () => {
  const item = useLoaderData() as any;

  const navigate = useNavigate();

  const renderDateTime = () => {
    let datetime = dayjs(item.EventDateTime);
    let dateString = datetime.format('HH:mm DD.MM.YYYY');
    return dateString;
  }

  const handleBack = () => {
    return navigate(-1);
  }

  const columns: TableProps['columns'] = [
    {
      title: 'Email',
      dataIndex: "Email",
      key: "email",
      width: "200px",
      render: (value, _, __) => <Text>{value}</Text>
    },
    {
      title: 'Name',
      dataIndex: "Name",
      key: "name",
      width: "160px",
      render: (value, record, __) => <Text>{record.Person.Name}</Text>
    },
    {
      title: 'Surname',
      dataIndex: "Surname",
      key: "surname",
      width: "160px",
      render: (value, record, __) => <Text>{record.Person.Surname}</Text>
    },
    {
      title: 'Birthday',
      dataIndex: "Birthday",
      key: "birthday",
      width: "160px",
      render: (value, record, __) => <Text>{dayjs(record.Person.BirthdayDate).format("DD.MM.YYYY")}</Text>
    },
    {
      title: 'Registration date',
      dataIndex: "EventRegistrationDate",
      key: "eventRegistrationDate",
      width: "160px",
      render: (value, record, __) => <Text>{dayjs(value).format("DD.MM.YYYY")}</Text>
    }
  ];

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
          <Card title={<Title level={3}>{item.Title}</Title>} style={{ margin: "20px 0px" }}>
            <Flex gap={20}>
              {item.Image == null ?
                <Image src={EventPlaceholder} style={{ maxHeight: "290px", maxWidth: "290px", borderRadius: "7px" }} alt="No image" preview={false} /> :
                <Image src={`http://127.0.0.1:10000/devstoreaccount1/images/${item.Image}`} alt="No image" preview={false}
                  style={{ maxHeight: "350px", maxWidth: "300px", borderRadius: "7px" }} />}
              <Flex vertical gap={20}>
                <Descriptions column={1} bordered>
                  <Descriptions.Item label="Description" style={{ fontSize: "16px" }}>
                    {item.Description}
                  </Descriptions.Item>
                  <Descriptions.Item label="Event date and time" style={{ fontSize: "16px" }}>
                    {renderDateTime()}
                  </Descriptions.Item>
                  <Descriptions.Item label="Maximum number of participants" style={{ fontSize: "16px" }}>
                    {item.ParticipantsMaxCount}
                  </Descriptions.Item>
                  <Descriptions.Item label="Place" style={{ fontSize: "16px" }}>
                    {item.Place?.Name}
                  </Descriptions.Item>
                  <Descriptions.Item label="Category" style={{ fontSize: "16px" }}>
                    {item.Category?.Name}
                  </Descriptions.Item>
                </Descriptions>
              </Flex>
            </Flex>
          </Card>
          <Title>Event's participants:</Title>
          <Table dataSource={item.Participants} columns={columns} pagination={false}/>
        </Flex>
      </Flex>
    </>
  );
};
