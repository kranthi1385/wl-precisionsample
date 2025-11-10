define(['app'], function (app) {
    app.register.controller('rewardController', ['$rootScope', '$scope', '$window', 'translationsLoadingService', 'httpService', '$cookies', 'pagerService', 'getCurrentPageList',
    function ($rootScope, $scope, $window, translationsLoadingService, httpService, $cookies, pagerService, getCurrentPageList) {
        translationsLoadingService.loadTranslatePagePath("mr");


        $scope.historyclick = function (buttonId) {
            if (buttonId == 1) {
                $rootScope.reward = true;
                $rootScope.redeemHistLstgrid = false;
            }
            else {
                $rootScope.reward = false;
                $rootScope.redeemHistLstgrid = true;
            }

        };

        $scope.historyclick(2); //setting defualt grid rewards reedemed history grid
        $scope.Rewards = [];
        $scope.Reward = [];
        $scope.totalItems = 0; //total page item count
        $scope.pager = {}; // pagging array
        $scope.tabIndex = 0;//default tab index

        //all avaliable tango cards list
        $scope.availableCards = [
        {
            cardName: 'Retail Gift Card',
            templateName: 'RetailGiftCard'
        },
        {
            cardName: 'Restaurant Gift Card ',
            templateName: 'RestaurantGiftCard'
        },
        {
            cardName: 'Online Services ',
            templateName: 'OnlineServices'
        },
        {
            cardName: 'Charitable Donation ',
            templateName: 'CharitableDonation'
        }]
        $scope.getCurrentDomailDetails = function () {
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
                if ($scope.orgDetails.ClientId == 73 || $scope.orgDetails.ClientId == 111 || $scope.orgDetails.ClientId == 385) {
                    $scope.showRewards = false;
                }
                else {
                    $scope.showRewards = true;
                }
            }, function (err) {
            });
        }
        $scope.getCurrentDomailDetails();
        //to get user details
        httpService.getData('/hm/GetUserDetails').then(function (response) {
            $scope.user = response;
            $scope.rewHistoryLst = [];
            $scope.userRewardsInfo = [];
            //------------ Get member all avaliable reward history --------------------------------//
            httpService.postData('/mr/GetRewardsHistory', "").then(function (response) {
                $scope.userRewardsInfo = response;
                if (response == '') {
                    $scope.userRewardsInfo.TotalEarnings = 0;
                    $scope.userRewardsInfo.TotalRedemptions = 0;
                    $scope.userRewardsInfo.AccountBalance = 0;
                }
                if (response.LstRewardHistory != '' & response.LstRewardHistory != null) {
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
                var accBalnce = 0;
                if ($scope.userRewardsInfo.AccountBalance == undefined || $scope.userRewardsInfo.AccountBalance == '') {
                    accBalnce = 0;
                }
                else {
                    accBalnce = $scope.userRewardsInfo.AccountBalance;
                }
                //Get all tango rewards list
                httpService.postData('/mr/GetRewardData?UserId=' + $scope.user.UserId + "&UserGuid=" + $scope.user.UserGuid + "&ClientId=" + $scope.user.OrgId + '&AccBalance=' + accBalnce, "").then(function (data) {

                    $scope.rewData = data;
                    //$scope.lstRetailGiftCard = $scope.rewData.filter((x) => x.Category == "Retail Gift Card");
                    //$scope.lstRestaurantGiftCard = $scope.rewData.filter((x) => x.Category == "Restaurant Gift Card");
                    //$scope.lstOnlineServices = $scope.rewData.filter((x) => x.Category == "Online Services");
                    //$scope.lstCharitableDonation = $scope.rewData.filter((x) => x.Category == "Charitable Donation")
                    $scope.lstRetailGiftCard = getRewardsBycatagories($scope.rewData, 'Retail Gift Card');
                    $scope.lstRestaurantGiftCard = getRewardsBycatagories($scope.rewData, 'Restaurant Gift Card');
                    $scope.lstOnlineServices = getRewardsBycatagories($scope.rewData, 'Online Services');
                    $scope.lstCharitableDonation = getRewardsBycatagories($scope.rewData, 'Charitable Donation');

                })
            }, function (err) {
                $scope.errMsg = true;
            });
        }, function (err) {
            $scope.errMsg = true;
        })


        function getRewardsBycatagories(rewards, type) {
            var lstRewards = [];
            angular.forEach(rewards, function (rew) {
                if (rew.Category == type) {
                    lstRewards.push(rew);
                }
            })
            return lstRewards;
        }
        //reward redeem click
        $scope.RewardClick = function (parent, obj, $event) {
            var _id = $event.currentTarget.id;
            $scope.RewardValue = _id.split(';')[1];
            var d = new Date();
            var year = d.getFullYear();
            $scope.currentYear = year;
            $scope.nextYear = year + 1;
            var now = new $window.Date(),
          exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
            $cookies.put('RewardValue', $scope.RewardValue, {
                expires: exp,
                path: '/'
            });
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
        //Pagging click event
        $scope.setPage = function (page) { // page  click count  
            if (page < 1 || page > $scope.pager.totalPages) {  //current page  count lessthan 1 or more than page count  then retu
                return;
            }
            // get pager object from service
            $scope.pager = pagerService.getPager($scope.totalItems, page, $scope.pager.startPage, $scope.pager.endPage); // set pager value and index 
            $scope.rewHistoryLst = [];
            //using service get current page list 
            $scope.rewHistoryLst = getCurrentPageList.getCurrentPageRecords($scope.userRewardsInfo.LstRewardHistory, $scope.pager.startIndex, $scope.pager.endIndex);
        }
        /*Memeber Sku Stored in Session*/
        function skustoredinsession(parent, event, id, rname) {
            //httpService.getData('/mr/SetSKUSes?sku=' + encodeURIComponent(id)).then(function (response) {
            if (rname == 'd') {
                var rewvalue = parseInt(event.RewardValue * 9876);
                window.location.href = "/mr/rc?img=" + encodeURIComponent(parent.ImageURL) + "&id=" + id + "&name=" + encodeURIComponent(parent.Description) + '&des=' + encodeURIComponent(parent.Description) + '&rn=' + encodeURIComponent(parent.Description) + '&cg=' + parent.CatalougeGuid + '&cid=' + $scope.user.OrgId;
            }
            else {
                var rewvalue = parseInt(event.RewardValue * 9876);
                window.location.href = "/mr/rc?img=" + encodeURIComponent(parent.ImageURL) + "&id=" + id + '&name=' + encodeURIComponent(parent.Description) + '&des=' + encodeURIComponent(parent.Description) + '&cg=' + parent.CatalougeGuid + '&cid=' + $scope.user.OrgId;
            }
            //}, function (err) {
            //});
        }

    }]);



    //redeemption controller
    app.register.controller('redeemController', ['$scope', '$window', 'translationsLoadingService', 'httpService', 'pagerService', 'getCurrentPageList',
    function ($scope, $window, translationsLoadingService, httpService, pagerService, getCurrentPageList) {
        translationsLoadingService.loadTranslatePagePath("mr");
        $scope.totalItems = 0; //total page item count
        $scope.redeemHistLst = [];//redeemption history list
        $scope.redmHistoryInfo = [];
        //button click even
        $scope.reddemclick = function () {
            $rootScope.redeemHistLst = !$rootScope.redeemHistLst;
            $rootScope.reward = !$rootScope.reward;
        };
        httpService.postData('/mr/GetRedeemHistory', "").then(function (response) {
            $scope.redmHistoryInfo = response;
            if (response.LstRedeemptionHistory != '' & response.LstRedeemptionHistory != null) {
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

        //Pagging click event
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
    }]);
});