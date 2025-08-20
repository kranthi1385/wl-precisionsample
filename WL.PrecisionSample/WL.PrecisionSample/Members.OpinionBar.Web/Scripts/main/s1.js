debugger;
//main module of the soltion
angular.module("loginApp", ['pascalprecht.translate', 'customSerivces', 'staticTranslationsModule', 'vcRecaptcha'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider', 'translatePluggableLoaderProvider',
   function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider, translatePluggableLoaderProvider) {
       //       angular.lowercase = angular.$$lowercase;
       $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
       //load all translations setting to main transtion loader.
       //$translateProvider.useLoader('$translatePartialLoader', {
       //    urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
       //});
       //add default language
       $translateProvider.useLoader('translatePluggableLoader');
       //regster providers

   }])

   //login controller section
   .controller("loginCtrl", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', 'vcRecaptchaService', 'getQueryParams', '$cookies',
function ($scope, $http, $window, $location, $rootScope, $timeout, httpService, translationsLoadingService, vcRecaptchaService, getQueryParams, $cookies) {
    //load current login json file
    //translationsLoadingService.writeNlogService();
    translationsLoadingService.loadTranslatePagePath("login");
    $scope.nameMsg = false;
    $scope.lnameMsg = false;
    $scope.subidmsg = true;
    var now = new $window.Date(),
    exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
    //$scope.isShowLoginFailErrmsg = false;
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
    //s1 params start
    $scope.rid = getUrlVars()["rid"];
    $scope.sid = getUrlVars()["sid"];
    $scope.fid = getUrlVars()["fid"];
    $scope.txnid = getUrlVars()["txid"];
    $scope.trans_id = getUrlVars()["trans_id"];
    $scope.fn = getUrlVars()["fn"];
    $scope.ln = getUrlVars()["ln"];
    $scope.em = getUrlVars()["em"];
    $scope.dob = getUrlVars()["dob"];
    $scope.lname = getUrlVars()["lname"];
    //s1 params end
    var leadid = getUrlVars()["leadid"]
    var pc = getUrlVars()["pc"]
    var txid = getUrlVars()["extid"]
    $scope.showtop20 = getUrlVars()["is_t"]
    $scope.captchaResponse = '';
    $cookies.put('leadid', leadid, {
        expires: exp,
        path: '/'
    });
    $cookies.put('pc', pc, {
        expires: exp,
        path: '/'
    });
    $cookies.put('txid', txid, {
        expires: exp,
        path: '/'
    });
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
        var expireTime = time + 1 * 24 * 60 * 60 * 1000;
        now.setTime(expireTime);
        document.cookie = escape(name) + "=" + escape(cook) + ';expires=' + now.toGMTString() + ';path=/';
    }
    $scope.checkCookie = function () {
       
        var user = $scope.getCookie("obcookie");
        if (user != null) {
            $scope.nomNomCookie = false;
        } else {
            var cook = 1;
            $scope.writeCookie("obcookie", cook, 365);
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
        var user = $scope.getCookie("obcookie");
        if (user != null) {
            var cook = 1;
            $scope.deleteCookie("obcookie", cook);
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
    //checking cookie for language
    $scope.recaptchalangCode = 'en';
    if (($cookies.get('UserLangCode') != undefined || ($cookies.get('MainLangCode') != undefined && $cookies.get('MainLangCode') != 'null')) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {

        translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
        $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
        $scope.recaptchalangCode = $cookies.get('UserLangCode') || $cookies.get('MainLangCode');
    }
    else {
        translationsLoadingService.setCurrentUserLanguage('en');
        $scope.flagimg = "/images/flag_usa.png";
        $scope.recaptchalangCode = 'en';
    }

    //for external members


    //$scope.step1 = "/PartialViews/ucl/step1.html";
    //$scope.step2 = "/PartialViews/ucl/step2.html";
    //$scope.step1Reg = "/PartialViews/ucl/step1-reg.html";
    //$scope.footer = "/PartialViews/ucl/footer.html";

    //User LogIn object
    $scope.loginDetails = {
        EmailAddress: '',
        Password: ''
    }


    //user login  click
    $scope.userLoginClick = function (valid) {
        //window.location.href = '/profile/index';
        var replacepswd = $scope.loginDetails.Password;
        $scope.showStep1ErrMsg = false;
        $scope.isShowLoginFailErrmsg = false;
        $scope.showLoginError = false;
        if (valid) {
            $scope.loginDetails.Password = replacepswd.replace("&", "||**||").replace("#", "||*||").replace("+", "||***||");
            httpService.postData('/Home/LogIn', $scope.loginDetails).then(function (response) {
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
    //get all avaliable ethnicities
    httpService.getData('/Common/GetEthnicityList').then(function (response) {
        $scope.ethnicityLst = response;
    }, function (err) {
        // $scope.errMsg = true;
    });

    //get ob lang 
    httpService.getData('/Common/GetObLang').then(function (response) {
        $scope.langLst = response;
    }, function (err) {
        // $scope.errMsg = true;
    });

    //binding country to registraion
    httpService.getData('/Common/GetCountrysAndStates').then(function (response) {
        $scope.countries = response.CountryList;
        $rootScope.stateslist = response.StateList;
        if ($scope.lid != undefined) {
            $scope.getLeaduser();

        }
    }, function (err) {
        // $scope.errMsg = true;
    });
    //binding states to registration
    $scope.countryByStates = function (cc) {
        $scope.states = [];
        for (var i = 0; i < $rootScope.stateslist.length; i++) {
            if (cc == $rootScope.stateslist[i].CId) {
                $scope.states.push($rootScope.stateslist[i]);

            }
        }
        //validateOpt();
    }
    if (leadid != undefined && leadid != '') {
        httpService.postData('/Home/join?leadid=' + leadid).then(function (response) {
            if (response.EmailAddress != null) {
                httpService.getData('/Common/GetCountrysAndStates').then(function (res) {
                    $scope.countries = res.CountryList;
                    $scope.stateslist = res.StateList;
                    if ($scope.user.CountryId != "") {
                        $scope.user.CountryId = 231;
                        $scope.countryByStates(231);
                    }
                    $scope.dob = $scope.user.Dob.split("/");
                    $scope.user.Month = $scope.dob[0];
                    $scope.user.Day = $scope.dob[1];
                    $scope.user.Year = $scope.dob[2];
                }, function (err) {
                    // $scope.errMsg = true;
                });

                $scope.user = response;
                $scope.user.SubId3 = $scope.txid;
                $scope.user.referrerid = $scope.rid;
                $scope.user.RouterReferrerId = $scope.rid;
                $scope.user.RouterSubId2 = $scope.sid;
                $scope.user.CreatedBy = 'Affiliate';
                $scope.leadshow = 'showlead';
                $scope.validateEmail();
                //if ($scope.user.Dob != null) {
                //    $scope.dateext = $scope.user.Dob.split('-');
                //    $scope.user.Day = $scope.dateext[0];
                //    $scope.user.Month = $scope.dateext[1];
                //    $scope.user.Year = $scope.dateext[2];
                //}

            }
            else {
                $scope.isShowLoginFailErrmsg = true;
            }
        }, function (err) {
            //  $scope.errMsg = true;
        });
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


    //user save
    $scope.submit = function (valid) {
        //if ($scope.orgDetails.ClientId != 38) {
        //    oi('#offers').optIn(function () {

        //    });
        //}

        saveUser(valid);
    }

    $scope.getCurrentDomailDetails = function () {
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
            $scope.orgDetails = res;
        }, function (err) {
        });
    }
    $scope.getCurrentDomailDetails();
    //validate email through xverify
    $scope.validateEmail = function () {
        $scope.showStep1ErrMsg = false;
        $scope.emailXVerify = -1;
        $scope.xVerifyMessageexist = false;
        $scope.xVerifyMessage = false;
        $scope.emailnotexist = false;
        if ($scope.user.EmailAddress != "") {
            //Emailaddress check
            httpService.getData('/Home/EmailAddressCheck?email=' + $scope.user.EmailAddress).then(function (response) {
                if (response.CpaCount >= 1) {
                    $scope.emailXVerify = 0;
                    $scope.xVerifyMessageexist = true;
                    $scope.xVerifyMessage = false;
                }
            }, function (err) {
            });
            httpService.getData('/Home/vldEmail?email=' + $scope.user.EmailAddress).then(function (response) {
                if (response == "accepted") {
                    $scope.emailXVerify = 1;
                    $scope.xVerifyMessage = true;
                    $scope.xVerifyMessageexist = false;
                    //var message = xVerify();
                    httpService.getData('/Home/EmailAddressCheck?email=' + $scope.user.EmailAddress).then(function (response) {
                        if (response.CpaCount >= 1) {
                            $scope.emailXVerify = 0;
                            $scope.xVerifyMessageexist = true;
                            $scope.xVerifyMessage = false;
                        }
                    }, function (err) {
                    });
                }
                else {
                    $scope.emailXVerify = 0;
                    $scope.emailnotexist = true;
                }
            }, function (err) {
            });
        }
        if (leadid != undefined) {
            $scope.validateSubid();
        }

    }


    //validate sub_id3 for local blox
    $scope.validateSubid = function () {
        if ($scope.user.EmailAddress != "" && leadid != undefined) {
            //Emailaddress check
            httpService.getData('/Home/SubidCheck?email=' + $scope.user.EmailAddress + '&subid=' + leadid).then(function (response) {
                if (response.CpaCount >= 1) {
                    $scope.subidmsg = false;
                }
            }, function (err) {
            });
        }

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

    $scope.checkname = function (fname) {
        var reg = /^[a-zA-Z]+$/;
        if (reg.test(fname)) {
            $scope.nameMsg = false;
        }
        else {
            $scope.nameMsg = true;
        }
    }
    $scope.checklname = function (lname) {
        var reg = /^[a-zA-Z]+$/;
        if (reg.test(lname)) {
            $scope.lnameMsg = false;
        }
        else {
            $scope.lnameMsg = true;
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
        if ($scope.nameMsg == false || $scope.lnameMsg == false) {
            if (valid && $scope.emailXVerify == 1 && $scope.subidmsg == true) {
                if ($scope.captchaResponse != '') {
                    //$scope.isRegErrMsg = false;
                    httpService.postData('/Common/ValidateCaptcha?googleResponse=' + $scope.captchaResponse).then(function (response) {
                        if (response == 1) {//google captcha valid
                            registration();
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
        else {
            $scope.nameMsg = true;
            $scope.lnameMsg == true;
        }
    }

    var registration = function () {
        $scope.user.SubId3 = $scope.txnid;
        $scope.user.SubId2 = $scope.sid;
        $scope.user.RefferId = $scope.rid;
        $scope.user.RouterReferrerId = $scope.rid;
        $scope.user.RouterSubId2 = $scope.sid;
        $scope.user.CreatedBy = 'Affiliate';

        httpService.postData('/Home/saveUser', $scope.user).then(function (res) {
            if (res != "" && res.UserGuid != "" && res.UserGuid != undefined) {
                $cookies.put('userGuid', res.UserGuid, {
                    expires: exp,
                    path: '/'
                });
                if (leadid != "" && leadid != undefined) {
                    if (pc == "t491") {
                        localStorage.setItem('res', JSON.stringify(res));
                        localStorage.setItem('leadid', leadid);
                        localStorage.setItem('pc', pc);
                        localStorage.setItem('pc', pc);
                        var href = '/Ms/Surveys?ug=' + res.UserGuid + "&pc=" + pc + "&txid=" + txid + "&leadid=" + leadid;
                        window.location.href = href;
                        // clickLoggerService.debug(document.referrer + '|' + href)
                    }
                    else {
                        window.location.href = '/Ms/Surveys?ug=' + res.UserGuid;
                    }
                }
                else {

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
        $window.open('/Footer/Privacy', 'SurveyDownline-Privacy', 'width=800,height=800')
    }
    //terms click
    $scope.termsClick = function () {
        $window.open('/Footer/TC', 'SurveyDownline-Terms', 'width=800,height=800')
    }
    //about click
    $scope.aboutClick = function () {
        window.location.href = '/Footer/MoreAbout';
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
           
            if (valid) {
                $scope.isShowContactErr = false;
                $scope.currentDomainDetails();
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

    //FAQ click
    $scope.faqClick = function () {
        $window.open($scope.orgDetails.MemberUrl + '/login/faq', 'SurveyDownline-FAQ', 'width=800,height=800')
    }
    //Do not sell my info
    $scope.doNotSellMyInfo = function () {
        $window.open($scope.orgDetails.MemberUrl + '/login/dns?lc=' + $scope.recaptchalangCode, 'SurveyDownline-DNS', 'width=800,height=800')
    }
    //forgot password
    $scope.forgetPswd = false;
    $scope.getUserData = {
        EmailAddress: ''
    }
    $scope.forgotPasswordClick = function (valid) {
        if (valid) {
            $scope.saveref = false;
            $scope.forgotPswdErrMsg = false;
            $scope.forgetPswd = true;
            $scope.emailnotexist = false;
            $scope.errMsg = false;
            if ($scope.getUserData.EmailAddress != "") {
                httpService.getData('/Login/GetUserDataEmail?EmailAddress=' + $scope.getUserData.EmailAddress).then(function (response) {
                    if (response.EmailAddress != null) {
                        $scope.getUserData = response;
                        //campaign id based on user country
                        if ($scope.getUserData.LanguageId == 140) {
                            $scope.CampaignID = 1874;
                            $scope.TemplateSubject = "Recordatorio de información de inicio de sesión";
                        }
                        else if ($scope.getUserData.LanguageId == 120) {
                            $scope.CampaignID = 1873;
                            $scope.TemplateSubject = "Информация для входа в систему";
                        }
                        else if ($scope.getUserData.LanguageId == 111) {
                            $scope.CampaignID = 1872;
                            $scope.TemplateSubject = "Lembrete de informações de login";
                        }
                        else if ($scope.getUserData.LanguageId == 110) {
                            $scope.CampaignID = 1871;
                            $scope.TemplateSubject = "Przypomnienie o danych logowania";
                        }
                        else if ($scope.getUserData.LanguageId == 80) {
                            $scope.CampaignID = 1870;
                            $scope.TemplateSubject = "로그인 정보 알림";
                        }
                        else if ($scope.getUserData.LanguageId == 70) {
                            $scope.CampaignID = 1869;
                            $scope.TemplateSubject = "ログイン情報リマインダー";
                        }
                        else if ($scope.getUserData.LanguageId == 69) {
                            $scope.CampaignID = 1868;
                            $scope.TemplateSubject = "Promemoria informazioni di accesso";
                        }
                        else if ($scope.getUserData.LanguageId == 51) {
                            $scope.CampaignID = 1867;
                            $scope.TemplateSubject = "Erinnerung an Anmeldeinformationen";
                        }
                        else if ($scope.getUserData.LanguageId == 44) {
                            $scope.CampaignID = 1866;
                            $scope.TemplateSubject = "Rappel des informations de connexion";
                        }
                        else if ($scope.getUserData.LanguageId == 36) {
                            $scope.CampaignID = 1865;
                            $scope.TemplateSubject = "Aanmeldingsgegevens Herinnering";
                        }
                        else if ($scope.getUserData.LanguageId == 180) {
                            $scope.CampaignID = 1864;
                            $scope.TemplateSubject = "登錄信息提醒";
                        }
                        else {
                            $scope.CampaignID = 803;
                            $scope.TemplateSubject = "Login Information Reminder";
                        }
                        $scope.getUserData.CustomAttribute = "first_name:" + $scope.getUserData.FirstName + ";last_name:"
                           + $scope.getUserData.LastName + ";email_address:" + $scope.getUserData.EmailAddress + ";password:"
                           + $scope.getUserData.Password + ";create_dt:" + $scope.getUserData.CreateDate + ";ip_address:"
                           + $scope.getUserData.IpAddress + ";org_logo:" + $scope.orgDetails.OrgLogo.replace("http://", "") + ";org_name:" + $scope.orgDetails.OrgName
                            + ";member_url:" + $scope.orgDetails.MemberUrl + ";user_guid:" + $scope.getUserData.UserGuid + ";subject:" + $scope.TemplateSubject;
                        httpService.postData('/Login/ForgetPassword?campid=' + $scope.CampaignID + '&CustomAttribute=' + $scope.getUserData.CustomAttribute, $scope.getUserData).then(function (response) {
                            if ($scope.getUserData.EmailAddress != null) {
                                $scope.errMsg = false;
                                $scope.saveref = true;
                            }
                        }, function (err) {
                            $scope.errMsg = true;
                        });
                    }
                    else {
                        $scope.emailnotexist = true;
                    }

                }, function (err) {
                    $scope.errMsg = true;
                });
            }
        }
        else {
            $scope.forgotPswdErrMsg = true;
        }
    }

    $scope.validationFlag = function () {
        $scope.saveref = false;
        $scope.emailnotexist = false;
        $scope.xVerifyMessageexist = false;
        $scope.xVerifyMessage = false;
    }

    $scope.divReg1 = false;
    $scope.onClick = function () {
        $scope.divReg1 = true;
    }
    $scope.save = function (valid, member) {

        httpService.postData('/Home/saveUser', $scope.user).then(function (res) {
            if (res != "") {
                if (leadid != undefined && pc == f) {
                    window.location.href = '/Ms/Surveys?ug=' + res.UserGuid;
                }
                if (leadid != undefined && pc == 't491') {
                    window.location.href = '/Ms/Surveys?ug=' + res.UserGuid + "&pc=" + pc;
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

    //New Site! Great Features
    $scope.newSiteclick = function () {
        var ln;
        var pagname;
        if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined || $cookies.get('MainLangCode') != null) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
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
