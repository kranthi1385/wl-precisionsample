define(['app'], function (app) {
    app.register.controller('pwdExpireController', ['$rootScope', '$scope', 'httpService', '$cookies', 'translationsLoadingService', '$window',
        function ($rootScope, $scope, httpService, $cookies, translationsLoadingService, $window) {
          
            var now = new $window.Date(),
     exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
            $scope.flagimg = "/images/flag_usa.png";
           
            translationsLoadingService.loadTranslatePagePath("abt");
            debugger;
            $scope.currentDomainDetails = function () {
                httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                    $scope.orgDetails = res;
                }, function (err) {
                });
            }
            $scope.currentDomainDetails();

            function getUrlVars() {
                var Url = window.location.href.toLowerCase();
                var vars = {};
                var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }
            $cookies.put('MainFlagCode', undefined, {
                expires: exp,
                path: '/'
            });
            //
            //Get query parmas value user guig
            ug = getUrlVars()["userguid"];
            if (ug == undefined) {
                ug = '';
            }
            lid = getUrlVars()["lid"];
            if (lid == undefined || lid == '') {
                lid = en;
            }
            if (lid == 7) {
                $cookies.put('MainLangCode', "Ar", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag_egypt.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("Ar");
                $scope.flagimg = "/images/images/flag_egypt.png";
            }
            if (lid == 35) {
                $cookies.put('MainLangCode', "en", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag_usa.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("en");
                $scope.flagimg = "/images/flag_usa.png";
            }
            if (lid == 36) {
                $cookies.put('MainLangCode', "nd", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag1.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("nd");
                $scope.flagimg = "/images/flag1.png";
            }
            if (lid == 27) {
                $cookies.put('MainLangCode', "Ch", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag5.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("Ch");
                $scope.flagimg = "/images/flag5.png";
            }
            if (lid == 69) {
                $cookies.put('MainLangCode', "It", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag_it.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("It");
                $scope.flagimg = "/images/flag_it.png";
            }
            if (lid == 44) {
                $cookies.put('MainLangCode', "Fr", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag_fr.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("Fr");
                $scope.flagimg = "/images/flag_fr.png";
            }
            if (lid == 111) {
                $cookies.put('MainLangCode', "pt", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', $scope.flag, {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("pt");
                $scope.flagimg = "/images/flag_usa.png";
            }
            if (lid == 51) {
                $cookies.put('MainLangCode', "De", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag4.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("De");
                $scope.flagimg = "/images/flag4.png";
            }
            if (lid == 140) {
                $cookies.put('MainLangCode', "es", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag_sp.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("es");
                $scope.flagimg = "/images/flag_sp.png";
            }
            if (lid == 70) {
                $cookies.put('MainLangCode', "Ja", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag3.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("Ja");
                $scope.flagimg = "/images/flag3.png";
            }
            if (lid == 120) {
                $cookies.put('MainLangCode', "Ru", {
                    expires: exp,
                    path: '/'
                });
                $cookies.put('MainFlagCode', "/images/flag_ru.png", {
                    expires: exp,
                    path: '/'
                });
                translationsLoadingService.setCurrentUserLanguage("Ru");
                $scope.flagimg = "/images/flag_ru.png";
            }
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
                $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }
            //Language translations
            // translationsLoadingService.setCurrentUserLanguage("en");
            //translationsLoadingService.loadTranslatePagePath("tc");
            $scope.DoNotSellInfo = {
                OldPassword: "",
                NewPassword: "",
                CnfNewPassword: "",
            }
            $scope.captchaResponse = '';
            $scope.setResponse = function (response) {
                $scope.captchaResponse = response;
            };

            //to get organization details
            $scope.changePassword = function (valid) {
                if (valid) {
                    $scope.captchaErr = false;
                    $scope.isShowError = false;
                    $scope.pwdmatchError = false;
                    $scope.msgOld = false;
                    $scope.weekpwdMsg = false;
                    $scope.samePwdError = false;
                    var newPassword = $scope.DoNotSellInfo.NewPassword;
                    var oldPassword = $scope.DoNotSellInfo.OldPassword;
                    var cnfNewPassword = $scope.DoNotSellInfo.CnfNewPassword;
                    $scope.DoNotSellInfo.NewPassword = newPassword.replace("&", "||**||").replace("#", "||*||").replace("+", "||***||");
                    $scope.DoNotSellInfo.OldPassword = oldPassword.replace("&", "||**||").replace("#", "||*||").replace("+", "||***||");
                    $scope.DoNotSellInfo.CnfNewPassword = cnfNewPassword.replace("&", "||**||").replace("#", "||*||").replace("+", "||***||");
                    if ($scope.DoNotSellInfo.NewPassword.length >= 8) {
                        if ($scope.DoNotSellInfo.NewPassword == $scope.DoNotSellInfo.CnfNewPassword) {
                            if ($scope.DoNotSellInfo.NewPassword != $scope.DoNotSellInfo.OldPassword) {
                                if ($scope.captchaResponse != '') {
                                    httpService.postData('/Common/ValidateCaptcha?googleResponse=' + $scope.captchaResponse).then(function (response) {
                                        if (response == 1) {
                                            httpService.postData('/Login/ChangePassword?OldPassword=' + $scope.DoNotSellInfo.OldPassword + '&NewPassword=' + $scope.DoNotSellInfo.NewPassword + '&CnfNewPassword=' + $scope.DoNotSellInfo.CnfNewPassword + '&ug=' + ug).then(function (response) {
                                                if (response == 1) {
                                                    $scope.pwdupdate = true;
                                                    $scope.pwdmatchError = false;
                                                    $scope.weekpwdMsg = false;
                                                    $scope.msgOld = false;
                                                    setTimeout(function () {
                                                        window.location.href = '/Home/LogIn';
                                                    }, 6000);
                                                }
                                                else if (response == 2) {
                                                    $scope.msgOld = true;
                                                    $scope.weekpwdMsg = false;
                                                    $scope.pwdmatchError = false;
                                                }
                                                $scope.errMsg = false;
                                            }, function (err) {
                                                $scope.errMsg = true;
                                            });
                                        }
                                    });                                   
                                } else {
                                    $scope.captchaErr = true;
                                }
                            }
                            else {
                                $scope.pwdmatchError = false;
                                $scope.weekpwdMsg = false;
                                $scope.msgOld = false;
                                $scope.samePwdError = true;
                            }
                        }
                        else {
                            $scope.pwdmatchError = true;
                            $scope.msgOld = false;
                            $scope.weekpwdMsg = false;
                            $scope.samePwdError = false;
                        }
                    }
                    else {
                        $scope.weekpwdMsg = true;
                        $scope.pwdmatchError = false;
                        $scope.msgOld = false;
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
})
