define(['gmApp'], function (gmApp) {
    gmApp.register.controller("likertScaleController", function ($scope, $compile) {
        console.log("Controller instantiated (after bootstrap).");
        var optionlst = [];
        angular.forEach($scope.question.OptionList, function (op, index) {

            optionlst.push({ value: op.OptionId, legend: op.OptionText });
        });
        $scope.slider = { //requires angular-bootstrap to display tooltips
            options: {
                hidePointerLabels: true,
                hideLimitLabels: true,
                showTicks: true,
                showTicksValues: false,
                stepsArray: optionlst
            }
        }
        $scope.sliderChange = function (optid, question) {
            if (optid != undefined) {
                question.OptionId = optid;
                question.ChqOptionId = optid;
            }
        }

    });
});

