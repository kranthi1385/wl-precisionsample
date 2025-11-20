define(['angular'], function (angular) {
    //main module of the soltion
    var app = angular.module("app", ['ngSanitize', 'customSerivces' ,])
    //config all required providers.(ex: color palette, routing, register all providers
    app.config(['$locationProvider', '$httpProvider', '$controllerProvider', '$provide', 
    function ($locationProvider, $httpProvider, $controllerProvider, $provide) {
        $httpProvider.interceptors.push('loadingInterceptorService');
        //add header for each  request to identify AntiForgeryToken
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        //$translateProvider.useLoader('translatePluggableLoader');
        //add default language
      //  $translateProvider.preferredLanguage('en');
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
        if (pgName == 'psr') {
            //lazyscriptLoading('/Scripts/js/sr.js')
        }
        else if (pgName.toLowerCase() == 'Tas') {
            lazyscriptLoading('/Scripts/pages/Tas.js')
        }

        else if (pgName.toLowerCase() == 'interstitial') {
            lazyscriptLoading('/Scripts/pages/recaptcha.js')
        }

       

        function lazyscriptLoading(currentUrl) {
            lazyscriptLoader.load(currentUrl).then(function () {
                //set pageload true because before js download cshtmlpage is loaded
                $rootScope.pgLoad = true;
              
            });
        }
    })
    //Menu controller section
    app.controller("menuController", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout',
    function ($scope, $http, $window, $location, $rootScop, $timeout) {

    }]);
    return app;
});