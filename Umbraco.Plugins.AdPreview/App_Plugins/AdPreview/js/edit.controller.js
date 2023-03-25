angular.module("umbraco").controller("AdPreview.Edit.Controller", function ($scope, localizationService) {
    'use strict';
    const vm = this;
    const mediaPicker = {
        show: false,
    }
    vm.buttonState = 'init';
    vm.loading = true;
    vm.mediaPicker = mediaPicker;
    vm.model = null;
    vm.content = {
        name: 'Configure Ad',
        description: '',
        nameLocked: true,
        hideAlias: true,
        hideDescription: false,
        descriptionLocked: false,
        hideIcon: true
    }

    vm.close = function () {
        $scope.cancel();
    }
    vm.save = function () {
        $scope.submit(vm.model);
    }
    vm.openMediaPicker = function () {
        vm.mediaPicker = {
            title: 'Select Ad Image',
            view: "mediapicker",
            section: "media",
            treeAlias: "media",
            entityType: "media",
            multiPicker: false,
            hideHeader: false,
            show: true,
            submit: function (model) {
                vm.model.img = model.selectedImages[0].image;
                vm.mediaPicker.show = false;
                vm.mediaPicker = Object.assign(mediaPicker);
            },
            close: function (oldModel) {
                vm.mediaPicker.show = false;
                vm.mediaPicker = Object.assign(mediaPicker);
            }
        };
    };

    function onInit() {
        vm.loading = false;
        vm.model = $scope.dialogData.model;
        localizationService.localize('local_instructions').then(data => {
            vm.content.description = data;
        });
    }

    onInit();
});