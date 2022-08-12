angular.module("umbraco").controller("UmbracoForms.Editors.Security.EditController", function ($scope, $routeParams, securityResource, notificationsService, navigationService) {

    //Ensure the current item we are editing is highlighted in the tree
    navigationService.syncTree({ tree: "formsecurity", path: [String($routeParams.id)], forceReload: true });

    securityResource.getByUserId($routeParams.id).then(function (resp) {
        $scope.security = resp.data;
        $scope.loaded = true;
    });

    $scope.save = function () {
        //Add a property to the object to save the Umbraco User ID taken from the routeParam
        $scope.security.userSecurity.user = $routeParams.id;

        securityResource.save($scope.security).then(function (response) {
            $scope.userSecurity = response.data;
            notificationsService.success("User's Form Security saved", "");

            //SecurityForm is the name of the <form name='securitForm'>
            //Set it back to Pristine after we save, so when we browse away we don't get the 'discard changes' notification
            $scope.securityForm.$setPristine();

        }, function (err) {
            notificationsService.error("User's Form Security failed to save", "");
        });

    };

});
