angular.module("umbraco")
.controller("Input.CharLimitEditorController",
	function ($scope) {
		$scope.limitChars = function(){
			var limit = parseInt($scope.model.config.limit);
			var thumbUp = "thumb-up", alert = "alert", warning = "stop-hand";
			$scope.counter = limit - $scope.model.value.length;
			$scope.countLeft = true;
			$scope.maxReached = false;
			$scope.limit = limit;

			if($scope.model.value.length === 0){
				$scope.icon = thumbUp;
				$scope.css = thumbUp;
			}

			if ($scope.model.value.length >= limit)
			{
				$scope.maxReached = true;
				$scope.countLeft = false;
				$scope.icon	= warning;
				$scope.css = warning;
				$scope.model.value = $scope.model.value.substr(0, limit );
			}
			
			else if($scope.model.value.length === 0 ){
				$scope.icon = thumbUp;
				$scope.css = thumbUp;
				$scope.maxReached = false;
				$scope.countLeft = true;
			}

			else
			{
				$scope.icon = alert;
				$scope.css = alert;
				$scope.maxReached = false;
				$scope.countLeft = true;
			}
		};
	});