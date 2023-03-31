angular.module("umbraco").controller("splatdev.HideContent.Controller", function ($scope) {
    'use strict';
    let checked = 'document-dashed-line',
        unchecked = 'document';
    const vm = this;
    vm.config = {
        checked: false,
        icon: unchecked
    };

    function update(intValue) {
        vm.icon = intValue ? checked : unchecked;

        return intValue ? true : false;
    }

    vm.change = function () {
        $scope.model.value = +vm.config.checked;
    };

    function init() {
        if ($scope.model.value !== "") {
            vm.config.checked = update(+$scope.model.value);
        }
        else {
            vm.config.checked = false;
            //default unchecked
            $scope.model.value = 0;
            update($scope.model.value);

        }
    }

    init();

});