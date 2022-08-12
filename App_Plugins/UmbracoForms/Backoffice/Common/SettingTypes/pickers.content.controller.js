angular.module("umbraco").controller("UmbracoForms.SettingTypes.Pickers.ContentController",
	function ($scope, $routeParams, dialogService, entityResource, iconHelper) {

	if (!$scope.setting) {
	    $scope.setting = {};
	}


	var val = parseInt($scope.setting.value);
	

	if (!isNaN(val) && angular.isNumber(val)) {
	    //node
	    $scope.showQuery = false;

	    entityResource.getById($scope.setting.value, "Document").then(function (item) {
	        item.icon = iconHelper.convertFromLegacyIcon(item.icon);
	        $scope.node = item;
	    });
	} 

	$scope.openContentPicker = function () {
	    var d = dialogService.treePicker({
	        section: "content",
	        treeAlias: "content",
	        multiPicker: false,
	        callback: populate
	    });
	};


	$scope.clear = function () {
	    $scope.id = undefined;
	    $scope.node = undefined;
	    $scope.setting.value = undefined;
	};

	function populate(item) {
	    $scope.clear();
	    item.icon = iconHelper.convertFromLegacyIcon(item.icon);
	    $scope.node = item;
	    $scope.id = item.id;
	    $scope.setting.value = item.id;
	}

});