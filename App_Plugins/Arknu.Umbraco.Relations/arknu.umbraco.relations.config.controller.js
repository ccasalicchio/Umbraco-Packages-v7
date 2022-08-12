angular.module("umbraco")
    .controller("Arknu.Umbraco.Relations.Config.Controller", function ($scope, arknuRelationsResource, editorState) {
        $scope.model.relationTypes = [];
        arknuRelationsResource.getRelationTypes(editorState.current.id).then(function (response) {
            $scope.model.relationTypes = response.data;
        });
    });