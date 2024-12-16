import { Button, Flex, Typography, TableProps, Table, Input, DatePicker } from "antd";
import React, { useState } from "react";
import { PAGE_MIN_HEIGHT } from "../../store/constants.ts";
import { useLoaderData, useNavigate } from "react-router-dom";
import { Event as EventEntity } from "../../utils/types";
import dayjs, { Dayjs } from "dayjs";
import { getRequestData } from "../../services/RequestRervice.ts";

const { Title, Text } = Typography;

const { RangePicker } = DatePicker;

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
  // console.log(res);
  return res;
}

export const UserHomePage: React.FC = () => {

  const [dateStart, setDateStart] = useState<Dayjs | null>(null);
  const [dateEnd, setDateEnd] = useState<Dayjs | null>(null);
  const [placeName, setPlaceName] = useState<string>("");
  const [categoryName, setCategoryName] = useState<string>("");

  const [pageNo, setPageNo] = useState<number>(1)
  const [pageSize, setPageSize] = useState<number>(10)

  const navigate = useNavigate();
  const eventsLoader = useLoaderData() as EventEntity[];

  const [events, setEvents] = useState<EventEntity[]>(eventsLoader);

  const dateFormat = 'YYYY.MM.DD';

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
      width: "200px",
      render: (value, record, index) => renderDateTime(value, record, index),
    },
    {
      title: 'Place',
      dataIndex: "place",
      key: "place",
      width: "180px",
      render: (value, _, __) => <Text>{value.name}</Text>
    },
    {
      title: 'Category',
      dataIndex: "category",
      key: "category",
      width: "180px",
      render: (value, _, __) => <Text>{value?.name}</Text>
    },
    {
      title: 'Actions',
      key: 'actions',
      width: "120px",
      render: (_, record) => (
        <Button onClick={() => handleMoreEventInfo(record)}>More</Button>
      ),
    },
  ];

  const handleDateChange = (dates: any | null) => {
    if (dates != null) {
      setDateStart(dates[0])
      setDateEnd(dates[1])
    }
    else {
      setDateStart(null);
      setDateEnd(null);
    }
    
  }

  const handleMakeFilter = async () => {
    const params = new URLSearchParams();

    if (dateStart) {
      params.append("DateStart", dateStart.toISOString());
    }
    if (dateEnd) {
      params.append("DateEnd", dateEnd.toISOString());
    }
    if (placeName) {
      params.append("PlaceName", placeName);
    }
    if (categoryName) {
      params.append("CategoryName", categoryName);
    }

    let result = await getRequestData(`/Events/by-filter?${params.toString()}`);
    setEvents(result);
  }

  const handleClearFilter = async () => {
    setCategoryName("");
    setPlaceName("");
    setDateStart(null);
    setDateEnd(null);
    let result = await getRequestData(`/Events?PageNo=${1}&PageSize=${10}`);
    setEvents(result);
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
          <Title level={2} style={{ marginTop: "0px" }}>All Events</Title>
          <Flex align="start" gap={15} vertical>
            <Text style={{ fontSize: "18px", marginLeft: "10px" }}>Filtering options:</Text>
            <Flex gap={20}>
              <RangePicker
                style={{ width: "300px" }}
                defaultValue={[dayjs('2024.09.01', dateFormat), dayjs('2025.01.01', dateFormat)]}
                value={[dateStart, dateEnd]}
                onChange={(dates, _) => handleDateChange(dates)}
                format={dateFormat} />
              <Input style={{ width: "200px" }} placeholder="Enter place name" value={placeName} onChange={(e) => setPlaceName(e.target.value)} />
              <Input style={{ width: "200px" }} placeholder="Enter category name" value={categoryName} onChange={(e) => setCategoryName(e.target.value)} />
            </Flex>
            <Flex gap={20} style={{marginLeft: "10px"}}>
              <Button type="primary" onClick={handleMakeFilter}>Filter</Button>
              <Button type="default" onClick={handleClearFilter}>Clear filter</Button>
            </Flex>
            <Table columns={columns} dataSource={events} />
          </Flex>
        </Flex>
      </Flex>
    </>
  );
};