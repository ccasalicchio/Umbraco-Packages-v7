angular.module("umbraco").controller("splatDev.CharLimitRestrict.Controller", function ($scope) {
    'use strict';
    const vm = this;
    const thumbUp = "thumb-up", alert = "alert", warning = "stop-hand";
    vm.characters = '';
    vm.limit = 0;
    vm.counter = 0;
    vm.countLeft = true;
    vm.maxReached = false;
    vm.icon = thumbUp;

    vm.limitChars = function () {
        vm.counter = vm.limit - vm.characters.length;
        CheckChars();
    };

    function CheckChars() {
        if (vm.characters.length === 0) {
            vm.countLeft = true;
            vm.maxReached = false;
            vm.icon = thumbUp;
        }

        else if (vm.characters.length >= vm.limit) {
            vm.maxReached = true;
            vm.countLeft = false;
            vm.icon = warning;
            vm.model.value = vm.characters.substr(0, vm.limit);
        }

        else if (vm.characters.length < (vm.limit / 2)) {
            vm.icon = thumbUp;
            vm.maxReached = false;
            vm.countLeft = true;
        }

        else if (vm.characters.length > (vm.limit / 2)) {
            vm.icon = alert;
            vm.maxReached = false;
            vm.countLeft = true;
        }

        else {
            vm.icon = alert;
            vm.maxReached = false;
            vm.countLeft = true;
        }
    }

    function init() {
        vm.limit = +$scope.model.config.limit;
        vm.characters = $scope.model.value;
        vm.counter = vm.limit - vm.characters.length;
        CheckChars();
    }

    init()
});