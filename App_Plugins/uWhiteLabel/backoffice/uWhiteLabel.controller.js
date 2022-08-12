angular.module("umbraco").controller("uWhiteLabel.DashboardController",
	function ($scope, $http, uWhiteLabelResource, notificationsService) {

	    $scope.mode = "notConfiged";

	    uWhiteLabelResource.getIFrameUrl().then(function (response) {
	        if (response.data.HasIframe) {
	            $scope.Url = response.data.Url;
	            $scope.mode = "iframe";
	        }
	    });

	    if ($scope.mode != "iframe") {

	        uWhiteLabelResource.getHtml().then(function (response) {
	            if (response.data.HasHtml) {
	                $scope.Html = response.data.Html;
	                $scope.mode = "html";
	            }
	        });

	    }


	}
);