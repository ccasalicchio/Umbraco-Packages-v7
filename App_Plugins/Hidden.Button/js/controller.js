angular.module("umbraco")
.controller("Hidden.Button",
  function ($scope) {
    'use strict';
    var checked = 'document-dashed-line', 
    unchecked = 'document';
    var engine = {
        checked : false,
        change : null,
        icon : unchecked
    };

    if ($scope.model.value) {
        if ($scope.model.value !== "") {
            engine.checked = ParseToBool($scope.model.value);
        }
    }
    function ParseToBool(intValue) {
        if(intValue==="0"){
            engine.icon = unchecked;
            return false;
        }
        if(intValue==="1"){
            engine.icon = checked;
            return true;
        }
    }
    function ParseToInt(boolValue) {
        if(boolValue===false){
            engine.icon = unchecked;
            return 0;
        }
        if(boolValue===true){
            engine.icon = checked;
            return 1;
        }
    }
    engine.change=function(){
        $scope.model.value = ParseToInt(engine.checked);
    };
      //default unchecked
    $scope.model.value = 0;
    $scope.engine = engine;
});