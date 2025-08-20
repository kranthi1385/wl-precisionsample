//main module of the soltion

angular.module("rcApp", ['pascalprecht.translate', 'customSerivces'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider',
function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider) {

    //add header for each  request to identify AntiForgeryToken
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    //$httpProvider.interceptors.push('loadingInterceptorService');
    //$httpProvider.interceptors.push('loadingInterceptorService');
    //load all translations setting to main transtion loader.
    $translateProvider.useLoader('$translatePartialLoader', {
        urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
    });
    //add default language
    $translateProvider.preferredLanguage('en');

    //regster providers

}])
.controller("rcController", ['$rootScope', '$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService',
function ($rootScope, $scope, $http, $window, $location, $rootScop, $timeout, httpService, translationsLoadingService) {
    $scope.ug = getUrlVars()["ug"];                    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&$scope.emailaddress=&address1=&$scope.city=&$scope.zip=&$scope.gender=&$scope.dob=&address2=&ethni$scope.city=
    if ($scope.ug == undefined) {
        $scope.ug = ''
    }
    $scope.cid = getUrlVars()["cid"];                    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&$scope.emailaddress=&address1=&$scope.city=&$scope.zip=&$scope.gender=&$scope.dob=&address2=&ethni$scope.city=
    if ($scope.cid == undefined) {
        $scope.cid = ''
    }
    // to get URL params
    function getUrlVars() {
        var Url = window.location.href;
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

    //RC means reward confirmattion page,we need to redirect the member back to Home page, after redemption.
    $scope.backClick = function () {
        window.location.href = '/reg/Home?ug=' + $scope.ug + '&cid=' + $scope.cid;
    }
}])