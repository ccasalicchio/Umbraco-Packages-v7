angular.module("umbraco")
.controller("Copy.Value",
	function ($scope, contentEditingHelper, editorState) {
		var from = $scope.model.config.from.split(",");
		var to = $scope.model.config.to.split(",");

		var content = editorState.current;
		var properties = contentEditingHelper.getAllProps(content);
		$scope.done = false;

		$scope.copy = function(){
			for(var i = 0; i < from.length; i++)
				_.findWhere(properties, { alias: to[i] }).value = _.findWhere(properties, { alias: from[i] }).value;
			
			
			$scope.done = true;
			setTimeout(function(){
				$scope.done=false;
			},3*1000);
		};

	});