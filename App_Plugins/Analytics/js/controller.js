'use strict';
angular.module("umbraco")
    .controller("Analytics",
    function ($scope, $http, $routeParams) {
        $scope.visits = 0;
        $scope.visitLog = {};
        $scope.nodeId = $routeParams.id;
        $scope.loading = true;
        $http.get("/Umbraco/Api/AnalyticsApi/GetVisitCount?nodeId=" + $scope.nodeId).then(function (response) {
            $scope.visits = response.data;
            $scope.loading = false;
        },
            function (data, status) {
                console.error('Api Error', status, data);
                $scope.loading = false;
            });
        $http.get("/Umbraco/Api/AnalyticsApi/GetVisits?nodeId=" + $scope.nodeId).then(function (response) {
            $scope.visitLog = response.data;
            $scope.loading = false;
        },
            function (data, status) {
                console.error('Api Error', status, data);
                $scope.loading = false;
            });
    });