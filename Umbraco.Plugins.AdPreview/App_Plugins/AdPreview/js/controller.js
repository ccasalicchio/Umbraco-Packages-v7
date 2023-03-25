angular.module("umbraco").controller("AdPreview.Controller",
    function ($scope, dialogService) {
        'use strict';
        const vm = this;
        let dialog = null;
        const defaultModel = {
            img: '',
            title: '',
            description: '',
            url: '',
            tooltip: '',
            referrer: '',
            css: '',
            overlay: false
        };
        vm.model = Object.assign(defaultModel);
        vm.edit = function () {
            const options = {
                template: '/App_Plugins/AdPreview/views/edit.html',
                size: 'small',
                show: true,
                dialogData: {
                    model: vm.model
                },
                callback: (data) => {
                    vm.model = data;
                    $scope.model.value = vm.model;
                    dialogService.close(dialog);
                },
                cancel: (data) => {
                    vm.model = data;
                    $scope.model.value = vm.model;
                    dialogService.close(dialog);
                }
            };
            dialog = dialogService.open(options);
        }
        vm.remove = function () {
            vm.model = Object.assign(defaultModel);
            $scope.model.value = vm.model;
        };
        function init() {
            if ($scope.model.value !== undefined && $scope.model.value !== '') vm.model = $scope.model.value;
        }
        init();
    });