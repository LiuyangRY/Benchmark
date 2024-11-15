import http from "k6/http";

export const options = {
    dns: {
        "ttl": "10m",
        "select": "first",
        "policy": "any"
    },
    insecureSkipTLSVerify: true,
    stages: [
        { duration: "1m", target: 200 },
        { duration: "2m", target: 400 },
    ]
};

// 立即下单压力测试
export default function () {
    const url = "https://192.168.190.30:34643/api/usecase/order/submit";
    const params = {
        headers: {
            "Authorization": "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkQ4NjdGNzEwMEM1OENDRDFBNUUzMzVFNEEzN0RGNTUwIiwidHlwIjoiSldUIn0.eyJuYmYiOjE3MzAzNTQyNDYsImV4cCI6MTczMTA3NDI0NiwiaXNzIjoiaHR0cDovL3d3dy5oeXN5eWwuY29tIiwiYXVkIjpbIkZvck91dFN5c3RlbUFwaSIsIkhZUy5TQy5BZG1pbkFwaSIsIkhZUy5TQy5BcGkiLCJIeXNCYWNrZ3JvdW5kU2VydmljZS5BcGkiLCJIeXNIeXl4VGVsZXNhbGVzLkFwaSIsIkh5c01hbGwuQ2xpZW50LkFQSSIsIkh5c01hbGwuU3lzTWFuYWdlbWVudC5BUEkiLCJaSlMuSU0uQ2xpZW50LkFwcEFwaSIsImh5cy5pbS5hcGkiLCJwZXJtaXNzaW9uLmFwaSIsInlzYWxlci5jbGllbnQuYXBpIiwiemJiLmFkbWluLmFwaSIsInpiYi5jbGllbnQuYXBpIiwiempzLm9zcy5hcGkiXSwiY2xpZW50X2lkIjoiWkJCLlN5c3RlbSIsInN1YiI6IjcyMjQ1ODc1MTQ3NDMzNTc1MDEiLCJhdXRoX3RpbWUiOjE3MzAzNTQyNDYsImlkcCI6ImxvY2FsIiwibmFtZSI6IueOi-S4uSIsIm5pY2tuYW1lIjoi546L5Li5IiwidGltZXN0YW1wIjoiNjM4NjU5Nzk4NDYyNDk4OTAwIiwibG9naW5pZCI6IjE5MjdhOTEyLTI4YzgtNDA3MS04ZWZkLTM0NDRhZjdiZTIyNCIsInJvbGUiOiJEb2N0b3IiLCJDdXN0b21lcklkIjoiNzIyNDU4NzUxOTY0MjMwNDU4NSIsIkRvY3RvcklkIjoiNzIyNDY5MjU1MjAwNjQwMjE2MiIsIkRvY3Rvck5hbWUiOiLnjovkuLnpl6jlupfljLvnlJ8iLCJDdXN0b21lck5hbWUiOiIiLCJEaXZpc2lvbkNvZGUiOiIiLCJCdXNpbmVzc0xpbmVJZCI6IjY5MDAwMDAwMDAwMDAwMDAwMDAiLCJTYWxlc21hbklkIjoiNzE5OTYwNTc1OTkxMzQ5MjUwNSIsInNpZ24iOiIwIiwianRpIjoiM0E5RjQyQzZEMUQyRUJBOUI4Rjk2Q0M4QTYyODgzRkMiLCJpYXQiOjE3MzAzNTQyNDYsInNjb3BlIjoiRm9yT3V0U3lzdGVtQXBpIEhZUy5TQy5BZG1pbkFwaSBIWVMuU0MuQXBpIEh5c0JhY2tncm91bmRTZXJ2aWNlLkFwaSBIeXNIeXl4VGVsZXNhbGVzLkFwaSBIeXNNYWxsLkNsaWVudC5BUEkgSHlzTWFsbC5TeXNNYW5hZ2VtZW50LkFQSSBaSlMuSU0uQ2xpZW50LkFwcEFwaSBoeXMuaW0uYXBpIG9wZW5pZCBwZXJtaXNzaW9uLmFwaSBwcm9maWxlIHlzYWxlci5jbGllbnQuYXBpIHpiYi5hZG1pbi5hcGkgemJiLmNsaWVudC5hcGkgempzLm9zcy5hcGkgb2ZmbGluZV9hY2Nlc3MiLCJhbXIiOlsiY3VzdG9tIl19.HSa3uc-F_tsCcW-dXQ0OYc7oDwJ3AlUgxzr_8-W8HXuDliN8ca7z2_oNTfiCMUdkSI9dL4sLkSRRZHfc4jMc1WTNU309DDjvGm5xBOmlK19VpVyvNujOgXZdKXcaYN8hUYwQ-JvdOYUI72uxi2gDwtDmOzLe9-molc_kcscmlqaAPY_WJdnPWrNDEC7ppxBPjETxGOMwEil_M-aRJhUifFWcTE9DqYjRbAgxjuyWLEX6D-0qDosa0FfiZFWF8FtTn11UwKGaolx8QZWYaJwlMjCzGKZAVoUlCwsgFh2ekeK4JtzFmjpDwXpX6_yeERKPeNlKql5Vbg-UUj5dPTG0Qw",
            "User-Agent": "Apifox/1.0.0 (https://apifox.com)",
            "Content-Type": "application/json",
            "Accept": "*/*",
            "Host": "192.168.190.30:34643",
            "Connection": "keep-alive"
        }
    };
    const payload = JSON.stringify({
        "patient": {
            "patientId": null,
            "name": "测试患者",
            "sex": 1,
            "age": 20,
            "phone": "11111111111",
            "id": "7257285075697565754",
            "idCard": "",
            "doctorId": "7224692552006402162",
            "creationTime": "0001-01-01 00:00:00",
            "previousHistory": "",
            "allergyHistory": "",
            "createTime": "2024-10-30 14:59:55",
            "height": null,
            "weight": null,
            "doctorName": "王丹门店医生"
        },
        "prescriptionInfo": {
            "description": "",
            "diagnosis": "3日35435",
            "doctorSAdvice": "",
            "totalDose": 1,
            "dailyDose": 1,
            "perDose": 1,
            "isEnabled": true
        },
        "commodityList": [
            {
                "id": "7219887293592846450",
                "count": 1,
                "herbsGramWeight": 40
            },
            {
                "id": "7224592533832212526",
                "count": 1,
                "herbsGramWeight": 20
            }
        ],
        "receivingInfo": {
            "id": "7224692552006402163",
            "isDefault": true,
            "name": "王丹门店医生",
            "phone": "15285016537",
            "areaFullName": "四川省/成都市/青羊区/",
            "address": "测试收货地址",
            "areaId": 1023007066000000,
            "areaCode": "10510105",
            "freightAmount": 10,
            "freePostagePrice": 0.01,
            "startingPrice": 0.01,
            "logisticsId": "7219643726098018308"
        },
        "orderActualAmount": 13
    });

    http.post(url, payload, params);
};


