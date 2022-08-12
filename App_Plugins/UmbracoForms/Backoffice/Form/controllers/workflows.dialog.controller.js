angular.module("umbraco").controller("UmbracoForms.Editors.Form.Dialogs.WorkflowsController",
	function ($scope, $routeParams, workflowResource, dialogService, notificationsService, $window) {

	    if ($scope.dialogData.workflow) {
	        //edit
	        $scope.workflow = $scope.dialogData.workflow;
	        workflowResource.getAllWorkflowTypesWithSettings()
	            .then(function (resp) {
	                $scope.types = resp.data;
	                setTypeAndSettings();
	            });

	        //workflowResource.getByGuid($scope.dialogData.workflow)
            //.then(function (response) {

            //    $scope.workflow = response.data;

            //    workflowResource.getAllWorkflowTypesWithSettings()
            //        .then(function (resp) {
            //            $scope.types = resp.data;
            //            setTypeAndSettings();
            //        });
                
            //});

	    } else {
	        //create
	        workflowResource.getScaffold()
	            .then(function(response) {
	                $scope.loaded = true;
	                $scope.workflow = response.data;
	                $scope.workflow.executesOn = $scope.dialogData.state;
	                $scope.workflow.form = $scope.dialogData.form;
	                $scope.workflow.active = true;

	                workflowResource.getAllWorkflowTypesWithSettings()
	                    .then(function(resp) {
	                        $scope.types = resp.data;

	                    });

	            });
	    }


	    $scope.setType = function () {
	        setTypeAndSettings();
	    };

	    $scope.close = function () {
	       
	        dialogService.closeAll();
	    };

	    $scope.add = function () {
	       
	        save();
	        
	    };

	    $scope.update = function () {
	       
	        save();
	        
	    };

        $scope.delete = function() {
            var deleteWorkflow = $window.confirm('Are you sure you want to delete the workflow?');

            if (deleteWorkflow) {
                workflowResource.deleteByGuid($scope.workflow.id).then(function () {
                    $scope.dialogOptions.workflows.splice($scope.dialogOptions.workflows.indexOf($scope.workflow), 1);

                    notificationsService.success("Workflow deleted", "");
                    //$scope.submit($scope.workflow);
                    dialogService.closeAll();

                });
            }
        }

	    var save = function() {
	        //set settings
	        $scope.workflow.settings = {};
	        angular.forEach($scope.workflow.$type.settings, function (setting) {
	            var key = setting.alias;
	            var value = setting.value;
	            $scope.workflow.settings[key] = value;
	        });
	        //validate settings
	        workflowResource.validateSettings($scope.workflow)
                .then(function (response) {

                    $scope.errors = response.data;

                    if ($scope.errors.length > 0) {
                        angular.forEach($scope.errors, function (error) {

                            notificationsService.error("Workflow failed to save", error.Message);
                        });
                    } else {
                        //save
                        workflowResource.save($scope.workflow)
                        .then(function (response) {

                            $scope.workflow = response.data;
                           
                            setTypeAndSettings();
                           
                            notificationsService.success("Workflow saved", "");
                            $scope.submit($scope.workflow);
                            dialogService.closeAll();

                        }, function (err) {
                            notificationsService.error("Workflow failed to save", "");
                        });
                    }

                }, function (err) {
                    notificationsService.error("Workflow failed to save", "Please check if your settings are valid");
                });
	    };

	    var setTypeAndSettings = function () {
	        $scope.workflow.$type = _.where($scope.types, { id: $scope.workflow.workflowTypeId })[0];
	        if (!$scope.workflow.name) {
	            $scope.workflow.name = $scope.workflow.$type.name;
	        }
	        //set settings
	        angular.forEach($scope.workflow.settings, function (setting) {
	            for (var key in $scope.workflow.settings) {
	                if ($scope.workflow.settings.hasOwnProperty(key)) {
	                    if (_.where($scope.workflow.$type.settings, { alias: key }).length > 0) {
	                        _.where($scope.workflow.$type.settings, { alias: key })[0].value = $scope.workflow.settings[key];
	                    }

	                }
	            }
	        });
	    };
	});