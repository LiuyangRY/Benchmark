import http from "k6/http";

export const options = {
    dns: {
        "ttl": "10m",
        "select": "first",
        "policy": "any"
    },
    insecureSkipTLSVerify: true,
    stages: [
        { duration: "1m", target: 10, rps: 10 },
        { duration: "2m", target: 20, rps: 20 },
    ]
};

const config = {
    isLocal: true,
    localHost: "http://localhost:61944",
    developHost: "https://192.168.190.30:34643",
    url: "/api/query/order/paging/infos",
    authorization: "eyJhbGciOiJSUzI1NiIsImtpZCI6IkQ4NjdGNzEwMEM1OENDRDFBNUUzMzVFNEEzN0RGNTUwIiwidHlwIjoiSldUIn0.eyJuYmYiOjE3MzEzNzkzNDEsImV4cCI6MTczMjA5OTM0MSwiaXNzIjoiaHR0cDovL3d3dy5oeXN5eWwuY29tIiwiYXVkIjpbIkZvck91dFN5c3RlbUFwaSIsIkhZUy5TQy5BZG1pbkFwaSIsIkhZUy5TQy5BcGkiLCJIeXNCYWNrZ3JvdW5kU2VydmljZS5BcGkiLCJIeXNIeXl4VGVsZXNhbGVzLkFwaSIsIkh5c01hbGwuQ2xpZW50LkFQSSIsIkh5c01hbGwuU3lzTWFuYWdlbWVudC5BUEkiLCJaSlMuSU0uQ2xpZW50LkFwcEFwaSIsImh5cy5pbS5hcGkiLCJwZXJtaXNzaW9uLmFwaSIsInlzYWxlci5jbGllbnQuYXBpIiwiemJiLmFkbWluLmFwaSIsInpiYi5jbGllbnQuYXBpIiwiempzLm9zcy5hcGkiXSwiY2xpZW50X2lkIjoiWkJCLlN5c3RlbSIsInN1YiI6IjcyMjQ1ODcyNjgyMDYzNjI3MTYiLCJhdXRoX3RpbWUiOjE3MzEzNzkzNDEsImlkcCI6ImxvY2FsIiwibmFtZSI6IuWRqOWAqSIsIm5pY2tuYW1lIjoi5ZGo5YCpIiwidGltZXN0YW1wIjoiNjM4NjcwMDQ5NDEyMTMxMjU0IiwibG9naW5pZCI6ImYyNTE5M2ViLWQ4NmEtNGY1My04YWQ5LWJlNDhlMjMzYzRkOCIsInJvbGUiOiJEb2N0b3IiLCJDdXN0b21lcklkIjoiNzIyNDU4NzI3MzE1OTgzNTY1NCIsIkRvY3RvcklkIjoiNzIyNDY5MTUwMzE5OTcxNTM1OSIsIkRvY3Rvck5hbWUiOiLlkajlgKnpl6jlupfljLvnlJ8iLCJDdXN0b21lck5hbWUiOiLmr5XljZror5rnvZHnu5zmnInpmZDlhazlj7gxNzIyNDc1ODI5NjM4IiwiRGl2aXNpb25Db2RlIjoiNjk3MDIwMjEwNDk3NTcyMDQ0OCIsIkJ1c2luZXNzTGluZUlkIjoiNjkwMDAwMDAwMDAwMDAwMDAwMCIsInNpZ24iOiIwIiwianRpIjoiQkJCMTczQjk0OEVGNzZCRTY1Njk4OTg5MDM3QzU2MzUiLCJpYXQiOjE3MzEzNzkzNDEsInNjb3BlIjoiRm9yT3V0U3lzdGVtQXBpIEhZUy5TQy5BZG1pbkFwaSBIWVMuU0MuQXBpIEh5c0JhY2tncm91bmRTZXJ2aWNlLkFwaSBIeXNIeXl4VGVsZXNhbGVzLkFwaSBIeXNNYWxsLkNsaWVudC5BUEkgSHlzTWFsbC5TeXNNYW5hZ2VtZW50LkFQSSBaSlMuSU0uQ2xpZW50LkFwcEFwaSBoeXMuaW0uYXBpIG9wZW5pZCBwZXJtaXNzaW9uLmFwaSBwcm9maWxlIHlzYWxlci5jbGllbnQuYXBpIHpiYi5hZG1pbi5hcGkgemJiLmNsaWVudC5hcGkgempzLm9zcy5hcGkgb2ZmbGluZV9hY2Nlc3MiLCJhbXIiOlsiY3VzdG9tIl19.KxvCSUja11J11xfU4xmFHo6qW9tDKKIhmkJNkf3vCZVJOiV9-KX-tmb91tTCqz_onmo3e5F_lLP83DxLFRm1H_r_iUVtVs3I1ElJvLLMiOiMAmfCRObhYdRnFVdvvOu0AvTXGw_5r29HBrpUvST4K5yriYOdPUUt2eSBOPobsIVo1bE6bQ6eZR5myxnzrrJJgIqZcd9RMX1ncY0yUgJrEjW1NGIotqbZSLdM91QE4gSP9JQKYB5OhqYoqWY9DyrI6kEV7uMOQeyZyySYA9PP0tvLhwuR5njchLdY6CNjY1OkIKQQbVztARYkG-gdbKaD944suNWcJzZ3ERQWjnYfRQ",
}

const parameter = {
    orderStatus: 0,
    pageIndex: 1,
    pageSize: 10,
}

const queryString = Object.keys(parameter)
   .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(parameter[key])}`)
   .join('&');

// 查询订单压力测试
export default function () {
    const url = `${config.isLocal ? config.localHost : config.developHost}${config.url}?${queryString}`;
    const headers = {
        "Authorization": `Bearer ${config.authorization}`,
    };
    http.get(url, { headers: headers});
};
