hbAdmApp.service("dataProviderService", function ($http) {
    var serviceBase = '';

    this.httpGet = function (url, fnCallback) {
        $http({
            method: "GET",
            url: serviceBase + url,
            timeout: 30000
        }).success(function (resultData) {
            console.log("get success");
            console.log(resultData);
            fnCallback({
                status: true,
                data: resultData,
                error: "",
                msgErr: ""
            });
        }).error(function (data, status) {
            fnCallback({
                status: false,
                data: [],
                error: status,
                msgErr: data.Message
            });
        });
    };

    this.httpPost = function (url, data, fnCallback) {
        $http.post(serviceBase + url, data)
            .success(function (resultData) {
                console.log(resultData);
                fnCallback({
                    status: true,
                    data: resultData,
                    error: "",
                    msgErr: ""
                });
            }).error(function (data, status) {
                console.log("ERROR");
                console.log(data, status);
                fnCallback({
                    status: false,
                    data: [],
                    error: status,
                    msgErr: data.Message
                });
            });
    };

    this.httpPut = function (url, data, fnCallback) {
        $http.put(serviceBase + url, data)
            .success(function (resultData) {
                console.log("put success");
                console.log(resultData);
                fnCallback({
                    status: true,
                    data: resultData,
                    error: "",
                    msgErr: ""
                });
            }).error(function (data, status) {
                console.log("ERROR");
                console.log(data, status);
                fnCallback({
                    status: false,
                    data: [],
                    error: status,
                    msgErr: data.Message
                });
            });
    };

    this.httpDeleteByUpdate = function (url, data, fnCallback) {
        $http.put(serviceBase + url, data)
            .success(function (resultData) {
                console.log(resultData);
                fnCallback({
                    status: true,
                    data: resultData,
                    error: "",
                    msgErr: ""
                });
            }).error(function (data, status) {
                console.log("ERROR");
                console.log(data, status);
                fnCallback({
                    status: false,
                    data: [],
                    error: status,
                    msgErr: data.Message
                });
            });
    };

    this.httpDelete = function (url, fnCallback) {
        $http.delete(serviceBase + url)
            .success(function (resultData) {
                console.log(resultData);
                fnCallback({
                    status: true,
                    data: resultData,
                    error: "",
                    msgErr: ""
                });
            }).error(function (data, status) {
                console.log("ERROR");
                console.log(data, status);
                fnCallback({
                    status: false,
                    data: [],
                    error: status,
                    msgErr: data.Message
                });
            });
    };

    this.httpGetById = function (url, param, fnCallback) {
        $http({
            method: "GET",
            url: serviceBase + url,
            params: param,
            timeout: 30000
        }).success(function (resultData) {
            fnCallback({
                status: true,
                data: resultData,
                error: "",
                msgErr: ""
            });
        }).error(function (data, status) {
            fnCallback({
                status: false,
                data: [],
                error: status,
                msgErr: data.Message
            });
        });
    };

});