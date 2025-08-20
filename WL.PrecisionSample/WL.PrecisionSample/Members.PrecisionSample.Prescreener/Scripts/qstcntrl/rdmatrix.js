define(['psApp'], function (psApp) {
    psApp.register.controller("rdMatrixController", function ($scope) {
        console.log("Controller instantiated (after bootstrap).");
        $scope.rdMatrixClick = function (Parent, question, optid, index) {
            $scope.radiobtnvalidation = index;
            question.OptionId = optid;
            question.ChqOptionId = optid;
            question.RdMatrixIndex = index;
        }
    })
});