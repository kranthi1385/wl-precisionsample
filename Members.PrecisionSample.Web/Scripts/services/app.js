/// <reference path="../pages/main/hm.js" />
/// <reference path="../pages/main/hm.js" />
define(['angular'], function (angular) {
    //main module of the soltion

    var app = angular.module("app", ['ngSanitize', 'pascalprecht.translate', 'customSerivces', 'staticTranslationsModule', 'partialModule', 'vcRecaptcha', 'ngCookies'])
    //config all required providers.(ex: color palette, routing, register all providers
    app.config(['$locationProvider', '$httpProvider', '$controllerProvider', '$provide', '$translateProvider', 'translatePluggableLoaderProvider',
    function ($locationProvider, $httpProvider, $controllerProvider, $provide, $translateProvider, translatePluggableLoaderProvider) {
        //loading image interceptor
        $httpProvider.interceptors.push('httpInterceptor');
        //add header for each  request to identify AntiForgeryToken
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $translateProvider.useLoader('translatePluggableLoader');
        //add default language
        $translateProvider.preferredLanguage('en');
        app.register = {
            controller: $controllerProvider.register,
            factory: $provide.factory,
            service: $provide.service
        };

        //Inject auth Service  all http calls.
        $httpProvider.interceptors.push('loadingInterceptorService');

    }]);
    app.run(function ($rootScope, $location, $window, $http, lazyscriptLoader) {
        //$http.defaults.headers.common['X-XSRF-Token'] =
        //angular.element('input[name="__RequestVerificationToken"]').attr('value');
        var url = window.location.pathname;
        var pgName = url.split("/")[2].toLocaleLowerCase();
        if (pgName == 'home') {
            lazyscriptLoading('/Scripts/pages/main/hm.js?v=' + version)
        }
        else if (pgName == 'surveys') {
            lazyscriptLoading('/Scripts/pages/main/ms.js?v=' + version)

        }
        else if (pgName == 'downline') {
            lazyscriptLoading('/Scripts/pages/main/rf.js?v=' + version)
        }
        else if (pgName == 'rewards') {
            lazyscriptLoading('/Scripts/pages/main/mr.js?v=' + version)
        }
        else if (pgName == 'account') {
            lazyscriptLoading('/Scripts/pages/main/ep.js?v=' + version)
        }
        else if (pgName == 'invite') {
            lazyscriptLoading('/Scripts/pages/main/taf.js?v=' + version)
        }
        else if (pgName == 'surveyhistory') {
            lazyscriptLoading('/Scripts/pages/main/sh.js?v=' + version)
        }
        else if (pgName == 'entry') {
            lazyscriptLoading('/Scripts/pages/main/entry.js?v=' + version)
        }
        if (pgName == 'unsub') {
            lazyscriptLoading('/Scripts/pages/us.js?v=' + version)
        }
        if (pgName == 'ext') {
            lazyscriptLoading('/Scripts/pages/e/ext.js?v=' + version)
        }
        if (pgName == 'rc') {
            lazyscriptLoading('/Scripts/pages/main/rr.js?v=' + version)
        }
        if (pgName == 'ForgotPsw') {
            lazyscriptLoading('/Scripts/pages/main/fp.js?v=' + version)
        }
        if (pgName == 'vld') {
            lazyscriptLoading('/Scripts/pages/e/vld.js?v=' + version)
        }
        if (pgName == 'entry') {
            lazyscriptLoading('/Scripts/pages/main/entry.js?v=' + version)
        }
        function lazyscriptLoading(currentUrl) {
            lazyscriptLoader.load(currentUrl).then(function () {
                //set pageload true because before js download cshtmlpage is loaded
                $rootScope.pgLoad = true;
                //if (pgName == 'home') {
                //    $rootScope.getPartialView = "/PartialViews/main/hm.html"
                //}
                //else if (pgName == 'surveys') {
                //    $rootScope.getPartialView = "/PartialViews/main/ms.html"
                //}
                //else if (pgName == 'downline') {
                //    $rootScope.getPartialView = "/PartialViews/main/rf.html"
                //}
                //else if (pgName == 'rewards') {
                //    $rootScope.getPartialView = "/PartialViews/main/mr.html"
                //}
                //else if (pgName == 'account') {
                //    $rootScope.getPartialView = "/PartialViews/main/ep.html"
                //}
                if (pgName == 'invite') {
                    $rootScope.getPartialView = "/PartialViews/main/taf.html"
                }
            });
        }
    });
    //Menu controller section
    app.controller("menuController", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'lazyscriptLoader', 'translationsLoadingService', 'httpService', 'organizationService', '$cookies',
    function ($scope, $http, $window, $location, $rootScop, $timeout, lazyscriptLoader, translationsLoadingService, httpService, organizationService, $cookies) {
        $scope.languageSelected = true;
        $scope.selectedLangCode = 0;
        var languageCodesLst = [];
        //$scope.acceptCookie = $cookies.get('google_fb_cookie_accept');
        //if ($scope.acceptCookie == 1) {
        //    (function (w, d, s, l, i) {
        //        w[l] = w[l] || []; w[l].push({
        //            'gtm.start':
        //            new Date().getTime(), event: 'gtm.js'
        //        }); var f = d.getElementsByTagName(s)[0],
        //        j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
        //        'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        //    })(window, document, 'script', 'dataLayer', 'GTM-N9DDS62')
        //}
        if ($cookies.get('LangCode') != undefined) {
            translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
        }


        $scope.selectLanaguage = function (index, selectedLanguage) {
            $scope.selectedImg = selectedLanguage.Img;
            $scope.selectedLangCode = index;
            $scope.languageSelected = !$scope.languageSelected;
            $cookies.put('LangCode', selectedLanguage.LangCode);
            translationsLoadingService.setCurrentUserLanguage(selectedLanguage.LangCode);
            $scope.UpdateLanguageCode(selectedLanguage);
        }
        //get current domain details
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (response) {
            $scope.orgDetails = response;
            var orgLst = organizationService.getAllOrganization()
            angular.forEach(orgLst, function (org, key) {
                if (org.orgId == response.ClientId) {
                    $scope.bgColor = org.bgcolor;
                }
            })
        }, function (err) {
        });
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
        //$scope.selectLanaguage = function (index, selectedLanguage) {
        //    $scope.selectedImg = selectedLanguage.Img;
        //    $scope.selectedLangCode = index;
        //    $scope.languageSelected = !$scope.languageSelected;
        //    $cookies.put('LangCode', selectedLanguage.LangCode);
        //    translationsLoadingService.setCurrentUserLanguage(selectedLanguage.LangCode);
        //    $scope.UpdateLanguageCode(selectedLanguage);
        //}
        //save language prefered by user
        $scope.UpdateLanguageCode = function (selectedLanguage) {
            httpService.getData('/hm/GetUserDetails').then(function (response) {
                $scope.user = response;
                httpService.postData('/Common/UpdateLanguageCode?LangCode=' + selectedLanguage.LangCode + '&userGuid' + $scope.user.UserGuid, $scope.user).then(function (response) {
                    if (response != "" && response != null) {
                        translationsLoadingService.setCurrentUserLanguage(response);
                    }
                }, function (err) {
                    $scope.errMsg = true;
                });
            }, function (err) {
                $scope.errMsg = true;
            });
        }
        $scope.submit = function () {
            httpService.getData('/Common/LogOut').then(function (response) {
                window.location.href = $scope.orgDetails.MgLoginPath;
            }, function (err) {
                window.location.href = $scope.orgDetails.MgLoginPath;
            });
        }

    }]);
    app.controller("footerController", ['$scope', '$http', '$window', '$location', 'httpService', function ($scope, $http, $window, $location, httpService) {
        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
        // get current doamin details
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
            $scope.orgDetails = res;
        }, function (err) {
        });
        //privacy click
        $scope.privacyClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/p', 'SurveyDownline-Privacy', 'width=800,height=800')
        }
        //terms click
        $scope.termsClick = function (url) {
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
            newwin = window.open(url, 'windowname5', params);
            if (window.focus) { newwin.focus() }
            return false;
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
                    $scope.currentDomainDetails();
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
            //if ($scope.orgDetails.ClientId == 111) {
            //    $scope.contactUsClick = function (valid) {
            //        if (valid) {
            //            $scope.isShowContactErr = false;
            //            httpService.postData('/Login/SendMail?fromaddress=' + $scope.emailDetails.fromaddress + '&comments='
            //                + $scope.emailDetails.comments).then(function (response) {
            //                }, function (err) {
            //                    $scope.errMsg = true;
            //                });
            //        }
            //        else {
            //            $scope.isShowContactErr = true;
            //        }
            //    }
            //}
            //else {
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
        //Do not sell my info
        $scope.doNotSellMyInfo = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/dns', 'SurveyDownline-DNS', 'width=800,height=800')
        }
    }]);
    return app;
});
