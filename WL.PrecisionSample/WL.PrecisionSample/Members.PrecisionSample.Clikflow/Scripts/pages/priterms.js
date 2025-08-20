define(['app'], function (app) {

    app.register.controller('priTermsController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'translationsLoadingService', '$interval', '$cookies',
        function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, translationsLoadingService, $interval, $cookies) {

            var uig = getQueryParams.getUrlVars()['uig'];
            var ug = getQueryParams.getUrlVars()['ug'];
            var pid = getQueryParams.getUrlVars()["pid"];
            var cid = getQueryParams.getUrlVars()["cid"];
            var cstrId = getQueryParams.getUrlVars()["cstring"];
            var lid = getQueryParams.getUrlVars()["lid"];
            var dvtype = getQueryParams.getUrlVars()["dvtype"];
            var uid = getQueryParams.getUrlVars()["uid"];
            if (pid == undefined) {
                pid = '';
            }
            tid = getQueryParams.getUrlVars()["tid"];
            if (tid == undefined) {
                tid = '';
            }
            usid = getQueryParams.getUrlVars()["usid"];
            if (usid == undefined) {
                usid = '';
            }
            if (cid == undefined) {
                cid = '';
            }
            $scope.showloading = true; // loading iamge flag
            $scope.gdprTerms = false;
            $scope.gdprErrorMsg = false;
            $scope.OrgID = cid;

            if (document.cookie != "") {
                $rootScope.orgLogo = getorgLogo("orgLogo");
            }

            function getorgLogo(name) {
                var re = new RegExp(name + "=([^;]+)");
                var value = re.exec(document.cookie);
                return (value != null) ? unescape(value[1]) : null;
            }
            var now = new $window.Date(),
                exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
            if (lid != '' && lid != undefined) {
                $cookies.put('LangCode', lid, {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
            }
            var preloader = document.getElementsByClassName('preloader')[0].style.visibility;
            var pri_container = document.getElementById('pri_container');
            $scope.showloading = false;
            $scope.isShowLogo = false;
            // get current doamin details
            httpService.getData("/cr/GetCurrentDomainDetails?cid=" + cid).then(function (res) {
                $scope.orgDetails = res;
                if ($scope.orgDetails.OrgLogo == "http://www.opinionetwork.com" || $scope.orgDetails.OrgLogo == null)
                    $scope.isShowLogo = true;
                pri_container.style.visibility = 'visible';
                preloader = 'hidden';
            }, function (err) {
            });
            $scope.gdprClick = function () {
                if ($scope.gdprTerms == true) {
                    pri_container.style.visibility = 'hidden';
                    preloader = 'visible';
                    $scope.gdprErrorMsg = false;
                    httpService.postData("/reg/gdprCompliancesave?ug=" + ug + "&usid=" + usid + "&cid=" + cid + "&uig=" + uig + "&uid=" + uid + "&dvtype=" + dvtype).then(function (res) {
                        window.location.href = res;
                    });
                }
                else {
                    $scope.gdprErrorMsg = true;
                }
            }

            //privacy policy link click open  popup
            $scope.privacy = function () {
                if (cid == 337 || cid == 491) {
                    urlPrivacy = 'https://opinionbar.com/Footer/Privacy';
                }
                else {
                    urlPrivacy = 'https://opinionetwork.com/WidgetPrivacy.html';
                }
                var width = 700;
                var height = 700;
                var left = (screen.width - width) / 2;
                var top = (screen.height - height) / 2;
                var params = 'width=' + width + ', height=' + height;
                params += ', top=' + top + ', left=' + left;
                params += ', directories=no';
                params += ', location=no';
                params += ', menubar=no';
                params += ', resizable=no';
                params += ', scrollbars=yes';
                params += ', status=no';
                params += ', toolbar=no';
                newwin = window.open(urlPrivacy, 'windowname5', params);
                if (window.focus) { newwin.focus() }
                return false;
            }
            //terms link click open  popup
            $scope.terms = function () {
                if (cid == 337 || cid == 491) {
                    urlPrivacy = 'https://opinionbar.com/Footer/TC';
                }
                else {
                    urlPrivacy = 'https://opinionetwork.com/WidgetTerms.html';
                }
                var width = 700;
                var height = 700;
                var left = (screen.width - width) / 2;
                var top = (screen.height - height) / 2;
                var params = 'width=' + width + ', height=' + height;
                params += ', top=' + top + ', left=' + left;
                params += ', directories=no';
                params += ', location=no';
                params += ', menubar=no';
                params += ', resizable=no';
                params += ', scrollbars=yes';
                params += ', status=no';
                params += ', toolbar=no';
                newwin = window.open(urlPrivacy, 'windowname5', params);
                if (window.focus) { newwin.focus() }
                return false;
            }
        }])
});