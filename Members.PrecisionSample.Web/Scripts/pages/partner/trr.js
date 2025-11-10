//main module of the soltion

angular.module("trrApp", ['pascalprecht.translate', 'customSerivces', 'vcRecaptcha','ngCookies', 'staticTranslationsModule'])
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
.controller("redController", ['$rootScope', '$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', 'vcRecaptchaService', '$cookies',
function ($rootScope, $scope, $http, $window, $location, $rootScop, $timeout, httpService, translationsLoadingService, vcRecaptchaService, $cookies) {
    //Get query parmas value referrer id
    //Get query parmas value referrer id

    $scope.ug = getUrlVars()["ug"];                    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&$scope.emailaddress=&address1=&$scope.city=&$scope.zip=&$scope.gender=&$scope.dob=&address2=&ethni$scope.city=
    if ($scope.ug == undefined) {
        $scope.ug = ''
    }
    var img = getUrlVars()["img"];
    var des = getUrlVars()["des"];
    var rn = getUrlVars()["rn"];
    var cid = getUrlVars()["cid"];
    var name = getUrlVars()["name"];
    $scope.val = getUrlVars()["val"];
    $scope.cg = getUrlVars()["cg"];
    var sku = getUrlVars()["sku"]
    var unitprice = getUrlVars()["unitprice"]
    $scope.rnam = getUrlVars()["rnam"]
    $scope.id = getUrlVars()["id"]
    var descp = '';
    var chdescp = '';
    var redescp = '';
    var payVal = 0;   //to know the paypal or not
    var _acbalance = '';
    var _sku = '';
    var _ut = '';
    var _unitprice = '';
    var _points = 0;
    $scope.val = parseInt($scope.val / 9876);
    if ($cookies.get('WidgetLangCode') != "" && $cookies.get('WidgetLangCode') != undefined && $cookies.get('WidgetLangCode') != "null") {
        translationsLoadingService.setCurrentUserLanguage($cookies.get('WidgetLangCode'));
    }
    else {
        translationsLoadingService.setCurrentUserLanguage('en');
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
        $scope.val = arr[1];
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
    function GetCatalogDetails() {
        httpService.getData('/Reg/GetCtlgDetailsById?cg=' + $scope.cg + '&ug=' + $scope.ug + '&cid=' + cid).then(function (data) {

            if (data != "") {

                $scope.trewards = data;
                $scope.trewards.Description = data.RewardDescription;
                $scope.trewards.RedeemAmount = $scope.val;

            }
        }, function (err) {
        });
    }
    function GetRewardDetails(sku) {
        httpService.getData('/Reg/GetRewardDetails?sku=' + encodeURIComponent(sku) + '&ug=' + $scope.ug + '&cid=' + cid + '&name=' + name).then(function (data) {
            if (data != "") {
                $scope.trewards = data;
                $scope.trewards.RedeemAmount = $scope.val;
                $('#dvImageLoading').hide();
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

                                httpService.getData('/mr/InserrtRewardRedeemprtions?amount=' + $scope.trewards.RedeemAmount
                                    + '&cg=' + $scope.cg + '&ug=' + ug + '&cid=' + cid).then(function (data) {
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
                            httpService.getData('/reg/RedeemMemberRewards?sku=' + $scope.sku + '&Ut=' + $scope.val + '&Points=' + _points + '&ug=' + $scope.ug + '&cid=' + cid).then(function (data) {
                                if (data == 1) {
                                    window.location.href = '/reg/rc?ug=' + $scope.ug + '&cid=' + cid;
                                }
                                else {
                                    $("#lblMessage").css('color', 'red');
                                    $("#lblMessage").text('Error occurred please try again');
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
        window.location.href = '/reg/Home?ug=' + $scope.ug + '&cid=' + cid;
    }
    //--------------------------- control events validations --------------------------------------------
    function Vaildations() {

        var isValid = true;
        httpService.getData('/Reg/ValidateCaptcha').then(function (response) {
            if (response == true)
                isValid = true;
            else
                isValid = false;
        }, function (err) {
            window.location.href = '/partner/Home?ug=' + '&cid=' + cid;
        });
        return isValid;
    }
}])