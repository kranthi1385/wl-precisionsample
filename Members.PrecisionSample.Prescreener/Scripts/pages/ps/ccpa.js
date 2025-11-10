define(['psApp'], function (psApp) {
    psApp.register.controller("ccpaController", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'getQueryParams', 'translationsLoadingService',
    function ($rootScope, $scope, $http, $window, httpService, getQueryParams, translationsLoadingService) {
        translationsLoadingService.writeNlogService();
        //get query params
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
        var conid = getQueryParams.getUrlVars()['conid'];
        if (conid == undefined) {
            conid = '';
        }
        var cc = getQueryParams.getUrlVars()["cc"];
        if (cc == undefined || cc == "undefined" || cc == null || cc == '') {
            cc = 114282;
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

        // to get URL params
        var lan = languagelst;
        angular.forEach(lan, function (ln) {
            if (cc == ln.OptionId) {
                $scope.para1 = ln.para1;
                $scope.para2 = ln.para2;
                $scope.para3 = ln.para3;
                $scope.AccessMill = ln.AccessMill;
                $scope.ComScore = ln.ComScore;
                $scope.Resonate = ln.Resonate;
                $scope.yes = ln.yes;
                $scope.no = ln.no;
                $scope.iagree = ln.iagree;
                $scope.agreeto = ln.agreeto;
                $scope.privacy = ln.privacy;
                $scope.termofuse = ln.termofuse;
                $scope.OptionText = ln.OptionText;
                $rootScope.footertex = ln.footer;
                $rootScope.privacy = ln.privacy;
                $rootScope.powerby = ln.powerby;
                $scope.Tapad = ln.Tapad;
                $scope.para4 = ln.para4;
            }
        });

        httpService.getData('/prs/getCookieData?ug=' + ug + '&uig=' + uig + '&cid=' + cid).then(function (res) {
            $scope.user = res;
            if ($scope.user.AccessMillCookie == false) {
                $scope.user.AccessMillCookie = true;
            }
            if ($scope.user.ComScoreCookie == false) {
                $scope.user.ComScoreCookie = true;
            }
            if ($scope.user.ResonateCookie == false) {
                $scope.user.ResonateCookie = true;
            }
            if ($scope.user.TapadCookie == false) {
                $scope.user.TapadCookie = true;
            }
            $rootScope.isShowFooter = true;
        });

        $scope.SaveUserCookie = function () {
            httpService.postData('/prs/SaveUserCookie?ug=' + ug + '&cid=' + cid + '&resid=' + $scope.user.ResonateCookie + '&tapid=' + $scope.user.TapadCookie).then(function (res) {
                window.location.href = "/prs/psl?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&conid=" + conid;
            });
        }

        //privacy policy click
        $scope.privacyClick = function () {
            $window.open('http://prescreener.precisionsample.com/privacy.htm', 'SurveyDownline-Privacy', 'width=600,height=600')
        }

        //radio button click
        $scope.rdClick = function ($event, value, index) {
            $scope.selectedIndex = index;
            if (value.toLowerCase() == 'yes') {
                $scope.showmbNumber = true;
                $scope.submitValidations = false;
            }
            else {
                $scope.showmbNumber = false;
                $scope.submitValidations = false;

            }
            $scope.questions[0].OptionText = value;
        }
    }]);
});

