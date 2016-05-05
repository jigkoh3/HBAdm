// ---------------------- HomeController.js ----------------------
hbAdmApp.controller('HomeController', function ($scope, $timeout, $location, dataProviderService, authService, $route) {
    //$scope.isLoading = true;
    $scope.responseMessage = "";
    $scope.isActionSuccess = true;

    $scope.imageSource = "../Photo/default.jpg";

    $scope.authentication = authService.authentication;
    console.log($scope.authentication);
    if ($scope.authentication.isAuth == false)
        $location.path("/login");

    var url = "api/RequestMember/";
    dataProviderService.httpGet(url, function (result) {
        $scope.requestMember = result.data;
    });

    $scope.approve = function (member) {
        $scope.isLoading = true;

        member.registerStatus = "APPROVED";
        dataProviderService.httpPut(url, member, function (result) {
            if (result.status) {
                //$scope.isLoading = false;
                $scope.isActionSuccess = true;
                $scope.responseMessage = "Approve successfully.";
                
                $route.reload();

                //startTimer();
            } else {
                $scope.isActionSuccess = false;
                //$scope.isLoading = false;
            }
        });
    };

    $scope.reject = function (member) {
        $scope.isLoading = true;
        member.registerStatus = "REJECTED";
        dataProviderService.httpPut(url, member, function (result) {
            if (result.status) {
                //$scope.isLoading = false;
                $scope.isActionSuccess = true;
                $scope.responseMessage = "Reject successfully.";
                $route.reload();
                //startTimer();
            } else {
                $scope.isActionSuccess = false;
                //$scope.isLoading = false;
            }
        });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $scope.responseMessage = "";
            //$location.path('/home');
        }, 3000);
    }
});