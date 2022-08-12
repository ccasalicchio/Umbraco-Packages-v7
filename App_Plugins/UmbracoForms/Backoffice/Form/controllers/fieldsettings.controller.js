angular.module("umbraco").controller("UmbracoForms.Editors.Form.Dialogs.FieldSettingController",
	function ($scope, formService, dialogService) {

	    $scope.deleteConditionRule = function(rules, rule) {
	        formService.deleteConditionRule(rules, rule);
	    };

	    $scope.addConditionRule = function (condition) {
	        formService.addConditionRule(condition);
	    };

	    $scope.getPrevalues = function (field) {
	        
	        formService.loadFieldTypePrevalues(field);

	    };

        $scope.close = function() {
            
            $scope.dialogOptions.field.settings = {};
            angular.forEach($scope.dialogOptions.field.$fieldType.settings, function (setting) {
                var key = setting.alias;
                var value = setting.value;
                $scope.dialogOptions.field.settings[key] = value;
                dialogService.closeAll();
            });
        }
	});
