angular.module("umbraco").controller("UmbracoForms.Editors.Form.Dialogs.FieldsetSettingController",
	function ($scope, formService, dialogService) {

	    $scope.deleteConditionRule = function(rules, rule) {
	        formService.deleteConditionRule(rules, rule);
	    };

	    $scope.addConditionRule = function (condition) {
	        formService.addConditionRule(condition);
	    };

        $scope.close = function() {
            dialogService.closeAll();
        }
	}
);
