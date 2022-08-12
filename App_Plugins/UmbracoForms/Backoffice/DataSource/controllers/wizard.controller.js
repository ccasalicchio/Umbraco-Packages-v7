﻿angular.module("umbraco")
.controller("UmbracoForms.Editors.DataSource.WizardController",
	function ($scope, $routeParams, dataSourceWizardResource, navigationService, notificationsService, dialogService) {

	    $scope.currentStep = 1;

	    dataSourceWizardResource.getScaffold($scope.dialogOptions.dataSourceId).then(function (response) {

	         $scope.wizard = response.data;

	         $scope.hasPrimaryKeys = $scope.wizard.mappings.length != _.where($scope.wizard.mappings, { prevalueKeyField: null }).length;

	         dataSourceWizardResource.getAllFieldTypes()
                 .then(function (resp) {
                     $scope.fieldtypes = resp.data;
                     $scope.ready = true;
                 });
	     });


	    $scope.createForm = function() {

	        dataSourceWizardResource.createForm($scope.wizard)
	            .then(function (resp) {
	                dialogService.closeAll();
	                notificationsService.success("Form created", "");
	            });
	    };

	    $scope.gotoStep = function (step) {
	        $scope.currentStep = step;
	    }

	    $scope.gotoThirdStep = function() {
	        if ($scope.hasPrimaryKeys) {
	            $scope.currentStep = 3;
	        } else {
	            $scope.currentStep = 4;
	        }
	    }
        $scope.goBackToThirdStep = function() {
            if ($scope.hasPrimaryKeys) {
                $scope.currentStep = 3;
            } else {
                $scope.currentStep = 2;
            }
        }
	});