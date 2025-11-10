//main module of the soltion

angular.module("phmApp", ['pascalprecht.translate', 'customSerivces', 'ngCookies', 'staticTranslationsModule'])
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
.controller("surveyPController", ['$rootScope', '$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', 'pagerService', 'getCurrentPageList', '$cookies',
function ($rootScope, $scope, $http, $window, $location, $rootScop, $timeout, httpService, translationsLoadingService, pagerService, getCurrentPageList, $cookies) {
    //Get query parmas value referrer id
    $scope.ug = getUrlVars()["ug"];                    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&$scope.emailaddress=&address1=&$scope.city=&$scope.zip=&$scope.gender=&$scope.dob=&address2=&ethni$scope.city=
    if ($scope.ug == undefined) {
        $scope.ug = ''
    }

    cid = getUrlVars()["cid"];                    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&$scope.emailaddress=&address1=&$scope.city=&$scope.zip=&$scope.gender=&$scope.dob=&address2=&ethni$scope.city=
    if (cid == undefined) {
        cid = ''
    }
    rid = getUrlVars()["rid"];                    //www.opinionetwork.com/partner/reg.htm?rid=&Country=&extmemberid=&Firstname=&Lastname=&$scope.emailaddress=&address1=&$scope.city=&$scope.zip=&$scope.gender=&$scope.dob=&address2=&ethni$scope.city=
    if (rid == undefined) {
        rid = 0
    }
    //if clientid contains # menu tag
    if (cid.indexOf('#') != -1) {
        var orgId = [];
        orgId = cid.split('#')
        if (orgId.length > 0) {
            cid = orgId[0];
        }
    }
    $scope.OrgID = cid;
    //get user details
    $scope.recaptchalangCode = 'en';
    var d = new Date();
    var year = d.getFullYear();
    $scope.currentYear = year;
    $scope.nextYear = year + 1;
    var now = new $window.Date(),
  exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
    httpService.getData('/reg/GetUserDetails?ug=' + $scope.ug + '&cid=' + cid).then(function (response) {
        $scope.user = response;
        $cookies.put('WidgetLangCode', response.LanguageCode, {
            expires: exp,
            path: '/'
        });
        debugger;
        languageSetup();
    }, function (err) {
        $scope.errMsg = true;
    })
    function languageSetup(){
        if ($cookies.get('WidgetLangCode') != "" && $cookies.get('WidgetLangCode') != undefined && $cookies.get('WidgetLangCode') != "null") {
            translationsLoadingService.setCurrentUserLanguage($cookies.get('WidgetLangCode'));
            $scope.recaptchalangCode = $cookies.get('WidgetLangCode');
        }
        else {
            httpService.getData('/reg/GetUserDetails?ug=' + $scope.ug + '&cid=' + cid).then(function (response) {
                $scope.user = response;
                $cookies.put('WidgetLangCode', response.LanguageCode, {
                    expires: exp,
                    path: '/'
                });
            }, function (err) {
                $scope.errMsg = true;
            })
            translationsLoadingService.setCurrentUserLanguage(response.LanguageCode);
            $scope.recaptchalangCode = $cookies.get('WidgetLangCode');
        }
}
   
    //button click event
    $rootScope.reward = true;
    $scope.showSurveys = true;
    $rootScope.redeemHistLstgrid = false;
    $scope.historyclick = function (buttonId) {
        if (buttonId == 1) {
            $rootScope.reward = false;
            $rootScope.redeemHistLstgrid = true;
            $scope.$broadcast("redeemHistoryClick");

        }
        else {
            $rootScope.reward = true;
            $rootScope.redeemHistLstgrid = false;
        }

    };

    $scope.profiles = [];
    $scope.surveys = [];
    $scope.Rewards = [];
    $scope.Reward = [];
    $scope.totalItems = 0; //total page item count
    $scope.pager = {}; // pagging array
    $scope.tabIndex = 0;//default tab index
    $scope.totalItems = 0; //total page item count
    $scope.redeemHistLst = [];//redeemption history list
    $scope.redmHistoryInfo = [];
    $scope.isShowRewards = false;
    $rootScope.orgName = '';
    $scope.availableCards = [
        {
            cardName: "retailGiftCard",
            templateName: 'RetailGiftCard'
        },
        {
            cardName: 'restaurantGiftCard',
            templateName: 'RestaurantGiftCard'
        },
        {
            cardName: 'onlineServices ',
            templateName: 'OnlineServices'
        },
        {
            cardName: 'charitableDonation ',
            templateName: 'CharitableDonation'
        }]

    $scope.RewardClick = function (parent, obj, $event) {
        var _id = $event.currentTarget.id;
        if (_id.split(';')[2] == 1 && obj.IsDisable == false) {
            skustoredinsession(parent, obj, _id, 'd');
        }
        else {
            if (obj.IsDisable == false) {
                skustoredinsession(parent, obj, _id, 'rn');
            }
        }
    }

    //Accordian tabs click event
    $scope.tabClick = function (index) {
        $scope.tabIndex = index;

    }
    //button click even
    $scope.reddemclick = function () {
        $rootScope.redeemHistLst = !$rootScope.redeemHistLst;
        $rootScope.reward = !$rootScope.reward;
    };

    httpService.getData('/Reg/GetClientDetails?ug=' + $scope.ug + '&cid=' + cid).then(function (response) {
        $rootScope.orgName = response.OrgName;
        $scope.isShowRewards = response.IsRewardsShow;
        if (response.ClientId == 120) {
            $('#healttext').show();
        }
        else {
            $('#healttext').hide();
        }

        if (response.ClientId == 170 || response.ClientId == 174) {
            $("#imgLogo").attr("src", response.OrgLogo);
        }
        else {
            $("#imgLogo").css("display", 'none');
        }
        if ($scope.ug != '') {
            //This is the first call on page load.
            GetSurveyList();
            //GetProfileList();
            // GetSurveyHistory();
            if (response.IsRewardsShow == true) {
                //GetRewardList();
                // GetRedeemHistory();
            }
        }
    })

    //So we pass Client ID to get all Client Properties like, is Show Rewards,


    function GetSurveyList() {
        httpService.getData('/Reg/GetUserSurveyList?ug=' + $scope.ug + '&cid=' + cid + '&rid=' + rid).then(function (data) {
            if (data != "") {
                $("#dvSurveyList").show();
                $('#dvSurveyErrorMessage').hide();
                $scope.lstSurveys = data;
                if (data.length == 1) {
                    if (data[0].ProjectId == 0) {
                        $scope.showSurveys = false;
                    }
                }
                $scope.surveys = data;
            }
            else {
                $scope.showSurveys = false;
                $("#dvSurveyList").hide();
            }

        }, function (err) {
            //$scope.errMgs = true;
        });
    }


    //-------------------------------------------------------Get Member SurveyList----------------------------------------------------------------------
    $scope.home = function () {
        GetSurveyList();
    }
    //---------------------------------------------Get Member ProfileList-------------------------------------------------------------
    $scope.menu1 = function () {
        GetProfileList();
        function GetProfileList() {
            httpService.getData('/Reg/GetProfileList?ug=' + $scope.ug + '&cid=' + cid).then(function (data) {
                if (data != "") {
                    $("#dvProfileList").show();
                    $('#dvProfileErrorMessage').hide();
                    //if profiles are completed show checkbox checked 
                    for (var i = 0; i < data.length; i++) {

                        if (data[i].ProfileStatus == 'Completed') {
                            data[i].IsChecked = true;
                        }
                    }
                    $scope.profiles = data;
                }
                else {
                    $("#dvProfileList").show();
                }
                $('#dvProfilesLoading').hide();
            }, function (err) {
                //$scope.errMgs = true;
            });
        }
    }
    // ----------------------------------------------Get survey history ---------------------------------------------------------------
    $scope.menu3 = function () {
        GetSurveyHistory();
        function GetSurveyHistory() {
            $scope.suvHisLst = [];
            httpService.getData('/Reg/GetSurveyHistory?ug=' + $scope.ug + '&cid=' + cid).then(function (data) {
                $scope.surveyHistory = data;
                $scope.totalItems = data.length;
                if ($scope.totalItems > 0) {
                    $scope.pager = pagerService.getPager($scope.totalItems, 1);// set pager value and index 
                    //using service get current page list 
                    $scope.suvHisLst = getCurrentPageList.getCurrentPageRecords($scope.surveyHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                }
                else {
                    $scope.suvHisLst = data;
                }
            }, function (err) {
                //$scope.errMgs = true;
            });


            //Pagging click event
            $scope.setPage = function (page) { // page  click count  
                if (page < 1 || page > $scope.pager.totalPages) {  //current page  count lessthan 1 or more than page count  then retu
                    return;
                }
                // get pager object from service
                $scope.pager = pagerService.getPager($scope.totalItems, page, $scope.pager.startPage, $scope.pager.endPage); // set pager value and index 
                $scope.suvHisLst = [];
                //using service get current page list 
                $scope.suvHisLst = getCurrentPageList.getCurrentPageRecords($scope.surveyHistory, $scope.pager.startIndex, $scope.pager.endIndex);
            }
        }
    }
    // ----------------------------------------------Get Rewards and Reward history ---------------------------------------------------------------
    $scope.menu2 = function () {
        GetRewardList();
        function GetRewardList() {
            //Get all tango rewards list
            httpService.getData('/Reg/GetRewardData?ug=' + $scope.ug + '&cid=' + cid).then(function (data) {
                $scope.rewData = data;
                $scope.lstRetailGiftCard = [];
                $scope.lstRestaurantGiftCard = [];
                $scope.lstOnlineServices = [];
                $scope.lstCharitableDonation = [];

                angular.forEach($scope.rewData, function (retail, key) {
                    if (retail.Category == "Retail Gift Card") {
                        $scope.lstRetailGiftCard.push(retail)
                    }
                });
                angular.forEach($scope.rewData, function (restaurant, key) {
                    if (restaurant.Category == "Restaurant Gift Card") {
                        $scope.lstRestaurantGiftCard.push(restaurant)
                    }
                });
                angular.forEach($scope.rewData, function (online, key) {
                    if (online.Category == "Online Services") {
                        $scope.lstOnlineServices.push(online)
                    }
                });
                angular.forEach($scope.rewData, function (Charitable, key) {
                    if (Charitable.Category == "Charitable Donation") {
                        $scope.lstCharitableDonation.push(Charitable)
                    }
                });
                //$scope.lstRetailGiftCard = $scope.rewData.filter((x) => x.Category == "Retail Gift Card");
                //$scope.lstRestaurantGiftCard = $scope.rewData.filter((x) => x.Category == "Restaurant Gift Card");
                //$scope.lstOnlineServices = $scope.rewData.filter((x) => x.Category == "Online Services");
                //$scope.lstCharitableDonation = $scope.rewData.filter((x) => x.Category == "Charitable Donation")
                //------------ Get member all avaliable reward history --------------------------------//

            })
            httpService.getData('/Reg/GetRewardsHistory?ug=' + $scope.ug + '&cid=' + cid).then(function (response) {
                $scope.rewHistoryLst = [];
                $scope.userRewardsInfo = response;
                if (response != null && response != "") {
                    $scope.totalItems = response.LstRewardHistory.length;
                    if ($scope.totalItems > 0) {
                        $scope.pager = pagerService.getPager($scope.totalItems, 1);// set pager value and index 
                        //using service get current page list 
                        $scope.rewHistoryLst = getCurrentPageList.getCurrentPageRecords($scope.userRewardsInfo.LstRewardHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                    }
                    else {
                        $scope.rewHistoryLst = response;
                    }
                }
            }, function (err) {
                $scope.errMsg = true;

            });
        }
    }
    function loadPopupBox() {
        // To Load the Popupbox
        $('#popup_box').fadeIn("slow");
    }

    function unloadPopupBox() {
        // TO Unload the Popupbox
        $('#popup_box').fadeOut("slow");

    }
    //-----------------------open new window position center of the body
    function popupCenter(url, title, w, h) {
        var left = (screen.width / 2) - (w / 2);
        var top = (screen.height / 2) - (h / 2);
        window.open(url, 'resizable = no', 'width =' + w + ', height =' + h + ',top=' + top + ',left=' + left);
    }
    //--------------------------Valiadations for contact us page-----------------
    function Vaildations() {
        var isValid = true;
        var ea = $("#txtEamilAddress").val();
        var hlp = $("#txtComment").val();
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        if (ea == '' || ea == null) {
            isValid = false;

        }
        else if (!emailReg.test(ea)) {
            isValid = false;
        }
        if (hlp == '' || hlp == null) {
            isValid = false;

        }
        return isValid;
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
    //------------------Contact us Button click event send email-------------------------------
    function SendEmail() {
        debugger;
        $('#dvConfMsg').hide();
        var _uname = $('#txtUsername').val();
        var _em = $('#txtEamilAddress').val();
        //var _email = $('#txtEmail').val();
        var _comment = $('#txtComment').val();
        httpService.getData('/Reg/SendEmail?uname=' + _uname + '&email=' + _em + '&comment=' + _comment).then(function (data) {
            if (data == 1) {
                $('#dvConfMsg').show();
            }
            else {
                $('#dvConfMsg').hide();
            }
        }, function (err) {
        });

    }
    $scope.GetProfile = function (ProfileUrl) {
        //------------------------------------button click event paricular  profile open in popup window------------------------------------
        window.open(ProfileUrl, '', 'scrollbars,resizable,screeny=top,width=940,height=640');
    }
    $scope.GetSurvey = function (SurveyUrl) {
        //------------------------------button click event paricular  survey open in popup window-----------------------------------------
        window.open(SurveyUrl, '', 'scrollbars,resizable,screeny=top,width=940,height=640',setTimeout(() => { GetSurveyList(); }, 5000));
    }
    //Do not sell my info
    $scope.doNotSellMyInfo = function () {
        $window.open('/login/wdns?lc=' + $scope.recaptchalangCode, 'Opinionetwork-DNS', 'width=800,height=800')
    }
    $('#spnDNS').click(function () {
        popupCenter('http://dev.affiliate.sdl.com/DoNotSellInfo.cshtml/', '', 940, 650);
    });

    $scope.spnContactUs = function () {
        loadPopupBox();
    }

    //$('#spnContactUs').click(function () {
    //    loadPopupBox();

    //});
    $('#spnFAQ').click(function () {
        popupCenter('https://precisionsample.zendesk.com/hc/en-us/articles/203609645-Frequently-Asked-Questions', '', 940, 650);
    });
    $('#popupBoxClose').click(function () {
        unloadPopupBox();
    });
    $("#txtEamilAddress").blur(function () {

        if (Vaildations()) {
            $('#btnSend').css('background-color', '#010100');
            $('#btnSend').css('cursor', 'pointer');
        }
        else {
            $('#btnSend').css('background-color', '#ddd');
        }
    });
    $("#txtComment").blur(function () {

        if (Vaildations()) {
            $('#btnSend').css('background-color', '#010100');
            $('#btnSend').css('cursor', 'pointer');
        }
        else {
            $('#btnSend').css('background-color', '#ddd');
        }
    });
    $('#btnSend').click(function () {

        if (Vaildations()) {
            SendEmail();
        }
        else {
            $('#btnSend').css('background-color', '#ddd');
        }
    });
    $(".dvSurveyHs").click(function () {
        var _ps1 = $("#dvSurveyHistoryPosition").position().top;
        var _ps2 = $("#dvRedeemHistoryPosition").position().top;
        var _ps3 = $("#dvCatalougesList").position().top;

        if (jQuery.browser.mobile) {
            var _ps4 = ((parseInt(_ps1)) + (parseInt(_ps3)));
            var _ps5 = ((parseInt(_ps4)) - (parseInt(_ps2)));
            var _ps6 = ((parseInt(_ps2)) - (parseInt(_ps5)));
            $('html,body').animate({ scrollTop: _ps6 });
        }
        else {
            var _ps4 = ((parseInt(_ps2)) + (parseInt(_ps3)));
            var _ps5 = ((parseInt(_ps4)) - (parseInt(_ps1)));
            var _ps6 = ((parseInt(_ps1)) - (parseInt(_ps5)));
            $('html,body').animate({ scrollTop: _ps6 });
        }
    });

    $('.dvRedeemHs').click(function () {
        $("html, body").animate({ scrollTop: $(document).height() }, "slow");
    });
    /*Memeber Sku Stored in Session*/
    function skustoredinsession(parent, event, id, rname) {
        //httpService.getData('/Reg/SetSKUSes?sku=' + encodeURIComponent(id)).then(function (response) {
        if (rname == 'd') {
            window.location.href = "/Reg/RedeemClick?img=" + encodeURIComponent(parent.ImageURL) + "&id=" + id + "&name=" + encodeURIComponent(parent.Description) + "&ug=" + $scope.ug + '&des=' + encodeURIComponent(parent.Description) + '&rn=' + encodeURIComponent(parent.Description) + '&cg=' + parent.CatalougeGuid + '&cid=' + cid;
        }
        else {
            var rewvalue = parseInt(event.RewardValue * 9876);
            window.location.href = "/Reg/RedeemClick?img=" + encodeURIComponent(parent.ImageURL) + "&id=" + id + '&name=' + encodeURIComponent(parent.Description) + "&ug=" + $scope.ug + '&des=' + encodeURIComponent(parent.Description) + '&name=' + parent.RewardName + '&cg=' + parent.CatalougeGuid + '&cid=' + cid;
        }
        //}, function (err) {
        //});
    }

}])


//redeemption controller
.controller('redeemController', ['$scope', '$window', 'translationsLoadingService', 'httpService', 'pagerService', 'getCurrentPageList',
function ($scope, $window, translationsLoadingService, httpService, pagerService, getCurrentPageList) {
    //button click even
    $scope.reddemclick = function () {
        $rootScope.redeemHistLst = !$rootScope.redeemHistLst;
        $rootScope.reward = !$rootScope.reward;
    };
    $scope.totalItems = 0; //total page item count
    $scope.redeemHistLst = [];//redeemption history list
    $scope.redmHistoryInfo = [];
    //catch the tab click
    $scope.$on("redeemHistoryClick", function (evt, data) {
        GetRedeemHistory();
    });
    // ----------------------------------------------Get Redeem history ---------------------------------------------------------------
    //$scope.rewardclick = function () {
    function GetRedeemHistory() {
        httpService.getData('/Reg/GetRedeemHistory?ug=' + $scope.ug + '&cid=' + cid).then(function (response) {
            $scope.redmHistoryInfo = response;
            if (response != null && response != "") {
                $scope.totalItems = response.LstRedeemptionHistory.length;
                if ($scope.totalItems > 0) {
                    $scope.pager = pagerService.getPager($scope.totalItems, 1);// set pager value and index 
                    //using service get current page list 
                    $scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords(response.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                }
                else {
                    $scope.redeemHistLst = response;
                }
            }

        }, function (err) {
        });

    }
    ////Pagging click event
    $scope.setPage = function (page) { // page  click count  
        if (page < 1 || page > $scope.pager.totalPages) {  //current page  count lessthan 1 or more than page count  then retu
            return;
        }
        // get pager object from service
        $scope.pager = pagerService.getPager($scope.totalItems, page, $scope.pager.startPage, $scope.pager.endPage); // set pager value and index 
        $scope.redeemHistLst = [];
        //using service get current page list 
        $scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords($scope.redmHistoryInfo.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);
    }
}])
