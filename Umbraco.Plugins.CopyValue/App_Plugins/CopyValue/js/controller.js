angular.module("umbraco").controller("splatDev.CopyValue.Controller", function ($scope, contentEditingHelper, editorState) {
    'use strict';
    const vm = this;
    vm.from = $scope.model.config.from.split(",");
    vm.to = $scope.model.config.to.split(",")

    let content = editorState.current;
    let properties = contentEditingHelper.getAllProps(content);
    vm.done = false;

    vm.copy = function () {
        for (var i = 0; i < vm.from.length; i++) {
            _.findWhere(properties, { alias: vm.to[i] }).value = _.findWhere(properties, { alias: vm.from[i] }).value;
        }

        vm.done = true;
        setTimeout(function () {
            vm.done = false;
        }, 3 * 1000);
    };

});