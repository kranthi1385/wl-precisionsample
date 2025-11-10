
//main module of the soltion
angular.module("loginApp", ['pascalprecht.translate', 'customSerivces', 'vcRecaptcha'])
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
   .controller("loginCtrl", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', 'vcRecaptchaService', 'getQueryParams','$cookies',
function ($scope, $http, $window, $location, $rootScope, $timeout, httpService, translationsLoadingService, vcRecaptchaService, getQueryParams, $cookies) {
    //load current login json file
    //translationsLoadingService.writeNlogService();
    translationsLoadingService.setCurrentUserLanguage("en");
    translationsLoadingService.loadTranslatePagePath("login");
    //$scope.EmailAddress = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]*\.([a-z]{2,4})$/;
    var rcheckr = getQueryParams.getUrlVars()["rcheckr"]
    if (rcheckr == undefined) {
        rcheckr = "";
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
    //Get query parmas value referrer id
    $scope.ug = getUrlVars()["ug"];
    $scope.lid = getUrlVars()["lid"];
    $scope.id = getUrlVars()["id"];
    $scope.captchaResponse = '';

    $scope.getCookie = function (redirectUrl) {
        //cookie is cookie logic. 
        if (document.cookie != "" && document.cookie != undefined) {
            var curentCookie = document.cookie;
            if (curentCookie.split('=')[1] != undefined) {
                return curentCookie.split('=')[1]
            }
            else {
                return null;
            }
        }
        else {
            return null;
        }

    }
    //write new cookie
    $scope.writeCookie = function (name, cook) {
        //write cookie for one day  it works all sub domains
        var now = new Date();
        var time = now.getTime();
        var expireTime = time + (3600 * 1000) * 8766;
        now.setTime(expireTime);
        document.cookie = escape(name) + "=" + escape(cook) + ';expires=' + now.toUTCString() + ';path=/';
    }
    $scope.checkCookie = function () {
        var user = $scope.getCookie("sdlcookie");
        if (user != null) {
            $scope.nomNomCookie = false;
        } else {
            var cook = 1;
            $scope.writeCookie("sdlcookie", cook, 365);
            $scope.nomNomCookie = true;
        }
    }
    //write new cookie
    $scope.deleteCookie = function (name, cook) {
        //write cookie for one day  it works all sub domains
        document.cookie = escape(name) + "=" + escape(cook) + ';expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/';
    }
    //if user dont want to save cookie 
    $scope.delCookie = function () {
        var user = $scope.getCookie("sdlcookie");
        if (user != null) {
            var cook = 1;
            $scope.deleteCookie("sdlcookie", cook);
            $scope.okCookie();
        }
        else {
            $scope.okCookie();
        }

    }
    $scope.okCookie = function () {
        $scope.nomNomCookie = false;
    }

    $scope.checkCookie();

    //Google tag manager and facebook cookie
    $scope.acceptCookie = $cookies.get('google_fb_cookie_accept');
    //$scope.checkGoogleFBCookie();
    $rootScope.googleCookie = false;
    if ($scope.acceptCookie == 0) {
        $rootScope.googleCookie = true;
    }
    $scope.acceptCookiest = function () {
        $rootScope.googleCookie = true;
        $scope.writeCookie("google_fb_cookie_accept", 0, 365);
        //(function (w, d, s, l, i) {
        //    w[l] = w[l] || []; w[l].push({
        //        'gtm.start':
        //        new Date().getTime(), event: 'gtm.js'
        //    }); var f = d.getElementsByTagName(s)[0],
        //    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
        //    'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        //})(window, document, 'script', 'dataLayer', 'GTM-N9DDS62')
        //!function (f, b, e, v, n, t, s) {
        //    if (f.fbq) return; n = f.fbq = function () {
        //        n.callMethod ?
        //        n.callMethod.apply(n, arguments) : n.queue.push(arguments)
        //    };
        //    if (!f._fbq) f._fbq = n; n.push = n; n.loaded = !0; n.version = '2.0';
        //    n.queue = []; t = b.createElement(e); t.async = !0;
        //    t.src = v; s = b.getElementsByTagName(e)[0];
        //    s.parentNode.insertBefore(t, s)
        //}(window, document, 'script',
        //'https://connect.facebook.net/en_US/fbevents.js');
        //fbq('init', '4326893500713220');
        //fbq('track', 'PageView');
        //var lines = "<script>(function (w, d, s, l, i) {w[l] = w[l] || []; w[l].push({'gtm.start':new Date().getTime(), event: 'gtm.js'}); var f = d.getElementsByTagName(s)[0],j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src ='https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);})(window, document, 'script', 'dataLayer', 'GTM-N9DDS62');</script> <noscript><iframe src='https://www.googletagmanager.com/ns.html?id=GTM-N9DDS62' ></iframe></noscript>            <script>                !function (f, b, e, v, n, t, s) {                    if (f.fbq) return; n = f.fbq = function () {                        n.callMethod ?                        n.callMethod.apply(n, arguments) : n.queue.push(arguments)                    };                    if (!f._fbq) f._fbq = n; n.push = n; n.loaded = !0; n.version = '2.0';                    n.queue = []; t = b.createElement(e); t.async = !0;                    t.src = v; s = b.getElementsByTagName(e)[0];                    s.parentNode.insertBefore(t, s)                }(window, document, 'script',                'https://connect.facebook.net/en_US/fbevents.js');            fbq('init', '4326893500713220');            fbq('track', 'PageView');            </script>            <noscript>                <img height='1' width='1'        src='https://www.facebook.com/tr?id=4326893500713220&ev=PageView&noscript=1' />   </noscript>"
        //document.write(lines);
        //window.location.reload();
    }
    if (document.cookie.includes("_ga") || document.cookie.includes("_ga_16KTRLFVQ3") || document.cookie.includes("_gcl_au") || document.cookie.includes("_fbp") || document.cookie.includes("fr")) {
        $rootScope.googleCookie = true;
        $scope.acceptCookiest();
    }
    $scope.declineCookie = function () {
        $scope.writeCookie("google_fb_cookie_accept", 0, 365);
        $rootScope.googleCookie = true;
        var cookies = document.cookie.split(";");
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;

            if (name == "_ga" || name == " _ga" || name == " _ga_16KTRLFVQ3" || name == "_ga_16KTRLFVQ3" || name == " _gcl_au" || name == "_gcl_au" || name == "_fbp" || name == " _fbp") {
                document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/;domain=";
                /*below line is to delete the google analytics cookies. they are set with the domain*/
                document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/;domain=.opinionbar.com";
            }

        }
    }
    $scope.closeDialog = function () {
        $rootScope.googleCookie = true;
    }

    $scope.cookieSettings = function () {
        $window.open('/Login/cookiesettings', '', 'width=800,height=800')
    }

    $scope.step1 = "/PartialViews/ucl/step1.html";
    $scope.step2 = "/PartialViews/ucl/step2.html";
    $scope.step1Reg = "/PartialViews/ucl/step1-reg.html";
    $scope.footer = "/PartialViews/ucl/footer.html";

    //User LogIn object
    $scope.loginDetails = {
        EmailAddress: '',
        Password: ''
    }
    //user login  click
    $scope.userLoginClick = function (valid) {
        var replacepswd = $scope.loginDetails.Password;
        $scope.showStep1ErrMsg = false;
        $scope.isShowLoginFailErrmsg = false;
        $scope.showLoginError = false;
        if (valid) {
            $scope.loginDetails.Password = replacepswd.replace("&", "||**||").replace("#", "||*||").replace("+", "||***||");
            httpService.postData('/Home/login?email=' + $scope.loginDetails.EmailAddress + '&psw=' + $scope.loginDetails.Password).then(function (response) {
                if (response != "") {
                    window.location.href = response;
                }
                else {
                    $scope.isShowLoginFailErrmsg = true;
                }
            }, function (err) {
                //  $scope.errMsg = true;
            });
        }
        else {

            $scope.showLoginError = true;
        }
    }
    //load optintelligence offers
    function validateOpt() {

        if ($scope.user.FirstName != "" && $scope.user.Gender != null && $scope.user.Day != null && $scope.user.Month != null &&
            $scope.user.Year != null && $scope.user.zip != "") {
            oi.profile.email = $scope.user.EmailAddress;
            oi.profile.firstname = $scope.user.FirstName;
            oi.profile.lastname = $scope.user.LastName;
            oi.profile.address1 = $scope.user.Address1;
            oi.profile.postalcode = $scope.user.ZipCode;
            oi.profile.gender = $scope.user.Gender;
            oi.profile.dateofbirth = $scope.user.Month + '-' + $scope.user.Day + '-' + $scope.user.Year; //MM-DD-YYYY
            oi.profile.homephone = $scope.user.PhoneNumber; //##########
            oi.profile.businessphone = $scope.user.PhoneNumber; //##########
            oi.profile.mobilephone = $scope.user.PhoneNumber; //##########
            oi.profile.SID = ''; //any value you choose
            oi.profile.SID2 = ''; //any value you choose
            oi.sda = ["email", "lastname", "address1", "homephone", "businessphone", "mobilephone"];

            //Impression Call
            oi('#offers').impression();

            ////Jump behavior
            //oi('#offers').on('jump', function () {

            //});

            document.getElementById("divShow").style.display = 'block';
        }
        else {
            document.getElementById("divShow").style.display = 'none';
        }
    }
    //beind years
    var d = new Date();
    $scope.currentYear = d.getFullYear();
    var yearLst = [];
    for (var i = 13; i < 100; i++) {
        yearLst.push({ key: $scope.currentYear - i, value: $scope.currentYear - i });

    }
    $scope.year = yearLst;
    $scope.languageSelected = true;
    $scope.selectedLangCode = 0;
    var languageCodesLst = [];
    //set all avaliable languages
    languageCodesLst = [
        { LangName: "English", Img: "//static.miniclipcdn.com/layout/flags/46x32/US.png", LangCode: "en" },
        { LangName: "Español", Img: "//static.miniclipcdn.com/layout/flags/46x32/ES.png", LangCode: "es" },
       { LangName: "Português", Img: "//static.miniclipcdn.com/layout/flags/46x32/PT.png", LangCode: "pt" }
    ]
    $scope.languageCodes = languageCodesLst;
    $scope.selectedImg = languageCodesLst[0].Img;
    //get all avaliable ethnicities
    httpService.getData('/Common/GetEthnicityList').then(function (response) {
        $scope.ethnicityLst = response;
    }, function (err) {
        // $scope.errMsg = true;
    });

    //get all avaliable Languages
    httpService.getData('/Common/GetLanguageList').then(function (response) {
        $scope.languageLst = response;
    }, function (err) {
        $scope.errMsg = true;
    });

    httpService.getData('/Common/GetCountrysAndStates').then(function (response) {
        $scope.countries = response.CountryList;
        $rootScope.stateslist = response.StateList;
        if ($scope.lid != undefined) {
            $scope.getLeaduser();

        }
    }, function (err) {
        // $scope.errMsg = true;
    });
    //get all userdata
    httpService.getData('/Home/GetUserData').then(function (response) {
        $scope.user = response;
    }, function (err) {
        //  $scope.errMsg = true;
    });

    //get all avaliables states
    $scope.countryByStates = function (cc) {
        $scope.states = [];
        for (var i = 0; i < $rootScope.stateslist.length; i++) {
            if (cc == $rootScope.stateslist[i].CountryId) {
                $scope.states.push($rootScope.stateslist[i]);

            }
        }
        //validateOpt();
    }

    $scope.languageChange = function () {
        $scope.languageSelected = !$scope.languageSelected;

    }

    //with respective of user selected language set tanslation language  
    $scope.selectLanaguage = function (index, selectedLanguage) {
        $scope.selectedImg = selectedLanguage.Img;
        $scope.selectedLangCode = index;
        $scope.languageSelected = !$scope.languageSelected;
        httpService.postData('/Common/UpdateLanguageCode?LangCode=' + selectedLanguage.LangCode).then(function (response) {
        }, function (err) {
            $scope.errMsg = true;
        });
        translationsLoadingService.setCurrentUserLanguage(selectedLanguage.LangCode);
    }
    //for loquedigo
    $scope.lqeStep2 = false;
    $scope.enLanguage = function () {
        translationsLoadingService.setCurrentUserLanguage("en");
        translationsLoadingService.loadTranslatePagePath("login");
    }
    $scope.esLanguage = function () {
        translationsLoadingService.setCurrentUserLanguage("es");
        translationsLoadingService.loadTranslatePagePath("login");
    }
    $scope.ptLanguage = function () {
        translationsLoadingService.setCurrentUserLanguage("pt");
        translationsLoadingService.loadTranslatePagePath("login");
    }
    //loquedigo image slides
    var imgArr = new Array( // relative paths of images
    '/Images/Loquedigo/WL/iStock_main_bg.jpg',
    '/Images/Loquedigo/WL/shutterstock_main_bg31.jpg', '/Images/Loquedigo/WL/shutterstock_main_bg4.jpg',
    '/Images/Loquedigo/WL/shutterstock_main_bg5.jpg', '/Images/Loquedigo/WL/shutterstock_main_bg6.jpg'
    );
    var preloadArr = new Array();
    var i; /* preload images */
    for (i = 0; i < imgArr.length; i++) {
        preloadArr[i] = new Image();
        preloadArr[i].src = imgArr[i];
    }
    $('.cp-bg').css('background', 'url(' + preloadArr[0].src + ') no-repeat');
    var curImg = 1;
    var intID = setInterval(changeImg, 6000);
    /* image rotator */function changeImg() {
        $('.cp-bg').animate({ opacity: 0 }, 1000, function () {
            $(this).css('background', 'url(' + preloadArr[curImg].src + ') no-repeat');
            //$(this).css('background-size', '70%');
            if (curImg < preloadArr.length - 1) {
                curImg++;
            }
            else {
                curImg = 0;
            }
        }).animate({ opacity: 1 }, 1000);
    }

    if ($scope.browser) {
        if ($.browser.version <= 9.0) {
            $('input.form-control').css('padding-left', '0px');
        }
    }

    // panel of gamers image slider
    var imgPgs = new Array( // relative paths of images
    '/Images/panelofgamers/slide1.png',
    '/Images/panelofgamers/slide2.png', '/Images/panelofgamers/slide3.png');
    var preloadArrPgs = new Array();
    var i; /* preload images */
    for (i = 0; i < imgPgs.length; i++) {
        preloadArrPgs[i] = new Image();
        preloadArrPgs[i].src = imgPgs[i];
    }
    $('.featured').css('background', 'url(' + preloadArrPgs[0].src + ') no-repeat');
    var curImgPgs = 1;
    var intIDPgs = setInterval(changeImgPgs, 6000);
    /* image rotator */function changeImgPgs() {
        $('.featured').animate({ opacity: 0 }, 1000, function () {
            $(this).css('background', 'url(' + preloadArrPgs[curImgPgs].src + ') no-repeat');
            //$(this).css('background-size', '70%');
            if (curImgPgs < preloadArrPgs.length - 1) {
                curImgPgs++;
            }
            else {
                curImgPgs = 0;
            }
        }).animate({ opacity: 1 }, 1000);
    }

    if ($scope.browser) {
        if ($.browser.version <= 9.0) {
            $('input.form-control').css('padding-left', '0px');
        }
    }

    //panel of gamers slide 2
    var imgPgs1 = new Array( // relative paths of images
    '/Images/panelofgamers/logo-slide1.png',
    '/Images/panelofgamers/logo-slide2.png');
    var preloadArrPgs1 = new Array();
    var i; /* preload images */
    for (i = 0; i < imgPgs1.length; i++) {
        preloadArrPgs1[i] = new Image();
        preloadArrPgs1[i].src = imgPgs1[i];
    }
    $('.logo-slide-pgs').css('background', 'url(' + preloadArrPgs1[0].src + ') no-repeat');
    var curImgPgs1 = 1;
    var intIDPgs1 = setInterval(changeImgPgs1, 6000);
    /* image rotator */function changeImgPgs1() {
        $('.logo-slide-pgs').animate({ opacity: 0 }, 1000, function () {
            $(this).css('background', 'url(' + preloadArrPgs1[curImgPgs1].src + ') no-repeat');
            //$(this).css('background-size', '70%');
            if (curImgPgs1 < preloadArrPgs1.length - 1) {
                curImgPgs1++;
            }
            else {
                curImgPgs1 = 0;
            }
        }).animate({ opacity: 1 }, 1000);
    }

    if ($scope.browser) {
        if ($.browser.version <= 9.0) {
            $('input.form-control').css('padding-left', '0px');
        }
    }

    // We-tell image slider
    //    var imgPgs = new Array( // relative paths of images
    //    ['/Images/rg/set1/amazon-gift-card.png', '/Images/rg/set1/paypal_logo.png', '/Images/rg/set1/best-buy-gift-card.png', '/Images/rg/set1/starbucks-gift-card.png', '/Images/rg/set1/itunes-gift-card.png'],
    //    ['/Images/rg/set2/chilis-gift-card.png', '/Images/rg/set2/gamestop-gift-card.png', '/Images/rg/set2/target-gift-card.png', '/Images/rg/set2/lowes-gift-card.png', '/Images/rg/set2/habitat-donation-gift-card.png'],
    //['/Images/rg/set3/rei-gift-card.png', '/Images/rg/set3/macys-gift-card.png', '/Images/rg/set3/petco-gift-card.png', '/Images/rg/set3/toysrus-gift-card.png', '/Images/rg/set3/sportsauthority-gift-card.png']);
    var preloadArrWtl = new Array();
    var i; /* preload images */
    for (i = 0; i < imgPgs.length; i++) {
        preloadArrWtl[i] = new Image();
        preloadArrWtl[i].src = imgPgs[i];
    }
    $('.We-tell-slides').css('background', 'url(' + preloadArrWtl[0].src + ') no-repeat');
    var curImgWtl = 1;
    var intIDPgs = setInterval(changeImgWtl, 6000);
    /* image rotator */function changeImgWtl() {
        $('.featured').animate({ opacity: 0 }, 1000, function () {
            $(this).css('background', 'url(' + preloadArrWtl[curImgWtl].src + ') no-repeat');
            //$(this).css('background-size', '70%');
            if (curImgWtl < preloadArrWtl.length - 1) {
                curImgWtl++;
            }
            else {
                curImgWtl = 0;
            }
        }).animate({ opacity: 1 }, 1000);
    }

    if ($scope.browser) {
        if ($.browser.version <= 9.0) {
            $('input.form-control').css('padding-left', '0px');
        }
    }

    //optintelligence check
    $scope.optIntelligenceCheck = function () {
        validateOpt();
    }

    //user save
    $scope.submit = function (valid) {
        if ($scope.orgDetails.ClientId != 38) {
            oi('#offers').optIn(function () {

            });
        }

        saveUser(valid);



        // oi(​'#offers'​).optIn();

        // oi('#offers').optIn(function () {
        // //  oi('form[name="loginForm"]').submit();
        // saveUser(valid);
        // });
        // return false;

        //if ($("#hfShow").val() == '0') {
        //    checkflag();
        //    Delay();
        //    $timeout(function () { saveUser(isValid) }, 3000);
        //}
        //else {
        //  saveUser(valid)
        // }
    }



    //set optIntelligence value
    function checkflag() {
        if ($("#hfShow").val() == 0) {
            setvalue(1);
        }
        function setvalue(value) {
            $('input[name=OI_button]').val(value);
            document.getElementById('hfvalue').value = value;
            return true;
        }
    }

    var sleep = function (ms) {
        var dt = new Date();
        dt.settime((new Date()).getTime() + ms);
        while (new date().gettime() < (new Date()).getTime());
    }
    function Delay() {
        return oi_send();
        sleep(1000);
        return true;
    }


    //get current doamin details
    $scope.getCurrentDomailDetails = function () {
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
            $scope.orgDetails = res;
            document.cookie = "orgLogo=" + res.OrgLogo;
        }, function (err) {
        });
    }
    $scope.getCurrentDomailDetails();
    //validate email through xverify
    $scope.validateEmail = function () {
        $scope.showStep1ErrMsg = false;
        $scope.emailXVerify = -1;
        $scope.xVerifyMessage = '';
        if ($scope.user.EmailAddress != "") {
            //Emailaddress check
            httpService.getData('/Home/EmailAddressCheck?email=' + $scope.user.EmailAddress).then(function (response) {
                if (response.CpaCount >= 1) {
                    $scope.emailXVerify = 0;
                    $scope.xVerifyMessage = "Email Address already exist."
                }
            }, function (err) {
            });
            httpService.getData('/Home/vldEmail?email=' + $scope.user.EmailAddress).then(function (response) {
                if (response == "accepted") {
                    $scope.emailXVerify = 1;
                    $scope.xVerifyMessage = "Verified."
                    //var message = xVerify();
                    httpService.getData('/Home/EmailAddressCheck?email=' + $scope.user.EmailAddress).then(function (response) {
                        if (response.CpaCount >= 1) {
                            $scope.emailXVerify = 0;
                            $scope.xVerifyMessage = "Email Address already exist."
                        }
                    }, function (err) {
                    });
                }
                else {
                    $scope.emailXVerify = 0;
                    $scope.xVerifyMessage = "Email Address not exist."
                }
            }, function (err) {
            });
        }

    }
    //validate Password
    $scope.validatePswd = function () {
        if ($scope.user.Password == $scope.user.Cpassword) {
            $scope.xVerifyPassword = ""
        }
        else {
            $scope.xVerifyPassword = "Password not match"
        }
    }
    ///step1 registration clicck
    $scope.conForClick = true;
    $scope.Step1RegClick = function (valid) {
        if (valid) {
            $scope.showLoginError = false;
            $scope.showStep1ErrMsg = false;
            if ($scope.user.EmailAddress != "") {
                httpService.getData('/Home/vldEmail?email=' + $scope.user.EmailAddress).then(function (response) {
                    //var message = xVerify();z
                    if (response == "accepted") {
                        httpService.getData('/Home/EmailAddressCheck?email=' + $scope.user.EmailAddress).then(function (response) {
                            if (response.CpaCount == 0) {
                                httpService.postData('/Home/Step1Registration', $scope.user).then(function (response) {
                                    if (response != "") {
                                        window.location.href = $scope.orgDetails.MgStep2Path + "?ug=" + response;
                                    }
                                    else {

                                    }
                                }, function (err) {
                                });
                            }
                            else {
                                $scope.emailXVerify = 0;
                                $scope.xVerifyMessage = "Email Address already exist."
                            }
                        }, function (err) {
                        });
                    }
                    else {
                        $scope.emailXVerify = false;
                        $scope.xVerifyMessage = "Email Address not exist."
                    }
                }, function (err) {
                });
            }
        }
        else {
            $scope.showStep1ErrMsg = true;
        }
    }
    //get step2 Details
    $scope.getLeaduser = function () {
        httpService.getData('/Home/GetStep2Details?lid=' + $scope.lid).then(function (response) {
            $scope.user = response;
            $scope.luser = $scope.user;
            $scope.user.RefferId = 20708;
            if ($scope.user.CountryId != "") {
                $scope.countryByStates($scope.user.CountryId);
            }
        }, function (err) {
        });
    }
    $scope.setResponse = function (response) {
        $scope.captchaResponse = response;
    };
    //save user click
    var saveUser = function (valid) {
        $scope.captchaErr = false;
        $scope.isRegErrMsg = false;
        if ($scope.orgDetails.ClientId == 383) {
            //$scope.validateEmail();
            $scope.emailXVerify = 1;
            $scope.user.SubId3 = $scope.lid;
        }
        if ($scope.orgDetails.ClientId == 385) {
            if ($scope.emailXVerify != undefined || $scope.emailXVerify != "")
                $scope.emailXVerify = 1;
        }
        if (valid && $scope.emailXVerify == 1) {
            if ($scope.captchaResponse != '') {
                //$scope.isRegErrMsg = false;
                if ($scope.orgDetails.ClientId == 385) {
                    httpService.getData('/Home/GetLinkedinData?id=' + $scope.id).then(function (response) {
                        $scope.linkedin = response;
                        $scope.user.FirstName = $scope.linkedin.FirstName;
                        $scope.user.LastName = $scope.linkedin.LastName;
                        $scope.user.EmailAddress = $scope.linkedin.EmailAddress;
                    }, function (err) {
                    });
                }
                httpService.postData('/Common/ValidateCaptcha?googleResponse=' + $scope.captchaResponse).then(function (response) {
                    if (response == 1) {//google captcha valid
                        if ($scope.lid != undefined) {
                            $scope.user.City = ".";
                            $scope.user.Address1 = $scope.luser.Address1;
                            $scope.user.StateId = $scope.luser.StateId;
                            $scope.user.EthnicityId = 1;
                        }

                        httpService.postData('/Home/saveUser', $scope.user).then(function (res) {
                            if (res != "") {
                                if ($scope.lid != undefined) {
                                    window.location.href = '/Ms/Surveys?ug=' + res.UserGuid;
                                }
                                else if (res.UserGuid != "" && res.UserGuid != undefined) {
                                    window.location.href = '/rg/relevant?ug=' + res.UserGuid + "&id=" + res.UserId + "&c=" + res.CountryCode + "&lpage=homepage&rcheckr=" + rcheckr + "&cid=" + $scope.orgDetails.ClientId;
                                }
                            }
                            else {
                                $scope.isRegErrMsg = true;
                            }

                        }, function (err) {
                            //  $scope.errMsg = true;
                        });
                    }
                    else {
                        $scope.captchaErr = true;
                    }
                }, function (err) {
                    //  $scope.errMsg = true;
                });
            }
        }
        else {
            if ($scope.emailXVerify == 0) {

            }
            else {
                //$scope.captchaErr = true;
            }
            $scope.isRegErrMsg = true;
        }
    }


    //Us Opinion Poll 
    //hide and show login and signup page
    $scope.userSignIn = true;
    $scope.userSignup = false;
    $scope.Reg = function (isValid) {
        //if (isValid) {
        $scope.showMessage = false;
        //$scope.user.EmailAddress = $scope.loginObj.userName
        //$scope.user.Password = $scope.loginObj.password
        $scope.userSignIn = false; //usersign flag
        $scope.userSignup = true; //usersignup falg
        //}
        //else {
        //    $scope.showMessage = true;
        //}
    }
    //home click
    $scope.homeClick = function () {
        window.location.href = $scope.orgDetails.MemberUrl + '/wl/lqestep1';
    }
    //privacy click
    $scope.privacyClick = function () {
        $window.open($scope.orgDetails.MemberUrl + '/login/p', 'SurveyDownline-Privacy', 'width=800,height=800')
    }
    //terms click
    //$scope.termsClick = function () {
    //    $window.open($scope.orgDetails.MemberUrl + '/login/t', 'SurveyDownline-Terms', 'width=800,height=800')
    //}
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
        $scope.getCurrentDomailDetails();
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
            debugger;
            if (valid) {
                $scope.isShowContactErr = false;
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
                $scope.isShowContactErr = true;
            }
        }
        if ($scope.orgDetails.ClientId == 111) {
            $scope.contactUsClick = function (valid) {
                if (valid) {
                    $scope.isShowContactErr = false;
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
                    $scope.isShowContactErr = true;
                }
            }
        }
        else {
            $window.open($scope.orgDetails.MemberUrl + '/login/cu', 'SurveyDownline-ContactUs', 'width=800,height=800')
        }
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
    //forgot password
    $scope.forgetPswd = false;
    $scope.forgotPassword = function () {
        $scope.lqeStep2 = true;
        $scope.forgetPswd = true;
        $scope.conForClick = false;
        $scope.contactUs = false;
        $scope.errMsg = false;
        $scope.saveref = false;
        $scope.getUserData = {
            EmailAddress: ''
        }
        $scope.forgotPasswordClick = function (valid) {
            if (valid) {
                $scope.saveref = false;
                $scope.forgotPswdErrMsg = false;
                $scope.forgetPswd = true;
                if ($scope.getUserData.EmailAddress != "") {
                    httpService.getData('/Login/GetUserDataEmail?EmailAddress=' + $scope.getUserData.EmailAddress).then(function (response) {
                        $scope.getUserData = response;
                        $scope.getUserData.CustomAttribute = "first_name:" + $scope.getUserData.FirstName + ";last_name:"
                            + $scope.getUserData.LastName + ";email_address:" + $scope.getUserData.EmailAddress + ";password:"
                            + $scope.getUserData.Password + ";create_dt:" + $scope.getUserData.CreateDate + ";ip_address:"
                            + $scope.getUserData.IpAddress + ";org_logo:" + $scope.orgDetails.OrgLogo.replace("http://", "") + ";org_name:" + $scope.orgDetails.OrgName
                             + ";member_url:" + $scope.orgDetails.MemberUrl + ";user_guid:" + $scope.getUserData.UserGuid;
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
            }
            else {
                $scope.forgotPswdErrMsg = true;
            }
        }
    }


    $scope.divReg1 = false;
    $scope.onClick = function () {
        $scope.divReg1 = true;
    }
    $scope.save = function (valid, member) {
        httpService.postData('/Home/saveUser', $scope.user).then(function (res) {
            if (res != "") {
                if ($scope.lid != undefined) {
                    window.location.href = '/Ms/Surveys?ug=' + res.UserGuid;
                }
                else if (res.UserGuid != "" && res.UserGuid != undefined) {
                    window.location.href = '/rg/relevant?ug=' + res.UserGuid + "&id=" + res.UserId + "&c=" + res.CountryCode + "&lpage=homepage&rcheckr=" + rcheckr + "&cid=" + $scope.orgDetails.ClientId;
                }
            }
            else {
                $scope.isRegErrMsg = true;
            }

        }, function (err) {
            //  $scope.errMsg = true;
        });

    }
    // for linked in data
    if ($scope.id != "" && $scope.id != undefined) {
        $scope.showreg = function () {
            document.getElementById("divReg1").style.display = 'inline-block';
            document.getElementById("linkedin").style.display = 'none';
        }
        $('#ApplyModal').show();
        $scope.showreg();
    }
    $scope.showreg = function () {

        document.getElementById("divReg").style.display = 'block';
        document.getElementById("linkedin").style.display = 'none';
    }
    $scope.Close = function () {
        document.getElementById("ApplyModal").style.display = 'none';
    }

    $scope.Linkedin = function () {
        $scope.popup('https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=78y5i0qq41i9rp&redirect_uri=http://dev.affiliate.sdl.com/Login/linkedinCallback&state=fooobar&scope=r_liteprofile%20r_emailaddress%20w_member_social%20w_share%20r_basicprofile%20rw_company_admin%20r_fullprofile');
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
