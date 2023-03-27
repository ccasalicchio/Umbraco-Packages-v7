angular.module("umbraco").controller("splatDev.DefaultValue.Controller",function ($scope) {
    'use strict';

    const vm = this;
    vm.dValue = null;

    vm.dValue = $scope.model.config.dValue;
    $scope.model.value = vm.dValue;

});