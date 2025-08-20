define(['app'], function (app) {
    app.register.controller("rdController", function ($scope) {
        console.log("Controller instantiated (after bootstrap).");
        $scope.radiobtnvalidation = -1;
        $scope.rdClick = function ($event, question, optid, index) {
            $scope.radiobtnvalidation = index;
            question.OptionId = optid;
        }
    });
});