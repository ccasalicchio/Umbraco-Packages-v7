/**
 * @ngdoc controller
 * @name UmbracoForms.Overlays.FieldsetSettingsOverlay
 * @function
 *
 * @description
 * The controller for the Fieldset Settings dialog
 */

(function() {
    "use strict";

    function FieldsetSettingsOverlay($scope, formService) {

        var vm = this;

        vm.actionTypes = [];
        vm.logicTypes = [];
        vm.operators = [];

        vm.deleteConditionRule = deleteConditionRule;
        vm.addConditionRule = addConditionRule;
        vm.conditionFieldSelected = conditionFieldSelected;
        vm.addColumn = addColumn;
        vm.removeColumn = removeColumn;

        function init() {
            vm.actionTypes = formService.getActionTypes();
            vm.logicTypes = formService.getLogicTypes();
            vm.operators = formService.getOperators();

            if(!$scope.model.fieldset.condition) {
                $scope.model.fieldset.condition = {};
                $scope.model.fieldset.condition.actionType = vm.actionTypes[0].value;
                $scope.model.fieldset.condition.logicType = vm.logicTypes[0].value;
            }
        }

        function deleteConditionRule (rules, rule) {
	        formService.deleteConditionRule(rules, rule);
	    }

	    function addConditionRule(condition) {
	        formService.addEmptyConditionRule(condition);
            // set default operator
            var lastIndex = condition.rules.length - 1;
            condition.rules[lastIndex].operator = vm.operators[0].value;
	    }

        function conditionFieldSelected(selectedField, rule) {
            formService.populateConditionRulePrevalues(selectedField, rule, $scope.model.fields);
        }

        function addColumn() {
            var index = $scope.model.fieldset.containers.length;
            formService.addContainer($scope.model.fieldset, index);
        }

        function removeColumn(container) {
            formService.deleteContainer($scope.model.fieldset, container);
        }

        init();
    }

    angular.module("umbraco").controller("UmbracoForms.Overlays.FieldsetSettingsOverlay", FieldsetSettingsOverlay);

})();
