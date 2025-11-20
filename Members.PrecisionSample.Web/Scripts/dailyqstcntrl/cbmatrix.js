define(['app'], function (app) {
    app.register.controller("checkboxwithmatrixController", function ($scope, $compile) {
        $scope.checkboxClick = function (childQst, option) {
            checkMemberOption(childQst, option);
        }
        function checkMemberOption(question, option) {
            if (option.IsChecked) {
                option.IsChecked = false;
                if (question.SubChildOptions.indexOf(option.OptionId) != -1) {
                    var currentIndex = question.OptionList.indexOf(option.OptionId);
                    question.SubChildOptions.splice(currentIndex, 1);
                }

            }
            else {
                option.IsChecked = true;
                if (question.SubChildOptions.indexOf(option.OptionId) == -1) {
                    if (question.SubChildOptions.indexOf(1) != -1) {
                        var currentIndex = question.indexOf(1);
                        question.SubChildOptions.splice(currentIndex, 1);
                    }
                    question.SubChildOptions.push(option.OptionId);
                }
            }
        }

    });

});