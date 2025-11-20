
define(['app'], function (app) {
    app.register.controller("checkboxwithradioController", function ($scope) {
        $scope.chekboxValidation = $scope.chekboxValidation || [];
        $scope.chkboxwithnoneClick = function (question, option, index) {
            if (option.IsChecked) {
                option.IsChecked = false;
                if ($scope.chekboxValidation.indexOf(option.OptionId) != -1) {
                    var currentIndex = $scope.chekboxValidation.indexOf(option.OptionId);
                    $scope.chekboxValidation.splice(currentIndex, 1);
                }

            }
            else {
                option.IsChecked = true;
                if ($scope.chekboxValidation.indexOf(option.OptionId) == -1) {
                    $scope.chekboxValidation.push(option.OptionId);
                }
            }

            angular.forEach(question.OptionList, function (op) {
                if (option.OptionText.toLowerCase() == 'none') {
                    if (option.OptionText.toLowerCase() != op.OptionText.toLowerCase()) {
                        if ($scope.chekboxValidation.indexOf(op.OptionId) != -1) {
                            op.IsChecked = false;
                            var currentIndex = $scope.chekboxValidation.indexOf(op.OptionId);
                            $scope.chekboxValidation.splice(currentIndex, 1);
                        }
                    }
                }
                else {
                    if (op.OptionText.toLowerCase() == 'none') {
                        if ($scope.chekboxValidation.indexOf(op.OptionId) != -1) {
                            var currentIndex = $scope.chekboxValidation.indexOf(op.OptionId);
                            $scope.chekboxValidation.splice(currentIndex, 1);
                            question.OptionId = 0;
                            op.IsChecked = false;
                            return false;
                        }

                    }
                }

            });
            return false;
        }

    });
});