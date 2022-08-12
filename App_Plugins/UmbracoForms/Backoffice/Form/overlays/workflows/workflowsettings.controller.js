(function () {
    "use strict";

    function WorkflowSettingsOverlayController($scope, workflowResource) {

        var vm = this;

        vm.workflowTypes = [];
        vm.focusWorkflowName = true;

        if($scope.model.workflowType && $scope.model.workflowType.id) {
            workflowResource.getScaffoldWorkflowType($scope.model.workflowType.id).then(function(response){
               $scope.model.workflow = response.data;
            });
        }

    }

    angular.module("umbraco").controller("UmbracoForms.Overlays.WorkflowSettingsOverlayController", WorkflowSettingsOverlayController);
})();
