define(['angular'], function (angular) {
    //main module of the soltion
    var psApp = angular.module("psApp", ['ngSanitize','rzModule', 'spqstModule', 'pascalprecht.translate', 'customSerivces'])
    //config all required providers.(ex: color palette, routing, register all providers
    psApp.config(['$locationProvider', '$httpProvider', '$controllerProvider', '$provide', '$translateProvider',
    function ($locationProvider, $httpProvider, $controllerProvider, $provide, $translateProvider) {
        //loading image interceptor
        //$httpProvider.interceptors.push('httpInterceptor');
        //add header for each  request to identify AntiForgeryToken
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $httpProvider.interceptors.push('loadingInterceptorService');
        $translateProvider.useLoader('$translatePartialLoader', {
            urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
        });
        //add default language
        $translateProvider.preferredLanguage('en');
        psApp.register = {
            controller: $controllerProvider.register,
            factory: $provide.factory,
            service: $provide.service
        };

    }]);
    psApp.run(function ($rootScope, $location, $window, $http, lazyscriptLoader) {
        $rootScope.psQstLoad = false;
        $rootScope.psPageLoad = false;
        $rootScope.isShowQuestions = false;
        $rootScope.isShowFooter = false;
        $rootScope.isShowBeraHeader = false;
        if (document.cookie != "") {
            $rootScope.orgLogo = getorgLogo("orgLogo");
        }

        function getorgLogo(name) {
            var re = new RegExp(name + "=([^;]+)");
            var value = re.exec(document.cookie);
            return (value != null) ? unescape(value[1]) : null;
        }
        function getUrlVars() {
            var vars = {};
            var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                vars[key] = value;
            });
            return vars;
        }
        // for local blox
        $rootScope.cidd = getUrlVars()["cid"];
     
       
        var url = window.location.pathname;
        var pgName = url.split("/")[2].toLocaleLowerCase();

        if (pgName == 'umq') {
            lazyscriptLoading('/Scripts/pages/ps/umq.js');
        }
        else if (pgName == "pp") {
            if ($rootScope.cidd == 450 || $rootScope.cidd == 491) {
                $rootScope.isShowOrgLogo = false;
            }
            else {
                $rootScope.isShowOrgLogo = true;
            }
            lazyscriptLoading('/Scripts/pages/ps/pp.js')
        }
        else if (pgName == "psl") {
            if ($rootScope.cidd == 450 || $rootScope.cidd == 491) {
                $rootScope.isShowOrgLogo = false;
            }
            else {
                $rootScope.isShowOrgLogo = true;
            }
            lazyscriptLoading('/Scripts/pages/ps/psl.js')
        }
        else if (pgName == "prsqst") {
            if ($rootScope.cidd == 450 || $rootScope.cidd == 491) {
                $rootScope.isShowOrgLogo = false;
            }
            else {
                $rootScope.isShowOrgLogo = true;
            }
           
            lazyscriptLoading('/Scripts/pages/ps/prs.js')
        }
        else if (pgName == "zip") {
            if ($rootScope.cidd == 450 || $rootScope.cidd == 491) {
                $rootScope.isShowOrgLogo = false;
            }
            else {
                $rootScope.isShowOrgLogo = true;
            }
            lazyscriptLoading('/Scripts/pages/ps/zip.js')
        }
        else if (pgName == "rep") {
            lazyscriptLoading('/Scripts/pages/ps/rep.js')
        }
        else if (pgName == "ccpa") {
            if ($rootScope.cidd == 450 || $rootScope.cidd == 491) {
                $rootScope.isShowOrgLogo = false;
            }
            else {
                $rootScope.isShowOrgLogo = true;
            }
            lazyscriptLoading('/Scripts/pages/ps/ccpa.js')
        }
        else if (pgName == "addvld") {
            if ($rootScope.cidd == 450 || $rootScope.cidd == 491) {
                $rootScope.isShowOrgLogo = false;
            }
            else {
                $rootScope.isShowOrgLogo = true;
            }

            lazyscriptLoading('/Scripts/pages/ps/AddVld.js')
        }
        function lazyscriptLoading(currentUrl) {
            lazyscriptLoader.load(currentUrl).then(function () {
                $rootScope.psPageLoad = true;
            });
        }
    });
    //Menu controller section
    psApp.controller("menuController", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout',
    function ($scope, $http, $window, $location, $rootScop, $timeout) {
        $scope.languageSelected = true;
        $scope.selectedLangCode = 0;

    }]);
    psApp.controller("footerController", ['$scope', '$http', '$window', '$location', function ($scope, $http, $window, $location) {

        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
        //privacy policy click
        $scope.privacyClick = function () {
            $window.open('http://prescreener.precisionsample.com/privacy.htm', 'SurveyDownline-Privacy', 'width=600,height=600')
        }
    }]);
    return psApp;
});
