angular.module("umbraco").controller("splatDev.VisitCounter.Controller", function ($scope) {
    'use strict';
    const vm = this;
    vm.visitCounter = 0;

    if ($scope.model.value !== "") {
        vm.visitCounter = $scope.model.value;
    }

});