define(['psApp'], function (psApp) {
    psApp.register.controller("ddlController", function ($scope, $compile) {
        //if ($scope.question.QuestionId == 175) {
        //    getVechileChildQuestions($scope.question);
        //}
        $scope.selectionChanged = function (qst, value) {
            qst.OptionId = value;
            // qst.ChqOptionId = value;
            if (qst.ChildQuestionList != null) {
                childQuestionInclude($scope.questions, qst, value);
                //$scope.submitValidations = false;
            }
        }
        function childQuestionInclude(question, qst, optid) {
            if (optid != null && optid != '' && optid != undefined) {
                var OptionId = optid;
                var childQuestions = [];
                var childOptions = [];
                angular.forEach(qst.OptionList, function (op) {
                    if (op.OptionId == OptionId) {
                        angular.forEach(qst.ChildQuestionList, function (chqst) {
                            var Count = 0;
                            var QuestionId = 0;
                            var ChQuestionId = 0;
                            angular.forEach(op.ListChildQuestionId, function (lstchid) {
                                ChQuestionId = lstchid;
                                QuestionId = chqst.QuestionId;
                                if (QuestionId == ChQuestionId) {
                                    Count = 1;
                                }
                            });
                            //this is for mapped data
                            if (Count == 0) {
                                childQuestions.push(chqst);
                                angular.forEach(chqst.OptionList, function (chqop) {
                                    if (chqst.QuestionId == chqop.QuestionId) {
                                        childOptions.push(chqop);
                                    }
                                });
                            }

                        });
                    }
                });
                //Bind the Parentquestions selected child questionlist
                angular.forEach(question, function (qstlst) {
                    if (qstlst.QuestionId == qst.QuestionId) {
                        qst.SelectedChildQuestionList = childQuestions;
                        angular.forEach(qst.SelectedChildQuestionList, function (selectqst) {
                            selectqst.OptionList = [];
                            angular.forEach(childOptions, function (chop) {
                                // for (var q = 0; q < childOptions.length; q++) {
                                if (selectqst.QuestionId == chop.QuestionId) {
                                    selectqst.OptionList.push(chop);
                                }
                            });

                        });
                    }
                        //Bind the childquestion selected subchildquestionlist
                    else {
                        angular.forEach(qstlst.ChildQuestionList, function (ch) {
                            if (ch.QuestionId == qst.QuestionId) {
                                ch.SelectedChildQuestionList = childQuestions;
                                if (ch.SelectedChildQuestionList != 0) {
                                    angular.forEach(ch.SelectedChildQuestionList, function (chqselect) {
                                        chqselect.OptionList = [];
                                        angular.forEach(qst.SubChildOptions, function (subchop) {
                                            if (optid == subchop.ParentOptionId) {
                                                chqselect.OptionList.push(subchop);
                                            }
                                        });

                                    });

                                }
                            }
                        });

                    }
                });
            }
        }
    });

});