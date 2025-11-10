debugger;
define(['app'], function (app) {
    app.register.controller('accountController', ['$rootScope', '$scope', '$timeout', '$window', 'httpService', 'translationsLoadingService', '$cookies',
    function ($rootScope, $scope, $timeout, $window, httpService, translationsLoadingService, $cookies) {
        translationsLoadingService.loadTranslatePagePath("ep");
        $scope.submitMsg = false;
        $scope.emailMsg = false;
        //beind years
        var d = new Date();
        var year = d.getFullYear();
        var yearLst = [];

        for (var i = 13; i < 100; i++) {
            yearLst.push({ key: year - i, value: year - i });
        }
        $scope.year = yearLst;
        $scope.stateslist = [];
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
            $scope.orgDetails = res;
        }, function (err) {
        });
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
        $scope.countryByStates = function (cc) {
            $scope.states = [];
            for (var i = 0; i < $scope.stateslist.length; i++) {
                if (cc == $scope.stateslist[i].CId) {
                    $scope.states.push($scope.stateslist[i]);

                }
            }
        }
        //get all states avalibles
        $scope.getUserDetails = function () {
            $scope.changeClick = false;
            httpService.getData('/Ep/GetUserData').then(function (response) {
                $scope.user = response;
                if ($scope.user.Gender == 'M') {
                    $scope.Gender = 'Male';
                }
                else if ($scope.user.Gender == 'F') {
                    $scope.Gender = 'Female';
                }
                else {
                    $scope.Gender = 'Non-Binary';
                }

                if ($scope.user.ChooseYourReward == 1) {
                    $scope.user.ChooseYourReward = 'Netflix';
                }
                else if ($scope.user.ChooseYourReward == 2) {
                    $scope.user.ChooseYourReward = 'Apple Music';
                }
                else if ($scope.user.ChooseYourReward == 3) {
                    $scope.user.ChooseYourReward = 'Apple TV+';
                }
                else if ($scope.user.ChooseYourReward == 4) {
                    $scope.user.ChooseYourReward = 'Spotify';
                }
                else if ($scope.user.ChooseYourReward == 5) {
                    $scope.user.ChooseYourReward = 'Xbox Live Gold';
                }
                else if ($scope.user.ChooseYourReward == 6) {
                    $scope.user.ChooseYourReward = 'EA Play';
                }
                else if ($scope.user.ChooseYourReward == 7) {
                    $scope.user.ChooseYourReward = 'Playstation Plus';
                }
                else {
                    $scope.user.ChooseYourReward = 'Choose Your Reward';
                }

                $scope.oldEmail = $scope.user.EmailAddress;
                $scope.oldCountryID = $scope.user.CountryId;
                httpService.getData('/Common/GetCountrysAndStates').then(function (res) {
                    $scope.countries = res.CountryList;
                    $scope.stateslist = res.StateList;
                    if ($scope.user.CountryId != "") {
                        $scope.countryByStates($scope.user.CountryId);
                    }
                }, function (err) {
                    // $scope.errMsg = true;
                });
                //campaign id based on user country
                if ($scope.user.LanguageId == 140) {
                    $scope.CampaignID = 1913;
                    $scope.TemplateSubject = "Mensaje importante de";
                }
                else if ($scope.user.LanguageId == 120) {
                    $scope.CampaignID = 1914;
                    $scope.TemplateSubject = "Важное сообщение от";
                }
                else if ($scope.user.LanguageId == 111) {
                    $scope.CampaignID = 1916;
                    $scope.TemplateSubject = "Mensagem importante de";
                }
                else if ($scope.user.LanguageId == 110) {
                    $scope.CampaignID = 1915;
                    $scope.TemplateSubject = "Ważna wiadomość od";
                }
                else if ($scope.user.LanguageId == 80) {
                    $scope.CampaignID = 1917;
                    $scope.TemplateSubject = "의 중요한 메시지";
                }
                else if ($scope.user.LanguageId == 70) {
                    $scope.CampaignID = 1918;
                    $scope.TemplateSubject = "からの重要なメッセージ";
                }
                else if ($scope.user.LanguageId == 69) {
                    $scope.CampaignID = 1920;
                    $scope.TemplateSubject = "Messaggio importante da";
                }
                else if ($scope.user.LanguageId == 51) {
                    $scope.CampaignID = 1923;
                    $scope.TemplateSubject = "Wichtige Nachricht von";
                }
                else if ($scope.user.LanguageId == 44) {
                    $scope.CampaignID = 1919;
                    $scope.TemplateSubject = "Message important de";
                }
                else if ($scope.user.LanguageId == 36) {
                    $scope.CampaignID = 1924;
                    $scope.TemplateSubject = "Belangrijk bericht van";
                }
                else if ($scope.user.LanguageId == 27) {
                    $scope.CampaignID = 1921;
                    $scope.TemplateSubject = "來自的重要訊息";
                }
                else {
                    $scope.CampaignID = 1922;
                    $scope.TemplateSubject = "Important Message from";
                }
                $scope.user.CampaignID = $scope.CampaignID;

            }, function (err) {
                $scope.errMsg = true;
            });
        }

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
        $scope.getUserDetails();
        $scope.saveDetails = function (valid, user) {
            if (valid) {
                if ($scope.oldEmail != $scope.user.EmailAddress && $scope.oldCountryID != $scope.user.CountryId) {
                    $scope.user.CustomAttribute = "first_name:" + $scope.user.FirstName + ";last_name:"
                                             + $scope.user.LastName + ";create_dt:" + $scope.user.CreateDate + ";ip_address:"
                                             + $scope.user.IpAddress + ";org_logo:" + $scope.orgDetails.OrgLogo.replace("http://", "") + ";org_name:" + $scope.orgDetails.OrgName
                                              + ";member_url:" + $scope.orgDetails.MemberUrl + ";subject:" + $scope.TemplateSubject;
                }
                else if ($scope.oldEmail != $scope.user.EmailAddress) {
                    $scope.user.CustomAttribute = "first_name:" + $scope.user.FirstName + ";last_name:"
                                             + $scope.user.LastName + ";email_address:" + $scope.user.EmailAddress + ";create_dt:" + $scope.user.CreateDate + ";ip_address:"
                                             + $scope.user.IpAddress + ";org_logo:" + $scope.orgDetails.OrgLogo.replace("http://", "") + ";org_name:" + $scope.orgDetails.OrgName
                                              + ";member_url:" + $scope.orgDetails.MemberUrl + ";subject:" + $scope.TemplateSubject;
                }
                else if ($scope.oldCountryID != $scope.user.CountryId) {
                    $scope.user.CustomAttribute = "first_name:" + $scope.user.FirstName + ";last_name:"
                                                                + $scope.user.LastName + ";create_dt:" + $scope.user.CreateDate + ";ip_address:"
                                                                + $scope.user.IpAddress + ";org_logo:" + $scope.orgDetails.OrgLogo.replace("http://", "") + ";org_name:" + $scope.orgDetails.OrgName
                                                                 + ";member_url:" + $scope.orgDetails.MemberUrl + ";subject:" + $scope.TemplateSubject;
                }
                httpService.postData('/Ep/saveUser',user).then(function (response) {
                    $scope.result = response;
                    if ($scope.oldEmail != $scope.user.EmailAddress || $scope.oldCountryID != $scope.user.CountryId) {
                        $scope.emailMsg = true;
                    }
                    if (response != "") {
                        $scope.submitMsg = true;
                        $scope.cancelMsg = false;
                        $scope.errMsg = false;
                        $scope.changeClick = false;
                        $scope.getUserDetails();
                    }
                }, function (err) {
                    $scope.errMsg = true;
                });
            }
            else {
                $scope.errMsg = true;
            }
        }

        $scope.Download = function () {
            url = 'http://dev.op.ps.com/Ep/downloadUser'
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

        $scope.showpopup = function () {
            $('#modal_popup').modal({ backdrop: 'static' });
        }

        $scope.cancel = function () {
            $('#modal_popup').modal('hide');
            if ($scope.user.LanguageId == 140) {
                $scope.CampaignID = 1936;
                $scope.TemplateSubject = "Mensaje importante de";
            }
            else if ($scope.user.LanguageId == 120) {
                $scope.CampaignID = 1933;
                $scope.TemplateSubject = "Важное сообщение от";
            }
            else if ($scope.user.LanguageId == 111) {
                $scope.CampaignID = 1935;
                $scope.TemplateSubject = "Mensagem importante de";
            }
            else if ($scope.user.LanguageId == 110) {
                $scope.CampaignID = 1932;
                $scope.TemplateSubject = "Ważna wiadomość od";
            }
            else if ($scope.user.LanguageId == 80) {
                $scope.CampaignID = 1931;
                $scope.TemplateSubject = "의 중요한 메시지";
            }
            else if ($scope.user.LanguageId == 70) {
                $scope.CampaignID = 1934;
                $scope.TemplateSubject = "からの重要なメッセージ";
            }
            else if ($scope.user.LanguageId == 69) {
                $scope.CampaignID = 1930;
                $scope.TemplateSubject = "Messaggio importante da";
            }
            else if ($scope.user.LanguageId == 51) {
                $scope.CampaignID = 1929;
                $scope.TemplateSubject = "Wichtige Nachricht von";
            }
            else if ($scope.user.LanguageId == 44) {
                $scope.CampaignID = 1928;
                $scope.TemplateSubject = "Message important de";
            }
            else if ($scope.user.LanguageId == 36) {
                $scope.CampaignID = 1927;
                $scope.TemplateSubject = "Belangrijk bericht van";
            }
            else if ($scope.user.LanguageId == 27) {
                $scope.CampaignID = 1926;
                $scope.TemplateSubject = "來自的重要訊息";
            }
            else {
                $scope.CampaignID = 1925;
                $scope.TemplateSubject = "Important Message from";
            }
            $scope.user.CampaignID = $scope.CampaignID;
            $scope.user.CustomAttribute = "first_name:" + $scope.user.FirstName + ";last_name:"
                                       + $scope.user.LastName + ";email_address:" + $scope.user.EmailAddress + ";create_dt:" + $scope.user.CreateDate + ";ip_address:"
                                       + $scope.user.IpAddress + ";org_logo:" + $scope.orgDetails.OrgLogo.replace("http://", "") + ";org_name:" + $scope.orgDetails.OrgName
                                        + ";member_url:" + $scope.orgDetails.MemberUrl + ";user_guid:" + $scope.user.UserGuid + ";subject:" + $scope.TemplateSubject;
            $scope.getUserDetails();
            httpService.postData('/Ep/DeleteUserDataEmail?SubId3=' + $scope.user.SubId3 + '&Reason=' + $scope.user.Reason + '&campaign_id=' + $scope.user.CampaignID + '&custom_attr=' + $scope.user.CustomAttribute).then(function (response) {
                $scope.result = response;
                if (response == "") {
                    //httpService.getData('/Common/LogOut').then(function (response) {
                    //    window.location.href = '/Home/LogIn';
                    //}, function (err) {
                    //    window.location.href = '/Home/LogIn';
                    //});
                    $scope.forgtMsg = true;
                    $scope.cancelMsg = true;
                    $scope.submitMsg = false;
                    $scope.errMsg = false;

                }
            }, function (err) {
                $scope.errMsg = true;
            });
        }

        $scope.notcancel = function () {
            $('#modal_popup').modal('hide');
        }
        //change details
        $scope.changePswd = function () {
            if ($scope.user.ChooseYourReward == 'Netflix') {
                $scope.user.ChooseYourReward = 1;
            }
            else if ($scope.user.ChooseYourReward == 'Apple Music') {
                $scope.user.ChooseYourReward = 2;
            }
            else if ($scope.user.ChooseYourReward == 'Apple TV+') {
                $scope.user.ChooseYourReward = 3;
            }
            else if ($scope.user.ChooseYourReward == 'Spotify') {
                $scope.user.ChooseYourReward = 4;
            }
            else if ($scope.user.ChooseYourReward == 'Xbox Live Gold') {
                $scope.user.ChooseYourReward = 5;
            }
            else if ($scope.user.ChooseYourReward == 'EA Play') {
                $scope.user.ChooseYourReward = 6;
            }
            else if ($scope.user.ChooseYourReward == 'Playstation Plus') {
                $scope.user.ChooseYourReward = 7;
            }           
            $scope.changeClick = true;
        }
        httpService.getData('/Common/GetCurrentDomainDetails').then(function (response) {
            $scope.orgDetails = response;
        }, function (err) {
        });
        $scope.recaptchalangCode = $cookies.get('UserLangCode') || $cookies.get('MainLangCode');
        //Do not sell my info
        $scope.doNotSellMyInfo = function () {
            httpService.postData('/Login/SaveDoNotSellMyInfo?fstName=' + $scope.user.FirstName + '&lstName=' + $scope.user.LastName + '&email=' + $scope.user.EmailAddress + '&presite=' + 'opinionbar.com' + '&reqid=' + 2).then(function (response) {
                if (response == '') {
                    $scope.showMsg = true;
                    $scope.errMsg = false;
                }
            }, function (err) {
                $scope.errMsg = true;
            });
            //  $window.open($scope.orgDetails.MemberUrl + '/login/dns?lc=' + $scope.recaptchalangCode, 'SurveyDownline-DNS', 'width=800,height=800')
        }
    }]);
});