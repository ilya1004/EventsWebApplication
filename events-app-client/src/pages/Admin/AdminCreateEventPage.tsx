import { Button, Card, DatePicker, Flex, Form, Image, Input, InputNumber, TimePicker, Typography } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { BASE_SERVER_API_URL, PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { useNavigate } from "react-router-dom";
import dayjs, { Dayjs } from "dayjs";
import { showMessageStc } from "../../services/ResponseErrorHandler.ts";

const { Title, Text } = Typography;

interface EventDTO {
  Title: string;
  Description: string;
  EventDateTime: string;
  ParticipantsMaxCount: number;
  ImageFile: File;
  PlaceName: string;
  CategoryName: string;
}

export const AdminCreateEventPage: React.FC = () => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [eventDate, setEventDate] = useState<Dayjs | null>(null);
  const [eventTime, setEventTime] = useState<Dayjs | null>(null);
  const [participantsMaxCount, setParticipantsMaxCount] = useState<number | null>(0);
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [placeName, setPlaceName] = useState("");
  const [categoryName, setCategoryName] = useState("");

  const navigate = useNavigate();

  const dateFormat = 'YYYY.MM.DD';

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0];
      setImageFile(file);
    }
  };

  const handleBack = () => {
    return navigate(-1);
  }

  const createEvent = async (eventData: any) => {
    const formData = new FormData();

    formData.append("EventDTO.Title", eventData.Title ?? "");
    formData.append("EventDTO.Description", eventData.Description ?? "");
    formData.append("EventDTO.EventDateTime", eventData.EventDateTime);
    formData.append(
      "EventDTO.ParticipantsMaxCount",
      eventData.ParticipantsMaxCount?.toString() ?? "0"
    );

    if (eventData.ImageFile) {
      formData.append("EventDTO.ImageFile", eventData.ImageFile);
    } else {
      formData.append("EventDTO.ImageFile", "");
    }

    formData.append("EventDTO.PlaceName", eventData.PlaceName ?? "");
    formData.append("EventDTO.CategoryName", eventData.CategoryName ?? "");

    const accessToken = localStorage.getItem("access_token");

    return axios.post(`${BASE_SERVER_API_URL}/Events`, formData, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
        "Content-Type": "multipart/form-data",
        Accept: "*/*",
      },
    });
  };


  const handleSubmit = async (e: React.FormEvent) => {
    // e.preventDefault();

    if (title == "") {
      showMessageStc("Please enter title of the event.", "warning");
      return;
    }

    if (!eventDate || !eventTime) {
      showMessageStc("Please enter date and time of the event.", "warning");
      return;
    }

    if (participantsMaxCount == null || participantsMaxCount <= 0) {
      showMessageStc("Please enther a correct maximum number of persons who can register to this event.", "warning");
      return;
    }

    if (placeName == null || placeName == "") {
      showMessageStc("Please enther the place of the event.", "warning");
      return;
    }

    const eventDateTime = dayjs(eventDate)
      .hour(eventTime.hour())
      .minute(eventTime.minute())
      .toISOString();

    const eventDataCreate = {
      Title: title,
      Description: description,
      EventDateTime: eventDateTime,
      ParticipantsMaxCount: participantsMaxCount,
      ImageFile: imageFile,
      PlaceName: placeName,
      CategoryName: categoryName,
    };

    try {
      await createEvent(eventDataCreate);
      showMessageStc("Event created successfully", "success");
      return navigate("/admin");
    } catch (error) {
      console.error("Error in creating event:", error);
      showMessageStc("Error in creating event", "error");
    }
  };

  const disabledDate = (current: Dayjs) => {
    const today = dayjs();
    return current && current.isBefore(today, "day");
  };


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
            <Title level={2} style={{ marginTop: "0px" }}>Create Event</Title>
          </Flex>
          <Card style={{ width: "500px" }}
            title={
              <Title style={{ margin: "0px" }} level={4}>
                Enter event data
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
                label="Title:"
                name="title"
                rules={[{ required: true, message: "Enter title of the event!", }]}  >
                <Input name="title" onChange={(e) => setTitle(e.target.value)} value={title} />
              </Form.Item>

              <Form.Item
                label="Description:"
                name="description">
                <Input name="description" onChange={(e) => setDescription(e.target.value)} value={description} />
              </Form.Item>

              <Form.Item
                label="Event date:"
                name="eventDateTime">
                <Flex gap={10}>
                  <DatePicker name="eventDateTime" onChange={(value: any) => setEventDate(value)} value={eventDate} disabledDate={disabledDate} defaultValue={dayjs()} />
                  <TimePicker onChange={(value: any) => setEventTime(value)} value={eventTime} defaultOpenValue={dayjs('00:00:00', 'HH:mm:ss')} />
                </Flex>
              </Form.Item>

              <Form.Item label="Max Participants:"
                rules={[{ required: true, message: "Enter max participants of the event!", }]} >
                <InputNumber value={participantsMaxCount} onChange={(value) => setParticipantsMaxCount(value)} min={0} />
              </Form.Item>

              <Form.Item
                label="Place:"
                name="place"
                rules={[{ required: true, message: "Enter place of the event!", },]}>
                <Input name="place" onChange={(e) => setPlaceName(e.target.value)} value={description} />
              </Form.Item>

              <Form.Item
                label="Category:"
                name="category">
                <Input name="category" onChange={(e) => setCategoryName(e.target.value)} value={description} />
              </Form.Item>

              <Form.Item label="Upload Image:" name="image">
                <input type="file" accept="image/*" onChange={handleFileChange} />
              </Form.Item>

              <Form.Item>
                <Flex justify="center" align="center">
                  <Button type="primary" htmlType="submit">
                    Create Event
                  </Button>
                </Flex>
              </Form.Item>
            </Form>
          </Card>
        </Flex>
      </Flex>
    </>
  );
};
