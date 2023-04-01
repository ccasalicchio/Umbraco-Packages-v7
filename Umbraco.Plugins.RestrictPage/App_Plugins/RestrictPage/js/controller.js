angular.module("umbraco").controller("splatDev.RestrictPage.Controller", function ($scope) {
    'use strict';
    const vm = this;
    vm.checkedIcon = 'lock',
        vm.uncheckedIcon = 'unlocked';
    vm.config = {
        checked: false,
        icon: vm.uncheckedIcon
    };

    function update(intValue) {
        vm.config.icon = intValue ? vm.checkedIcon : vm.uncheckedIcon;
        return intValue ? true : false;
    }

    vm.change = function () {
        $scope.model.value = +vm.config.checked;
        update(vm.config.checked);
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