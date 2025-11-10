define(['app'], function (app) {
    app.register.controller('redmController', ['$rootScope', '$scope', 'translationsLoadingService', 'httpService', '$cookies', 'vcRecaptchaService',
        function ($rootScope, $scope, translationsLoadingService, httpService, $cookies, vcRecaptchaService) {
            translationsLoadingService.loadTranslatePagePath("rr");
            $scope.ug = getUrlVars()["ug"];                    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&$scope.emailaddress=&address1=&$scope.city=&$scope.zip=&$scope.gender=&$scope.dob=&address2=&ethni$scope.city=
            if ($scope.ug == undefined) {
                $scope.ug = ''
            }
            var img = getUrlVars()["img"];
            var des = getUrlVars()["des"];
            var rn = getUrlVars()["rn"];
            var cid = getUrlVars()["cid"];
            var name = getUrlVars()["name"];
            $scope.cg = getUrlVars()["cg"];
            $scope.val = getUrlVars()["val"];
            $scope.rnam = getUrlVars()["rnam"]
            $scope.id = getUrlVars()["id"]
            $scope.countryID = getUrlVars()["countryId"]
            var sku = getUrlVars()["sku"]
            var unitprice = getUrlVars()["unitprice"]
            var descp = '';
            var chdescp = '';
            var redescp = '';
            var payVal = 0;   //to know the paypal or not
            var _acbalance = '';
            var _sku = '';
            var _ut = '';
            var _unitprice = '';
            var _points = 0;
            if (cid == 73 || cid == 111) {
                var rwValue = parseInt($scope.val / 9876)
                $scope.val = rwValue * 100;
                //$('#txtRedeemAmount').val(_ut * 100);
            }
            else {
                $scope.val = parseInt($scope.val / 9876);
            }
            GetSkuDetails();
            $('#imgRewardLogo')[0].src = decodeURIComponent(img);

            // to get URL params
            function getUrlVars() {
                var Url = window.location.href;
                var vars = {};
                var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }
            function GetSkuDetails() {
                var arr = $scope.id.split(';'); //sku  assign to vraiables
                $scope.sku = arr[0];
                $scope.val = $cookies.get('RewardValue');
                $scope.unitprice = arr[2];
                if (des.toLowerCase().match('pal') || des.toLowerCase().match('magazine')) {
                    $scope.isPayPal = true;
                    $('#ddlRedeemAmount').show();
                    $('#txtRedeemAmount').hide();
                    $('#divDiscl').hide();
                    $('#divDesc').show();
                    GetCatalogDetails()
                }
                else {
                    $scope.isPayPal = false;
                    $('#ddlRedeemAmount').hide();
                    $('#txtRedeemAmount').show();
                    $('#divDesc').hide();
                    $('#divDiscl').show();
                    GetRewardDetails(arr[0]);
                }
            }

            //get organization details
            $scope.getCurrentDomailDetails = function () {
                httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                    $scope.orgDetails = res;
                }, function (err) {
                });
            }
            $scope.getCurrentDomailDetails();

            //to get user details
            //httpService.getData('/hm/GetUserDetails').then(function (response) {
            //    $scope.user = response;
            //}, function (err) {
            //    $scope.errMsg = true;
            //});
            function GetCatalogDetails() {
                httpService.getData('/mr/GetCtlgDetailsById?cg=' + $scope.cg + '&cid=' + cid).then(function (data) {

                    if (data != "") {

                        $scope.trewards = data;
                        $scope.trewards.Description = data.RewardDescription;
                        $scope.trewards.RedeemAmount = $scope.val;

                    }
                }, function (err) {
                });
            }
            function GetRewardDetails(sku) {
                httpService.getData('/mr/GetRewardDetails?sku=' + encodeURIComponent(sku) + '&cid=' + cid + '&name=' + name).then(function (data) {
                    if (data != "") {
                        $scope.trewards = data;
                        $scope.trewards.RedeemAmount = $scope.val;
                    }
                }, function (err) {
                });
            }

           

            //---------------------------- reddem click redeem money ------------------------------------------------
            $scope.RedeemClick = function () {
                if ($scope.captchaResponse != '') {
                    httpService.postData('/Common/ValidateCaptcha?googleResponse=' + $scope.captchaResponse).then(function (response) {

                        if (response == 1) {//google captcha valid 

                            if (parseInt($scope.trewards.RedeemAmount) <= parseInt($scope.trewards.AccountBalance)) {
                                if (parseInt($scope.unitprice) == -1) {
                                    _points = parseInt($scope.val * 100);
                                }
                                else {
                                    _points = 0;
                                }

                                if ($scope.isPayPal) {
                                    if (parseInt($scope.trewards.RedeemAmount) >= $scope.trewards.MinRedemptionAmount) {

                                        httpService.getData('/mr/InserrtRewardRedeemprtions?amount=' + $scope.trewards.RedeemAmount + '&cg=' + $scope.cg).then(function (data) {
                                            if (data == 0) {
                                                $("#lblMessage").css('color', 'red');
                                                $("#lblMessage").text('Error occurred please try again');
                                            }
                                            else {
                                                $("#lblMessage").css('color', 'green');
                                                $("#lblMessage").text('You have successfully redeemed your reward. Your redemption will be verified and approved in 2 to 3 mins.');
                                            }
                                        }, function (err) {
                                            $("#lblMessage").css('color', 'red');
                                            $("#lblMessage").text('Error occurred please try again');
                                        });
                                    }
                                    else {
                                        $("#lblMessage").css('color', 'red');
                                        $("#lblMessage").text("You Can't Redeem less than " + $scope.trewards.MinRedemptionAmount + '$.');
                                    }

                                }
                                else {
                                    httpService.getData('/mr/RedeemMemberRewards?sku=' + $scope.sku + '&Ut=' + $scope.val + '&Points=' + _points + '&cid=' + cid + '&name=' + name ).then(function (data) {
                                        if (data == 1) {
                                            //window.location.href = '/reg/rc?ug=' + $scope.ug;
                                            $("#lblMessage").css('color', 'green');
                                            $("#lblMessage").text('You have successfully redeemed your reward. Your redemption will be verified and approved in 2 to 3 mins.');
                                        }
                                        if (data == 0) {
                                            $("#lblMessage").css('color', 'red');
                                            $("#lblMessage").text('We are unable to verify the country you live in, Can you please contact (mailto:support@opinionetwork.com) with a copy of your utility bill showing your address?');
                                        }
                                    }, function (err) {
                                        $("#lblMessage").css('color', 'red');
                                        $("#lblMessage").text('Error occurred please try again');
                                    });

                                }
                            }
                            else {
                                $("#lblMessage").css('color', 'red');
                                $("#lblMessage").text("You don't have enough balance to redeem the reward.");
                            }
                        }
                        else {
                            $scope.captchaErr = true;
                        }
                    }, function (err) {
                        //  $scope.errMsg = true;
                    });
                }
            }
            $scope.setResponse = function (response) {
                $scope.captchaResponse = response;
            };
            $scope.CancelClick = function () {
                window.location.href = '/mr/rewards';
                //httpService.getData('/mr/ClearSKUSes?sku' + $scope.sku).then(function (response) {
                //    window.location.href = '/mr/rewards';
                //}, function (err) {
                //    window.location.href = '/mr/rewards';
                //});
            }
            //--------------------------- control events validations --------------------------------------------
            function Vaildations() {

                var isValid = true;
                httpService.getData('/mr/ValidateCaptcha').then(function (response) {
                    if (response == true)
                        isValid = true;
                    else
                        isValid = false;
                }, function (err) {
                    window.location.href = '/partner/Home?ug=' + ug;
                });
                return isValid;
            }
        }]);

});