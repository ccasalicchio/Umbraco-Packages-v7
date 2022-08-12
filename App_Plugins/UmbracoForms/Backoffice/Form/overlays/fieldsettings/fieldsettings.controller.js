/**
 * @ngdoc controller
 * @name UmbracoForms.Overlays.FieldSettingsOverlay
 * @function
 *
 * @description
 * The controller for the Field Settings dialog
 */

(function() {
    "use strict";

    function FieldSettingsOverlay($scope, localizationService, formService) {

        var vm = this;

        vm.showValidationPattern = false;
        vm.focusOnPatternField = false;
        vm.focusOnMandatoryField = false;
        vm.selectedValidationType = {};
        vm.actionTypes = [];
        vm.logicTypes = [];
        vm.operators = [];
        vm.validationTypes = [{
            "name": localizationService.localize("validation_validateAsEmail"),
            "key": "email",
            "pattern": "[a-zA-Z0-9_\.\+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-\.]+",
            "enableEditing": true
        }, {
            "name": localizationService.localize("validation_validateAsNumber"),
            "key": "number",
            "pattern": "^[0-9]*$",
            "enableEditing": true
        }, {
            "name": localizationService.localize("validation_validateAsUrl"),
            "key": "url",
            "pattern": "https?\:\/\/[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,}",
            "enableEditing": true
        }, {
            "name": localizationService.localize("validation_enterCustomValidation"),
            "key": "custom",
            "pattern": "",
            "enableEditing": true
        }];



        vm.changeValidationType = changeValidationType;
        vm.changeValidationPattern = changeValidationPattern;
        vm.openFieldTypePickerOverlay = openFieldTypePickerOverlay;
        vm.deleteConditionRule = deleteConditionRule;
        vm.addConditionRule = addConditionRule;
        vm.getPrevalues = getPrevalues;
        vm.conditionFieldSelected = conditionFieldSelected;

        function activate() {
            vm.actionTypes = formService.getActionTypes();
            vm.logicTypes = formService.getLogicTypes();
            vm.operators = formService.getOperators();

            if(!$scope.model.field.condition) {
                $scope.model.field.condition = {};
                $scope.model.field.condition.actionType = vm.actionTypes[0].value;
                $scope.model.field.condition.logicType = vm.logicTypes[0].value;
            }

            matchValidationType();
        }

        function changeValidationPattern() {
            matchValidationType();
        }

        function openFieldTypePickerOverlay(field) {

            vm.focusOnMandatoryField = false;

            vm.fieldTypePickerOverlay = {
                view: "/app_plugins/UmbracoForms/Backoffice/Form/overlays/fieldtypepicker/field-type-picker.html",
                title: "Choose answer type",
                hideSubmitButton: true,
                show: true,
                submit: function(model) {

                    formService.loadFieldTypeSettings(field, model.fieldType);

                    // this should be removed in next major version
                    field.removePrevalueEditor = true;

                    vm.fieldTypePickerOverlay.show = false;
                    vm.fieldTypePickerOverlay = null;
                }
            };

        }

        function matchValidationType() {

            if ($scope.model.field.regex !== null && $scope.model.field.regex !== "" && $scope.model.field.regex !== undefined) {

                var match = false;

                // find and show if a match from the list has been chosen
                angular.forEach(vm.validationTypes, function(validationType, index) {
                    if ($scope.model.field.regex === validationType.pattern) {
                        vm.selectedValidationType = validationType;
                        vm.showValidationPattern = true;
                        match = true;
                    }
                });

                // if there is no match - choose the custom validation option.
                if (!match) {
                    angular.forEach(vm.validationTypes, function(validationType) {
                        if (validationType.key === "custom") {
                            vm.selectedValidationType = validationType;
                            vm.showValidationPattern = true;
                        }
                    });
                }
            }

        }

        function changeValidationType(selectedValidationType) {

            if (selectedValidationType) {
                $scope.model.field.regex = selectedValidationType.pattern;
                vm.showValidationPattern = true;

                // set focus on textarea
                if (selectedValidationType.key === "custom") {
                    vm.focusOnPatternField = true;
                }

            } else {
                $scope.model.field.regex = "";
                vm.showValidationPattern = false;
            }

        }

        function conditionFieldSelected(selectedField, rule) {
            formService.populateConditionRulePrevalues(selectedField, rule, $scope.model.fields);
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

        function getPrevalues(field) {
            formService.loadFieldTypePrevalues(field);
        }

        activate();

    }

    angular.module("umbraco").controller("UmbracoForms.Overlays.FieldSettingsOverlay", FieldSettingsOverlay);

})();
