angular.module("umbraco").controller("splatDev.OnOffButton.Controller", function ($scope) {
    //based off https://proto.io/freebies/onoff/
    'use strict';
    const vm = this;
    vm.checked = false;

    vm.change = function () {
        $scope.model.value = update(vm.checked);
    }
    function update(intValue) {
        return intValue ? 1 : 0;
    }

    function init() {
        if ($scope.model.value !== "") {
            vm.checked = $scope.model.value === "1";
        }
        $scope.model.value = 0;
    }

    init();
});