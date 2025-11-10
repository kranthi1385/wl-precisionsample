define(['gmApp'], function (gmApp) {
    gmApp.register.controller("rdController", function ($scope) {
        $scope.OptionId = '';
        console.log("Controller instantiated (after bootstrap).");
        $scope.radiobtnvalidation = -1;
        $scope.rdClick = function ($event, question, optid, index) {
            $scope.radiobtnvalidation = index;
            question.OptionId = optid;
        }
    });
});