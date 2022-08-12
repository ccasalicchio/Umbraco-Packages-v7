'use strict';
angular.module("umbraco")
.controller("VisitCounter",
    function ($scope) {
        var engine = {
            visitCounter:0
        }
        
        if ($scope.model.value !== "") {
            engine.visitCounter = $scope.model.value;
        }

        $scope.engine = engine;
    });