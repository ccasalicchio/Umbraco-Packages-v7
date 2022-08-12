angular.module("umbraco").controller("UmbracoForms.Editors.Form.CreateController", function ($scope, $routeParams, formResource, editorState, notificationsService, utilityService) {
		
		//Use the vm approach as opposed to $scope
		var vm = this;
		vm.editUrl = 'edit';
		
		//Get the current umbraco version we are using
		var umbracoVersion = Umbraco.Sys.ServerVariables.application.version;
		
		var compareOptions = {
			zeroExtend: true
		};
		
		//Check what version of Umbraco we have is greater than 7.4 or not 
		//So we can load old or new editor UI
		var versionCompare = utilityService.compareVersions(umbracoVersion, "7.4", compareOptions);
		
		//If value is 0 then versions are an exact match
		//If 1 then we are greater than 7.4.x
		//If it's -1 then we are less than 7.4.x
		if(versionCompare < 0) {
			//I am less than 7.4 - load the legacy editor
			vm.editUrl = 'edit-legacy';
		}
		
		formResource.getAllTemplates().then(function(response) {
		   vm.formTemplates = response.data;
		});
});