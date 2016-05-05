// ---------------------- HomeController.js ----------------------
hbAdmApp.controller('DashboardController', function ($scope, $timeout, $location, dataProviderService, authService, $route, Hub, $filter) {
   
    //alert('dashboad : ' + $location.path());
    $scope.responseMessage = "";
    $scope.isActionSuccess = true;

    $scope.authentication = authService.authentication;
    console.log($scope.authentication);
    if ($scope.authentication.isAuth == false)
        $location.path("/login");

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    }

    //-------------------Left Menu-------------------
    var runTime = new Date().getTime();
    $scope.templateList = [
    { name: 'Approve', color: 'orange', icon: 'fa-check-square-o', url: '../app/views/home.html?' + runTime },
    { name: 'Member List', color: 'green', icon: 'fa-users', url: '../app/views/memberList.html?' + runTime }
    ];

    //Default on Approve menu
    $scope.templateUrl = $scope.templateList[0].url;
    $scope.isActive = $scope.templateList[0].name;

    //On menu selected
    $scope.selectedTab = function (currentPage) {
        $scope.templateUrl = currentPage.url;
        $scope.isActive = currentPage.name;
    };
    //-------------------//Left Menu-------------------
    
    var url = "api/RequestMember/";
    dataProviderService.httpGet(url, function (result) {
       
        $scope.requestMember = result.data;
    });

    $scope.registerStatusFilter = function (member) {
        return (member.registerStatus=='Wait');
    }

    $scope.approvedStatusFilter = function (member) {
        return (member.registerStatus != 'Wait');
    }

    $scope.approve = function (member) {
        
        member.registerStatus = "Approved";
       
        dataProviderService.httpPut(url, member, function (result) {
            if (result.status) {               
                $scope.isActionSuccess = true;
                $scope.responseMessage = "Approve successfully.";
                startTimer();
            } else {
                $scope.isActionSuccess = false;
            }
        });
    };

    $scope.reject = function (member) {
        member.registerStatus = "Rejected";
        dataProviderService.httpPut(url, member, function (result) {
            if (result.status) {
                $scope.isActionSuccess = true;
                $scope.responseMessage = "Reject successfully.";
                startTimer();
            } else {
                $scope.isActionSuccess = false;
            }
        });
    };

    $scope.delete = function (member) {
        var url = "api/RequestMember/?email=" + member.email;
        dataProviderService.httpDelete(url, function (result) {
            if (result.status) {
                var url = "api/RequestMember/";
                dataProviderService.httpGet(url, function (result) {

                    $scope.requestMember = result.data;
                });
                $scope.isActionSuccess = true;
                $scope.responseMessage = "Delete successfully.";
                startTimer();
            } else {
                $scope.isActionSuccess = false;
            }
        });
    };


    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $scope.responseMessage = "";
        }, 3000);
    }

  
    //Hub setup
    var hub = new Hub('healthbook', {
        listeners: {          
            'hasNewMember': function (id) {
                var url = "api/RequestMember/";
                dataProviderService.httpGet(url, function (result) {
                    $scope.requestMember = result.data;
                    $scope.$apply();
                });
            }
        },
        methods: ['hBNotify'],
        errorHandler: function (error) {
            console.error(error);
        }
    });

    $scope.testHub = function () {
        alert("test hub");
        hub.hBNotify(1);
        alert("end test hub");
    };
});