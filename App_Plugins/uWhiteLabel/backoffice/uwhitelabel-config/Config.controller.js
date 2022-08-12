'use strict';
(function () {
    //create the controller
    function uWhiteLabelWelcomeScreenController($scope, $routeParams, $http, uWhiteLabelResource, notificationsService, navigationService) {

        navigationService.syncTree({ tree: 'uwhitelabel-config', path: ["-1", "2222"], forceReload: false });

        $scope.content = { tabs: [{ id: 1, label: "Help" }, { id: 2, label: "Iframe" }, { id: 3, label: "Html" }] };

        var vm = this;

        uWhiteLabelResource.GetUmbracoVersion().then(function (response) {
            vm.isUmbButton = SupportsUmbButton(response.data);
        });


        vm.showUmbButton = true;

        uWhiteLabelResource.IsWelcomeScreenConfiged().then(function (response) {
            vm.isConfiged = response.data.isConfiged;
        });

        uWhiteLabelResource.getIFrameUrl().then(function (response) {
            vm.url = response.data.Url;
        });
        uWhiteLabelResource.getHtml(true).then(function (response) {
            vm.html = response.data.Html;
        });

        vm.saveButtonState = "init";
        $scope.SaveIframe = function (saveUrl) {
            vm.saveButtonState = "busy";
            uWhiteLabelResource.saveIFrameUrl(saveUrl).then(function (response) {
                notificationsService.success("Success", "iFrame URL has been saved");
                vm.saveButtonState = "success";
                vm.isConfiged = true;
            }, function (response) {
                notificationsService.error("Error", "iFrame URL is not valid");
                vm.saveButtonState = "error";
            });
        }

        vm.saveHtmlButtonState = "init";
        $scope.SaveHtml = function (saveHtml) {
            vm.saveHtmlButtonState = "busy";
            uWhiteLabelResource.saveHtml(saveHtml).then(function (response) {
                notificationsService.success("Success", "Your custom HTML has been saved");
                vm.saveHtmlButtonState = "success";
                vm.isConfiged = true;
            }, function (response) {
                notificationsService.error("Error", "Unable to save your HTML");
                vm.saveHtmlButtonState = "error";
            });
        }
        $scope.GetDefaultHtml = function () {
            vm.resetHtmlButtonState = "busy";
            uWhiteLabelResource.getDefaultHtml().then(function (response) {
                vm.html = response.data.Html;
                vm.resetHtmlButtonState = "success";
            }, function (response) {
                notificationsService.error("Error", "Unable to get reset HTML");
                vm.resetHtmlButtonState = "error";
            });
            return $scope.html;
        }

    };
    //register the controller
    angular.module("umbraco").controller('uWhiteLabel.Config.WelcomeScreenController', uWhiteLabelWelcomeScreenController);



    function uWhiteLabelLoginScreenController($scope, $routeParams, $http, uWhiteLabelResource, notificationsService, navigationService) {
        navigationService.syncTree({ tree: 'uwhitelabel-config', path: ["-1", "2223"], forceReload: false });


        $scope.content = { tabs: [{ id: 1, label: "Setup" }] };

        var vm = this;

        uWhiteLabelResource.GetUmbracoVersion().then(function (response) {
            vm.isUmbButton = SupportsUmbButton(response.data);
        });


        uWhiteLabelResource.GetLoginDetails().then(function (response) {
            vm.logo = response.data.LogoUrl;
            vm.greeting = response.data.Greeting;
        });

        vm.saveLogoButtonState = "init";
        $scope.SaveLogin = function (logoUrl, greeting) {
            vm.saveLogoButtonState = "busy";
            uWhiteLabelResource.SaveLoginDetails(logoUrl, greeting).then(function (response) {
                notificationsService.success("Success", "Settings have been saved");
                vm.saveLogoButtonState = "success";
                vm.isConfiged = true;
            }, function (response) {
                notificationsService.error("Error", "Settings not saved!");
                vm.saveLogoButtonState = "error";
            });
        }
    }
    angular.module("umbraco").controller('uWhiteLabel.Config.LoginScreenController', uWhiteLabelLoginScreenController);
})();

function SupportsUmbButton(version) {
    var majorVersion = version._Major;
    var minorVersion = version._Minor;
    return majorVersion >= 7 && minorVersion >= 4;
}


(function () {
    'use strict';

    //umbButton doesn't exist before v7.4 so lets create a basic one for us to use
    function uWhiteLabelButtonDirective() {


        var directive = {
            transclude: true,
            restrict: 'E',
            replace: true,
            templateUrl: '/App_Plugins/uWhiteLabel/backoffice/uwhitelabel-config/umb-button.htm',
            scope: {
                action: "&?",
                href: "@?",
                type: "@",
                buttonStyle: "@?",
                state: "=?",
                shortcut: "@?",
                shortcutWhenHidden: "@",
                label: "@?",
                labelKey: "@?",
                icon: "@?",
                disabled: "="
            }
        };

        return directive;

    }

    angular.module('umbraco.directives').directive('uwlButton', uWhiteLabelButtonDirective);

})();