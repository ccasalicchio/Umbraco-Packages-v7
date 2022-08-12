
angular.module("umbraco")
.controller("YouTube.Editor",
    function ($scope) {
        'use strict';

        var engine = {
            videoId :'',
            update : null
        };

        if ($scope.model.value) {
            if ($scope.model.value !== "") {
                engine.videoId = $scope.model.value;
            }
            else
                engine.videoId = '';
        }

        engine.update=function(){
            $scope.model.value = engine.videoId;
        };
        $scope.engine = engine;
    });