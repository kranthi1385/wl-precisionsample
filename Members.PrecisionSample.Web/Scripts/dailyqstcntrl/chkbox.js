define(['app'], function (app) {
    app.register.controller("chkBoxController", function ($scope) {
        console.log("Controller instantiated (after bootstrap).");
        //$scope.showRadio = false;
        //if (op.OptionId)
        $scope.chekboxValidation = $scope.chekboxValidation || [];
        $scope.checkboxClick = function (question, option, index) {
            if (option.IsChecked) {
                option.IsChecked = true;
                if ($scope.chekboxValidation.indexOf(option.OptionId) != -1) {
                    var currentIndex = $scope.chekboxValidation.indexOf(option.OptionId);
                    $scope.chekboxValidation.splice(currentIndex, 1);
                }

            }
            else {
                option.IsChecked = false;
                if ($scope.chekboxValidation.indexOf(option.OptionId) == -1) {
                    $scope.chekboxValidation.push(option.OptionId);
                }
            }
            event.stopPropagation();
            var _optionId = 0;
            if (question.QuestionId == 4093) {
                _optionId = 125642;
            }
            else if (question.QuestionId == 4105) {
                _optionId = 125703;
            }
            else if (question.QuestionId == 4109) {
                _optionId = 125746;
            }
            else if (question.QuestionId == 4110) {
                _optionId = 125765;
            }

            if (question.QuestionId == 4093 || question.QuestionId == 4105 || question.QuestionId == 4109 || question.QuestionId == 4110) {

                if (option.OptionId == _optionId) {

                    angular.forEach(question.OptionList, function (opt, ind) {
                        {
                            if (opt.OptionId != _optionId) {
                                opt.IsChecked = false;
                            }
                        }
                    })
                }
                else {
                    angular.forEach(question.OptionList, function (opt, ind) {
                        {
                            if (opt.OptionId == _optionId) {
                                opt.IsChecked = false;
                            }
                        }
                    })
                }
            }
        }
    })
});