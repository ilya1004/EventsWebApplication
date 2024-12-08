import { message } from "antd";

export const handleResponseError = (response : Response) => {
  if (response.status === 401) {
    console.error(response);
    showMessageStc("Произошла ошибка аутентификации", "error");
  } else if (response.status === 403) {
    console.error(response);
    showMessageStc("Произошла ошибка авторизации", "error");
  } else if (response.status === 500) {
    console.error(response);
    showMessageStc("Произошла ошибка на сервере", "error");
  } else {
    console.error(response);
    showMessageStc("Произошла неизвестная ошибка", "error");
  }
};

export const showMessageStc = (msg : string, msgType : string) => {
  message.config({
    top: 40,
    duration: 3,
    maxCount: 1,
  });
  if (msgType === "error") {
    message.error(msg);
  } else if (msgType === "success") {
    message.success(msg);
  } else {
    message.info(msg);
  }
};