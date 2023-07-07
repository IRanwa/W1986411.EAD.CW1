import NotificationStatusTypes from "../Enums/NotificationStatusTypes";
import { openNotification } from "./CommonUI";

const basicURL = "";

export const CommonPost = (url, parameters, formData) => {
    //const encryptResult = EncryptContent(JSON.stringify(formData));
    const encryptResult = JSON.stringify(formData);
    return new Promise((resolve, reject) => {
        let request = new XMLHttpRequest();
        if (parameters !== null) {
            request.open("POST", basicURL + url + "?" + parameters);
        } else {
            request.open("POST", basicURL + url);
        }
        const token = localStorage.getItem("token");

        request.setRequestHeader("Content-type", "application/json; charset=utf-8");
        if (token !== null && token !== "null" && token !== undefined)
            request.setRequestHeader("Authorization", "Bearer " + token);
        request.setRequestHeader("Access-Control-Allow-Methods", '*');
        request.setRequestHeader("Access-Control-Allow-Origin", '*');
        request.setRequestHeader("Accept-Language", "en-US");
        request.onload = () => {
            if (request.status === 401) {
                localStorage.setItem("token", null);
                //InformAuthenticatedChange();
                window.location.href = "/";
                openNotification(NotificationStatusTypes.Error, "", "User session expired.")
                reject(request.statusText);
            } else {
                if (request.status >= 200 & request.status < 300) {
                    //resolve(JSON.parse(DecryptContent(request.response)));
                    resolve(JSON.parse(request.response));
                } else {
                    reject(request.statusText);
                }
            }
        };
        request.onerror = () => {
            console.error("status Text ", request);
            reject(request.statusText);
        };
        request.send(encryptResult);
    });
}

export const CommonGet = (url, parameters) => {
    return new Promise((resolve, reject) => {
        let request = new XMLHttpRequest();
        if (parameters !== null) {
            request.open("GET", basicURL + url + "?" + parameters);
        } else {
            request.open("GET", basicURL + url);
        }
        const token = localStorage.getItem("token");

        request.setRequestHeader("Content-type", "application/json; charset=utf-8");
        if (token !== null && token !== "null" && token !== undefined)
            request.setRequestHeader("Authorization", "Bearer " + token);
        request.setRequestHeader("Access-Control-Allow-Methods", '*');
        request.setRequestHeader("Access-Control-Allow-Origin", '*');
        request.setRequestHeader("Accept-Language", "en-US");
        request.onload = () => {
            if (request.status === 401) {
                localStorage.setItem("token", null);
                //InformAuthenticatedChange();
                window.location.href = "/";
                openNotification(NotificationStatusTypes.Error, "", "User session expired.")
                reject(request.statusText);
            } else {
                if (request.status >= 200 & request.status < 300) {
                    //resolve(JSON.parse(DecryptContent(request.response)));
                    resolve(JSON.parse(request.response));
                } else {
                    reject(request.statusText);
                }
            }
        };
        request.onerror = () => {
            console.error("status Text ", request);
            reject(request.statusText);
        };
        request.send();
    });
}