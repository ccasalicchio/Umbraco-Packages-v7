(function() {
	var app = angular.module("umbraco");

	/* controller per versione AngularJS >= 1.3 */
	app.controller("Xilium.groupLabel.groupLabelController_ng1_3",
		["$scope", "$sce",
		function ($scope, $sce) {
			var ctrl = this;

			$scope.trustAsHtml = function (htmlCode) {
				return $sce.trustAsHtml(htmlCode);
			};

			_init();
		}
	]);


})();