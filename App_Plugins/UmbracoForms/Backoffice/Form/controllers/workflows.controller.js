angular.module("umbraco").controller("UmbracoForms.Editors.Form.WorkflowsController", function ($scope, $routeParams, workflowResource, editorState, dialogService, $window, userService, securityResource, notificationsService, navigationService) {

       
    //On load/init of 'editing' a form then
    //Let's check & get the current user's form security
        var currentUserId = null;
        var currentFormSecurity = null;

    //By default set to have access (in case we do not find the current user's per indivudal form security item)
        $scope.hasAccessToCurrentForm = true;

        userService.getCurrentUser().then(function (response) {
            currentUserId = response.id;

            //Now we can make a call to form securityResource
            securityResource.getByUserId(currentUserId).then(function (response) {
                $scope.security = response.data;

                //Use _underscore.js to find a single item in the JSON array formsSecurity 
                //where the FORM guid matches the one we are currently editing (if underscore does not find an item it returns undefinied)
                currentFormSecurity = _.where(response.data.formsSecurity, { Form: $routeParams.id });

                if (currentFormSecurity.length === 1) {
                    //Check & set if we have access to the form
                    //if we have no entry in the JSON array by default its set to true (so won't prevent)
                    $scope.hasAccessToCurrentForm = currentFormSecurity[0].HasAccess;
                }

               //Check if we have access to current form OR manage forms has been disabled
                if (!$scope.hasAccessToCurrentForm || !$scope.security.userSecurity.manageWorkflows || !$scope.security.userSecurity.manageForms) {
                    
                    //Show error notification
                    notificationsService.error("Access Denied", "You do not have access to edit this form's workflow");

                    //Resync tree so that it's removed & hides
                    navigationService.syncTree({ tree: "form", path: ['-1'], forceReload: true, activate: false }).then(function (response) {

                        //Response object contains node object & activate bool
                        //Can then reload the root node -1 for this tree 'Forms Folder'
                        navigationService.reloadNode(response.node);
                    });

                    //Don't need to wire anything else up
                    return;
                }
            });
        });
        
        workflowResource.getAllWorkflows($routeParams.id)
            .then(function(resp) {
                $scope.workflows = resp.data;
                $scope.loaded = true;

                //As we are editing an item we can highlight it in the tree
                navigationService.syncTree({ tree: "form", path: [String($routeParams.id), String($routeParams.id) + "_workflows"], forceReload: true });

            }, function(reason) {
                //Includes ExceptionMessage, StackTrace etc from the WebAPI
                var jsonErrorResponse = reason.data;
                
                //Show notification message, a sticky Error message
                notificationsService.add({ headline: "Unable to load form", message: jsonErrorResponse.ExceptionMessage, type: 'error', sticky: true  });
                
                //Hide the entire workflows UI
                $scope.loaded = false;
            });

        $scope.sortableOptions = {
            handle: '.handle',
            cursor: "move",
            connectWith: '.workflows',
            update: function (e, ui) {
                var wfGuids = [];
                var wfcount = 0;
                var state = ui.item.parent().attr("rel");
                ui.item.parent().children().each(function () {
                    if ($(this).attr("rel") != null) {
                        wfGuids[wfcount] = $(this).attr("rel");
                        wfcount++;
                    }
                });

                workflowResource.updateSortOrder(state,wfGuids).then(function () {


                });

            },
        };

        $scope.deleteWorkflow = function (workflow) {
            var deleteWorkflow = $window.confirm('Are you sure you want to delete the workflow?');

            if (deleteWorkflow) {
                workflowResource.deleteByGuid(workflow.id).then(function() {
                    $scope.workflows.splice($scope.workflows.indexOf(workflow), 1);

                });
            }
        };

        $scope.updateWorkflow = function(state, workflow) {
            data = {};
            data.workflow = workflow;
            data.state = state;
            data.form = $routeParams.id;
            data.add = false;

            dialogService.open({
                template: '/app_plugins/UmbracoForms/Backoffice/Form/dialogs/workflow.html',
                show: true,
                callback: update,
                dialogData: data,
                workflows: $scope.workflows
        });
        };

        $scope.addWorkflow = function(state) {
            data = {};
            data.state = state;
            data.form = $routeParams.id;
            data.add = true;

            dialogService.open({
                template: '/app_plugins/UmbracoForms/Backoffice/Form/dialogs/workflow.html',
                show: true,
                callback: add,
                dialogData: data
            });
        };

        function add(data) {

            $scope.workflows.push(data);
        }

        function update(data) {
            
        }
    });