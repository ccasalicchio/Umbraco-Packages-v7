angular.module("umbraco").controller("UmbracoForms.SettingTypes.DocumentMapperController",
	function ($scope, $routeParams,pickerResource) {

	    if (!$scope.setting.value) {
	       
	    } else {
	        var value = JSON.parse($scope.setting.value);
	        $scope.doctype = value.doctype;
	        $scope.nameField = value.nameField;
	        $scope.nameStaticValue = value.nameStaticValue;
	        $scope.properties = value.properties;
	    }

	    pickerResource.getAllDocumentTypesWithAlias().then(function (response) {
	        $scope.doctypes = response.data;
	    });

	    pickerResource.getAllFields($routeParams.id).then(function (response) {
	        $scope.fields = response.data;
	    });

	    $scope.setDocType = function() {

	        pickerResource.getAllProperties($scope.doctype).then(function (response) {
	            $scope.properties = response.data;
	        });
	    };

	    $scope.setValue = function() {
	       
	        var val = {};
	        val.doctype = $scope.doctype;
	        val.nameField = $scope.nameField;
	        val.nameStaticValue = $scope.nameStaticValue;
	        val.properties = $scope.properties;

	        $scope.setting.value = JSON.stringify(val);
	    };
	});