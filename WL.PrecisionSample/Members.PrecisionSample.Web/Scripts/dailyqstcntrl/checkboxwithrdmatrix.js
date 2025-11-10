define(['app'], function (app) {
    app.register.controller("checkboxwithrdmatrixController", function ($scope, $compile, $rootScope) {
        $rootScope.healthQstValidations = false;
        $scope.counter = 0;
        $scope.checkboxClick = function (question, option, parentindex) {
            checkMemberOption(question, option, parentindex);
            angular.forEach(question.SpecialOptinLst, function (opt, spindex) {
                var _qstcount = 0;
                angular.forEach(opt, function (op, opindex) {
                    if (option.QuestionId == op.QuestionId) {
                        if (op.OptionText.toLowerCase() == 'prefer not to answer' || op.OptionText.toLowerCase() == "none of the above") {
                            if (opt.OptidsLst.length > 0) {
                                if (opt.OptidsLst.indexOf(op.OptionId) != -1) {
                                    var currentIndex = opt.indexOf(option.OptionId);
                                    question.SpecialOptinLst[parentindex].OptidsLst.splice(currentIndex, 1);
                                }
                            }
                            op.IsChecked = false;
                        }
                    }
                });
            });
        }
        $scope.rdClick = function (question, option, parentindex) {
            checkMemberOption(question, option, parentindex);
            if (option.IsChecked) {
                angular.forEach(question.SpecialOptinLst, function (opt, spindex) {
                    var _qstcount = 0;
                    angular.forEach(opt, function (op, opindex) {
                        if (option.QuestionId == op.QuestionId) {
                            if (option.OptionId != op.OptionId) {
                                if (opt.OptidsLst.length > 0) {
                                    _qstcount = 1;
                                    if (opt.OptidsLst.indexOf(op.OptionId) != -1) {
                                        var currentIndex = opt.indexOf(option.OptionId);
                                        opt.OptidsLst.splice(currentIndex, 1);
                                        if (opt.OptidsLst.length == 0) {
                                            opt.OptidsLst.push(1);
                                        }
                                    }
                                }
                                else {
                                    opt.OptidsLst.push(1);
                                }
                                op.IsChecked = false;
                            }

                        }
                    });
                });
            }
        }

        function checkMemberOption(question, option, index) {
            if (option.IsChecked) {
                option.IsChecked = false;
                if (question.SpecialOptinLst[index].OptidsLst.indexOf(option.OptionId) != -1) {
                    var currentIndex = question.SpecialOptinLst[index].OptidsLst.indexOf(option.OptionId);
                    question.SpecialOptinLst[index].OptidsLst.splice(currentIndex, 1);
                    $scope.counter--;
                }
                if ($scope.counter == 0) {
                    $rootScope.healthQstValidations = false;
                }
                else {
                    $rootScope.healthQstValidations = true;
                }

            }
            else {
                option.IsChecked = true;
                if (question.SpecialOptinLst[index].OptidsLst.indexOf(option.OptionId) == -1) {
                    if (question.SpecialOptinLst[index].OptidsLst.indexOf(1) != -1) {
                        var currentIndex = question.SpecialOptinLst[index].indexOf(1);
                        question.SpecialOptinLst[index].OptidsLst.splice(currentIndex, 1);
                        $scope.counter--;
                    }
                    question.SpecialOptinLst[index].OptidsLst.push(option.OptionId);
                    $scope.counter++;
                    if ($scope.counter == 0) {
                        $rootScope.healthQstValidations = false;
                    }
                    else {
                        $rootScope.healthQstValidations = true;
                    }
                }
            }
        }
        function getIndex(question, op) {
            angular.forEach(question.SpecialOptinLst, function (optlst, index) {
                angular.forEach(optlst, function (opt) {
                    if (opt.OptionId == op.OptionId) {
                        parentindex = index;
                        return false;
                    }
                });
            });
        }
    });

});