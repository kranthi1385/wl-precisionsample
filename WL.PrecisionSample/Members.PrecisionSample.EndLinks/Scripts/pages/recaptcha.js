// to get URL params
function getUrlVars() {
    var Url = window.location.href;
    var vars = {};
    var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}
var cc = getUrlVars()["cc"];
if (cc == undefined) {
    cc = 'en';
}

//main module of the soltion
angular.module("endLinksApp", ['pascalprecht.translate', 'customSerivces'])
    //config all required providers.(ex: color palette, routing, register all providers
    .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider',
        function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider) {

            ////load all translations setting to main transtion loader.
            $translateProvider.useLoader('$translatePartialLoader', {
                urlTemplate: '/Scripts/i18n/{lang}/{part}.json'
            });
            ////add default language
            $translateProvider.preferredLanguage(cc);
            //regster providers

        }])

    //login controller section
    .controller("captchaCntrl", ['$scope', '$http', '$window', '$location', 'translationsLoadingService', 'getQueryParams', 'httpService', '$translate', 'clickLoggerService',
        function ($scope, $http, $window, $location, translationsLoadingService, getQueryParams, httpService, $translate, clickLoggerService) {
            //translationsLoadingService.writeNlogService();
            var ccr = getQueryParams.getUrlVars()["cc"];
            if (ccr == undefined) {
                ccr = 'en';
            }
            $scope.retryCount = 0;
            var ug = getQueryParams.getUrlVars()["ug"];
            var uig = getQueryParams.getUrlVars()["uig"];
            var cid = getQueryParams.getUrlVars()["cid"];
            translationsLoadingService.setCurrentUserLanguage(ccr);
            translationsLoadingService.loadTranslatePagePath("rcp");
            // $translate.refresh();
            $scope.src1 = '';
            $scope.src2 = '';
            var dc = '';
            //chek mobile browser
            function checkdeviceInfo() {
                var regExp = '';
                regExp = new RegExp('Android|webOS|iPhone|iPad|' +
                    'BlackBerry|Windows Phone|' +
                    'Opera Mini|IEMobile|Mobile',
                    'i');

                if (regExp.test(navigator.userAgent))
                    return 'mobile'
                else
                    return "unknown";
            }
            $scope.CaptchaErrorLog = function () {
                var errorInput = document.getElementById("captchaError");
                var captchaId = document.getElementById("recaptchaId").value;
                var captchaName =   captchaId == "1" ? "CloudFlare" :
                                    captchaId == "2" ? "HCaptcha" :
                                                       "G Captcha";
                clickLoggerService.debug(captchaName + ":" + errorInput.value + "|" + window.location.href);
                if (retryCount < 2) {
                    retryCount++;
                    var captchaReset = captchaId == "1" ? turnstile :
                        captchaId == "2" ? hcaptcha : null;
                    captchaReset.reset();
                }
                
            }

            //$scope.renderRecaptcha();
            $scope.privacyClick = function () {
                $window.open('/privacy.html', 'Opinionetwork - Privacy', 'width=800,height=800')
            }


            $scope.termsClick = function () {
                $window.open('/termsofuse.html', 'Opinionetwork - Terms of Serviec', 'width=800,height=800')
            }
            $scope.donosellinfo = function () {
                $window.open('http://dev.affiliate.sdl.com/login/dns/', 'Opinionetwork - Terms of Serviec', 'width=800,height=800')
            }
            if (ug != uig) { //Need to Fire Cookie Logic for Internal Mebmers only.
                httpService.getData('/E/getCookie?ug=' + ug + "&cid=" + cid).then(function (response) {
                    var cookies = [];
                    if (response != null) {
                        cookievalues = response.split(',');
                        cookies = cookievalues[0].split(';');
                        angular.forEach(cookies, function (cookie, index) {
                            if (cookie == '1' && cookie != '') {
                                dc += '1;';
                                var src = "https://img.macromill.com/psmyp/SetCookie.html?" + ug.toUpperCase();
                                document.getElementById("myframe").src = src;
                                clickLoggerService.debug(document.referrer + '|' + src)
                            }
                            if (cookie == '2' && cookie != '') {
                                (function () {
                                    dc += '2;';
                                    var js = document.createElement('script');
                                    js.src = '//gwiqcdn.globalwebindex.net/gwiq/gwiq.js';
                                    js.type = 'text/javascript';
                                    js.async = 'true';
                                    js.onload = js.onreadystatechange = function () {

                                        var rs = this.readyState;
                                        if (rs && rs != 'complete' && rs != 'loaded') return;
                                        try {
                                            gwiq.asyncjs("cid=c0063&memberID=" + ug.toUpperCase());
                                        } catch (e) { }
                                    };
                                    var s = document.getElementsByTagName('script')[0];
                                    s.parentNode.insertBefore(js, s);
                                })();
                            }
                            if (cookie == '3' && cookie != '') {
                                //var _dvtype = checkdeviceInfo();
                                //if (_dvtype == "unknown" || _dvtype == '') { // Condition has been commented by Giri on 04/09/2019 as per chat with partha
                                dc += '3;';
                                $scope.src1 = "https://b.voicefive.com/p?c1=14&c2=22118396&c3=" + ug.toUpperCase() + "&c4=v&cj=1";
                                $scope.src2 = "https://b.scorecardresearch.com/p?c1=14&c2=22118396&c3=" + ug.toUpperCase() + "&c4=s&cj=1";
                                //}
                            }

                            //if (cookie == '4' && cookie != '') {
                            //    dc += '4;';
                            //    var src = "https://p.adsymptotic.com/d/px/?_pid=15299&_psign=28fc16a19b66dca17631a8d4401a5f19&_pp=02&_puuid=ps_" + ug.toUpperCase();
                            //    document.getElementById("drawbridge").src = src;
                            //}
                            //if (cookie == '5' && cookie != '') {
                            //    (function () {
                            //        dc += '5;';
                            //        var src = "https://pippio.com/api/sync?pid=500046&it=4&iv=" + cookievalues[2] + '&it=' + 4 + '&iv=' + cookievalues[3] + '&it=' + 4 + '&iv=' + cookievalues[4];
                            //        document.getElementById("liveramp").src = src;
                            //    })();
                            //}
                            if (cookie == '6' && cookie != '') {
                                (function () {
                                    dc += '6;';
                                    var src = "https://pixel.mathtag.com/sync/img?redir=https://ds.reson8.com/sync/vendormm.gif?user_id=[MM_UUID]"
                                    document.getElementById("mymathtag").src = src;
                                    var resonatesrc = "https://survey.reson8.com/survey/check.gif?waveKey=vendor10"
                                    document.getElementById("resonate").src = resonatesrc;
                                })();
                            }
                            if (cookie == '7' && cookie != '') {
                                dc += '7;';
                                var src = "https://pixel.tapad.com/idsync/ex/receive?partner_id=3231&partner_device_id=" + ug.toUpperCase();
                                document.getElementById("tapad").src = src;
                            }
                        }
                        )
                        //To save All Cookies Fired Info.
                        if (dc != '') {
                            httpService.postData('/E/saveCookie?ug=' + ug + '&CookieIds=' + dc + '&cid=' + cid).then(function (response) {
                                $scope.result = response;
                            }, function (err) {
                            });
                        }
                    }
                }, function (err) {

                    $scope.errMsg = true;
                });
            }
    }])

   .controller("tasController", ['$rootScope', '$scope', 'httpService', 'getQueryParams', 'translationsLoadingService',
 function ($rootScope, $scope, httpService, getQueryParams, translationsLoadingService) {
     //translationsLoadingService.writeNlogService();
     var SurveyUrl = "";
     ug = getQueryParams.getUrlVars()["ug"];
     if (ug == undefined) {
         ug = '';
     }

     usg = getQueryParams.getUrlVars()["usg"];
     if (usg == undefined) {
         usg = '';
     }


     uig = getQueryParams.getUrlVars()["uig"];
     if (uig == undefined) {
         uig = '';
     }


     usid = getQueryParams.getUrlVars()["usid"];
     if (usid == undefined) {
         usid = "";
     }
     pid = getQueryParams.getUrlVars()["project"];
     if (pid == undefined) {
         pid = "";
     }

     var cid = getQueryParams.getUrlVars()["cid"];
     var ccr = getQueryParams.getUrlVars()["cc"];
     if (ccr == undefined) {
         ccr = 'en';
     }
     translationsLoadingService.setCurrentUserLanguage(ccr);
     translationsLoadingService.loadTranslatePagePath("tas");
     $scope.orgID = cid;
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


