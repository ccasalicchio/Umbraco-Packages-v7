angular.module("umbraco")
.controller("Default.Value",
  function ($scope) {
    'use strict';
    var engine = {
        dValue:''
    };

    engine.dValue = $scope.model.config.dValue;

    $scope.engine = engine;
});