define(['gmApp'], function (gmApp) {
    gmApp.register.controller("userGmQuestionsController", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'loadQuestionFilter', 'questionService', 'specialQuestionsService', 'getQueryParams', 'translationsLoadingService',
    function ($rootScope, $scope, $http, $window, httpService, loadQuestionFilter, questionService, specialQuestionsService, getQueryParams, translationsLoadingService) {
        var lid = getQueryParams.getUrlVars()['lid'];
        if (lid == undefined) {
            lid = '';
        }
        var pid = getQueryParams.getUrlVars()['pid'];
        if (pid == undefined) {
            pid = '';
        }
        else {
            if (parseInt(pstest) == 1) {
            }
            else {
                pstest = 0;
            }
        }

        $scope.submitValidations = false;
        var getQstResponse = function (res) {
            if (res.length > 0) {
                //redirect url is not null
                if (res[0].RedirectUrl != "" && res[0].RedirectUrl != null) {
                    window.location.href = res[0].RedirectUrl;
                }
                else {
                    //redirect url is null and and questionid is -1 then member redirect to next survey
                    //if (res[0].QuestionId == -1) {
                    //    $scope.redirectNextSurvey(res);
                    //}

                    //else {
                    //    angular.forEach(res.data, function (qst) {
                    //        if (res[0].QuestionId == 4108) { //special matricx qustions
                    //            document.getElementById("dvSurveyQuestions").style.maxWidth = "100%";
                    //        }
                    //        else {
                    //            document.getElementById("dvSurveyQuestions").style.maxWidth = "850px";
                    //        }

                    //   });
                    $rootScope.isShowQuestions = true;
                    $rootScope.isShowFooter = true;
                    $scope.questions = specialQuestionsService.questions(res);


                }
            }
            //else {
            //    if ($rootScope.isStandalonePartner == 't') { // Means if the Partner is Stand alone Partner.

            //        window.location.href = "https://sdl.precisionsample.com/e/psr.aspx?usg=2B9038B6-DB53-429A-8854-7BB83338B2D4&ug=" + ug + "&uig=" + uig + "&project=" + prjid;
            //    }
            //    else {
            //        window.location.href = '/ps/rep.html?ug=' + ug + '&uig=' + uig + '&lid=' + lid + "&project=" + prjid;
            //    }
            //}

        }

        httpService.getData('/lead/getquestions?lid=' + lid).then(function (res) {
            getQstResponse(res);
        });
        $scope.save = function (validate) {
            if (validate) {
                var sortOrder = 0;
                var xml = questionService.buildXml($scope.questions);
                var qId = $scope.questions[0].QuestionId;
                if ($scope.questions[0].CurrentSortOrder != 0) {
                    sortOrder = $scope.questions[0].CurrentSortOrder;
                }
                var data = { 'xml': xml };
                httpService.postData('/lead/SaveUserPrescreenerOptions?lid=' + lid + '&qId=' + qId + '&sr=' + sortOrder, data).then(function (res) {
                    $scope.submitValidations = false;
                    $scope.radiobtnvalidation = -1;
                    $rootScope.psQstLoad = false;
                    // member not having pending questions
                    //memer has pending questions
                    getQstResponse(res);
                });
            }
            else {
                $scope.submitValidations = true;
            }
        }

        //redirect to next survey
        //$scope.redirectNextSurvey = function (res) {
        //    if (res[0].SessionCount == 0) {
        //        if (res[0].RedirectUrl != "" && res[0].RedirectUrl != null) {
        //            window.location.href = res[0].RedirectUrl + '&pid=' + pid;;
        //        }
        //        else {
        //            //memeber has been terminated then redirect to next survey
        //            httpService.getData('/lead/takeanothersurvey?lid=' + lid).then(function (dataResponse) {
        //                if (dataResponse.SurveyUrl != "" && dataResponse.SurveyUrl != null) {
        //                    window.location.href = dataResponse.SurveyUrl;
        //                }
        //            })
        //        }
        //    }
        //    else {
        //        //member has previous survey terminated and current prescreen successed then redirect to top survey. 
        //        window.location.href = '/gm/end?lid=' + lid;
        //    }
        //}
    }]);
});