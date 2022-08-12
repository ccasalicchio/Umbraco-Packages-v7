angular.module("umbraco").controller("UmbracoForms.SettingTypes.Pickers.ContentWithXpathController",
	function ($scope, $routeParams, dialogService, entityResource, iconHelper, utilityService) {

	var umbracoVersion = Umbraco.Sys.ServerVariables.application.version;

	$scope.queryIsVisible = false;
	$scope.helpIsVisible = false;
	$scope.query = "";


	if (!$scope.setting) {
	    $scope.setting = {};
	}

	function init() {

		if(angular.isNumber($scope.setting.value)){

			entityResource.getById($scope.setting.value, "Document").then(function (item) {
				item.icon = iconHelper.convertFromLegacyIcon(item.icon);
				$scope.node = item;
			});

		} else if($scope.setting.value) {

			$scope.queryIsVisible = true;
			$scope.query = $scope.setting.value;

		}

	}

	$scope.openContentPicker = function () {

		var compareOptions = {
			zeroExtend: true
		};

		var versionCompare = utilityService.compareVersions(umbracoVersion, "7.4", compareOptions);

		if(versionCompare === 0 || versionCompare === 1) {

			$scope.treePickerOverlay = {
				view: "treepicker",
				section: "content",
				treeAlias: "content",
				multiPicker: false,
				title: "Where to save",
				subtitle: "Choose location to save this node",
				hideSubmitButton: true,
				show: true,
				submit: function(model) {

					var selectedItem = model.selection[0];
					populate(selectedItem);

					$scope.treePickerOverlay.show = false;
					$scope.treePickerOverlay = null;
				}
			};

		} else {

			var d = dialogService.treePicker({
	        	section: "content",
	        	treeAlias: "content",
	        	multiPicker: false,
	        	callback: populate
	    	});

		}

	};

	$scope.showQuery = function() {
	    $scope.queryIsVisible = true;
	};

	$scope.toggleHelp = function() {
		$scope.helpIsVisible = !$scope.helpIsVisible;
	};

	$scope.setXpath = function() {
	    $scope.setting.value = $scope.query;
	};

	$scope.clear = function () {
	    $scope.id = undefined;
	    $scope.node = undefined;
	    $scope.setting.value = undefined;
		$scope.query = undefined;
		$scope.queryIsVisible = false;
	};

	function populate(item) {
	    $scope.clear();
	    item.icon = iconHelper.convertFromLegacyIcon(item.icon);
	    $scope.node = item;
	    $scope.id = item.id;
	    $scope.setting.value = item.id;
	}

	init();

});
