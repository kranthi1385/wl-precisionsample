/// <reference path="../pages/main/hm.js" />
/// <reference path="../pages/main/hm.js" />
define(['angular'], function (angular) {
    //main module of the soltion
    var rgApp = angular.module("rgApp", ['ngSanitize', 'pascalprecht.translate', 'customSerivces'])
    //config all required providers.(ex: color palette, routing, register all providers
    rgApp.config(['$locationProvider', '$httpProvider', '$controllerProvider', '$provide', '$translateProvider',
    function ($locationProvider, $httpProvider, $controllerProvider, $provide, $translateProvider) {
        //loading image interceptor
        //$httpProvider.interceptors.push('httpInterceptor');
        //add header for each  request to identify AntiForgeryToken
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $translateProvider.useLoader('$translatePartialLoader', {
            urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
        });
        //add default language
        $translateProvider.preferredLanguage('en');
        rgApp.register = {
            controller: $controllerProvider.register,
            factory: $provide.factory,
            service: $provide.service
        };

    }]);
    rgApp.run(function ($rootScope, $location, $window, $http, lazyscriptLoader) {
        //$http.defaults.headers.common['X-XSRF-Token'] =
        //angular.element('input[name="__RequestVerificationToken"]').attr('value');
        var url = window.location.pathname;
        var pgName = url.split("/")[2].toLocaleLowerCase();
        if (pgName == 'step2') {
            lazyscriptLoading('/Scripts/pages/rg/step2.js')
        }
        if (pgName == 'step1') {
            lazyscriptLoading('/Scripts/pages/rg/step1.js')
        }
        function lazyscriptLoading(currentUrl) {
            lazyscriptLoader.load(currentUrl).then(function () {
                $rootScope.pgLoad = true;
                // if (pgName == 'step2') {
                //    $rootScope.getPartialView = "/PartialViews/Rg/step2.html"
                //}
            });
        }
    });
    //Menu controller section
    rgApp.controller("menuController", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'lazyscriptLoader', 'translationsLoadingService', 'httpService', 'organizationService',
    function ($scope, $http, $window, $location, $rootScop, $timeout, lazyscriptLoader, translationsLoadingService, httpService, organizationService) {
        $scope.languageSelected = true;
        $scope.selectedLangCode = 0;
        var languageCodesLst = [];
        //set all avaliable languages
        languageCodesLst = [
            { LangName: "English", Img: "//static.miniclipcdn.com/layout/flags/46x32/US.png", LangCode: "en" },
            { LangName: "Español", Img: "//static.miniclipcdn.com/layout/flags/46x32/ES.png", LangCode: "es" },
           { LangName: "Português", Img: "//static.miniclipcdn.com/layout/flags/46x32/PT.png", LangCode: "pt" }
        ]
        $scope.languageCodes = languageCodesLst;
        $scope.selectedImg = languageCodesLst[0].Img;

        $scope.languageChange = function () {
            $scope.languageSelected = !$scope.languageSelected;

        }
        //with respective of user selected language set tanslation language  
        $scope.selectLanaguage = function (index, selectedLanguage) {
            $scope.selectedImg = selectedLanguage.Img;
            $scope.selectedLangCode = index;
            $scope.languageSelected = !$scope.languageSelected;
            translationsLoadingService.setCurrentUserLanguage(selectedLanguage.LangCode);
        }


    }]);
    rgApp.controller("footerController", ['$scope', '$http', '$window', '$location', function ($scope, $http, $window, $location) {
        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
    }]);
    return rgApp;
});
