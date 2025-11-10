debugger;
//main module of the soltion
angular.module("footerApp", ['pascalprecht.translate', 'customSerivces', 'staticTranslationsModule', 'vcRecaptcha'])
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
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                //$scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
                $scope.flagimg = "/images/flag_usa.png";
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }
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
            translationsLoadingService.loadTranslatePagePath("tc");
            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
        }])
.controller('privacyController', ['$scope', '$http', '$location', '$rootScope', '$window', '$anchorScroll', 'httpService', 'translationsLoadingService', 'getQueryParams', '$cookies',
        function ($scope, $http, $location, $rootScope, $window, $anchorScroll, httpService, translationsLoadingService, getQueryParams, $cookies) {
            debugger;
            $scope.sitesList = function () {
                $window.open($scope.orgDetails.MemberUrl + '/siteslist.html', 'SurveyDownline-About', 'width=800,height=800')
            }
            //checking cookie
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }
            //Language translations

            translationsLoadingService.loadTranslatePagePath("p");
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
            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
            //download PrivacyPolicy
            $scope.dowPrivacyPolicy = function () {
                url = $scope.orgDetails.MemberUrl + '/login/DownloadPrivacyPDF'
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
                    //  $scope.currentDomainDetails();
                    httpService.postData('/Login/SendMail?fromaddress=' + $scope.emailDetails.fromaddress + '&comments='
                        + $scope.emailDetails.comments + '&fromname=' + $scope.emailDetails.fromname).then(function (response) {
                            if (response == 1) {
                                $scope.showMsg = true;
                                $scope.emailDetails.fromaddress = "";
                                $scope.emailDetails.comments = "";
                                $scope.emailDetails.fromname = "";
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
//helpdesk controller
.controller('helpdeskController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', '$cookies',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, $cookies) {
            //checking cookie
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }
            //Language translations

            translationsLoadingService.loadTranslatePagePath("abt");
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
            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
        }])
//Cookie Statement
.controller('cookiestController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', '$cookies',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, $cookies) {
            //checking cookie
            debugger;
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }
            //Language translations

            //translationsLoadingService.loadTranslatePagePath("abt");

            //// get current doamin details
            //httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
            //    $scope.orgDetails = res;
            //}, function (err) {
            //});
            $scope.analyticalCok = false;
            $scope.trackingCok = false;
            //write new cookie
            $scope.writeCookie = function (name, cook) {
                //write cookie for one day  it works all sub domains
                var now = new Date();
                var time = now.getTime();
                var expireTime = time + (3600 * 1000) * 8766;
                now.setTime(expireTime);
                document.cookie = escape(name) + "=" + escape(cook) + ';expires=' + now.toUTCString() + ';path=/';
            }
            $scope.analyticalCookie = function () {
                (function (w, d, s, l, i) {
                    w[l] = w[l] || []; w[l].push({
                        'gtm.start':
                        new Date().getTime(), event: 'gtm.js'
                    }); var f = d.getElementsByTagName(s)[0],
                    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
                    'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
                })(window, document, 'script', 'dataLayer', 'GTM-N9DDS62')
                $scope.analyticalCok = true;
                $scope.writeCookie("google_fb_cookie_aacept", 1, 365);
                window.opener.location.reload();
            }
            $scope.trackingCookie = function (cookie1, cookie2) {
                !function (f, b, e, v, n, t, s) {
                    if (f.fbq) return; n = f.fbq = function () {
                        n.callMethod ?
                        n.callMethod.apply(n, arguments) : n.queue.push(arguments)
                    };
                    if (!f._fbq) f._fbq = n; n.push = n; n.loaded = !0; n.version = '2.0';
                    n.queue = []; t = b.createElement(e); t.async = !0;
                    t.src = v; s = b.getElementsByTagName(e)[0];
                    s.parentNode.insertBefore(t, s)
                }(window, document, 'script',
                'https://connect.facebook.net/en_US/fbevents.js');
                fbq('init', '4326893500713220');
                fbq('track', 'PageView');
                $scope.trackingCok = true;
                window.opener.location.reload();
            }
        }])

//Forgor Password controller
.controller('forgotPwdCntrl', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', '$cookies',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, $cookies) {
            //Language translations
            translationsLoadingService.setCurrentUserLanguage("en");
            translationsLoadingService.loadTranslatePagePath("faq");
            $scope.acceptCookie = $cookies.get('google_fb_cookie_accept');
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
            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                debugger
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
            //signup click
            $scope.signup = function () {
                window.location.href = '/Views/Account/OPSignup.cshtml';
            }
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

//Do Not Sell My Info Controller
.controller('doNotSellMyInfoController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', '$cookies', 'getQueryParams',
        function ($scope, $http, $location, $rootScope, $window, httpService, translationsLoadingService, $cookies, getQueryParams) {
            //Language translations
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                //$scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
                $scope.flagimg = "/images/flag_usa.png";
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }

            translationsLoadingService.loadTranslatePagePath("tc");

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

            $scope.DoNotSellInfo = {
                FirstName: "",
                LastName: "",
                EmailAddress: "",
                PrecisionSampleSite: "",
                RequestID: 0
            }

            $scope.reqID = [{ id: 1, name: "Request to delete my personal info" },
                           { id: 2, name: "Request to opt-out of sale of personal info" },
                           { id: 3, name: "Request to know about my personal info" },
                           { id: 4, name: "Other privacy or account management request" }]

            // get current doamin details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
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
                            $scope.showMsg = true;
                            $scope.errMsg = false;
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
define(['app'], function (app) {
    app.register.controller('contactUsController', ['$rootScope', '$scope', '$window', 'httpService', '$cookies', 'translationsLoadingService',
        function ($rootScope, $scope, $window, httpService, $cookies, translationsLoadingService) {
            $scope.isShowError = false;
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }
            translationsLoadingService.loadTranslatePagePath("abt");
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
            $scope.emailDetails = {
                fromaddress: '',
                comments: '',
                fromname: ''
                //fromsurvey: '',
                //fromcountry:''
            }
            $scope.checkname = function (fname) {
                var reg = /^([^0-9]*)$/;
                if (reg.test(fname)) {
                    $scope.nameMsg = false;
                }
                else {
                    $scope.nameMsg = true;
                    $scope.xVerifyMessage = '';
                }
            }
            //to get organization details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });

            $scope.contactUsClick = function (valid) {
                if (valid) {
                    $scope.isShowError = false;
                    //  $scope.currentDomainDetails();
                    httpService.postData('/Login/SendMail?fromaddress=' + $scope.emailDetails.fromaddress + '&comments='
                         + $scope.emailDetails.comments + '&fromname=' + $scope.emailDetails.fromname).then(function (response) {
                             if (response == 1) {
                                 $scope.showMsg = true;
                                 $scope.emailDetails.fromaddress = "";
                                 $scope.emailDetails.comments = "";
                                 $scope.emailDetails.fromname = "";
                             }
                         }, function (err) {
                             $scope.errMsg = true;
                         });
                }
                else {
                    $scope.isShowError = true;
                }
            }


            $scope.profileClick = function () {
                window.location.href = '/profile/index';
            }
            //About Click
            $scope.aboutClick = function () {
                window.location.href = '/home/About';
            }
            //contact click
            $scope.contactClick = function () {
                window.location.href = '/home/contact';
            }

            //help desk
            $scope.helpDesk = function () {
                $window.open('/Login/HelpDesk?v=2', '', 'width=800,height=800')
            }
        }])
})
