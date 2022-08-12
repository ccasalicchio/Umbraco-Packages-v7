(function () {
    "use strict";

    function WorkflowsOverviewOverlayController($scope, workflowResource, notificationsService) {

        var vm = this;

        // massive hack to fix submit when pressing enter
        vm.focusOverlay = true;

        vm.openWorkflowsTypesOverlay = openWorkflowsTypesOverlay;
        vm.editWorkflow = editWorkflow;
        vm.removeWorkflow = removeWorkflow;
        vm.editSubmitMessageWorkflow = editSubmitMessageWorkflow;

        vm.workflowsSortableOptions = {
            distance: 10,
            tolerance: "pointer",
            connectWith: ".umb-forms-workflows__sortable-wrapper",
            opacity: 0.7,
            scroll: true,
            cursor: "move",
            zIndex: 6000,
            handle: ".sortable-handle",
            items: ".sortable",
            placeholder: "umb-forms-workflow__workflow-placeholder",
            start: function(e, ui) {
                ui.placeholder.height(ui.item.height());
            },
            stop: function(event, ui) {
                updateSortOrder($scope.model.formWorkflows.onSubmit);
                updateSortOrder($scope.model.formWorkflows.onApprove);
            }
        };

        function updateSortOrder(array) {
            var sortOrder = 0;
            for(var i = 0; i < array.length; i++) {
                var arrayItem = array[i];
                if(arrayItem.isDeleted === false) {
                    arrayItem.sortOrder = sortOrder;
                    sortOrder++;
                }
            }
        }

        function openWorkflowsTypesOverlay(workflowArray) {

            // set overlay settings and open overlay
            vm.workflowsTypesOverlay = {
                view: "/app_plugins/UmbracoForms/Backoffice/Form/overlays/workflows/workflow-types.html",
                title: "Choose workflow",
                fields: $scope.model.fields,
                hideSubmitButton: true,
                show: true,
                submit: function(model) {

                    // set sortOrder
                    workflowArray.push(model.workflow);
                    updateSortOrder(workflowArray);

                    vm.workflowsTypesOverlay.show = false;
                    vm.workflowsTypesOverlay = null;
                }
            };

        }

        function editWorkflow(workflow) {
            vm.workflowSettingsOverlay = {
                view: "/app_plugins/UmbracoForms/Backoffice/Form/overlays/workflows/workflow-settings.html",
                title: workflow.name,
                workflow: workflow,
                fields: $scope.model.fields,
                show: true,
                submit: function(model) {

                    //Validate settings
                    workflowResource.validateWorkflowSettings(model.workflow).then(function(response){
                        if (response.data.length > 0) {
                            angular.forEach(response.data, function (error) {
                                notificationsService.error("Workflow failed to save", error.Message);
                            });
                        } else {
                            vm.workflowSettingsOverlay.show = false;
                            vm.workflowSettingsOverlay = null;
                        }

                    });

                }
            };
        }

        function editSubmitMessageWorkflow() {

            vm.submitMessageWorkflowOverlay = {
                view: "/app_plugins/UmbracoForms/Backoffice/Form/overlays/workflows/submit-message-workflow-settings.html",
                title: "Message on submit",
                messageOnSubmit: $scope.model.messageOnSubmit,
                goToPageOnSubmit: $scope.model.goToPageOnSubmit,
                show: true,
                submit: function(model) {

                    $scope.model.messageOnSubmit = model.messageOnSubmit;
                    $scope.model.goToPageOnSubmit = model.goToPageOnSubmit;

                    vm.submitMessageWorkflowOverlay.show = false;
                    vm.submitMessageWorkflowOverlay = null;

                }
            };

        }

        function removeWorkflow(workflow, event, workflowTypeArray) {
            workflow.isDeleted = true;
            updateSortOrder(workflowTypeArray);
            event.stopPropagation();
        }

    }

    angular.module("umbraco").controller("UmbracoForms.Overlays.WorkflowsOverviewOverlayController", WorkflowsOverviewOverlayController);
})();
