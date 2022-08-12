angular.module("umbraco")
.controller("UmbracoForms.Editors.Form.DeleteController",
	function ($scope, formResource, navigationService, treeService) {
	    $scope.delete = function (id) {
	        formResource.deleteByGuid(id).then(function () {

	            treeService.removeNode($scope.currentNode);
	            navigationService.hideNavigation();

	        });

	    };
	    $scope.cancelDelete = function () {
	        navigationService.hideNavigation();
	    };
	});