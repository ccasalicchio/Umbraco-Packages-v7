'use strict';
angular.module("umbraco")
.controller("VisitCounter",
    function ($scope,  contentEditingHelper, editorState) {
        var engine = {
            visitCounter:0
        }
        var content = editorState.current;
        var properties = contentEditingHelper.getAllProps(content);
        var srv = _.findWhere(properties, { alias: "visitCounter" }).value;

        if (srv >= 0) 
            engine.visitCounter = srv;
        if(srv == "") 
            engine.visitCounter=0;


        $scope.engine = engine;
    });