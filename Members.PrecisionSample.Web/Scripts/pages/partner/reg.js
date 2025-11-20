//main module of the soltion

var app = angular.module("regApp", ['pascalprecht.translate', 'customSerivces', 'ngCookies', 'staticTranslationsModule'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider', 'translatePluggableLoaderProvider',
   function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider, translatePluggableLoaderProvider) {

       //add header for each  request to identify AntiForgeryToken
       $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
       $httpProvider.interceptors.push('loadingInterceptorService');

       //load all translations setting to main transtion loader.
       $translateProvider.useLoader('$translatePartialLoader', {
           urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
       });
       //add default language
       $translateProvider.useLoader('translatePluggableLoader');

       //regster providers

   }])

//login controller section
app.controller("regFController", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', '$cookies',
function ($scope, $http, $window, $location, $rootScop, $timeout, httpService, translationsLoadingService, $cookies) {

    // to get URL params
    function getUrlVars() {
        var Url = window.location.href;
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }
    $scope.details = {
        TypeId: '1',
        Guids: ''
    }
    //Get query parmas value referrer id
    $scope.ug = getUrlVars()["ug"];                  //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&emailaddress=&address1=&city=&Zip=&gender=&Dob=&address2=&ethnicity=
    if ($scope.ug == undefined) {
        $scope.ug = ''
    }
    $scope.is_show = getUrlVars()["is_show"];
    if ($scope.is_show == undefined) {
        $scope.is_show = false;
    }
    //Get query parmas value referrer id
    $scope.rid = getUrlVars()["rid"];
    $scope.pubid = getUrlVars()["pubid"];
    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&emailaddress=&address1=&city=&Zip=&gender=&Dob=&address2=&ethnicity=
    $scope.rid = decodeURIComponent((getUrlVars()["rid"] + '').replace(/\+/g, '%20'));
    if ($scope.rid == undefined) {
        $scope.rid = 14176
    }
    //Get query parmas value country
    $scope.c = getUrlVars()["ct"];
    if ($scope.c == undefined) {
        $scope.c = '';
    }
    //Get query parmas value external member id
    $scope.extmid = getUrlVars()["extmid"];
    if ($scope.extmid == undefined) {
        $scope.extmid = '';
    }
    //Get query parmas value sub referrer_id
    $scope.sub_ref_id = getUrlVars()["subid"];
    if ($scope.sub_ref_id == undefined) {
        $scope.sub_ref_id = '';
    }
    //Get query parmas value firstname
    $scope.fn = getUrlVars()["fn"];
    if ($scope.fn == undefined) {
        $scope.fn = '';
    }
    //Get query parmas value lastname  
    $scope.ln = getUrlVars()["ln"];
    if ($scope.ln == undefined) {
        $scope.ln = '';
    }
    //Get query parmas value user id
    $scope.email = getUrlVars()["em"];
    if ($scope.email == undefined) {
        $scope.email = '';
    }

    //Get query parmas value address1
    $scope.addrs1 = getUrlVars()["a1"];
    if ($scope.addrs1 == undefined) {
        $scope.addrs1 = '';
    }
    //Get query parmas value city
    $scope.city = getUrlVars()["ci"];
    if ($scope.city == undefined) {
        $scope.city = '';
    }
    //Get query parmas value country
    $scope.state = getUrlVars()["st"];
    if ($scope.state == undefined) {
        $scope.state = '';
    }
    //Get query parmas value zipcode
    $scope.zip = getUrlVars()["zi"];
    if ($scope.zip == undefined) {
        $scope.zip = '';
    }
    //Get query parmas value zipcode
    $scope.gender = getUrlVars()["g"];
    if ($scope.gender == undefined) {
        $scope.gender = '';
    }
    else {
        $scope.gender = $scope.gender.toLowerCase()
    }
    //Get query parmas value dob
    $scope.dob = getUrlVars()["dob"];
    if ($scope.dob == undefined) {
        $scope.dob = getUrlVars()["Dob"];
        if ($scope.dob == undefined) {
            $scope.dob = '';
        }
    }

    //Get query parmas value address1
    $scope.addrs2 = getUrlVars()["a2"];
    if ($scope.addrs2 == undefined) {
        $scope.addrs2 = '';
    }
    //Get query parmas value address1
    $scope.ethincity = getUrlVars()["et"];
    if ($scope.ethincity == undefined) {
        $scope.ethincity = '';
    }
    $scope.existingemail = false;
    $scope.signup = false;   
    httpService.getData('/Reg/GetWidgetDomainDetails?rid=' + $scope.rid).then(function (res) {
        $scope.orgDetails = res;
        if ($scope.orgDetails.IsEnablelogin == true) {
            $scope.signup = true;
        }
        if ($scope.is_show == true) {
            $scope.signup = false;
            $scope.resetpswd = true;
        }
        else {
            $scope.resetpswd = false;
        }
        getEmailfromcookie();
    },
 function (err) {

 });

    $scope.DoNotSellInfo = {};
    $scope.pwdupdate = false;
    $scope.pwdmatchError = false;
    $scope.weekpwdMsg = false;
    $scope.msgOld = false;
    $scope.samePwdError = false;
    $scope.errMsg = false;
    $scope.isShowError = false;
    $scope.passwordResetSuccess = false;
    $scope.passwordResetError = false;

    $scope.submitNewPassword = function (valid) {

        $scope.pwdmatchError = false;
        $scope.weekpwdMsg = false;
        $scope.msgOld = false;
        $scope.samePwdError = false;
        $scope.errMsg = false;
        $scope.isShowError = false;
        $scope.passwordResetSuccess = false;
        $scope.passwordResetError = false;
        if (valid) {

            if ($scope.DoNotSellInfo.NewPassword.length >= 8) {
                httpService.postData('/LogIn/ChangePassword', {
                    NewPassword: $scope.DoNotSellInfo.NewPassword,
                    email: $scope.extmid,
                    // rid: $scope.rid
                }).then(function (response) {
                    // Handle different server responses
                    if (response !== "0") {
                        debugger;
                        $scope.passwordResetSuccess = true;
                        var rid = $scope.rid;
                        var extmid = $scope.extmid;
                        window.location.href = 'https://widget.reachcollective.com/reg/signup?rid=' + encodeURIComponent(rid) + '&extmid=' + (response);

                    }

                    else if (response == 0) {
                        $scope.passwordResetError = true;
                    }

                }, function (err) {
                    $scope.errMsg = true;
                });
            } else {
                $scope.weekpwdMsg = true; // Weak password message
            }
        } else {
            $scope.isShowError = true; // Show error for invalid form
        }
    }



    $scope.FogotPswd = false;
    $scope.showMemberExist = false;
    $scope.step1 = "/PartialViews/ucl/step1.html";

    if (parseInt($scope.rid) == 14687) { // show image for MadMoneyGPT 
        $("#imgGPTLogo").show();
        $('#dvLogo').show();
    }
    else if (parseInt($scope.rid) == 14691) { // show image for keep regarding 
        $("#imglivesurveys").show();
        $('#dvLogo').show();
    }
    else {
        $('#dvLogo').hide();
    }
    if ($cookies.get('WidgetLangCode') != "" && $cookies.get('WidgetLangCode') != undefined && $cookies.get('WidgetLangCode') != "null") {
        translationsLoadingService.setCurrentUserLanguage($cookies.get('WidgetLangCode'));
    }
    else {
        translationsLoadingService.setCurrentUserLanguage('en');
    }
    var statesList = [];
    $scope.showErroMsg = false;
    $scope.states = [];
    var clientId = 0;
    var CountryId = 0;
    //declare variable      
    var getEmailfromcookie = function () {
        if (parseInt($scope.rid) == 14253 || parseInt($scope.rid) == 14749 || parseInt($scope.rid) == 20795 || parseInt($scope.rid) == 20876 || parseInt($scope.rid) == 20897
        || parseInt($scope.rid) == 20898 || parseInt($scope.rid) == 21010 || parseInt($scope.rid) == 21011 || parseInt($scope.rid) == 21035 || parseInt($scope.rid) == 21034
        || parseInt($scope.rid) == 20982 || parseInt($scope.rid) == 20837 || parseInt($scope.rid) == 22078 || parseInt($scope.rid) == 22517 || parseInt($scope.rid) == 22529
        || parseInt($scope.rid) == 22540 || ($scope.orgDetails.IsEnablelogin == true)) { //added on 06/27/2016 by Giri For Talons Media
            // GetEmailFromCookie
            httpService.getData('/Reg/GetEmailfromcookie?cookieId=' + $scope.email + ',' + $scope.rid).then(function (response) {
                if (response == null || response == "") {
                    $('#dvImageLoading').hide();
                    $('#dvForm').hide();
                    $('#border').show();
                    $('#dvFreebies').show();
                }
                else {
                    var memberCookie = [];
                    memberCookie = response.split(',')
                    $scope.extmid = memberCookie[0];
                    GetMemberDetails();
                }
            }, function (err) {
                alert('error has occured');
            });
        }
        else {
            var cookieExtId = '';
            if ($scope.extmid == '') {
                cookieExtId = $scope.email;
            }
            else {
                cookieExtId = $scope.extmid
            }
            //GetExtenalMemberIdFromCookie 
            httpService.getData('/Reg/GetEmailfromcookie?cookieId=' + cookieExtId + ',' + $scope.rid).then(function (response) {
                if (response == null || response == "") {
                    GetMemberDetails();
                }
                else {
                    var memberCookie = [];
                    memberCookie = response.split(',')
                    $scope.extmid = memberCookie[0];
                    GetMemberDetails();
                }
            }, function (err) {
                alert('error has occured');
            });
        }
    }
    $scope.message = '';

    // Function to reset password and show a message

    //User signup
    $scope.signupClick = function () {
        $scope.signup = false;
        $scope.FogotPswd = false;
    }
    $scope.loginClick = function () {
        $scope.signup = true;
        $scope.FogotPswd = false;
    }
    $scope.ForgotPswd = function () {
        $scope.FogotPswd = true;
        $scope.signup = false;
    }

    $scope.message = '';
    $scope.resetpswd = false;
    $scope.signup = true;
    $scope.passwordResetSuccess = false;
    $scope.resetPasswordError = false;
    $scope.showMessage = false;

    // Function to handle password reset
    $scope.resetPassword = function () {
        // Reset message and flags
        $scope.showMessage = true;
        $scope.passwordResetSuccess = false;
        $scope.resetPasswordError = false;

        // Define the data to be sent to the API
        var data = {
            rid: $scope.rid,
            extmid: $scope.extmid,
            OrgId: $scope.orgDetails.ClientId
        };

        // Make the API call to request a password reset
        httpService.postData('/LogIn/SendResetLink', data)
            .then(function (response) {

                    $scope.passwordResetSuccess = true;
                    $scope.resetPasswordError = false;
                
            }, function (err) {
                $scope.passwordResetSuccess = false; // Show error message on error
                $scope.resetPasswordError = true; // Ensure error message is shown
            });
    };



    //forgot password
    $scope.getUserData = {
        EmailAddress: ''
    }
    $scope.forgetPswd = false;
    $scope.errMsg = false;
    $scope.saveref = false;
    $scope.forgotPasswordClick = function (valid) {
        if (valid) {
            $scope.saveref = false;
            $scope.forgotPswdErrMsg = false;
            $scope.forgetPswd = true;
            httpService.getData('/Reg/GetUserDataEmailWidget?EmailAddress=' + $scope.getUserData.EmailAddress + '&rid=' + $scope.rid).then(function (response) {                
                $scope.getUserData = response;
                $scope.getUserData.CustomAttribute = "first_name:" + $scope.getUserData.FirstName + ";last_name:"
                    + $scope.getUserData.LastName + ";email_address:" + $scope.getUserData.EmailAddress + ";password:"
                    + $scope.getUserData.Password + ";create_dt:" + $scope.getUserData.CreateDate + ";ip_address:"
                    + $scope.getUserData.IpAddress + ";org_logo:" + $scope.getUserData.OrgLogo + ";org_name:" + $scope.getUserData.OrgName +
                    ";member_url:" + $scope.getUserData.MemberUrl + ";user_guid:" + $scope.getUserData.UserGuid;
                httpService.postData('/Reg/ForgetPassword?campid=' + 803 + '&CustomAttribute=' + $scope.getUserData.CustomAttribute + '&rid=' + $scope.rid, $scope.getUserData).then(function (response) {
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

    $scope.checkEmailAddress = function (valid) {
        if (valid) {            
            httpService.getData('/Reg/emailAddressvaild?EmailAddress=' + $scope.getUserData.EmailAddress + '&rid=' + $scope.rid).then(function (response) {
                debugger;
                $scope.getUserData = response;
                if ($scope.getUserData.EmailAddress == null) {
                    $scope.existingemail = false;
                    $scope.signup = false;
                    $scope.FogotPswd = false;                   
                }
                if ($scope.getUserData.EmailAddress != null) {
                    $scope.existingemail = true;
                }
            }, function (err) {
                $scope.errMsg = true;
            });
        }
        else {
            $scope.forgotPswdErrMsg = true;
        }
    }
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
            httpService.postData('/Reg/login?email=' + $scope.loginDetails.EmailAddress + '&psw=' + $scope.loginDetails.Password + '&rid=' + $scope.rid).then(function (response) {
                if (response.UserGuid != "" && response.UserGuid != null && response.UserGuid != '00000000-0000-0000-0000-000000000000') {
                    window.location.href = '/Reg/Home?ug=' + response.UserGuid + '&cid=' + response.OrgId;
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

    $scope.validateEmail = function () {
        httpService.getData('/Home/vldEmail?email=' + $scope.user.EmailAddress).then(function (response) {
            if (response == "accepted") {
                $('#imgInvalid').hide();
                $('#imgValid').show();
            }
            else {
                $('#imgInvalid').show();
                $('#imgValid').hide();
            }
        }, function (err) {
        });
    }
    //$scope.validateEmail1 = function () {
    //    httpService.getData('/Home/vldEmail?email=' + $scope.user.EmailAddress).then(function (response) {
    //        if (response == "accepted") {
    //            $('#imgInvalid1').hide();
    //            $('#imgValid1').show();
    //        }
    //        else {
    //            $('#imgInvalid1').show();
    //            $('#imgValid1').hide();
    //        }
    //    }, function (err) {
    //    });
    //}
    //get all avaliables states
    $scope.countryByStates = function (cc) {
        //$scope.states = statesList.filter(s=>s.CountryId == cc)
        $scope.states = [];
        angular.forEach(statesList, function (state, key) {
            if (state.CountryId == cc) {
                $scope.states.push(state);
            }
        });
        //httpService.getData('/Common/GetStates?Cid=' + cc).then(function (response) {
        //    $scope.states = response;
        //}, function (err) {
        //    // $scope.errMsg = true;
        //});
    }

    $scope.saveFreebie = function (isValid) {
        $scope.ConfPswd = false;
        $scope.password = $('#txtPassword').val();
        $scope.confirmPassword = $('#txtConPassword').val();
        if (isValid == true && $scope.password == $scope.confirmPassword) {
            $scope.feebieForm.$setPristine();
            $scope.FreebyEmail = $('#txtEmailForFreebies').val();
            httpService.getData('/Reg/WriteCookie?ea=' + $scope.FreebyEmail + ',' + $scope.rid).then(function (response) {
                $('#border').show();
                $('#dvImageLoading').hide();
                $('#dvFreebies').hide();
                $('#dvForm').show();
                $scope.extmid = $scope.FreebyEmail;
                GetMemberDetails();
            }, function (err) {
                alert('error has occured');
            });
        }
        else {
            $scope.ConfPswd = true;
        }
    }
    $scope.saveReg = function (isValid) {
        SaveDetails(isValid)
    }
    var now = new $window.Date(),
    exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
    $scope.nextStep = function (id) {
        $scope.langId = id;
        $cookies.put('WidgetLangCode', $scope.langId, {
            expires: exp,
            path: '/'
        });
        if ($cookies.get('WidgetLangCode') != "" && $cookies.get('WidgetLangCode') != undefined && $cookies.get('WidgetLangCode') != "null") {
            translationsLoadingService.setCurrentUserLanguage($cookies.get('WidgetLangCode'));
        }
        else {
            translationsLoadingService.setCurrentUserLanguage('en');
        }
        $scope.langShw = false;
        $scope.formhide = true;
        if ($scope.langId == 'Ar') {
            $scope.user.LanguageId = 7;
        }
        else if ($scope.langId == 'Ch') {
            $scope.user.LanguageId = 27;
        }
        else if ($scope.langId == 'nd') {
            $scope.user.LanguageId = 36;
        }
        else if ($scope.langId == 'en') {
            $scope.user.LanguageId = 35;
        }
        else if ($scope.langId == 'Fr') {
            $scope.user.LanguageId = 44;
        }
        else if ($scope.langId == 'De') {
            $scope.user.LanguageId = 51;
        }
        else if ($scope.langId == 'It') {
            $scope.user.LanguageId = 69;
        }
        else if ($scope.langId == 'Ja') {
            $scope.user.LanguageId = 70;
        }
        else if ($scope.langId == 'kr') {
            $scope.user.LanguageId = 80;
        }
        else if ($scope.langId == 'pl') {
            $scope.user.LanguageId = 110;
        }
        else if ($scope.langId == 'pt') {
            $scope.user.LanguageId = 111;
        }
        else if ($scope.langId == 'Ru') {
            $scope.user.LanguageId = 120;
        }
        else if ($scope.langId == 'es') {
            $scope.user.LanguageId = 140;
        }
        else if ($scope.langId == 'Tu') {
            $scope.user.LanguageId = 158;
        }
        else {
            $scope.user.LanguageId = 35;
        }
        //BindMemberEthnicity($scope.langId);
        httpService.getData('/Common/GetEthnicityList?langid=' + $scope.langId).then(function (ethLst) {
            $scope.ethnicityLst = ethLst;
        }, function (err) {
            // $scope.errMsg = true;
        });
    }
    //get member reg details
    function GetMemberDetails() {
        $scope.showMemberExist = false;
        if ($scope.ug != '') {
            //My guess this Use case Never exists.
            window.location.href = '/Reg/Home?ug=' + $scope.ug;
        }
        else {
            if ($scope.rid != '' && $scope.extmid != '') {
                httpService.getData('/Reg/CheckMemberDetails?ExtId=' + $scope.extmid + '&r=' + String($scope.rid)).then(function (response) {
                    clientId = response.OrgId;
                    if (response.Count == 0 || response.Count == undefined) {
                        if ($scope.rid != 20890 && $scope.rid != 20899 && $scope.rid != 21043 && $scope.rid != 21044 && $scope.rid != 21090 && $scope.rid != 21514) {
                            $scope.langShw = true;
                            $scope.formhide = false;
                            //BindLanguage();
                            BindMember(response);  //if member not exist bind viewModel                          
                        }
                        else if ($scope.rid != 21043 || $scope.rid != 21044) {
                            $scope.langShw = false
                            $scope.formhide = true;
                            //BindLanguage();
                            translationsLoadingService.setCurrentUserLanguage('en');
                            BindMember(response);  //if member not exist bind viewModel
                            $scope.user.LanguageId = 35;
                            httpService.getData('/Common/GetEthnicityList?langid=' + 'en').then(function (ethLst) {
                                $scope.ethnicityLst = ethLst;
                            }, function (err) {
                                // $scope.errMsg = true;
                            });
                        }
                        else if ($scope.rid == 20899) {
                            $scope.langShw = false
                            $scope.formhide = true;
                            //BindLanguage();
                            translationsLoadingService.setCurrentUserLanguage('Fr');
                            BindMember(response);  //if member not exist bind viewModel
                            $scope.user.LanguageId = 44;
                            httpService.getData('/Common/GetEthnicityList?langid=' + 'Fr').then(function (ethLst) {
                                $scope.ethnicityLst = ethLst;
                            }, function (err) {
                                // $scope.errMsg = true;
                            });
                        }
                        else {
                            $scope.langShw = false
                            $scope.formhide = true;
                            //BindLanguage();
                            translationsLoadingService.setCurrentUserLanguage('De');
                            BindMember(response);  //if member not exist bind viewModel
                            $scope.user.LanguageId = 51;
                            httpService.getData('/Common/GetEthnicityList?langid=' + 'De').then(function (ethLst) {
                                $scope.ethnicityLst = ethLst;
                            }, function (err) {
                                // $scope.errMsg = true;
                            });
                        }

                    }
                    else {
                        //We need to have Client Id on each View, so that we will decide the Connection String.                        
                        if (($scope.orgDetails.IsEnablelogin == true) && response.Count == 1) {
                            $scope.showMemberExist = true;
                            $('#border').show();
                            $('#dvImageLoading').hide();
                            $('#dvFreebies').show();
                            $('#dvForm').hide();
                            BindMember(response);
                        }
                        else {
                            window.location.href = '/Reg/Home?ug=' + response.UserGuid + '&cid=' + response.OrgId;
                        }
                    }
                }, function (err) {
                    alert('error has occured');
                });
            }
            else {
                //Member Rid And Emaild Address are  null
                httpService.getData('/Reg/BindData').then(function (response) {
                    BindMember(response);
                }, function (err) {
                    alert('error has occured');
                });
            }
        }
    }
    //Load Year DropDown Dynamically
    function LoadYear() {
        var d = new Date();
        var year = d.getFullYear();
        var yearLst = [];
        for (var i = 13; i < 100; i++) {
            yearLst.push({ key: year - i, value: year - i });

        }
        $scope.year = yearLst;
    }

    function BindMember(response) {
        $scope.user = response;
        if ($scope.orgDetails.IsEnablelogin == true) {
            $scope.user.EmailAddress = $scope.FreebyEmail;
            $scope.user.Password = $scope.password;

        }
        //get all avaliable ethnicities
        httpService.getData('/Common/GetEthnicityList').then(function (ethLst) {
            $scope.ethnicityLst = ethLst;
            BindCountryStates();
            BindLanguage();
            LoadYear();
        }, function (err) {
            // $scope.errMsg = true;
        });

    }
    function BindLanguage() {
        //get all avaliable Languages
        httpService.getData('/Common/GetObLang').then(function (response) {
            $scope.langLst = response;
            //BindCountryStates();
            LoadYear();

        }, function (err) {
            // $scope.errMsg = true;
        });

    }
    function BindCountryStates() {

        httpService.getData('/Common/GetCountrysAndStates?orgId=' + $scope.orgDetails.ClientId).then(function (data) {
            $scope.country = data.CountryList;
            statesList = data.StateList;
            BinduserData();
        }, function (err) {
            // $scope.errMsg = true;
        })

    }
    function BinduserData() {

        var isValid = false;
        var vc = 0;
        var scount = 0; //sate count
        var ccount = 0; // country count
        var etcount = 0; //ethnicitycount
        var yearcount = 0;
        if ($scope.rid != '') {
            $scope.rid = decodeURIComponent(($scope.rid + '').replace(/\+/g, '%20'))
            isValid = true;
            $scope.user.Rid = parseInt($scope.rid);
        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.rid != 14253 || $scope.rid != 14749 || $scope.rid != 20795 || $scope.rid != 20876 || $scope.rid != 20897 || $scope.rid != 20898
            || $scope.rid != 21010 || $scope.rid != 21011 || $scope.rid != 21035 || $scope.rid != 21034 || $scope.rid != 20982 || $scope.rid != 20837 || $scope.rid != 22078 || $scope.rid != 22517 || $scope.rid != 22529 || $scope.rid != 22540) { //added 14749 on 06/27/2016 by Giri For Talons Media
            if ($scope.extmid != '') {
                $scope.extmid = decodeURIComponent(($scope.extmid + '').replace(/\+/g, '%20'))
                if (vc == 0) {
                    isValid = true;
                }
                $scope.user.ExtId = $scope.extmid;

            }
        }
        else {
            $scope.user.ExtId = $scope.FreebyEmail;

        }
        if ($scope.sub_ref_id != '') {
            $scope.user.SubId3 = $scope.sub_ref_id;
        }
        //else {
        //    vc = 1;
        //    isValid = false;
        //}

        if ($scope.fn != '') {
            if (vc == 0) {
                isValid = true;
            }
            $scope.fn = decodeURIComponent(($scope.fn + '').replace(/\+/g, '%20')); // any extra spaces decoding param value

            $scope.user.FirstName = $scope.fn;

        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.ln != '') {
            if (vc == 0) {
                isValid = true;
            }
            $scope.ln = decodeURIComponent(($scope.ln + '').replace(/\+/g, '%20'));

            $scope.user.LastName = $scope.ln;

        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.rid != 14253 && $scope.rid != 14749 && $scope.rid != 20795 || $scope.rid != 20876 || $scope.rid != 20897 || $scope.rid != 20898 || $scope.rid != 21010
            || $scope.rid != 21011 || $scope.rid != 21035 || $scope.rid != 21034 || $scope.rid != 20982 || $scope.rid != 20837 || $scope.rid != 22078 || $scope.rid != 22517 || $scope.rid != 22529 || $scope.rid != 22540) { //added on 06/27/2016 by Giri For Talons Media
            if ($scope.email != '') {
                if (vc == 0) {
                    isValid = true;
                }
                $scope.email = decodeURIComponent(($scope.email + '').replace('%20'));
                $scope.user.EmailAddress = $scope.email;

            }
            else {
                vc = 1;
                isValid = false;
            }
        }
        else {
            $scope.user.EmailAddress = $scope.FreebyEmail;
        }
        if ($scope.addrs1 != '') {
            if (vc == 0) {
                isValid = true;
            }
            $scope.addrs1 = decodeURIComponent(($scope.addrs1 + '').replace(/\+/g, '%20'));
            $scope.user.Address1 = $scope.addrs1;

        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.addrs2 != '') {
            $scope.addrs2 = decodeURIComponent(($scope.addrs2 + '').replace(/\+/g, '%20'));
            $scope.user.Address2 = $scope.addrs2;
        }

        if ($scope.c != '') {  // bind country dropdown
            $scope.c = decodeURIComponent(($scope.c + '').replace(/\+/g, '%20'));
            for (var j = 0; j < $scope.country.length; j++) {
                if ($scope.c.toLocaleLowerCase() == $scope.country[j].CountryCode.toLocaleLowerCase() || $scope.c.toLocaleLowerCase() == $scope.country[j].CountryName.toLocaleLowerCase() ||
                    $scope.c.toLocaleLowerCase() == $scope.country[j].Code || $scope.c.toLocaleLowerCase() == $scope.country[j].CountryNameforPartner) {
                    ccount = 1;
                    CountryId = $scope.country[j].CountryId;
                    $scope.user.CountryId = $scope.country[j].CountryId;
                }
            }
            if ($scope.state == '') {
                if (ccount == 1) {
                    $scope.state = decodeURIComponent(($scope.state + '').replace(/\+/g, '%20'));
                    // $scope.states = statesList.filter(s=>s.CountryId == CountryId)
                    $scope.states = [];
                    angular.forEach(statesList, function (state, key) {
                        if (state.CountryId == CountryId) {
                            $scope.states.push(state);
                        }
                    });
                    for (var k = 0; k < $scope.states.length; k++) {
                        if ($scope.state.toLocaleLowerCase() == $scope.states[k].StateCode.toLocaleLowerCase() || $scope.state.toLocaleLowerCase() == $scope.states[k].StateName.toLocaleLowerCase()) {
                            $scope.user.StateCode = $scope.states[k].StateId;
                        }
                    }
                }
                if (vc == 0) {
                    isValid = true;
                }
            }
        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.state != '') { //bind state dropdown
            if (vc == 0) {
                isValid = true;
            }
            $scope.states = [];
            angular.forEach(statesList, function (state, key) {
                if (state.CountryId == CountryId) {
                    $scope.states.push(state);
                }
            });
            // $scope.states = statesList.filter(s=>s.CountryId == CountryId)
            for (var k = 0; k < $scope.states.length; k++) {
                if ($scope.state.toLocaleLowerCase() == $scope.states[k].StateCode.toLocaleLowerCase() || $scope.state.toLocaleLowerCase() == $scope.states[k].StateName.toLocaleLowerCase()) {
                    scount = 1;
                    $scope.user.StateId = $scope.states[k].StateId;
                }
            }
        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.city != '') {
            $scope.city = decodeURIComponent(($scope.city + '').replace(/\+/g, '%20'));
            if (vc == 0) {
                isValid = true;
            }
            $scope.user.City = $scope.city;

        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.zip != '') {
            if (vc == 0) {
                isValid = true;
            }
            $scope.user.ZipCode = $scope.zip

        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.dob != '') {
            $scope.dob = decodeURIComponent(($scope.dob + '').replace(/\+/g, '%20'));
            if (vc == 0) {
                isValid = true;

            }
            $scope.dob = $scope.dob.split("/");
            $scope.user.Month = $scope.dob[0];
            $scope.user.Day = $scope.dob[1];
            $scope.user.Year = $scope.dob[2];
            for (var k = 0; k < $scope.year.length; k++) {
                if ($scope.user.Year == $scope.year[k].key) {
                    yearcount = 1;
                }
            }

        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.gender != '') {
            $scope.gender = decodeURIComponent(($scope.gender + '').replace(/\+/g, '%20'));
            if (vc == 0) {
                isValid = true;
            }
            $scope.user.Gender = $scope.gender;

        }
        else {
            vc = 1;
            isValid = false;
        }
        if ($scope.ethincity != '') {
            if (vc == 0) {
                isValid = true;
            }
            $scope.ethincity = decodeURIComponent(($scope.ethincity + '').replace(/\+/g, '%20'));
            for (var m = 0; m < $scope.ethnicityLst.length; m++) {
                if ($scope.ethincity.toLocaleLowerCase() == $scope.ethnicityLst[m].EthnicityId || $scope.ethincity.toLocaleLowerCase() == $scope.ethnicityLst[m].EthnicityType.toLocaleLowerCase()) {
                    etcount = 1;
                    $scope.user.EthnicityId = $scope.ethnicityLst[m].EthnicityId;
                }
            }


        }
        else {
            vc = 1;
            isValid = false;
        }

        if (scount != 1 || ccount != 1 || etcount != 1 || yearcount != 1) {
            isValid = false;
        }

        if (isValid == true) {
            SaveDetails(isValid);

        } else {
            $('#border').show();
            $('#dvImageLoading').hide();
            //            $('#tbRegDetails').show();
        }
    }
    function SaveDetails(isValid) {
        if (isValid) {
            $scope.showErrorMsg = false;
            var selectedDob = $scope.user.Month + '/' + $scope.user.Day + '/' + $scope.user.Year;
            var CountryCode = $("#ddlCountry option:selected").val();
            $scope.Country = '';
            if ($scope.user.CountryId == '231') {
                $scope.Country = 'us';
            }
            if ($scope.user.CountryId == 38) {
                $scope.Country = 'ca';
            }
            if ($scope.user.CountryId == '229') {
                $scope.Country = 'uk';
            }
            if ($scope.user.CountryId == '15') {
                $scope.Country = 'au';
            }
            $scope.user.Dob = selectedDob;
            if ($scope.rid != '' && $scope.rid != null) {
                //if (Vaildations()) {
                if ($scope.rid != 14253 || $scope.rid != 14749 || $scope.rid != 20795 || $scope.rid != 20876 || $scope.rid != 20897 || $scope.rid != 20898
                    || $scope.rid != 21010 || $scope.rid != 21011 || $scope.rid != 21035 || $scope.rid != 21034 || $scope.rid != 20982 || $scope.rid != 20837 || $scope.rid != 22078 || $scope.rid != 22517 || $scope.rid != 22529 || $scope.rid != 22540) {
                    if ($scope.extmid == '') {
                        $scope.user.ExtId = $scope.user.EmailAddress;
                        InsertCookie($scope.user.EmailAddress);
                        CheckMemberByEmail(isValid);
                    }
                    else {
                        InsertCookie($scope.user.ExtId);
                        CheckMemberByEmail(isValid)
                    }
                }
                //   }

            }
        }
        else {
            $scope.showErrorMsg = true;
        }
    }
    //CheckMemberByEmail by rid and email address 
    function CheckMemberByEmail(isValid) {
        //Check member Rid and Existed or not .
        $scope.showMemberExist = false;
        httpService.getData('/Reg/CheckMemberByEmail?r=' + $scope.rid + '&EmailAddress=' + $scope.user.EmailAddress).then(function (data) {
            if (data.Count == 0 || data.Count == undefined) {
                save(isValid);
            }
            else {
                if ($scope.orgDetails.IsEnablelogin == true) {
                    $scope.showMemberExist = true;
                }
                else {
                    window.location.href = '/Reg/Home?ug=' + data.UserGuid + '&rid=' + $scope.rid + '&cid=' + data.OrgId;
                }
            }
        }, function (err) {

            alert('error has occured');
        });
    }
    function save(isValid) {

        $scope.user.RefferId = $scope.rid;
        $scope.user.pubId = $scope.pubid;
        httpService.postData('/Reg/Save', $scope.user).then(function (response) {
            if (response.UserGuid != null && response.UserGuid != '' && response.UserGuid != '00000000-0000-0000-0000-000000000000') { // relevant check for member
                if ($scope.rid == 14864) { //skip relevent logic for make money survey
                    window.location.href = '/Reg/Home?ug=' + response.UserGuid + '&rid=' + $scope.rid + '&cid=' + 255
                }
                else {
                    window.location.href = '/Reg/Rel?ug=' + response.UserGuid + '&uid=' + response.UserId + '&c=' + response.CountryCode + '&cid=' + response.ClientID;
                }

            }
        }, function (err) {
            alert('error has occured');
        });

    }
    function InsertCookie(ea) {
        var cookie = ea + ',' + $scope.rid;
        httpService.getData('/Reg/WriteCookie?ea=' + cookie).then(function (response) {
            if (response == null || response == "") {
            }
            else {
                //   $scope.extmid = response;
                ////   GetMemberDetails();
            }
        }, function (err) {
            alert('error has occured');
        });
    }

}]);

