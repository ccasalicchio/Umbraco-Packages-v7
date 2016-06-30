'use strict';
//based off https://proto.io/freebies/onoff/
angular.module("umbraco")
.controller("On.Off.Button",
    function ($scope) {
        var engine = {
            checked:true,
            change:null
        }

        if ($scope.model.value) {
            if ($scope.model.value !== "") {
                engine.checked = ParseToBool($scope.model.value);
            }
        }
        function ParseToBool(intValue) {
          if(intValue==="0")return false;
          if(intValue==="1")return true;
      }
      function ParseToInt(boolValue) {
          if(boolValue===false)return 0;
          if(boolValue===true)return 1;
      }
      engine.change=function(){
          $scope.model.value = ParseToInt(engine.checked);
      }
      $scope.engine = engine;
  });