(function() {
    "use strict";

    function WorkflowTypesOverlayController($scope, workflowResource, notificationsService) {

        var vm = this;

        vm.workflowTypes = [];
        vm.searchTerm = "";

        vm.pickWorkflowType = pickWorkflowType;
        vm.filterItems = filterItems;
        vm.showDetailsOverlay = showDetailsOverlay;
        vm.hideDetailsOverlay = hideDetailsOverlay;

        function init() {

            // get workflows with settings
            workflowResource.getAllWorkflowTypesWithSettings()
                .then(function(response) {
                    vm.workflowTypes = response.data;
                    setDefaultWorkflowIcon(vm.workflowTypes);
                });

        }

        function setDefaultWorkflowIcon(workflowTypes) {

            for(var i = 0; i < workflowTypes.length; i++) {
                var workflowType = workflowTypes[i];
                if(!workflowType.icon) {
                    workflowType.icon = "icon-mindmap";
                }
            }
        }

        function pickWorkflowType(selectedWorkflowType) {

            // set overlay settings + open overlay
            vm.workflowSettingsOverlay = {
                view: "/app_plugins/UmbracoForms/Backoffice/Form/overlays/workflows/workflow-settings.html",
                title: selectedWorkflowType.name,
                workflow: $scope.model.workflow,
                workflowType: selectedWorkflowType,
                fields: $scope.model.fields,
                show: true,
                submit: function(model) {

                    workflowResource.validateWorkflowSettings(model.workflow).then(function(response){
                        if (response.data.length > 0) {
                            angular.forEach(response.data, function (error) {
                                notificationsService.error("Workflow failed to save", error.Message);
                            });
                        } else {

                            //Need to add the properties to the $scope from this submitted model
                            $scope.model.workflow = model.workflow;

                            // submit overlay and return the model
                            $scope.model.submit($scope.model);

                            // close the overlay
                            vm.workflowSettingsOverlay.show = false;
                            vm.workflowSettingsOverlay = null;

                        }

                    });
                }
            };
        }

        function filterItems() {
            // clear item details
            $scope.model.itemDetails = null;
        }

        function showDetailsOverlay(workflowType) {

            var workflowDetails = {};
            workflowDetails.icon = workflowType.icon;
            workflowDetails.title = workflowType.name;
            workflowDetails.description = workflowType.description;

            $scope.model.itemDetails = workflowDetails;

        }

        function hideDetailsOverlay() {
            $scope.model.itemDetails = null;
        }

        init();

    }

    angular.module("umbraco").controller("UmbracoForms.Overlays.WorkflowTypesOverlayController", WorkflowTypesOverlayController);
})();
