define(['angular'], function (angular) {
    //main module of the soltion
    var gmApp = angular.module("gmApp", ['ngSanitize', 'rzModule', 'spqstModule', 'pascalprecht.translate', 'customSerivces'])
    //config all required providers.(ex: color palette, routing, register all providers
    gmApp.config(['$locationProvider', '$httpProvider', '$controllerProvider', '$provide', '$translateProvider',
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
        gmApp.register = {
            controller: $controllerProvider.register,
            factory: $provide.factory,
            service: $provide.service
        };

    }]);
    gmApp.run(function ($rootScope, $location, $window, $http, lazyscriptLoader) {
        $rootScope.psQstLoad = false;
        $rootScope.gmPageLoad = false;
        $rootScope.isShowQuestions = false;
        $rootScope.isShowFooter = false;
        if (document.cookie != "") {
            $rootScope.orgLogo = document.cookie.split('=')[1];
        }

        var url = window.location.pathname;
        var pgName = url.split("/")[2].toLocaleLowerCase();

        if (pgName == 'q') {
            lazyscriptLoading('/Scripts/lead/q.js');
        }
        else if (pgName == 'start') {
            lazyscriptLoading('/Scripts/lead/start.js')
        }
        else if (pgName == 'end') {
            lazyscriptLoading('/Scripts/lead/end.js')
        }
        function lazyscriptLoading(currentUrl) {
            lazyscriptLoader.load(currentUrl).then(function () {
                $rootScope.gmPageLoad = true;
            });
        }
    });
    //Menu controller section
    gmApp.controller("menuController", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout',
    function ($scope, $http, $window, $location, $rootScop, $timeout) {
        $scope.languageSelected = true;
        $scope.selectedLangCode = 0;

    }]);
    gmApp.controller("footerController", ['$scope', '$http', '$window', '$location', function ($scope, $http, $window, $location) {
        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
        //privacy click
        $scope.privacyClick = function () {
            $window.open('http://marketplace-insider.com/privacy.html', 'marketplace-insider-Privacy', 'width=800,height=800')
        }
        //terms Click
        $scope.termsClick = function () {
            $window.open('http://marketplace-insider.com/terms.html', 'marketplace-insider-Privacy', 'width=800,height=800')
        }
    }]);
    return gmApp;
});
