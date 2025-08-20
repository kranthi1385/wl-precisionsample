define(['psApp'], function (psApp) {
    psApp.register.controller("zipController", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'loadQuestionFilter', 'questionService', 'specialQuestionsService', 'getQueryParams', 'translationsLoadingService', 'loggerService',
    function ($rootScope, $scope, $http, $window, httpService, loadQuestionFilter, questionService, specialQuestionsService, getQueryParams, translationsLoadingService, loggerService) {
        debugger;
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
        var river = getQueryParams.getUrlVars()["river"];
        if (river == undefined) {
            river = '';
        }
        var lid = getQueryParams.getUrlVars()["lid"];
        if (lid == undefined || lid == null || lid == '') {
            lid = 0;
        }
        var zip = getQueryParams.getUrlVars()["zip"];
        if (zip == undefined || zip == "null" || zip == '') {
            zip = 0;
        }
        var pstest = getQueryParams.getUrlVars()["pstest"];
        if (pstest == undefined) {
            pstest = 0;
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
        $scope.question = {
            OptionText: ""
        }
        $scope.submitValidations = false;
        $rootScope.psQstLoad = false;
        var lan = languagelst;
        $scope.InvitationGuid = "";
        if (lid > 0) {
            angular.forEach(lan, function (ln) {
                if (lid == ln.OptionId) {
                    $scope.buttontext = ln.OptionText;
                    $rootScope.footertex = ln.footer;
                    $rootScope.privacy = ln.privacy;
                    $rootScope.powerby = ln.powerby;
                }
            });
        }
        else {
            $scope.buttontext = lan[0].OptionText;
            $rootScope.footertex = lan[0].footer;
            $rootScope.privacy = lan[0].privacy;
            $rootScope.powerby = lan[0].powerby;
        }
        $scope.save = function () {
            $scope.submitValidations = false;
            if ($scope.question.OptionText != "" || zip != 0) {
                if ($scope.question.OptionText == "") {
                    $scope.question.OptionText = zip;
                }
                //var objData = data.replaceAll("\"","");
                httpService.postData('/prs/SaveZipcode?ug=' + ug + '&uig=' + uig + '&zip=' + $scope.question.OptionText + '&cid=' + cid).then(function (res) {
                    $scope.submitValidations = false
                    $rootScope.psQstLoad = false;
                    //member not terminated then redirect to redreict url
                    //if (res == "0") {
                    //    res = 'https://e.opinionetwork.com/e/interstitial?ug=' + ug + '&uig=' + uig + '&sr=' + sr + '&cid=' + cid + '&cc=' + cc + '&fc=' + fc;
                    //}
                    if (res.ZipRadius > 400) {
                        if (lid == 114282) {
                            //Show the Step1 Penidng Question.
                            window.location.href = '/prs/Addvld?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&conid=' + conid + '&auig=' + res.ActualInvitationGuid;
                        }
                        else if (lid != 114282) {
                            window.location.href = '/prs/Addvld?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + lid + '&conid=' + conid + '&auig=' + res.ActualInvitationGuid;
                        }
                    }
                    else {
                        if (res.RedirectUrl == "0" & lid == 114282) {
                            //Show the Step1 Penidng Question.
                            window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&conid=' + conid;
                        }
                        else if (res.RedirectUrl == "0" & lid != 114282) {
                            window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + lid + '&conid=' + conid;
                        }
                        else {
                            window.location.href = res.RedirectUrl;
                        }
                    }
                });
            }
            else {
                $scope.submitValidations = true;
            }
        }

        if (zip == 0) {
            $rootScope.isShowQuestions = true;
            $rootScope.isShowFooter = true;
        }
        else {
            $scope.save();
        }
    }]);
});