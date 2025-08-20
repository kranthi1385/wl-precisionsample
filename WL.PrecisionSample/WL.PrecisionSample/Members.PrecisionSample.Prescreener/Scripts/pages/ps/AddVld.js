define(['psApp'], function (psApp) {
    psApp.register.controller("addressVldController", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'loadQuestionFilter', 'questionService', 'specialQuestionsService', 'getQueryParams', 'translationsLoadingService', 'loggerService',
    function ($rootScope, $scope, $http, $window, httpService, loadQuestionFilter, questionService, specialQuestionsService, getQueryParams, translationsLoadingService, loggerService) {
        debugger;
        $rootScope.isShowFooter = true;
        $scope.isRegErrMsg = false;
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
        var auig = getQueryParams.getUrlVars()['auig'];
        if (auig == undefined) {
            auig = '';
        }

        httpService.getData('/prs/GetCountrysAndStates').then(function (response) {
            $scope.countries = response.CountryList;
            $rootScope.stateslist = response.StateList;
        }, function (err) {
            // $scope.errMsg = true;
        });
        //get all avaliables states
        $scope.countryByStates = function (cc) {
            $scope.states = [];
            for (var i = 0; i < $rootScope.stateslist.length; i++) {
                if (cc == $rootScope.stateslist[i].CountryId) {
                    $scope.states.push($rootScope.stateslist[i]);

                }
            }
            //validateOpt();
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
        //$scope.user = {
        //    Address1: "",
        //    Address2: "",
        //    CountryId: -2,
        //    StateId: -2,
        //    city: "",
        //    ZipCode: ""
        //}
        $scope.saveandContinue = function (valid) {
            $scope.isRegErrMsg = false;
            if (valid) {
                httpService.postData('/prs/SaveUserGeoData?cid=' + cid + '&ug=' + ug + '&uig=' + uig, $scope.user).then(function (res) {
                    $rootScope.psQstLoad = false;
                    if (res == "0" & lid == 114282) {
                        //Show the Step1 Penidng Question.
                        window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&conid=' + conid;
                    }
                    else if (res == "0" & lid != 114282) {
                        window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + lid + '&conid=' + conid;
                    }
                    else {
                        window.location.href = res;
                    }
                });
            }
            else {
                $scope.isRegErrMsg = true;
            }
        }
        $scope.continue = function () {
            window.location.href = 'https://e.opinionetwork.com/e/psr?usg=3452B0B1-3209-471C-AEB3-63F1A616D4F3&uig=' + auig
        }

    }]);
});