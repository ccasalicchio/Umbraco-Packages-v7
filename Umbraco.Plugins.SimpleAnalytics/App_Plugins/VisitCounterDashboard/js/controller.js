angular.module("umbraco").controller("splatDev.SimpleAnalytics.Controller", function ($scope, $http, $routeParams) {
    'use strict';
    const vm = this;
    const apiUrl = '/umbraco/Analytics/AnalyticsApi/';
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