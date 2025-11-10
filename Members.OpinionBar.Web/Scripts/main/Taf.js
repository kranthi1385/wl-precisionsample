define(['app'], function (app) {
    app.register.controller('tafController', ['$rootScope', '$scope', '$window', 'translationsLoadingService', 'httpService',
        function ($rootScope, $scope, $window, translationsLoadingService, httpService) {
            $scope.showErrorField = false;
            $scope.sucessMsg = false;
            $scope.captchaResponse = '';
            $scope.invite = [{
                FriendFirstName: '',
                FriendEmailAddress: '',
            }, {
                FriendFirstName1: '',
                FriendEmailAddress1: '',
            }, {
                FriendFirstName2: '',
                FriendEmailAddress2: '',
            }, {
                FriendFirstName3: '',
                FriendEmailAddress3: '',
            }, {
                FriendFirstName4: '',
                FriendEmailAddress4: '',
            }
            ]
            //get current user details
            httpService.getData('/Ep/GetUserData').then(function (response) {
                $scope.user = response;
                //campaign id based on user country
                if ($scope.user.LanguageId == 140) {
                    $scope.CampaignID = 1863;
                    $scope.TemplateSubject = "Mensaje importante de";
                }
                else if ($scope.user.LanguageId == 120) {
                    $scope.CampaignID = 1862;
                    $scope.TemplateSubject = "Важное сообщение от";
                }
                else if ($scope.user.LanguageId == 111) {
                    $scope.CampaignID = 1861;
                    $scope.TemplateSubject = "Mensagem importante de";
                }
                else if ($scope.user.LanguageId == 110) {
                    $scope.CampaignID = 1860;
                    $scope.TemplateSubject = "Ważna wiadomość od";
                }
                else if ($scope.user.LanguageId == 80) {
                    $scope.CampaignID = 1859;
                    $scope.TemplateSubject = "의 중요한 메시지";
                }
                else if ($scope.user.LanguageId == 70) {
                    $scope.CampaignID = 1875;
                    $scope.TemplateSubject = "からの重要なメッセージ";
                }
                else if ($scope.user.LanguageId == 69) {
                    $scope.CampaignID = 1857;
                    $scope.TemplateSubject = "Messaggio importante da";
                }
                else if ($scope.user.LanguageId == 51) {
                    $scope.CampaignID = 1856;
                    $scope.TemplateSubject = "Wichtige Nachricht von";
                }
                else if ($scope.user.LanguageId == 44) {
                    $scope.CampaignID = 1855;
                    $scope.TemplateSubject = "Message important de";
                }
                else if ($scope.user.LanguageId == 36) {
                    $scope.CampaignID = 1854;
                    $scope.TemplateSubject = "Belangrijk bericht van";
                }
                else if ($scope.user.LanguageId == 27) {
                    $scope.CampaignID = 1853;
                    $scope.TemplateSubject = "來自的重要訊息";
                }
                else {
                    $scope.CampaignID = 20;
                    $scope.TemplateSubject = "Important Message from";
                }
            }, function (err) {
            });
            //get current domain details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
            //validate email through xverify
            $scope.validateEmail = function () {
                $scope.emailXVerify = -1;
                $scope.xVerifyMessageexist = false;
                $scope.xVerifyMessage = false;
                $scope.emailnotexist = false;
                $scope.sucessMsg = false;
                $scope.xVerifyMessage = '';
                if ($scope.invite[0].FriendEmailAddress != undefined || $scope.invite[1].FriendEmailAddress1 != "" || $scope.invite[2].FriendEmailAddress2 != ""
                    || $scope.invite[3].FriendEmailAddress3 != "" || $scope.invite[4].FriendEmailAddress4 != "") {
                    httpService.getData('/Home/vldEmail?email=' + $scope.invite[0].FriendEmailAddress +
                        '&email=' + $scope.invite[1].FriendEmailAddress1 + '&email=' + $scope.invite[2].FriendEmailAddress2 +
                        '&email=' + $scope.invite[3].FriendEmailAddress3 + '&email=' + $scope.invite[4].FriendEmailAddress4).then(function (response) {
                            //var message = xVerify();
                            if (response == "accepted") {
                                $scope.emailXVerify = 1;
                                $scope.xVerifyMessage = true;
                                $scope.showErrorField = false;
                                $scope.sucessMsg = false;
                            }
                            else {
                                $scope.emailXVerify = 0;
                                $scope.emailnotexist = true;
                                $scope.showErrorField = false;
                            }
                        }, function (err) {
                        });
                }
                else {
                    $scope.emailnotexist = false;
                }

            }
            //Language translations
            translationsLoadingService.loadTranslatePagePath("Taf");
            //opens new window to login to facebook
            $scope.openWindow = function () {
                $window.open('http://www.facebook.com', 'Facebook', 'width=1000,height=800');
            }
            //opens new window to login to twitter
            $scope.windowOpen = function () {
                $window.open('http://www.twitter.com', 'Twitter', 'width=1000,height=800');
            }
            $scope.setResponse = function (response) {
                $scope.captchaResponse = response;
            };
            $scope.invites = function (isValid) {
                $scope.captchaErr = false;
                $scope.emailnotexist = false;
                if ($scope.invite[0].FriendEmailAddress != "" && $scope.invite[0].FriendFirstName != "") {
                    if ($scope.invite[0].FriendEmailAddress != null && $scope.invite[0].FriendFirstName != null) {
                        if ($scope.captchaResponse != '') {
                            httpService.getData('/Home/EmailAddressCheck?email=' + $scope.invite[0].FriendEmailAddress).then(function (response) {
                                if (response.CpaCount >= 1) {
                                    $scope.emailXVerify = 0;
                                    $scope.xVerifyMessageexist = true;
                                    //$scope.sucessMsg = false;
                                    $scope.xVerifyMessage = false;
                                }
                                else if (response.CpaCount == 0) {
                                    httpService.postData('/Taf/InviteFriends?camID=' + $scope.CampaignID + '&tid=' + $scope.TemplateSubject, $scope.invite).then(function (response) {
                                        $scope.invite = response;
                                        if (response != "") {
                                            $scope.sucessMsg = true;
                                            $scope.errMsg = false;
                                            $scope.xVerifyMessageexist = false;

                                        }
                                    }, function (err) {
                                        $scope.errMsg = true;
                                        $scope.xVerifyMessage = false;
                                    });
                                }
                            }, function (err) {
                            });
                        }
                        else {
                            $scope.captchaErr = true;
                        }
                    }
                    else {
                        $scope.showErrorField = true;
                        $scope.xVerifyMessage = "";
                        $scope.captchaErr = false;
                    }
                }
                else {
                    $scope.showErrorField = true;
                    $scope.xVerifyMessage = "";
                    $scope.captchaErr = false;
                }
            }
        }]);
});