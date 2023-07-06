import { LoadingOutlined } from "@ant-design/icons";
import { notification } from "antd";
import NotificationStatusTypes from "../Enums/NotificationStatusTypes";

export const openNotification = (status, header, description) => {
    if (status == NotificationStatusTypes.Success)
        notification["success"]({
            message: header,
            description: description,
        });
    else if (status == NotificationStatusTypes.Error)
        notification["error"]({
            message: header,
            description: description,
        });
    else if (status == NotificationStatusTypes.Warning)
        notification["warning"]({
            message: header,
            description: description,
        });
}

export const loadingIcon = <LoadingOutlined className="loading-icon" spin />;