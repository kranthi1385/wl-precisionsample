//main module of the soltion
angular.module("unsubApp", ['pascalprecht.translate', 'customSerivces', 'staticTranslationsModule'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider', 'translatePluggableLoaderProvider',
   function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider, translatePluggableLoaderProvider) {
       $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
       //load all translations setting to main transtion loader.
       $translateProvider.useLoader('$translatePartialLoader', {
           urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
       });
       //add default language
       $translateProvider.useLoader('translatePluggableLoader');

       //regster providers

   }])
    //terms controller
.controller('unsubController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', 'getQueryParams', '$cookies',
    function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, getQueryParams,$cookies) {
        $scope.isRegErrMsg = false;
        if ($cookies.get('LangCode') != undefined) {
            translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
        }
        else {
            translationsLoadingService.setCurrentUserLanguage('en');
        }
        translationsLoadingService.loadTranslatePagePath("ep");
        // to get URL params
        var ug = getQueryParams.getUrlVars()['ug'];
        $scope.showMsg = false;
        //translationsLoadingService.loadTranslatePagePath("ep");


        $scope.userDetails = {
            EmailAddress: ''
        }
        $scope.unsubClick = function (valid) {
            if (valid) {
                if ($scope.userDetails.EmailAddress != "") {
                    httpService.postData('/Home/unsubUser?EmailAddress=' + $scope.userDetails.EmailAddress).then(function (response) {
                        $scope.userDetails = response;
                        if (response == "accepted") {
                            $scope.showMsg = true;
                        }
                    }, function (err) {
                        $scope.errMsg = true;
                    });
                }
                else {
                    $scope.isRegErrMsg = true;
                }
            }
            else {
                $scope.isRegErrMsg = true;
            }
        }

    }])