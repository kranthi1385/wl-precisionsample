
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

.controller("page3Controller", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'getQueryParams', '$timeout', '$cookies',
function ($rootScope, $scope, $http, $window, httpService, getQueryParams, $timeout, $cookies) {
    //Google cookie
    $scope.acceptCookie = $cookies.get('google_fb_cookie_aacept');
    if ($scope.acceptCookie == 1) {
        (function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start':
                new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
            j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
            'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', 'GTM-N9DDS62')
    }
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
.controller("smController", ['$scope', '$http', '$window', 'httpService', 'getQueryParams', 'translationsLoadingService', '$cookies',
function ($scope, $http, $window, httpService, getQueryParams, translationsLoadingService, $cookies) {
    //Google cookie
    $scope.acceptCookie = $cookies.get('google_fb_cookie_aacept');
    if ($scope.acceptCookie == 1) {
        (function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start':
                new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
            j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
            'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', 'GTM-N9DDS62')
    }
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
                    window.location.href = "/Ms/Surveys";
                }, function (err) {
                    window.location.href = "/Ms/Surveys";
                });
            }
            else {
                $scope.showErrFileds = false;
            }
        }
        else {
            httpService.getData('/rg/RegistrationStepUpdate?ug=' + ug + '&rgstep=D').then(function (res) {
                window.location.href = "/Ms/Surveys";
            }, function (err) {
                window.location.href = "/Ms/Surveys";
            });
        }
    }
}])
.controller("footerController", ['$scope', '$http', '$window', '$location', 'httpService',
    function ($scope, $http, $window, $location, httpService) {
        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = new Date().getFullYear();
        //$scope.currentYear = year;
        //$scope.nextYear = year + 1;
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (response) {
            $scope.orgDetails = response;
            //var orgLst = organizationService.getAllOrganization()
            //angular.forEach(orgLst, function (org, key) {
            //    if (org.orgId == response.ClientId) {
            //        $scope.bgColor = org.bgcolor;
            //    }
            //})
        }, function (err) {
        });

        //privacy click
        $scope.privacyClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/Footer/Privacy', 'SurveyDownline-Privacy', 'width=800,height=800')
        }
        //terms click
        $scope.termsClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/Footer/TC', 'SurveyDownline-Terms', 'width=800,height=800')
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
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
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
            $window.open($scope.orgDetails.MemberUrl + '/home/contact', 'SurveyDownline-ContactUs', 'width=800,height=800')
        }
        //about click
        $scope.aboutClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/home/About', 'SurveyDownline-About', 'width=800,height=800')
        }
    }]);

