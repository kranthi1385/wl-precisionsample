
//main module of the soltion
angular.module("regOffers", ['ngSanitize', 'pascalprecht.translate', 'customSerivces'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider',
function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider) {
    //loading image interceptor
    $httpProvider.interceptors.push('httpInterceptor');
    //load all translations setting to main transtion loader.
    // $translateProvider.useLoader('translatePluggableLoader');
    //add default language
    //load all translations setting to main transtion loader.
    $translateProvider.useLoader('$translatePartialLoader', {
        urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
    });
    //add default language
    $translateProvider.preferredLanguage('en');

    //Inject auth Service  all http calls.
    $httpProvider.interceptors.push('loadingInterceptorService');

}])

.controller("page3Controller", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'getQueryParams', '$timeout',
function ($rootScope, $scope, $http, $window, httpService, getQueryParams, $timeout) {
    $timeout(function () {
        $rootScope.$broadcast("loader_hide");
    }, 100);
    $scope.submit = function () {
        httpService.getData('/rg/RegistrationStepUpdate?ug=' + getQueryParams.getUrlVars()["ug"] + '&rgstep=C').then(function (res) {
            window.location.href = "/rg/sm?ug=" + getQueryParams.getUrlVars()["ug"];
        }, function (err) {
            window.location.href = "/rg/sm?ug=" + getQueryParams.getUrlVars()["ug"];
        });
    }
}])
.controller("smController", ['$scope', '$http', '$window', 'httpService', 'getQueryParams', 'translationsLoadingService',
function ($scope, $http, $window, httpService, getQueryParams, translationsLoadingService) {
    //Language translations
    translationsLoadingService.setCurrentUserLanguage("en");
    translationsLoadingService.loadTranslatePagePath("reg");
    $scope.selectedOffers = [];
    $scope.showErrFileds = true;
    var ug = getQueryParams.getUrlVars()["ug"];
    //Get perks list

    httpService.getData('/rg/GetShowMeSurveysList?ug=' + ug).then(function (res) {
        $scope.perks = res;
    }, function (err) {
    });


    $scope.showMeSurveys = function (index, perk, type) {
        $scope.selectedOffers.push(index)
        if (type == "showme") {
            url = "/rg/sm1?ug=" + ug + '&pg=' + perk.PerkGuid;
            window.open(url, "_blank", "menubar=1,resizable=1,,width=800,height=500");
        }

    }
    $scope.submit = function () {
        if ($scope.perks.length > 0) {
            if ($scope.selectedOffers.length > 0) {
                httpService.getData('/rg/RegistrationStepUpdate?ug=' + ug + '&rgstep=D').then(function (res) {
                    window.location.href = "/hm/Home";
                }, function (err) {
                    window.location.href = "/hm/Home";
                });
            }
            else {
                $scope.showErrFileds = false;
            }
        }
        else {
            httpService.getData('/rg/RegistrationStepUpdate?ug=' + ug + '&rgstep=D').then(function (res) {
                window.location.href = "/hm/Home";
            }, function (err) {
                window.location.href = "/hm/Home";
            });
        }
    }
}])
.controller("footerController", ['$scope', '$http', '$window', '$location', 'httpService',
    function ($scope, $http, $window, $location, httpService) {

        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
        //get current domain details
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
            $scope.orgDetails = res;
        }, function (err) {
        });
        //privacy click
        $scope.privacyClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/p', 'SurveyDownline-Privacy', 'width=800,height=800')
        }
        //terms click
        $scope.termsClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/t', 'SurveyDownline-Terms', 'width=800,height=800')
        }
        //contact click
        $scope.contactUs = false;
        $scope.contactClick = function () {
            $scope.lqeStep2 = true;
            $scope.forgetPswd = false;
            $scope.conForClick = false;
            $scope.contactUs = true;
            $scope.emailDetails = {
                fromaddress: '',
                comments: '',
                fromname: ''
            }
            
            $scope.contactUsClick = function (valid) {
                if (valid) {
                    $scope.isShowContactErr = false;
                    httpService.postData('/Login/SendMail?fromaddress=' + $scope.emailDetails.fromaddress + '&comments='
                        + $scope.emailDetails.comments).then(function (response) {
                        }, function (err) {
                            $scope.errMsg = true;
                        });
                }
                else {
                    $scope.isShowContactErr = true;
                }
            }
            $window.open($scope.orgDetails.MemberUrl + '/login/cu', 'SurveyDownline-ContactUs', 'width=800,height=800')
        }
        //about click
        $scope.aboutClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/abt', 'SurveyDownline-About', 'width=800,height=800')
        }
        //FAQ click
        $scope.faqClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/faq', 'SurveyDownline-FAQ', 'width=800,height=800')
        }
    }]);

