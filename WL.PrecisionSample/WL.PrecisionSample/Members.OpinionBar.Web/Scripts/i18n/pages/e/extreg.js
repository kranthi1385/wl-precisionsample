
//main module of the soltion
angular.module("extregApp", ['pascalprecht.translate', 'customSerivces', 'vcRecaptcha'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider',
   function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider) {
       $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
       //load all translations setting to main transtion loader.
       $translateProvider.useLoader('$translatePartialLoader', {
           urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
       });
       //add default language
       $translateProvider.preferredLanguage('en');
       //regster providers

   }])

   //login controller section
   .controller("extregCtrl", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', 'vcRecaptchaService', 'getQueryParams',
function ($scope, $http, $window, $location, $rootScope, $timeout, httpService, translationsLoadingService, vcRecaptchaService, getQueryParams) {

   
    //load current login json file
    //translationsLoadingService.writeNlogService();
    translationsLoadingService.setCurrentUserLanguage("en");
    translationsLoadingService.loadTranslatePagePath("login");
    //$scope.EmailAddress = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]*\.([a-z]{2,4})$/;
    var rcheckr = getQueryParams.getUrlVars()["rcheckr"]
    if (rcheckr == undefined) {
        rcheckr = "";
    }
    var authkey = getQueryParams.getUrlVars()["authkey"]
    if (authkey == undefined) {
        authkey = "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761";
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
    $scope.imgloading = false;
    //Get query parmas value referrer id
    $scope.rgid = getUrlVars()["rgid"];
    $scope.rid = getUrlVars()["rid"];
    $scope.sid = getUrlVars()["sid"];
    $scope.txid = getUrlVars()["txid"];
    $scope.transid = getUrlVars()["trans_id"];
    $scope.authkey = getUrlVars()["authkey"];
    $scope.prjid = getUrlVars()["prjid"];
    if ($scope.prjid == undefined) {
        $scope.prjid = '0';
    }
    $scope.fbclid = getUrlVars()["fbclid"];
    if ($scope.fbclid != undefined && $scope.txid == undefined) {
        $scope.txid = $scope.fbclid;
    }
    $scope.captchaResponse = '';
    $scope.showButton = false;
    //beind years
    var d = new Date();
    $scope.currentYear = d.getFullYear();
    var yearLst = [];

    if (window.location.href.indexOf("conversant") > -1) {
        $scope.authkey = '71CC02CD-B8A5-4A5A-AF4D-9988B2F21761';
    }
    else {
        $scope.authkey = '9EE17B1A-6882-4CAE-9AED-3C4D4A92DFA9';
    }

    if ($scope.authkey == '71CC02CD-B8A5-4A5A-AF4D-9988B2F21761') {
        $scope.showButton = true;
        for (var i = 18; i < 100; i++) {
            yearLst.push({ key: $scope.currentYear - i, value: $scope.currentYear - i });
        }
    }
    else {
        $scope.showButton = false;
        for (var i = 13; i < 100; i++) {
            yearLst.push({ key: $scope.currentYear - i, value: $scope.currentYear - i });
        }
    }
    $scope.year = yearLst;
    $scope.step1 = "/PartialViews/ucl/step1.html";





    //get all avaliable ethnicities
    httpService.getData('/Common/GetEthnicityList').then(function (response) {
        $scope.ethnicityLst = response;
    }, function (err) {
        // $scope.errMsg = true;
    });

    //image loading
    //var imgPgs1 = new Array( // relative paths of images
    // '/Images/background-1.jpg',
    //'/Images/background-2.jpg',
    //'/Images/background-3.jpg',
    //'/Images/background-4.jpg');
    //var preloadArrPgs1 = new Array();
    //var i; /* preload images */
    //for (i = 0; i < imgPgs1.length; i++) {
    //    preloadArrPgs1[i] = new Image();
    //    preloadArrPgs1[i].src = imgPgs1[i];
    //}
    //$('.cp-bg').css('background', 'url(' + preloadArrPgs1[0].src + ') no-repeat');
    //var curImgPgs1 = 1;
    //var intIDPgs1 = setInterval(changeImgPgs1, 6000);
    ///* image rotator */function changeImgPgs1() {
    //    $('.cp-bg').animate({ opacity: 0 }, 1000, function () {
    //        $(this).css('background', 'url(' + preloadArrPgs1[curImgPgs1].src + ') no-repeat');
    //        //$(this).css('background-size', '70%');
    //        if (curImgPgs1 < preloadArrPgs1.length - 1) {
    //            curImgPgs1++;
    //        }
    //        else {
    //            curImgPgs1 = 0;
    //        }
    //    }).animate({ opacity: 1 }, 1000);
    //}
    //$scope.ZipValidate = function (ZipCode) {
    //    var patternUs = new RegExp("^\\d{5}(-{0,1}\\d{4})?$");
    //    var patternCa = new RegExp(/([ABCEGHJKLMNPRSTVXY]\d)([ABCEGHJKLMNPRSTVWXYZ]\d){2}/i);
    //    var patternAus = new RegExp("^(0[289][0-9]{2})|([1-9][0-9]{3})$");
    //    var patternUk = new RegExp("^([A-Za-z][A-Ha-hJ-Yj-y]?[0-9][A-Za-z0-9]? ?[0-9][A-Za-z]{2}|[Gg][Ii][Rr] ?0[Aa]{2})$");
    //    if (patternUs.test(ZipCode) || patternCa.test(ZipCode) || patternAus.test(ZipCode) || patternUk.test(ZipCode)) {
    //        $scope.isZiprMsg = false;
    //    }
    //    else {
    //        $scope.isZiprMsg = true;
    //    }
    //}
    //user save
    $scope.submit = function (valid) {
        if (valid) {
            $scope.imgloading = true;
            $scope.isZiprMsg = false;
            var patternUs = new RegExp("^\\d{5}(-{0,1}\\d{4})?$");
            var patternCa = new RegExp("^[0-9]{5}$|^[A-Z][0-9][A-Z] ?[0-9][A-Z][0-9]$"); //[ABCEGHJKLMNPRSTVXY]d[A-Z] d[A-Z]d
            //var patternAus = new RegExp("^(0[289][0-9]{2})|([1-9][0-9]{3})$");
            //var patternUk = new RegExp("^([A-Za-z][A-Ha-hJ-Yj-y]?[0-9][A-Za-z0-9]? ?[0-9][A-Za-z]{2}|[Gg][Ii][Rr] ?0[Aa]{2})$");
            var patternInteger = new RegExp("^[0-9]+$");
            if ($scope.user.CountryId == 231) {
                if (patternUs.test($scope.user.ZipCode)) {
                    $scope.isZiprMsg = false;
                }
                else {
                    $scope.isZiprMsg = true;
                }
            }
            if ($scope.user.CountryId == 38) {//for CA
                if (($scope.user.ZipCode.length >= 5 && $scope.user.ZipCode.length <= 8) && (!patternInteger.test($scope.user.ZipCode))) {
                    $scope.isZiprMsg = false;
                }
                else {
                    $scope.isZiprMsg = true;
                }
            }
            if ($scope.user.CountryId == 15) {//for AU
                if ($scope.user.ZipCode.length == 4 && patternInteger.test($scope.user.ZipCode)) {
                    $scope.isZiprMsg = false;
                }
                else {
                    $scope.isZiprMsg = true;
                }
            }
            if ($scope.user.CountryId == 229) {//uk
                if (($scope.user.ZipCode.length >= 3 && $scope.user.ZipCode.length <= 10)) {
                    $scope.isZiprMsg = false;
                }
                else {
                    $scope.isZiprMsg = true;
                }
            }
        }
        $scope.imgloading = false;
        saveUser(valid);
    }


    $scope.setResponse = function (response) {
        $scope.captchaResponse = response;
        //alert(response);
    };

    //save user click
    var saveUser = function (valid) {
        $scope.imgloading = true;
        $scope.captchaErr = false;
        $scope.isRegErrMsg = false;
        $scope.issessionErrMsg = false;
        if (valid) {
            if ($scope.isZiprMsg == false) {
                if ($scope.captchaResponse != '') {
                    httpService.postData('/Common/ValidateCaptcha?googleResponse=' + $scope.captchaResponse).then(function (response) {
                        if ($scope.captchaResponse != '') {//google captcha valid
                            $scope.user.rfid = $scope.rid;
                            $scope.user.SubId3 = $scope.txid;
                            $scope.user.SubId2 = $scope.sid;
                            $scope.user.TransactionId = $scope.transid;
                            $scope.user.ExtId = $scope.authkey;
                            httpService.postData('/cor/extuserInsert', $scope.user).then(function (res) {
                                if (res != "" && res != undefined && res != "00000000-0000-0000-0000-000000000000") {

                                    if ($scope.authkey.toLowerCase() == "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761".toLowerCase())// for CP 
                                    {
                                        cid = 542
                                    }
                                    if ($scope.authkey.toLowerCase() == "9EE17B1A-6882-4CAE-9AED-3C4D4A92DFA9".toLowerCase())// for OB 
                                    {
                                        cid = 541
                                    }

                                    window.location.href = '/cor/p?ug=' + res + "&pg=" + $scope.authkey + "&cid=" + cid + "&prjid=" + $scope.prjid;
                                }
                                else {
                                    // $scope.isRegErrMsg = true;
                                    $scope.issessionErrMsg = true;
                                    $scope.imgloading = false;
                                }

                            }, function (err) {
                                //  $scope.errMsg = true;
                                $scope.imgloading = false;
                            });
                        }
                        else {
                            $scope.captchaErr = true;
                            $scope.imgloading = false;
                        }
                    }, function (err) {
                        //  $scope.errMsg = true;
                        $scope.imgloading = false;
                    });
                }
                else {
                    $scope.captchaErr = true;
                    $scope.imgloading = false;
                }
            }
            else {
                $scope.isZiprMsg = true;
                $scope.imgloading = false;
            }
        }
        else {
            $scope.isRegErrMsg = true;
            $scope.imgloading = false;
        }
    }



    //privacy click
    $scope.privacyClickOB = function () {
        $window.open('https://www.opinionbar.com/Footer/Privacy', 'OB-Privacy', 'width=800,height=800')
    }
    //terms click
    $scope.termsClickOB = function () {
        $window.open('https://www.opinionbar.com/Footer/TC', 'OB-Terms', 'width=800,height=800')
    }

    //Conversant privacy
    $scope.privacyClickCon = function () {
        $window.open('https://wl.conversant.pro/login/p', 'Conversant-Privacy', 'width=800,height=800')
    }
    //Conversant terms
    $scope.termsClickCon = function () {
        $window.open('https://wl.conversant.pro/login/t', 'Conversant-Terms', 'width=800,height=800')
    }

    $scope.popup = function (url) {
        var doc;
        var width = 700;
        var height = 550;
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
        if (url.includes('iframe') || url.includes('script') || url.includes('img')) {
            newwin = window.open('', 'windowname5', params);
            doc = newwin.document;
            doc.write("<html><body>" + url + "</body></html>")
            if (window.focus) { newwin.focus() }
        }
        else {
            newwin = window.open(url, 'windowname5', params);
            if (window.focus) { newwin.focus() }
        }

        return false;
    }
}])

//LBS controller
 .controller("lbsController", ['$scope', '$rootScope', 'httpService', 'translationsLoadingService', 'vcRecaptchaService', 'getQueryParams',
 function ($scope, $rootScope, httpService, translationsLoadingService, vcRecaptchaService, getQueryParams) {
     //translationsLoadingService.writeNlogService();
     var SurveyUrl = "";
     ug = getQueryParams.getUrlVars()["ug"];
     if (ug == undefined) {
         ug = '';
     }

     pg = getQueryParams.getUrlVars()["pg"];
     if (pg == undefined) {
         pg = '';
     }

     var cid = getQueryParams.getUrlVars()["cid"];
     if (ccr == undefined) {
         ccr = 'en';
     }
     translationsLoadingService.setCurrentUserLanguage(ccr);
     translationsLoadingService.loadTranslatePagePath("tas");
     //Take Another Survey Button Click 
     $scope.takeme = function (SurveyUrl) {
         if (SurveyUrl != '' && SurveyUrl != undefined) {
             //Redirect the Member to Next top 1 Survey Click page URL.
             window.location.href = _surveyUrl;
         }
         else {
             window.location.href = "/mem/ms?ug=" + ug + "&usg=" + usg + "&uig=" + uig + "&usid=" + usid + "&pid=" + pid + '&cid=' + cid;
         }
     }
     //No Thank you click page.
     $scope.thankyou = function () {
         window.location.href = "/mem/ms?ug=" + ug + "&usg=" + usg + "&uig=" + uig + "&usid=" + usid + "&pid=" + pid + '&cid=' + cid;
     }
 }]);
