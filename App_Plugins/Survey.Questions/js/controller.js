'use strict';
angular.module("umbraco")
    .controller("Survey.Questions",
    function ($scope, $http, $routeParams) {
        $scope.Options = [];
        $scope.index = $scope.Options.length > 1 ? $scope.Options[$scope.Options.length - 1].Id : 0;
        $scope.loading = true;
        $scope.nodeId = $routeParams.id;
        $scope.VoteCount = 0;

        $scope.Add = function () {
            $scope.Options.splice($scope.Options.length + 1, 0, { Id: $scope.index, Order: $scope.Options.length, Value: "", Votes: 0 });
            $scope.model.value = $scope.Options;
            $scope.index++;
        }
        $scope.Delete = function (index) {
            $scope.Options.splice(index, 1);
            $scope.model.value = $scope.Options;
        }
        $scope.sorterFunc = function (option) {
            return parseInt(option.Order);
        };
        if ($scope.model.value) {
            if ($scope.model.value !== "") {
                $scope.Options = $scope.model.value;
                $scope.index = $scope.Options.length;
                var options = [];
                $http.get("/Umbraco/Api/SurveyApi/VotesForSurvey?nodeId=" + $scope.nodeId).then(function (response) {
                    options = response.data.Options;
                    $scope.VoteCount = response.data.VoteCount;
                    for (var i = 0; i < $scope.Options.length; i++) {
                        $scope.Options[i].Votes = options[i].Votes;
                        $scope.Options[i].Percentage = parseFloat(options[i].Percentage).toFixed(2);
                    }
                },
                    function (data, status) {
                        console.error('Api Error', status, data);
                    });
            }
        }
        $scope.loading = false;
    });