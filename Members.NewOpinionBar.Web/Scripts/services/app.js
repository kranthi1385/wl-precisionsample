debugger;
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
        //$translateProvider.preferredLanguage('en');
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
        if (pgName == 'surveys') {
            lazyscriptLoading('/Scripts/main/profile.js?v=' + version)

        }
        else if (pgName == 'about') {
            lazyscriptLoading('/Scripts/main/about.js?v=' + version)
        }
        else if (pgName == 'changepassword') {
            lazyscriptLoading('/Scripts/main/pwdExpire.js?v=' + version)
        }
        else if (pgName == 'contact') {
            lazyscriptLoading('/Scripts/main/footer/tc.js?v=' + 2)
        }
        else if (pgName == 'account') {
            lazyscriptLoading('/Scripts/main/myDetails.js?v=' + 2)

        }
        else if (pgName == 'piiconfirm') {
            lazyscriptLoading('/Scripts/main/myDetails.js?v=' + 2)

        }
        else if (pgName == 'deleteuserdata') {
            lazyscriptLoading('/Scripts/main/myDetails.js?v=' + 2)

        }
        else if (pgName == 'rewards') {
            lazyscriptLoading('/Scripts/main/mr.js?v=' + version)

        }
        else if (pgName == 'downline') {
            lazyscriptLoading('/Scripts/main/DownLine.js?v=' + version)
        }
        else if (pgName == 'invite') {
            lazyscriptLoading('/Scripts/main/Taf.js?v=' + version)
        }
        else if (pgName == 'rc') {
            lazyscriptLoading('/Scripts/main/rr.js?v=' + version)
        }
        else if (pgName == 'language') {
            lazyscriptLoading('/Scripts/main/login.js?v=' + version)
        }
        else if (pgName == 'obprofile') {
            lazyscriptLoading('/Scripts/main/pr.js?v=' + version)
        }
        else if (pgName == 'moreabout') {
            lazyscriptLoading('/Scripts/main/footer/ma.js?v=' + version)
        }
        else if (pgName == 'helpdesk') {
            lazyscriptLoading('/Scripts/main/footer/tc.js?v=' + version)
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
                    $rootScope.getPartialView = "/PartialViews/main/Taf.html?v=1"
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
        var now = new $window.Date(),
        exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
        debugger;
        if ($cookies.get('LangCode') != undefined) {
            translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
        }
        $scope.selectLanaguage = function (index, selectedLanguage) {
            $scope.selectedImg = selectedLanguage.Img;
            $scope.selectedLangCode = index;
            $scope.languageSelected = !$scope.languageSelected;
            $cookies.put('LangCode', selectedLanguage.LangCode, {
                expires: exp,
                path: '/'
            });
            translationsLoadingService.setCurrentUserLanguage(selectedLanguage.LangCode);
        }

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
        //save language prefered by user
        //$scope.UpdateLanguageCode = function (selectedLanguage) {
        //    httpService.getData('/ep/GetUserData').then(function (response) {
        //        $scope.user = response;
        //        httpService.postData('/Common/UpdateLanguageCode', $scope.user).then(function (response) {
        //            if (response != "" && response != null) {
        //                translationsLoadingService.setCurrentUserLanguage(response);
        //            }
        //        }, function (err) {
        //            $scope.errMsg = true;
        //        });
        //    }, function (err) {
        //        $scope.errMsg = true;
        //    });
        //}
        $scope.submit = function () {
            httpService.getData('/Common/LogOut').then(function (response) {

                window.location.href = $scope.orgDetails.MgLoginPath;
            }, function (err) {
                window.location.href = $scope.orgDetails.MgLoginPath;
            });
        }
        $scope.homeClick = function () {
            window.location.href = '/Ms/Surveys';
        }
        $scope.homeClick = function () {
            window.location.href = '/Ms/Surveys';
        }
        // About Click
        $scope.aboutClick = function () {
            window.location.href = '/home/About';
        }
        //contact click
        $scope.contactClick = function () {
            window.location.href = '/home/Contact';
        }
        $scope.termsClick = function () {
            window.location.href = '/Footer/tc';
        }
    }]);

    app.controller("bannerrController", ['$scope', '$http', '$window', '$location', 'httpService', '$cookies', '$rootScope', 'translationsLoadingService', function ($scope, $http, $window, $location, httpService, $cookies, $rootScope, translationsLoadingService) {
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
        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
        var limiter = 767;
        var now = new $window.Date(),
      exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
        $scope.moreAbout = true;
        $scope.recaptchalangCode = 'en';
        $scope.selectLanaguage = function (index, selectedLanguage) {
            $scope.selectedImg = selectedLanguage.Img;
            $scope.selectedLangCode = index;
            $scope.languageSelected = !$scope.languageSelected;
            if (selectedLanguage.Region == "Europe") {
                $scope.flag = eucountry[selectedLanguage.CountryId + ""].flag;
                $scope.langCode = eucountry[selectedLanguage.CountryId + ""].LangCode;
            }
            else if (selectedLanguage.Region == "America") {
                $scope.flag = uscountry[selectedLanguage.CountryId + ""].flag;
                $scope.langCode = uscountry[selectedLanguage.CountryId + ""].LangCode;
            }
            else if (selectedLanguage.Region == "Asia") {
                $scope.flag = asiacountry[selectedLanguage.CountryId + ""].flag;
                $scope.langCode = asiacountry[selectedLanguage.CountryId + ""].LangCode;
            }
            else if (selectedLanguage.Region == "Middle East") {
                $scope.flag = mecountry[selectedLanguage.CountryId + ""].flag;
                $scope.langCode = mecountry[selectedLanguage.CountryId + ""].LangCode;
            }
            else {
                $scope.flag = othercountry[selectedLanguage.CountryId + ""].flag;
                $scope.langCode = othercountry[selectedLanguage.CountryId + ""].LangCode;
            }
            $cookies.put('UserLangCode', $scope.langCode, {
                expires: exp,
                path: '/'
            });
            $cookies.put('UserFlagCode', $scope.flag, {
                expires: exp,
                path: '/'
            });
            translationsLoadingService.setCurrentUserLanguage(selectedLanguage.LangCode);
            window.location.href = '/Ms/Surveys';
        }

        //to get language Cookie
        eucountry = {
            "229": {
                Region: "Europe", LangName: "United Kingdom", LangCode: "en", flag: "/images/dutch_flag.png", CountryId: "229",
            },
            "647": {
                Region: "Europe", LangName: "Netherland", LangCode: "nd", flag: "/images/flag1.png", CountryId: "647"
            },
            "572": {
                Region: "Europe", LangName: "Deutchland", LangCode: "De", flag: "/images/flag4.png", CountryId: "572"
            },
            "700": {
                Region: "Europe", LangName: "España", LangCode: "es", flag: "/images/flag_sp.png", CountryId: "700"
            },
            "669": {
                Region: "Europe", LangName: "Português", LangCode: "pt", flag: "/images/flag6.png", CountryId: "669",
            },
            "565": {
                Region: "Europe", LangName: "France", LangCode: "Fr", flag: "/images/flag1.png", CountryId: "565"
            },
            "599": {
                Region: "Europe", LangName: "Italia", LangCode: "It", flag: "/images/flag_it.png", CountryId: "599"
            },
            "505": {
                Region: "Europe", LangName: "Österreich", LangCode: "De", flag: "/images/flag_austria.png", CountryId: "505"
            },
            "512": {
                Region: "Europe", LangName: "België / Belgique", LangCode: "nd", flag: "/images/flag_belgium.png", CountryId: "512",
            },
            "668": {
                Region: "Europe", LangName: "Polska", LangCode: "po", flag: "/images/flag_po.png", CountryId: "668"
            },
            "707": {
                Region: "Europe", LangName: "Schweiz / Suisse", LangCode: "De", flag: "/images/flag_sw.png", CountryId: "707"
            },
        }
        $scope.EUCountryCode = eucountry;

        uscountry = {
            "231": {
                Region: "America", LangName: "United States", LangCode: "en", flag: "/images/flag_usa.png", CountryId: "231",
            },
            "38": {
                Region: "America", LangName: "Canada(EN)", LangCode: "en", flag: "/images/flag_ca.png", CountryId: "38"
            },
            "8": {
                Region: "America", LangName: "Canada(FR)", LangCode: "Fr", flag: "/images/flag_ca.png", CountryId: "8"
            },
            "502": {
                Region: "America", LangName: "Argentina", LangCode: "es", flag: "/images/flag_arg.png", CountryId: "502"
            },
            "522": {
                Region: "America", LangName: "Brazil", LangCode: "pt", flag: "/images/flag_brazil.png", CountryId: "522"
            },
            "534": {
                Region: "America", LangName: "Chile", LangCode: "es", flag: "/images/flag_chile.png", CountryId: "534"
            },
            "538": {
                Region: "America", LangName: "Colombia", LangCode: "es", flag: "/images/flag_colombia.png", CountryId: "538"
            },
            "700": {
                Region: "America", LangName: "Spanish", LangCode: "es", flag: "/images/flag_sp.png", CountryId: "700"
            },
            "665": {
                Region: "America", LangName: "Peru", LangCode: "es", flag: "/images/flag_peru.png", CountryId: "665"
            },
            "730": {
                Region: "America", LangName: "Venezuela", LangCode: "es", flag: "/images/flag_ven.png", CountryId: "730"
            },
        }
        $scope.USCountryCode = uscountry;

        asiacountry = {
            "601": {
                Region: "Asia", LangName: "日本", LangCode: "Ja", flag: "/images/flag3.png", CountryId: "601",
            },
            "1196": {
                Region: "Asia", LangName: "한국", LangCode: "kr", flag: "/images/flag_sk.png", CountryId: "1196"
            },
            "535": {
                Region: "Asia", LangName: "中国", LangCode: "Ch", flag: "/images/flag5.png", CountryId: "535"
            },
            "592": {
                Region: "Asia", LangName: "India", LangCode: "en", flag: "/images/flag_ind.png", CountryId: "592"
            },
        }
        $scope.AsiaCountryCode = asiacountry;

        mecountry = {
            "686": {
                Region: "Middle East", LangName: "المملكة العربية السعودي", LangCode: "Ar", flag: "/images/flag_sr.png", CountryId: "686",
            },
            "555": {
                Region: "Middle East", LangName: "جمهورية مصر العربية", LangCode: "Ar", flag: "/images/flag_egypt.png", CountryId: "555"
            },
            "719": {
                Region: "Middle East", LangName: "Turkey", LangCode: "Tu", flag: "/images/flag_tk.png", CountryId: "719"
            },
        }
        $scope.MiddleEastCountryCode = mecountry;

        othercountry = {
            "674": {
                Region: "Other", LangName: "Российская Федерация", LangCode: "Ru", flag: "/images/flag_ru.png", CountryId: "674",
            },
            "653": {
                Region: "Other", LangName: "Nigeria", LangCode: "en", flag: "/images/flag_nigeria.png", CountryId: "653"
            },
            "15": {
                Region: "Other", LangName: "Australia", LangCode: "en", flag: "/images/flag_aus.png", CountryId: "15"
            },
        }
        $scope.OtherCountryCode = othercountry;

        //    languageCodesLst = [
        //{ LangName: "United Kingdom", flag: "/images/dutch_flag.png", LangCode: "en" },
        //        { LangName: "Netherland", flag: "/images/flag1.png", LangCode: "nd" },
        //           { LangName: "Deutchland", flag: "/images/flag4.png", LangCode: "De" },
        //        {
        //            LangName: "España", flag: "/images/flag_sp.png", LangCode: "es"
        //        },
        //      { LangName: "Português", flag: "/images/flag6.png", LangCode: "pt" },

        //      { LangName: "France", flag: "/images/flag_fr.png", LangCode: "Fr" },
        //      { LangName: "Italia", flag: "/images/flag_it.png", LangCode: "It" },
        //       { LangName: "Österreich", flag: "/images/flag_austria.png", LangCode: "De" },
        //      //{ LangName: "Ireland", LangCode: "Ir" },
        //      { LangName: "België / Belgique", flag: "/images/flag_belgium.png", LangCode: "nd" },
        //     // { LangName: "Belgique", flag: "/images/flag_belgium.png", LangCode: "De" },
        //      { LangName: "Polska", flag: "/images/flag_po.png", LangCode: "po" },
        //        { LangName: "Schweiz / Suisse", flag: "/images/flag_sw.png", LangCode: "De" },
        //       // { LangName: "", flag: "/images/flag_sw.png", LangCode: "Fr" },
        //    ]
        //    $scope.languageCodes = country;

        //    usLanguageLst = [
        //           { LangName: "United States", flag: "/images/flag_usa.png", LangCode: "en", },
        //           { LangName: "Canada(EN)", flag: "/images/flag_ca.png", LangCode: "en", },
        //           { LangName: "Canada(FR)", flag: "/images/flag_ca.png", LangCode: "Fr", },
        //           { LangName: "Argentina", flag: "/images/flag_arg.png", LangCode: "es" },
        //           { LangName: "Brazil ", flag: "/images/flag_brazil.png", LangCode: "pt" },
        //           { LangName: "Chile", flag: "/images/flag_chile.png", LangCode: "es" },
        //           { LangName: "Colombia", flag: "/images/flag_colombia.png", LangCode: "es" },
        //           { LangName: "Spanish", flag: "/images/flag_sp.png", LangCode: "es" },
        //           { LangName: "Peru", flag: "/images/flag_peru.png", LangCode: "es" },
        //           { LangName: "Venezuela", flag: "/images/flag_ven.png", LangCode: "es" },

        //    ]
        //    $scope.usLanguageCodes = usLanguageLst;

        //    asiaLanguageLst = [
        //        { LangName: "日本", flag: "/images/flag3.png", LangCode: "Ja", },
        //         { LangName: "한국", flag: "/images/flag_sk.png", LangCode: "kr", },
        //            { LangName: "中国", flag: "/images/flag5.png", LangCode: "Ch", },
        //        { LangName: "India", flag: "/images/flag_ind.png", LangCode: "en", },
        //    ]
        //    $scope.asLanguageCodes = asiaLanguageLst;

        //    middleLanguageLst = [
        //          { LangName: "المملكة العربية السعودي", flag: "/images/flag_sr.png", LangCode: "Ar", },
        //       { LangName: "جمهورية مصر العربية", flag: "/images/flag_egypt.png", LangCode: "Ar", },
        //     { LangName: "Turkey", flag: "/images/flag_tk.png", LangCode: "Tu", },
        //    ]
        //    $scope.middleLanguageCodes = middleLanguageLst;
        //    otherLanguageLst = [
        //         { LangName: "Российская Федерация", flag: "/images/flag_ru.png", LangCode: "Ru", },
        //       { LangName: "Nigeria", flag: "/images/flag_nigeria.png", LangCode: "en", },
        //       { LangName: "Australia", flag: "/images/flag_aus.png", LangCode: "en", },
        //    ]
        //    $scope.otherLanguageCodes = otherLanguageLst;


        $scope.languageChange = function () {
            $scope.languageSelected = !$scope.languageSelected;

        }
        //save language prefered by user
        $scope.UpdateLanguageCode = function (selectedLanguage) {
            httpService.getData('/ep/GetUserData').then(function (response) {
                $scope.user = response;
                httpService.postData('/Common/UpdateLanguageCode?LangCode=' + selectedLanguage.LangCode + '&userGuid' + $scope.user.UserGuid, $scope.user).then(function (response) {
                    if (response != "" && response != null) {
                        translationsLoadingService.setCurrentUserLanguage(response);
                        window.location.href = '/profile/main';
                    }
                }, function (err) {
                    $scope.errMsg = true;
                });
            }, function (err) {
                $scope.errMsg = true;
            });
        }

        $scope.getUserData = function () {
            httpService.getData('/ep/GetUserData').then(function (response) {
                $scope.user = response;
                $rootScope.countryId = $scope.user.CountryId;
                if ($cookies.get('MainLangCode') == "" || $cookies.get('MainLangCode') == undefined) {
                    $cookies.put('UserLangCode', undefined, {
                        expires: exp,
                        path: '/'
                    });
                    $cookies.put('UserFlagCode', undefined, {
                        expires: exp,
                        path: '/'
                    });
                    if (($scope.user.CountryId != undefined && $scope.user.LanguageCode != null) || ($cookies.get('UserLangCode') != undefined || $cookies.get('UserFlagCode') != undefined)) {
                        if ($scope.user.CountryId == 229 || $scope.user.CountryId == 647 || $scope.user.CountryId == 572 || $scope.user.CountryId == 700 ||
                            $scope.user.CountryId == 669 || $scope.user.CountryId == 565 || $scope.user.CountryId == 599 || $scope.user.CountryId == 505 ||
                            $scope.user.CountryId == 512 || $scope.user.CountryId == 668 || $scope.user.CountryId == 707) {
                            $scope.flag = eucountry[$scope.user.CountryId + ""].flag;
                            $scope.langCode = $scope.user.LanguageCode;
                        }
                        else if ($scope.user.CountryId == 231 || $scope.user.CountryId == 38 || $scope.user.CountryId == 8 || $scope.user.CountryId == 502 ||
                            $scope.user.CountryId == 522 || $scope.user.CountryId == 534 || $scope.user.CountryId == 538 || $scope.user.CountryId == 700 ||
                            $scope.user.CountryId == 665 || $scope.user.CountryId == 730) {
                            $scope.flag = uscountry[$scope.user.CountryId + ""].flag;
                            $scope.langCode = $scope.user.LanguageCode;
                        }
                        else if ($scope.user.CountryId == 601 || $scope.user.CountryId == 1196 || $scope.user.CountryId == 535 || $scope.user.CountryId == 592) {
                            $scope.flag = asiacountry[$scope.user.CountryId + ""].flag;
                            $scope.langCode = $scope.user.LanguageCode;
                        }
                        else if ($scope.user.CountryId == 686 || $scope.user.CountryId == 555 || $scope.user.CountryId == 719) {
                            $scope.flag = mecountry[$scope.user.CountryId + ""].flag;
                            $scope.langCode = $scope.user.LanguageCode;
                        }
                        else {
                            $scope.flag = othercountry[$scope.user.CountryId + ""].flag;
                            $scope.langCode = $scope.user.LanguageCode;
                        }
                        $cookies.put('MainLangCode', $scope.langCode, {
                            expires: exp,
                            path: '/'
                        });
                        $cookies.put('MainFlagCode', $scope.flag, {
                            expires: exp,
                            path: '/'
                        });
                        if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                            translationsLoadingService.setCurrentUserLanguage($cookies.get('MainLangCode') || $cookies.get('UserLangCode'));
                            $scope.flagimg = $cookies.get('MainFlagCode') || $cookies.get('UserFlagCode');
                            $scope.recaptchalangCode = $cookies.get('UserLangCode') || $cookies.get('MainLangCode');
                        }
                        else {
                            translationsLoadingService.setCurrentUserLanguage('en');
                            $scope.flagimg = "/images/flag_usa.png";
                        }
                    }
                    else {
                        $cookies.put('MainLangCode', 'en', {
                            expires: exp,
                            path: '/'
                        });
                        $cookies.put('MainFlagCode', '/images/flag_usa.png', {
                            expires: exp,
                            path: '/'
                        });
                        translationsLoadingService.setCurrentUserLanguage('en');
                        $scope.flagimg = "/images/flag_usa.png";
                    }


                }
                else {
                    if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                        translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                        $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
                        $scope.recaptchalangCode = $cookies.get('UserLangCode') || $cookies.get('MainLangCode');
                    }
                    else {
                        translationsLoadingService.setCurrentUserLanguage('en');
                        $scope.flagimg = "/images/flag_usa.png";
                    }
                }
                //to get correct language codes
                if ($scope.recaptchalangCode == 'nd') {
                    $scope.recaptchalangCode = 'nl';
                }
                if ($scope.recaptchalangCode == 'Ch') {
                    $scope.recaptchalangCode = 'zh-CN';
                }
                if ($scope.recaptchalangCode == 'kr') {
                    $scope.recaptchalangCode == 'ko'
                }
                if ($scope.recaptchalangCode == 'po') {
                    $scope.recaptchalangCode == 'pl'
                }
                GetRewardsHistory();
            }, function (err) {
                $scope.errMsg = true;
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            });
        }



        $(document).scroll(function () {
            if ($(window).width() > limiter) {
                if ($(window).scrollTop() != 0) {
                    $('.header').css('background-color', 'rgba(184,39,43,0.9)');
                } else {
                    $('.header').css('background-color', 'transparent');
                }
            } else {
                $('.header').css('background-color', 'rgba(184,39,43,1)');
            }
        });
        //hover navigation top
        $(".nav_item, .country, .country_mob").hover(
            function () {
                $(this).addClass("nav_item_h");
            }, function () {
                $(this).removeClass("nav_item_h");
            });

        //hover change buttons
        $(".change, .delete").hover(
            function () {
                $(this).addClass("change_h");
            }, function () {
                $(this).removeClass("change_h");
            });

        //hamburger navigation
        $('.hamburger_icon').click(function () {
            $('.navigation_mobile').slideToggle(500);
        });

        $('.nav_item_mob').click(function () {
            $(this).addClass('nav_item_mob_h');
        });
        var url = window.location.pathname;
        var pgName = url.split("/")[2].toLocaleLowerCase();
        if (pgName == 'surveys') {
            $scope.tabActive = 'surveys';
        }
        if (pgName == 'rewards') {
            $scope.tabActive = 'rewards';
        }
        if (pgName == 'account') {
            $scope.tabActive = 'account';
        }
        if (pgName == 'piiconfirm') {
            $scope.tabActive = 'account';
        }
        if (pgName == 'deleteuserdata') {
            $scope.tabActive = 'account';
        }
        if (pgName == 'downline') {
            $scope.tabActive = 'downline';
        }
        if (pgName == 'invite') {
            $scope.tabActive = 'downline';
        }
        if (pgName == 'rc') {
            $scope.tabActive = 'rewards';
        }
        if (pgName == 'obprofile') {
            $scope.tabActive = 'obprofile';
        }
        if (pgName == 'moreabout') {
            $scope.moreAbout = false;
        }
        if (pgName == 'changepassword') {
            $scope.hidebar = true;
        }
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
        //
        //httpService.getData('/Ep/GetUserData').then(function (response) {
        //    $scope.user = response;
        //}, function (err) {
        //    $scope.errMsg = true;
        //});
        var GetRewardsHistory = function () {
            httpService.postData('/mr/GetRewardsHistory', "").then(function (response) {
                $scope.userRewardsInfo = response;
                if (response == '') {
                    $scope.userRewardsInfo.TotalEarnings = 0;
                    $scope.userRewardsInfo.TotalRedemptions = 0;
                    $scope.userRewardsInfo.AccountBalance = 0;
                }
                if ($scope.user.CountryId == 572 || $scope.user.CountryId == 494 || $scope.user.CountryId == 497 || $scope.user.CountryId == 502 || $scope.user.CountryId == 503 || $scope.user.CountryId == 505 || $scope.user.CountryId == 506 || $scope.user.CountryId == 511 || $scope.user.CountryId == 512 || $scope.user.CountryId == 1171 || $scope.user.CountryId == 519 || $scope.user.CountryId == 522 || $scope.user.CountryId == 525 || $scope.user.CountryId == 529 || $scope.user.CountryId == 38 || $scope.user.CountryId == 534 || $scope.user.CountryId == 538 || $scope.user.CountryId == 543 || $scope.user.CountryId == 545 || $scope.user.CountryId == 546 || $scope.user.CountryId == 548 || $scope.user.CountryId == 549 || $scope.user.CountryId == 549 || $scope.user.CountryId == 553 || $scope.user.CountryId == 559 || $scope.user.CountryId == 564 || $scope.user.CountryId == 565 || $scope.user.CountryId == 572 || $scope.user.CountryId == 571 || $scope.user.CountryId == 575 ||
                                                    $scope.user.CountryId == 576 || $scope.user.CountryId == 588 || $scope.user.CountryId == 590 || $scope.user.CountryId == 591 || $scope.user.CountryId == 593 || $scope.user.CountryId == 599 || $scope.user.CountryId == 604 || $scope.user.CountryId == 613 || $scope.user.CountryId == 614 || $scope.user.CountryId == 619 || $scope.user.CountryId == 620 || $scope.user.CountryId == 1180 || $scope.user.CountryId == 1166 || $scope.user.CountryId == 638 || $scope.user.CountryId == 641 || $scope.user.CountryId == 647 || $scope.user.CountryId == 657 || $scope.user.CountryId == 662 || $scope.user.CountryId == 664 || $scope.user.CountryId == 665 || $scope.user.CountryId == 668 || $scope.user.CountryId == 669 || $scope.user.CountryId == 673 || $scope.user.CountryId == 460 || $scope.user.CountryId == 688 || $scope.user.CountryId == 693 || $scope.user.CountryId == 694 || $scope.user.CountryId == 697 || $scope.user.CountryId == 700 ||
                                                    $scope.user.CountryId == 706 || $scope.user.CountryId == 718 || $scope.user.CountryId == 719 || $scope.user.CountryId == 724 || $scope.user.CountryId == 727 || $scope.user.CountryId == 728 || $scope.user.CountryId == 1164 || $scope.user.CountryId == 731) {
                    $scope.userRewardsInfo.AccountBalance = ($scope.userRewardsInfo.AccountBalance + "").replace(".", ",");
                    $scope.userRewardsInfo.TotalEarnings = ($scope.userRewardsInfo.TotalEarnings + "").replace(".", ",");
                    $scope.userRewardsInfo.TotalRedemptions = ($scope.userRewardsInfo.TotalRedemptions + "").replace(".", ",");
                }
                //Get all tango rewards list
            });
        }
        //logo click
        $scope.homeClick = function () {
            window.location.href = '/Ms/Surveys';
        }
        $scope.profileClick = function () {
            window.location.href = '/profile/index';
        }
        // About Click
        $scope.aboutClick = function () {
            window.location.href = '/home/About';
        }
        //contact click
        $scope.contactClick = function () {
            window.location.href = '/home/contact?lc=' + $scope.recaptchalangCode;
        }
        //privacy click
        $scope.privacyClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/Footer/Privacy', 'SurveyDownline-Privacy', 'width=800,height=800')
        }
        //terms click
        $scope.termsClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/Footer/TC', 'SurveyDownline-Terms', 'width=800,height=800')
        }
        $scope.aboutClick = function () {
            window.location.href = '/home/About';
        }
        //Do not sell my info
        $scope.doNotSellMyInfo = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/dns?lc=' + $scope.recaptchalangCode, 'SurveyDownline-DNS', 'width=800,height=800')
        }
        $scope.submit = function () {
            httpService.getData('/Common/LogOut').then(function (response) {
                if ($cookies.get('UserLangCode') != "" && $cookies.get('UserLangCode') != undefined) {
                    $cookies.put('MainLangCode', undefined, {
                        expires: exp,
                        path: '/'
                    });
                    $cookies.put('MainFlagCode', undefined, {
                        expires: exp,
                        path: '/'
                    });
                }
                window.location.href = $scope.orgDetails.MgLoginPath;
            }, function (err) {
                window.location.href = $scope.orgDetails.MgLoginPath;
            });
        }
        $scope.newSiteclick = function () {
            var ln;
            var pagname;
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
                ln = $cookies.get('UserLangCode') || $cookies.get('MainLangCode');
                if (ln == "es") {
                    pagname = '/es.html';
                }
                else if (ln == "Ch") {
                    pagname = '/ch.html';
                }
                else if (ln == "De") {
                    pagname = '/de.html';
                }
                else if (ln == "Fr") {
                    pagname = '/fr.html';
                }
                else if (ln == "It") {
                    pagname = '/it.html';
                }
                else if (ln == "Ja") {
                    pagname = '/jp.html';
                }
                else if (ln == "nd") {
                    pagname = '/nd.html';
                }
                else if (ln == "pt") {
                    pagname = '/pt.html';
                }
                else if (ln == "Ru") {
                    pagname = '/ru.html';
                }
                else {
                    pagname = '/en.html';
                }
            }
            else {
                ln = translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
                ln = "en";
                pagname = '/en.html';
            }
            $window.open($scope.orgDetails.MemberUrl + pagname, '', 'width=800,height=800')
        }
    }]);

    app.controller("footerController", ['$scope', '$http', '$window', '$location', 'httpService', '$cookies', function ($scope, $http, $window, $location, httpService, $cookies) {
        $scope.recaptchalangCode = 'en';
        var d = new Date();
        var year = d.getFullYear();
        $scope.currentYear = year;
        $scope.nextYear = year + 1;
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
        if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
            $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
            $scope.recaptchalangCode = $cookies.get('UserLangCode') || $cookies.get('MainLangCode');
        }
        //privacy click
        $scope.privacyClick = function () {
            $window.open('/Footer/Privacy', 'SurveyDownline-Privacy', 'width=800,height=800')
        }
        //terms click
        $scope.termsClick = function () {
            $window.open('/Footer/TC', 'SurveyDownline-Terms', 'width=800,height=800')
        }
        //about click
        $scope.aboutClick = function () {
            $window.open('/Footer/MoreAbout', 'SurveyDownline-Terms', 'width=800,height=800')
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
            // $window.open($scope.orgDetails.MemberUrl + '/home/Contact', 'SurveyDownline-ContactUs', 'width=800,height=800')
        }
        //about click
        $scope.aboutClick = function () {
            window.location.href = '/Footer/MoreAbout';
        }
        //FAQ click
        $scope.faqClick = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/faq', 'SurveyDownline-FAQ', 'width=800,height=800')
        }
        //Do not sell my info
        $scope.doNotSellMyInfo = function () {
            $window.open($scope.orgDetails.MemberUrl + '/login/dns?lc=' + $scope.recaptchalangCode, 'SurveyDownline-DNS', 'width=800,height=800')
        }
    }]);
    //obout controller
    app.controller('aboutController', ['$scope', '$http', '$location', '$rootScope', '$window', 'httpService', 'translationsLoadingService', 'getQueryParams',
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
    return app;
});
