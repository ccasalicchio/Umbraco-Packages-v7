angular.module("umbraco")
.controller("UmbracoForms.Dashboards.LicensingController",
    function ($scope, $location, $routeParams, $cookieStore, formResource, licensingResource, updatesResource, notificationsService, userService, utilityService) {

        $scope.overlay = {
            show: false,
            title: "Congratulations",
            description: "You've just installed Umbraco Forms - Let's create your first form"
        };

        var packageInstall = $cookieStore.get("umbPackageInstallId");
        if (packageInstall) {
            $scope.overlay.show = true;
            $cookieStore.put("umbPackageInstallId", "");
        }

        //if not initial install, but still do not have forms
        if (!$scope.overlay.show) {
            formResource.getOverView().then(function (response) {
                if (response.data.length === 0) {
                    $scope.overlay.show = true;
                    $scope.overlay.title = "Create a form";
                    $scope.overlay.description = "You do not have any forms setup yet, how about creating one now?";
                }
            });
        }

        $scope.getLicenses = function (config) {

            $scope.loginError = false;
            $scope.hasLicenses = undefined;

            licensingResource.getAvailableLicenses(config).then(function (response) {
                var licenses = response.data;
                var currentDomain = window.location.hostname;

                $scope.hasLicenses = licenses.length > 0;
                _.each(licenses, function (lic) {
                    if (lic.bindings && lic.bindings.indexOf(currentDomain) >= 0) {
                        lic.currentDomainMatch = true;
                    }
                });

                $scope.configuredLicenses = _.filter(licenses, function (license) { return license.configured; });
                $scope.openLicenses = _.filter(licenses, function (license) { return license.configured === false; });

            }, function (err) {
                $scope.loginError = true;
                $scope.hasLicenses = undefined;
            });
        };


        $scope.configure = function (config) {
            licensingResource.configureLicense(config).then(function (response) {
                $scope.configuredLicenses.length = 0;
                $scope.openLicenses.length = 0;
                $scope.loadStatus();

                notificationsService.success("License configured", "Umbraco forms have been configured to be used on this website");
            });
        };

        $scope.loadStatus = function () {
            licensingResource.getLicenseStatus().then(function (response) {
                $scope.status = response.data;
            });


            //Get Current User - To Check if the user Type is Admin
            userService.getCurrentUser().then(function (response) {
                $scope.currentUser = response;
                $scope.isAdminUser = response.userType.toLowerCase() === "admin";
            });

            updatesResource.getUpdateStatus().then(function (response) {
                $scope.version = response.data;
            });

            updatesResource.getVersion().then(function (response) {
                $scope.currentVersion = response.data;
            });


        };

        $scope.upgrade = function () {

            //Let's tripple check the user is of the userType Admin
            if (!$scope.isAdminUser) {
                //The user is not an admin & should have not hit this method but if they hack the UI they could potnetially see the UI perhaps?
                notificationsService.error("Insufficient Permissions", "Only Admin users have the ability to upgrade Umbraco Forms");
                return;
            }

            $scope.installing = true;
            updatesResource.installLatest($scope.version.remoteVersion).then(function (response) {
                window.location.reload();
            }, function (reason) {
                //Most likely the 403 Unauthorised back from server side
                //The error is caught already & shows a notification so need to do it here
                //But stop the loading bar from spinnging forever
                $scope.installing = false;
            });
        };


        $scope.create = function () {
            
            //Get the current umbraco version we are using
            var umbracoVersion = Umbraco.Sys.ServerVariables.application.version;
            
            var compareOptions = {
                zeroExtend: true
            };
            
            //Check what version of Umbraco we have is greater than 7.4 or not 
            //So we can load old or new editor UI
            var versionCompare = utilityService.compareVersions(umbracoVersion, "7.4", compareOptions);
            
            //If value is 0 then versions are an exact match
            //If 1 then we are greater than 7.4.x
            //If it's -1 then we are less than 7.4.x
            if(versionCompare < 0) {
                //I am less than 7.4 - load the legacy editor
                $location.url("forms/form/edit-legacy/-1?template=&create=true");
            }
            else {
                //I am 7.4 or newer - load in shiny new UI
                $location.url("forms/form/edit/-1?template=&create=true");
            }
            
            
        };


        $scope.configuration = { domain: window.location.hostname };
        $scope.loadStatus();
    });
