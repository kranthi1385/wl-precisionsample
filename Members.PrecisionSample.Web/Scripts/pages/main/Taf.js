define(['app'], function (app) {
    app.register.controller('tafController', ['$rootScope', '$scope', '$window', 'translationsLoadingService', 'httpService',
        function ($rootScope, $scope, $window, translationsLoadingService, httpService) {
            $scope.showErrorField = false;
            $scope.sucessMsg = false;
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
            httpService.getData('/hm/GetUserDetails').then(function (response) {
                $scope.user = response;
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
                $scope.xVerifyMessage = '';
                if ($scope.invite[0].FriendEmailAddress != "" || $scope.invite[1].FriendEmailAddress1 != "" || $scope.invite[2].FriendEmailAddress2 != ""
                    || $scope.invite[3].FriendEmailAddress3 != "" || $scope.invite[4].FriendEmailAddress4 != "") {
                    httpService.getData('/Home/vldEmail?email=' + $scope.invite[0].FriendEmailAddress +
                        '&email=' + $scope.invite[1].FriendEmailAddress1 + '&email=' + $scope.invite[2].FriendEmailAddress2 +
                        '&email=' + $scope.invite[3].FriendEmailAddress3 + '&email=' + $scope.invite[4].FriendEmailAddress4).then(function (response) {
                            //var message = xVerify();
                            if (response == "accepted") {
                                $scope.emailXVerify = 1;
                                $scope.xVerifyMessage = "Verified."
                                $scope.showErrorField = false;
                            }
                            else {
                                $scope.emailXVerify = 0;
                                $scope.xVerifyMessage = "Email Address not exist."
                                $scope.showErrorField = false;
                            }
                        }, function (err) {
                        });
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
            $scope.invites = function (isValid) {
                if ($scope.invite[0].FriendEmailAddress != "") {
                    if ($scope.invite[0].FriendEmailAddress != null) {
                        httpService.getData('/Home/EmailAddressCheck?email=' + $scope.invite[0].FriendEmailAddress).then(function (response) {
                            if (response.CpaCount >= 1) {
                                $scope.emailXVerify = 0;
                                $scope.xVerifyMessage = "Email Address already exist."
                            }
                            else if (response.CpaCount == 0) {
                                httpService.postData('/Taf/InviteFriends', $scope.invite).then(function (response) {
                                    $scope.invite = response;
                                    if (response != "") {
                                        $scope.sucessMsg = true;
                                        $scope.errMsg = false;
                                    }
                                }, function (err) {
                                    $scope.errMsg = true;
                                });
                            }
                        }, function (err) {
                        });
                    }
                    else {
                        $scope.showErrorField = true;
                        $scope.xVerifyMessage = ""
                    }
                }
                else {
                    $scope.showErrorField = true;
                    $scope.xVerifyMessage = ""
                }
            }

        }]);
});