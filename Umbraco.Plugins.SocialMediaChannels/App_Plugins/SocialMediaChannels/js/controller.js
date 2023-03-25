angular.module("umbraco").controller("splatDev.SocialMediaChannels.Controller", function ($scope, $http, mediaResource, entityResource) {
    'use strict';
    const vm = this;
    vm.model = {
        packages: [],
        selectedTheme: null,
        bg: '',
        showLabels: true,
        path: '/App_Plugins/SocialMediaChannels/themes/'
    }

    vm.updateSelection = function () {
        vm.model.selectedTheme.showLabels = vm.model.showLabels;
        $scope.model.value = vm.model.selectedTheme;
    }

    async function populatePackages() {
        let endpoint = '/umbraco/backoffice/SocialMedia/socialmediachannels/getthemes';
        vm.model.packages = (await $http.get(endpoint)).data;
        if ($scope.model.value !== "") {
            var replace = vm.model.packages.find(x => x.Name == $scope.model.value.Name)
            let index = vm.model.packages.indexOf(replace);
            vm.model.packages[index] = $scope.model.value;
            vm.model.selectedTheme = vm.model.packages[index];
        }
    }

    $scope.sortableOptions = {
        axis: 'y',
        cursor: "move",
        handle: ".handle",
        update: function (ev, ui) {
        },
        stop: function (ev, ui) {
        }
    };

    populatePackages();
});