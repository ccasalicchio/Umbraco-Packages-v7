angular.module("umbraco").controller("TagManager.TagManagerEditController",
	function ($scope, $routeParams, TagManagerResource, notificationsService) {
	   
	    //get a person id -> service
	    TagManagerResource.getById($routeParams.id).then(function (response) {
	        $scope.cmsTags = response.data;
	        $scope.selectedTag = $routeParams.id;
	    });

	    $scope.save = function (cmsTags) {
	        TagManagerResource.save(cmsTags).then(function (response) {
	            $scope.cmsTags = response.data;
	            notificationsService.success("Success", cmsTags.tag + " has been saved");
	        });
	    };

	    $scope.deleteTag = function (cmsTags) {
	        TagManagerResource.deleteTag(cmsTags).then(function (response) {
	            $scope.cmsTags = response.data;
	            notificationsService.success("Success", cmsTags.tag + " has been deleted.");
	            treeService.removeNode($scope.currentNode);
	        });
	    };
	});