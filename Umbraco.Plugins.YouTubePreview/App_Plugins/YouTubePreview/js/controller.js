angular.module("umbraco").controller("splatDev.YouTubePreview.Controller", function ($scope) {
    'use strict';
    const vm = this;
    vm.videoId = null;

    vm.update = function () {
        $scope.model.value = vm.videoId;
    };

    function init() {
        if ($scope.model.value !== "") {
            engine.videoId = $scope.model.value;
        }
    }

    init()
});