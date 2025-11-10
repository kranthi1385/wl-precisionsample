//main module of the soltion
angular.module("footerApp", ['pascalprecht.translate', 'customSerivces', 'vcRecaptcha', 'ngCookies', 'footerTranslationsModule'])
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
.controller('termsController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', 'getQueryParams', '$cookies',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, getQueryParams, $cookies) {
            debugger;
            //Language translations
            if ($cookies.get('WidgetLangCode') != "" && $cookies.get('WidgetLangCode') != undefined && $cookies.get('WidgetLangCode') != "null") {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('WidgetLangCode'));
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
            }
            // get current doamin details
        }])
     //privacy controller
.controller('privacyController', ['$scope', '$timeout', '$http', '$location', '$rootScope', '$window', '$anchorScroll', 'httpService', 'translationsLoadingService', 'getQueryParams', '$cookies',
        function ($scope, $timeout, $http, $location, $rootScope, $window, $anchorScroll, httpService, translationsLoadingService, getQueryParams, $cookies) {
            debugger;
            if ($cookies.get('WidgetLangCode') != "" && $cookies.get('WidgetLangCode') != undefined && $cookies.get('WidgetLangCode') != "null") {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('WidgetLangCode'));
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
            }
            $scope.sitesList = function () {
                $window.open($scope.orgDetails.MemberUrl + '/siteslist.html', 'SurveyDownline-About', 'width=800,height=800')
            }
            //Language translations
            //httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
            //    $scope.orgDetails = res;
            //}, function (err) {
            //});
            //download PrivacyPolicy
            $scope.dowPrivacyPolicy = function () {
                url = 'http://dev.affiliate.sdl.com/login/DownloadPrivacyPDF'
                var hiddenIFrameID = 'hiddeniframe',
                iframe = document.getElementById(hiddenIFrameID);
                if (iframe === null) {
                    iframe = document.createElement('iframe');
                    iframe.id = hiddenIFrameID;
                    iframe.style.display = 'none';
                    document.body.appendChild(iframe);
                }
                iframe.src = url;
            }
            $scope.gotoBottom = function () {
                $location.hash('bottom');
                $anchorScroll();
            };
        }])
//contactus controller
.controller('contactUsController', ['$rootScope', '$scope', '$window', 'httpService',
        function ($rootScope, $scope, $window, httpService) {
            $scope.isShowError = false;
            // get current doamin details
            $scope.currentDomainDetails = function () {
                httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                    $scope.orgDetails = res;
                }, function (err) {
                });
            }
            $scope.currentDomainDetails();
            //Language translations
            // translationsLoadingService.setCurrentUserLanguage("en");
            //translationsLoadingService.loadTranslatePagePath("tc");
            $scope.emailDetails = {
                fromaddress: '',
                comments: '',
                fromname: ''
            }
            //to get organization details
            $scope.contactUsClick = function (valid) {
                if (valid) {
                    $scope.isShowError = false;
                    $scope.currentDomainDetails();
                    httpService.postData('/Login/SendMail?fromaddress=' + $scope.emailDetails.fromaddress + '&comments='
                        + $scope.emailDetails.comments + '&fromname=' + $scope.emailDetails.fromname).then(function (response) {
                            if (response == 1) {
                                $scope.showMsg = true;
                            }
                        }, function (err) {
                            $scope.errMsg = true;
                        });
                }
                else {
                    $scope.isShowError = true;
                }
            }
        }])
//obout controller
.controller('aboutController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', 'getQueryParams',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, getQueryParams) {
            debugger;
            //Language translations
            translationsLoadingService.setCurrentUserLanguage("en");
            translationsLoadingService.loadTranslatePagePath("abt");
            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
        }])
//FAQ controller
.controller('faqController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', 'getQueryParams',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, getQueryParams) {
            debugger;
            //Language translations
            translationsLoadingService.setCurrentUserLanguage("en");
            translationsLoadingService.loadTranslatePagePath("faq");
            // get current doamin details
        }])
//Do Not Sell My Info Controller
.controller('doNotSellMyInfoController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', 'vcRecaptchaService', 'getQueryParams', '$cookies',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, vcRecaptchaService, getQueryParams, $cookies) {
            $scope.DoNotSellInfo = {
                FirstName: "",
                LastName: "",
                EmailAddress: "",
                PrecisionSampleSite: "",
                RequestID: 0
            }
            if ($cookies.get('WidgetLangCode') != "" && $cookies.get('WidgetLangCode') != undefined && $cookies.get('WidgetLangCode') != "null") {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('WidgetLangCode'));
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
            }
            $scope.reqID = [{ id: 1, name: "Request to delete my personal info" },
                           { id: 2, name: "Request to opt-out of sale of personal info" },
                           { id: 3, name: "Request to know about my personal info" },
                           { id: 4, name: "Other privacy or account management request" }]
            // get current doamin details
            //$scope.setResponse = function (response) {
            //    $scope.captchaResponse = response;
            //};
            $scope.doNotSellMyInfo = function (valid) {
                if (valid) {
                    $scope.captchaErr = false;
                    $scope.isShowError = false;
                    if ($scope.captchaResponse != '') {
                        //httpService.postData('/Common/ValidateCaptcha?googleResponse=' + $scope.captchaResponse).then(function (response) {
                        //    if (response == 1) {//google captcha valid
                        var itemSelected = $scope.DoNotSellInfo.RequestID;
                        var id = $scope.reqID[itemSelected].id;
                        var name = $scope.reqID[itemSelected].name;
                        httpService.postData('/Login/SaveDoNotSellMyInfo?fstName=' + $scope.DoNotSellInfo.FirstName + '&lstName=' + $scope.DoNotSellInfo.LastName + '&email=' + $scope.DoNotSellInfo.EmailAddress + '&presite=' + $scope.DoNotSellInfo.PrecisionSampleSite + '&reqid=' + id + '&reqname=' + name).then(function (response) {
                            if (response == 1) {
                                $scope.showMsg = true;
                                $scope.errMsg = false;
                            }
                        }, function (err) {
                            $scope.errMsg = true;
                        });
                    }
                    else {
                        $scope.captchaErr = true;
                    }
                    //    }, function (err) {
                    //        //  $scope.errMsg = true;
                    //    });
                    //}
                    //else {
                    //    $scope.captchaErr = true;
                    //}
                }
                else {
                    $scope.isShowError = true;
                }
            }
        }])
//Forgor Password controller
.controller('forgotPswCntrl', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', 'getQueryParams',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, getQueryParams) {

            //Language translations
            translationsLoadingService.setCurrentUserLanguage("en");
            translationsLoadingService.loadTranslatePagePath("faq");
            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
            $scope.getUserData = {
                EmailAddress: ''
            }
            //forgot password
            $scope.forgetPswd = false;
            $scope.errMsg = false;
            $scope.saveref = false;
            $scope.forgotPasswordClick = function (valid) {
                if (valid) {
                    $scope.saveref = false;
                    $scope.forgotPswdErrMsg = false;
                    $scope.forgetPswd = true;
                    httpService.getData('/Login/GetUserDataEmail?EmailAddress=' + $scope.getUserData.EmailAddress).then(function (response) {
                        $scope.getUserData = response;
                        $scope.getUserData.CustomAttribute = "first_name:" + $scope.getUserData.FirstName + ";last_name:"
                            + $scope.getUserData.LastName + ";email_address:" + $scope.getUserData.EmailAddress + ";password:"
                            + $scope.getUserData.Password + ";create_dt:" + $scope.getUserData.CreateDate + ";ip_address:"
                            + $scope.getUserData.IpAddress + ";org_logo:" + $scope.orgDetails.OrgLogo + ";org_name:" + $scope.orgDetails.OrgName +
                            ";member_url:" + $scope.orgDetails.MemberUrl + ";user_guid:" + $scope.getUserData.UserGuid;
                        httpService.postData('/Login/ForgetPassword?campid=' + 803 + '&CustomAttribute=' + $scope.getUserData.CustomAttribute, $scope.getUserData).then(function (response) {
                            if ($scope.getUserData.EmailAddress != null) {
                                $scope.errMsg = false;
                                $scope.saveref = true;
                            }
                        }, function (err) {
                            $scope.errMsg = true;
                        });

                    }, function (err) {
                        $scope.errMsg = true;
                    });
                }
                else {
                    $scope.forgotPswdErrMsg = true;
                }
            }
            //privacy click
            $scope.privacyClick = function () {
                $window.open($scope.orgDetails.MemberUrl + '/login/p', 'SurveyDownline-Privacy', 'width=800,height=800')
            }
            //terms click
            $scope.termsClick = function () {
                $window.open($scope.orgDetails.MemberUrl + '/login/t', 'SurveyDownline-Terms', 'width=800,height=800')
            }
            //contact us Click
            $scope.contactClick = function () {
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
        }])

//Cancel Account controller
.controller('cancelAccCntrl', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService) {
            //Language translations
            translationsLoadingService.setCurrentUserLanguage("en");
            translationsLoadingService.loadTranslatePagePath("ca");
            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
            //to cancel user account
            $scope.cancelAccount = function () {
                httpService.postData('/Home/CancelUser').then(function (res) {
                }, function (err) {
                });
            }
            //privacy click
            $scope.privacyClick = function () {
                $window.open($scope.orgDetails.MemberUrl + '/login/p', 'SurveyDownline-Privacy', 'width=800,height=800')
            }
            //terms click
            $scope.termsClick = function () {
                $window.open($scope.orgDetails.MemberUrl + '/login/t', 'SurveyDownline-Terms', 'width=800,height=800')
            }
            //contact us Click
            $scope.contactClick = function () {
                $window.open($scope.orgDetails.MemberUrl + '/login/cu', 'SurveyDownline-ContactUs', 'width=800,height=800')
            }
            //about click
            $scope.aboutClick = function () {
                $window.open($scope.orgDetails.MemberUrl + '/login/abt', 'SurveyDownline-About', 'width=800,height=800')
            }
        }])