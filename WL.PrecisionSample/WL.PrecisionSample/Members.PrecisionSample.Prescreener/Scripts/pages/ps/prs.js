define(['psApp'], function (psApp) {
    psApp.register.controller("userPsQuestionsController", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'loadQuestionFilter', 'questionService', 'specialQuestionsService', 'getQueryParams', 'translationsLoadingService', 'loggerService',
    function ($rootScope, $scope, $http, $window, httpService, loadQuestionFilter, questionService, specialQuestionsService, getQueryParams, translationsLoadingService, loggerService) {
        translationsLoadingService.writeNlogService();
        var ug = getQueryParams.getUrlVars()['ug'];
        if (ug == undefined) {
            ug = '';
        }
        var cid = getQueryParams.getUrlVars()['cid'];
        if (cid == undefined) {
            cid = '';
        }
        var uig = getQueryParams.getUrlVars()['uig'];
        if (uig == undefined) {
            uig = '';
        }

        var pid = getQueryParams.getUrlVars()["pid"];
        if (pid == undefined || pid == "undefined" || pid == null || pid == '') {
            pid = '';
        }
        var river = getQueryParams.getUrlVars()["isr"]; //river identification 
        if (river == undefined || river == "undefined" || river == null || river == '') {
            river = '';
        }
        var pstest = getQueryParams.getUrlVars()["pstest"];
        if (pstest == undefined) {
            pstest = 0;
        }
        var lid = getQueryParams.getUrlVars()["lid"];
        if (lid == undefined || lid == null || lid == '') {
            lid = 114282;
        }
        else {
            if (parseInt(pstest) == 1) {
            }
            else {
                pstest = 0;
            }
        }
        var conid = getQueryParams.getUrlVars()['conid'];
        if (conid == undefined) {
            conid = '';
        }
        var lan = languagelst;
        $scope.isButtonDisabled = false;
        $scope.submitValidations = false;
        $scope.gdprValidations = false;
        $scope.MinandMaxErrorMsg = false;
        $scope.gdprCheck = false;
        $scope.InvitationGuid = "";
        $scope.gdprRadio = 0;
        $scope.gdprChange = function (gdprRadio) {
            $scope.gdprRadio = gdprRadio;
        }
        var getQstResponse = function (res) {
            $scope.isButtonDisabled = false;
            $scope.gdprRadio = 0;
            if (res.length > 0) {
                //redirect url is not null
                if (res[0].RedirectUrl != "" && res[0].RedirectUrl != null) {
                    window.location.href = res[0].RedirectUrl + '&pid=' + pid;
                }
                else {
                    //redirect url is null and and questionid is -1 then member redirect to next survey
                    if (res[0].QuestionId == -1) {
                        $scope.redirectNextSurvey(res);
                    }
                    else {
                        angular.forEach(res.data, function (qst) {
                            if (qst.QuestionId == 1568) { //special matricx qustions
                                document.getElementById("dvSurveyQuestions").style.maxWidth = "100%";
                            }
                            else {
                                document.getElementById("dvSurveyQuestions").style.maxWidth = "850px";
                            }

                        });
                        $rootScope.isShowQuestions = true;
                        $rootScope.isShowFooter = true;
                        $scope.questions = specialQuestionsService.questions(res);
                        $scope.questionId = $scope.questions[0].QuestionId
                        $scope.cid = cid;
                        $scope.conid = conid;
                    }
                }
            }
            //else {
            //    if ($rootScope.isStandalonePartner == 't') { // Means if the Partner is Stand alone Partner.

            //        window.location.href = "http://sdl.precisionsample.com/e/psr.aspx?usg=2B9038B6-DB53-429A-8854-7BB83338B2D4&ug=" + ug + "&uig=" + uig + "&project=" + prjid;
            //    }
            //    else {
            //        window.location.href = '/ps/rep.html?ug=' + ug + '&uig=' + uig + '&lid=' + lid + "&project=" + prjid;
            //    }
            //}
        }
        httpService.getData('/prs/getquestions?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&lngId=' + lid).then(function (res) {
            getQstResponse(res);

            if (lid > 0) {
                angular.forEach(lan, function (ln) {
                    if (lid == ln.OptionId) {
                        $scope.buttontext = ln.OptionText;
                        $scope.gdprvalidate1 = ln.gdprvalidate1;
                        $scope.gdprvalidate2 = ln.gdprvalidate2;
                        $scope.gdprvalidate3 = ln.gdprvalidate3;
                        $scope.gdprvalidate4 = ln.gdprvalidate4;
                        $scope.gdprvalidate5 = ln.gdprvalidate5;
                        $scope.errorMessage = ln.txtboxval;
                        $scope.errorEmail = ln.emailaddressvld;
                        $scope.errorAge = ln.agevld;
                        $rootScope.footertex = ln.footer;
                        $rootScope.privacy = ln.privacy;
                        $rootScope.powerby = ln.powerby;
                    }
                });
            }
            else {
                $scope.buttontext = lan[0].OptionText;
                $scope.gdprvalidate1 = lan[0].gdprvalidate1;
                $scope.gdprvalidate2 = lan[0].gdprvalidate2;
                $scope.gdprvalidate3 = lan[0].gdprvalidate3;
                $scope.gdprvalidate4 = lan[0].gdprvalidate4;
                $scope.gdprvalidate5 = lan[0].gdprvalidate5;
                $scope.errorMessage = lan[0].ln.txtboxval;
                $scope.errorEmail = lan[0].ln.emailaddressvld;
                $scope.errorAge = lan[0].ln.agevld;
                $rootScope.footertex = lan[0].footer;
                $rootScope.privacy = lan[0].privacy;
                $rootScope.powerby = lan[0].powerby;
            }
        });
        $scope.save = function (validate) {
            if (validate) {
                var sortOrder = 0;
                $scope.gdprCheck = false;
                $scope.MinandMaxErrorMsg = false;
                var xml = questionService.buildXml($scope.questions);
                var qId = $scope.questions[0].QuestionId;
                if ((qId == 1638 || qId == 573 || qId == 580 || qId == 850 || qId == 4667) && $scope.gdprRadio == 0 && (cid == 337 || cid == 491)) {
                    $scope.gdprCheck = true;
                }
                if ((qId == 1638 || qId == 573 || qId == 580 || qId == 850 || qId == 4667) && $scope.gdprRadio == 0 && cid != 337 && cid != 491 &&
                    (conid == 505 || conid == 512 || conid == 525 || conid == 545 || conid == 548 || conid == 549 || conid == 550 || conid == 559 ||
                    conid == 564 || conid == 565 || conid == 572 || conid == 575 || conid == 590 || conid == 596 || conid == 599 || conid == 613 ||
                    conid == 618 || conid == 619 || conid == 620 || conid == 628 || conid == 647 || conid == 657 || conid == 668 || conid == 669 ||
                    conid == 673 || conid == 693 || conid == 694 || conid == 700 || conid == 706)) {
                    $scope.gdprCheck = true;
                }
                if ($scope.gdprCheck == false) {
                    if ($scope.questions[0].CurrentSortOrder != 0) {
                        sortOrder = $scope.questions[0].CurrentSortOrder;
                    }
                    var count = 0
                    if ($scope.questions[0].MinQuestionsCount > 0 || $scope.questions[0].MaxQuestionsCount > 0) {
                        angular.forEach($scope.questions[0].OptionList, function (qst) {
                            if (qst.IsChecked == true) {
                                count = count + 1;
                            }
                            if (qst.OptionId == 138777) {
                                count = $scope.questions[0].MinQuestionsCount
                            }
                        });
                    }
                    var data = { 'xml': xml };
                    if (count >= $scope.questions[0].MinQuestionsCount && count <= $scope.questions[0].MaxQuestionsCount) {
                        $scope.isButtonDisabled = true;
                        httpService.postData('/prs/saveUserprescreeneroptions?ug=' + ug + '&uig=' + uig + '&qId=' + qId + '&sr=' + sortOrder + '&lngId=' + lid + '&cid=' + cid + '&gdprRadio=' + $scope.gdprRadio, data).then(function (res) {
                            $scope.submitValidations = false;
                            $scope.gdprValidations = false;
                            $scope.radiobtnvalidation = -1;
                            $rootScope.psQstLoad = false;
                            // member not having pending questions
                            if (res[0].QuestionId == -1) {
                                //member not terminated then redirect to redreict url
                                $scope.redirectNextSurvey(res);
                            }
                            else {
                                //memer has pending questions
                                getQstResponse(res);
                            }
                        });
                    }
                    else {
                        $scope.MinandMaxErrorMsg = true;
                    }
                }
                else {
                    $scope.gdprValidations = true;
                }
            }
            else {
                $scope.submitValidations = true;
            }
        }

        //redirect to next survey
        $scope.redirectNextSurvey = function (res) {
            if (res[0].SessionCount == 0) {
                if (res[0].RedirectUrl != "" && res[0].RedirectUrl != null) {
                    if (uig == ug)
                        window.location.href = res[0].RedirectUrl + '&pid=' + pid;
                    else
                        window.location.href = res[0].RedirectUrl + '&pid=' + pid;
                }
                else {
                    //Pass the Step 6 GUID and get the Final GUID.
                    httpService.getData('/prs/GetInvitationGuid?uig=' + uig + '&cid=' + cid).then(function (dataResponse) {
                        if (dataResponse.UserInvitationGuid != "" && dataResponse.UserInvitationGuid != undefined) {
                            $scope.InvitationGuid = dataResponse.UserInvitationGuid;
                            //   alert($scope.InvitationGuid);
                            //memeber has been terminated then redirect to next survey
                            //Here we are passing User Invitation GUID.
                            httpService.getData('/prs/takeanothersurvey?ug=' + ug + '&cid=' + cid + '&uig=' + dataResponse.UserInvitationGuid).then(function (dataResponse1) {
                                if (dataResponse1.SurveyUrl != "" && dataResponse1.SurveyUrl != null) {
                                    window.location.href = dataResponse1.SurveyUrl + '&osid=' + uig;
                                }
                                if (cid == 110) {
                                    window.location.href = 'http://opt.opinionetwork.com/river/rfep.aspx?ug=' + ug;
                                }
                                else {
                                    window.location.href = 'https://e.opinionetwork.com/e/psr?usg=2B9038B6-DB53-429A-8854-7BB83338B2D4&uig=' + dataResponse.UserInvitationGuid;
                                }
                            })
                        }
                    })
                }
            }
            else {

                //Here we are passing Step6 GUID.
                //member has previous survey terminated and current prescreen successed then redirect to top survey. 
                window.location.href = '/prs/rep?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid;
            }
        }
    }]);
});