import { Button, Card, DatePicker, Flex, Form, Input, InputNumber, TimePicker, Typography } from "antd";
import axios from "axios";
import React, { useEffect, useState } from "react";
import { BASE_SERVER_API_URL, PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { useLoaderData, useNavigate } from "react-router-dom";
import { Event as EventEntity } from "../../utils/types";
import dayjs, { Dayjs } from "dayjs";
import { showMessageStc } from "../../services/ResponseErrorHandler.ts";

const { Title } = Typography;

interface EventDTO {
  Title: string;
  Description: string | null;
  EventDateTime: string;
  ParticipantsMaxCount: number;
  ImageFile?: File | null;
  PlaceName: string;
  CategoryName: string | null;
}

export const adminEditEventLoader = async ({ params }) => {
  const { eventId } = params;
  const response = await axios.get(`${BASE_SERVER_API_URL}/Events/${eventId}`);
  return response.data;
};

export const AdminEditEventPage: React.FC = () => {
  const item = useLoaderData() as EventEntity;

  const [title, setTitle] = useState(item.title || "");
  const [description, setDescription] = useState<string | null>(item.description || "");
  const [eventDate, setEventDate] = useState<Dayjs | null>(dayjs(item.eventDateTime) || null);
  const [eventTime, setEventTime] = useState<Dayjs | null>(dayjs(item.eventDateTime) || null);
  const [participantsMaxCount, setParticipantsMaxCount] = useState<number | null>(item.participantsMaxCount || 0);
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [placeName, setPlaceName] = useState<string>(item.place?.name || "");
  const [categoryName, setCategoryName] = useState<string | null>(item.category?.name || "");

  const navigate = useNavigate();

  useEffect(() => {
    setTitle(item.title);
    setDescription(item.description);
    setEventDate(dayjs(item.eventDateTime));
    setEventTime(dayjs(item.eventDateTime));
    setParticipantsMaxCount(item.participantsMaxCount);
    setPlaceName(item.place?.name ?? "");
    setCategoryName(item.category?.name ?? "");
  }, [item]);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      setImageFile(e.target.files[0]);
    }
  };

  const handleBack = () => navigate(-1);

  const updateEvent = async (eventData: EventDTO) => {
    const formData = new FormData();

    formData.append("Title", eventData.Title);
    formData.append("Description", eventData.Description ?? "");
    formData.append("EventDateTime", eventData.EventDateTime);
    formData.append("ParticipantsMaxCount", eventData.ParticipantsMaxCount.toString());

    if (eventData.ImageFile) {
      formData.append("ImageFile", eventData.ImageFile);
    }

    formData.append("PlaceName", eventData.PlaceName);
    formData.append("CategoryName", eventData.CategoryName ?? "");

    return axios.put(`${BASE_SERVER_API_URL}/Events`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!eventDate || !eventTime) {
      alert("Please enter date and time of the event.");
      return;
    }

    const eventDateTime = dayjs(eventDate)
      .hour(eventTime.hour())
      .minute(eventTime.minute())
      .toISOString();

    const updatedEventData: EventDTO = {
      Title: title,
      Description: description,
      EventDateTime: eventDateTime,
      ParticipantsMaxCount: participantsMaxCount!,
      ImageFile: imageFile,
      PlaceName: placeName,
      CategoryName: categoryName,
    };

    try {
      await updateEvent(updatedEventData);
      showMessageStc("Event updated successfully", "success");
      navigate("/admin");
    } catch (error) {
      console.error("Error updating event:", error);
      showMessageStc("Error updating event", "error");
    }
  };

  const disabledDate = (current: Dayjs) => current && current.isBefore(dayjs(), "day");

  return (
    <Flex justify="center" align="flex-start" style={{ margin: "10px 15px", minHeight: PAGE_MIN_HEIGHT }}>
      <Flex align="center" vertical>
        <Flex gap={20}>
          <Button onClick={() => handleBack()} style={{ margin: "5px 0px 0px 0px" }}>Back</Button>
          <Title level={2} style={{ marginTop: "0px" }}>Create Event</Title>
        </Flex>
        <Card style={{ width: "500px" }} title="Edit event data">
          <Form
            labelCol={{ span: 10, offset: 0 }}
            wrapperCol={{ offset: 0 }}
            layout="horizontal"
            onFinish={handleSubmit}
          >
            <Form.Item label="Title:" rules={[{ required: true, message: "Enter title!" }]}>
              <Input value={title} onChange={(e) => setTitle(e.target.value)} />
            </Form.Item>

            <Form.Item label="Description:">
              <Input value={description || ""} onChange={(e) => setDescription(e.target.value)} />
            </Form.Item>

            <Form.Item label="Event Date and Time:">
              <Flex gap={10}>
                <DatePicker value={eventDate} onChange={setEventDate} disabledDate={disabledDate} />
                <TimePicker value={eventTime} onChange={setEventTime} />
              </Flex>
            </Form.Item>

            <Form.Item label="Max Participants:">
              <InputNumber value={participantsMaxCount} onChange={(value) => setParticipantsMaxCount(value)} min={0} />
            </Form.Item>

            <Form.Item label="Place:">
              <Input value={placeName} onChange={(e) => setPlaceName(e.target.value)} />
            </Form.Item>

            <Form.Item label="Category:">
              <Input value={categoryName || ""} onChange={(e) => setCategoryName(e.target.value)} />
            </Form.Item>

            <Form.Item label="Upload Image:">
              <input type="file" accept="image/*" onChange={handleFileChange} />
            </Form.Item>

            <Form.Item>
              <Button type="primary" htmlType="submit">
                Update Event
              </Button>
            </Form.Item>
          </Form>
        </Card>
      </Flex>
    </Flex>
  );
};
