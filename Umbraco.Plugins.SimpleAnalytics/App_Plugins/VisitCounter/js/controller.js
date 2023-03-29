angular.module("umbraco").controller("splatDev.VisitCounter.Controller", function ($http, $routeParams) {
    'use strict';
    const vm = this;
    vm.visitCounter = 0;

    function init() {
        $http.get(`/Umbraco/Analytics/AnalyticsApi/GetVisitCount?nodeId=${$routeParams.id}`).then(result => {
            vm.visitCounter = +result.data;
        });
    }

    init();
});