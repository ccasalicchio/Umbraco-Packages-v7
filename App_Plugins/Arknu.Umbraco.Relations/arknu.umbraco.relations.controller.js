angular.module("umbraco")
    .controller("Arknu.Umbraco.Relations.Controller", function ($scope, arknuRelationsResource, editorState, dialogService) {
        var type = $scope.model.config.relationTypeAlias;

        arknuRelationsResource.getRelations(editorState.current.id, type).then(function (response) {
            $scope.model.items = response.data;
        });

        var dialogOptions = {
            multiPicker: true,
            entityType: "content",
            filterCssClass: "not-allowed not-published",
            startNodeId: null,
            callback: function (data) {
                if (angular.isArray(data)) {
                    _.each(data, function (item, i) {
                        $scope.add(item);
                    });
                } else {
                    $scope.clear();
                    $scope.add(data);
                }
            },
            treeAlias: "content",
            section: "content"
        };

        //dialog
        $scope.openContentPicker = function () {
            var d = dialogService.treePicker(dialogOptions);
        };

        $scope.remove = function (index) {
            var item = $scope.model.items[index];
            arknuRelationsResource.deleteRelation(item.relationId).then(function (response) {
                $scope.model.items.splice(index, 1);
            });
            
        };

        $scope.add = function (item) {
            arknuRelationsResource.saveRelation(editorState.current.id, item.id, type).then(function (response) {
                $scope.model.items.push(response.data); 
            });
            
            
        };
    });