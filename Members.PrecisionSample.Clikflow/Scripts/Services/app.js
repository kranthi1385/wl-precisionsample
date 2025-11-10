define(['angular'], function (angular) {
    //main module of the soltion
    var app = angular.module("app", ['ngSanitize', 'pascalprecht.translate', 'customSerivces', 'staticTranslationsModule', 'partialModule', 'ngCookies'])
    //config all required providers.(ex: color palette, routing, register all providers
    app.config(['$locationProvider', '$httpProvider', '$controllerProvider', '$provide', '$translateProvider', 'translatePluggableLoaderProvider',
    function ($locationProvider, $httpProvider, $controllerProvider, $provide, $translateProvider, translatePluggableLoaderProvider) {
        //loading image interceptor
        //$httpProvider.interceptors.push('httpInterceptor');
        $httpProvider.interceptors.push('loadingInterceptorService');
        //add header for each  request to identify AntiForgeryToken
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $translateProvider.useLoader('translatePluggableLoader');
        //add default language
        // $translateProvider.preferredLanguage('en');
        app.register = {
            controller: $controllerProvider.register,
            factory: $provide.factory,
            service: $provide.service
        };

    }]);
    app.run(function ($rootScope, $location, $window, $http, lazyscriptLoader) {
        //$http.defaults.headers.common['X-XSRF-Token'] =
        //angular.element('input[name="__RequestVerificationToken"]').attr('value');
        var url = window.location.pathname;
        var pgName = url.split("/")[2].toLocaleLowerCase();

        if (pgName.toLowerCase() == 'start') {
            lazyscriptLoading(['/Scripts/pages/cr.js?v=' + version])
        }
        if (pgName.toLowerCase() == 'pii') {
            lazyscriptLoading(['/Scripts/pages/reg.js?v=' + version])
        }
        if (pgName.toLowerCase() == 'v5') {
            lazyscriptLoading(['/Scripts/pages/cv.js?v=' + version])
        }
        else if (pgName.toLowerCase() == 'v6') {
            lazyscriptLoading(['/Scripts/pages/cvq.js?v=' + version])
        }
        else if (pgName == 'surveys') {
            lazyscriptLoading(['/Scripts/pages/main/ms.js'])
        }       
        if (pgName.toLowerCase() == 'valid') {
            lazyscriptLoading(['/Scripts/pages/vld.js?v=' + version])
        }
        if (pgName.toLowerCase() == 'relevent') {
            lazyscriptLoading(['/Scripts/pages/r.js?v=' + version])
        }
        if (pgName.toLowerCase() == 'sms') {
            lazyscriptLoading(['/Scripts/pages/sms.js?v=' + version])
        }
        if (pgName.toLowerCase() == 'priterms') {
            lazyscriptLoading(['/Scripts/pages/priterms.js?v=' + version])
        }

        function lazyscriptLoading(currentUrl) {
            lazyscriptLoader.load(currentUrl).then(function () {
                //set pageload true because before js download cshtmlpage is loaded
                $rootScope.pgLoad = true;
                //if (pgName == 'cr') {
                //    $rootScope.getPartialView = "/Views/cr.html"
                //}
                if (pgName == 'invite') {
                    $rootScope.getPartialView = "/PartialViews/main/taf.html"
                }
            });
        }
    });

    app.controller("footerController", ['$scope', '$http', '$window', '$location', function ($scope, $http, $window, $location) {
        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
    }]);
    return app;
});
