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
        httpService.getData('/ep/GetUserData').then(function (response) {
            $scope.user = response;
            $scope.countryId = $scope.user.CountryId;
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
                if ($scope.countryId == 572 || $scope.countryId == 494 || $scope.countryId == 497 || $scope.countryId == 502 || $scope.countryId == 503 || $scope.countryId == 505 || $scope.countryId == 506 || $scope.countryId == 511 || $scope.countryId == 512 || $scope.countryId == 1171 || $scope.countryId == 519 || $scope.countryId == 522 || $scope.countryId == 525 || $scope.countryId == 529 || $scope.countryId == 38 || $scope.countryId == 534 || $scope.countryId == 538 || $scope.countryId == 543 || $scope.countryId == 545 || $scope.countryId == 546 || $scope.countryId == 548 || $scope.countryId == 549 || $scope.countryId == 549 || $scope.countryId == 553 || $scope.countryId == 559 || $scope.countryId == 564 || $scope.countryId == 565 || $scope.countryId == 572 || $scope.countryId == 571 || $scope.countryId == 575 ||
                                                   $scope.countryId == 576 || $scope.countryId == 588 || $scope.countryId == 590 || $scope.countryId == 591 || $scope.countryId == 593 || $scope.countryId == 599 || $scope.countryId == 604 || $scope.countryId == 613 || $scope.countryId == 614 || $scope.countryId == 619 || $scope.countryId == 620 || $scope.countryId == 1180 || $scope.countryId == 1166 || $scope.countryId == 638 || $scope.countryId == 641 || $scope.countryId == 647 || $scope.countryId == 657 || $scope.countryId == 662 || $scope.countryId == 664 || $scope.countryId == 665 || $scope.countryId == 668 || $scope.countryId == 669 || $scope.countryId == 673 || $scope.countryId == 460 || $scope.countryId == 688 || $scope.countryId == 693 || $scope.countryId == 694 || $scope.countryId == 697 || $scope.countryId == 700 ||
                                                   $scope.countryId == 706 || $scope.countryId == 718 || $scope.countryId == 719 || $scope.countryId == 724 || $scope.countryId == 727 || $scope.countryId == 728 || $scope.countryId == 1164 || $scope.countryId == 731) {
                    $scope.userRewardsInfo.AccountBalance = (($scope.userRewardsInfo.AccountBalance + "").replace(".", ","));
                    $scope.userRewardsInfo.TotalEarnings = (($scope.userRewardsInfo.TotalEarnings + "").replace(".", ","));
                    $scope.userRewardsInfo.TotalRedemptions = (($scope.userRewardsInfo.TotalRedemptions + "").replace(".", ","));
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
                    accBalnce = ($scope.userRewardsInfo.AccountBalance + "").replace(",", ".");
                }
                //Get all tango rewards list
                httpService.postData('/mr/GetRewardData?UserId=' + $scope.user.UserId + "&UserGuid=" + $scope.user.UserGuid + "&ClientId=" + $scope.user.OrgId + '&AccBalance=' + accBalnce, "").then(function (data) {

                    $scope.rewData = data;
                    //$scope.lstRetailGiftCard = $scope.rewData.filter((x) => x.Category == "Retail Gift Card");
                    //$scope.lstRestaurantGiftCard = $scope.rewData.filter((x) => x.Category == "Restaurant Gift Card");
                    //$scope.lstOnlineServices = $scope.rewData.filter((x) => x.Category == "Online Services");
                    //$scope.lstCharitableDonation = $scope.rewData.filter((x) => x.Category == "Charitable Donation")
                    if (getRewardsBycatagories($scope.rewData, 'Retail Gift Card').length > 0) {
                        $scope.lstRetailGiftCard = getRewardsBycatagories($scope.rewData, 'Retail Gift Card');
                    }
                    else {
                        $scope.availableCards[0].cardName = '';
                    }
                    if (getRewardsBycatagories($scope.rewData, 'Restaurant Gift Card').length > 0) {
                        $scope.lstRestaurantGiftCard = getRewardsBycatagories($scope.rewData, 'Restaurant Gift Card');
                    }
                    else {
                        $scope.availableCards[1].cardName = '';
                    }
                    if (getRewardsBycatagories($scope.rewData, 'Online Services').length > 0) {
                        $scope.lstOnlineServices = getRewardsBycatagories($scope.rewData, 'Online Services');
                    }
                    else {
                        $scope.availableCards[2].cardName = '';
                    }
                    if (getRewardsBycatagories($scope.rewData, 'Charitable Donation').length > 0) {
                        $scope.lstCharitableDonation = getRewardsBycatagories($scope.rewData, 'Charitable Donation');
                    }
                    else {
                        $scope.availableCards[3].cardName = '';
                    }


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
                window.location.href = "/mr/rc?img=" + encodeURIComponent(parent.ImageURL) + "&id=" + id + "&name=" + encodeURIComponent(parent.Description) + '&des=' + encodeURIComponent(parent.Description) + '&rn=' + encodeURIComponent(parent.Description) + '&cg=' + parent.CatalougeGuid + '&cid=' + $scope.user.OrgId + '&countryId=' + $scope.user.CountryId;
            }
            else {
                var rewvalue = parseInt(event.RewardValue * 9876);
                window.location.href = "/mr/rc?img=" + encodeURIComponent(parent.ImageURL) + "&id=" + id + '&name=' + encodeURIComponent(parent.Description) + '&des=' + encodeURIComponent(parent.Description) + '&cg=' + parent.CatalougeGuid + '&cid=' + $scope.user.OrgId + '&countryId=' + $scope.user.CountryId;
            }
            //}, function (err) {
            //});
        }

    }]);


    //var redeemController = angular.module("redeemController",['ngSanitize'])
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
            $scope.htmlString = response.RedeemptionInstructions[0];
            if (response.LstRedeemptionHistory != '' & response.LstRedeemptionHistory != null) {
                $scope.totalItems = response.LstRedeemptionHistory.length;
                if ($scope.totalItems > 0) {
                    $scope.pager = pagerService.getPager($scope.totalItems, 1);// set pager value and index 
                    //using service get current page list 
                    //$scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords(response.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                    if ($scope.countryId == 572 || $scope.countryId == 494 || $scope.countryId == 497 || $scope.countryId == 502 || $scope.countryId == 503 || $scope.countryId == 505 || $scope.countryId == 506 || $scope.countryId == 511 || $scope.countryId == 512 || $scope.countryId == 1171 || $scope.countryId == 519 || $scope.countryId == 522 || $scope.countryId == 525 || $scope.countryId == 529 || $scope.countryId == 38 || $scope.countryId == 534 || $scope.countryId == 538 || $scope.countryId == 543 || $scope.countryId == 545 || $scope.countryId == 546 || $scope.countryId == 548 || $scope.countryId == 549 || $scope.countryId == 549 || $scope.countryId == 553 || $scope.countryId == 559 || $scope.countryId == 564 || $scope.countryId == 565 || $scope.countryId == 572 || $scope.countryId == 571 || $scope.countryId == 575 ||
                                                                     $scope.countryId == 576 || $scope.countryId == 588 || $scope.countryId == 590 || $scope.countryId == 591 || $scope.countryId == 593 || $scope.countryId == 599 || $scope.countryId == 604 || $scope.countryId == 613 || $scope.countryId == 614 || $scope.countryId == 619 || $scope.countryId == 620 || $scope.countryId == 1180 || $scope.countryId == 1166 || $scope.countryId == 638 || $scope.countryId == 641 || $scope.countryId == 647 || $scope.countryId == 657 || $scope.countryId == 662 || $scope.countryId == 664 || $scope.countryId == 665 || $scope.countryId == 668 || $scope.countryId == 669 || $scope.countryId == 673 || $scope.countryId == 460 || $scope.countryId == 688 || $scope.countryId == 693 || $scope.countryId == 694 || $scope.countryId == 697 || $scope.countryId == 700 ||
                                                                     $scope.countryId == 706 || $scope.countryId == 718 || $scope.countryId == 719 || $scope.countryId == 724 || $scope.countryId == 727 || $scope.countryId == 728 || $scope.countryId == 1164 || $scope.countryId == 731) {
                        $scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords($scope.redmHistoryInfo.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                        angular.forEach($scope.redeemHistLst, function (res, key) {
                            res.RedemptionAmount = ((res.RedemptionAmount + "").replace(".", ","));
                        });

                    } else {
                        $scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords($scope.redmHistoryInfo.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                    }
                }
                else {
                    $scope.redeemHistLst = response;
                }
            }
        }, function (err) {
        });

        $scope.showpopup = function (id) {
            httpService.postData('/mr/GetApiResponsebyId?id=' + id).then(function (response) {
                $scope.apiResponse = response;
            })
            $('#modal_popup').modal({ backdrop: 'static' });
        }

        $scope.notcancel = function () {
            $('#modal_popup').modal('hide');
        }

        //Pagging click event
        $scope.setPage = function (page) { // page  click count  
            if (page < 1 || page > $scope.pager.totalPages) {  //current page  count lessthan 1 or more than page count  then retu
                return;
            }
            // get pager object from service
            $scope.pager = pagerService.getPager($scope.totalItems, page, $scope.pager.startPage, $scope.pager.endPage); // set pager value and index 
            $scope.redeemHistLst = [];
            //using service get current page list 
            if ($scope.countryId == 572 || $scope.countryId == 494 || $scope.countryId == 497 || $scope.countryId == 502 || $scope.countryId == 503 || $scope.countryId == 505 || $scope.countryId == 506 || $scope.countryId == 511 || $scope.countryId == 512 || $scope.countryId == 1171 || $scope.countryId == 519 || $scope.countryId == 522 || $scope.countryId == 525 || $scope.countryId == 529 || $scope.countryId == 38 || $scope.countryId == 534 || $scope.countryId == 538 || $scope.countryId == 543 || $scope.countryId == 545 || $scope.countryId == 546 || $scope.countryId == 548 || $scope.countryId == 549 || $scope.countryId == 549 || $scope.countryId == 553 || $scope.countryId == 559 || $scope.countryId == 564 || $scope.countryId == 565 || $scope.countryId == 572 || $scope.countryId == 571 || $scope.countryId == 575 ||
                                                                     $scope.countryId == 576 || $scope.countryId == 588 || $scope.countryId == 590 || $scope.countryId == 591 || $scope.countryId == 593 || $scope.countryId == 599 || $scope.countryId == 604 || $scope.countryId == 613 || $scope.countryId == 614 || $scope.countryId == 619 || $scope.countryId == 620 || $scope.countryId == 1180 || $scope.countryId == 1166 || $scope.countryId == 638 || $scope.countryId == 641 || $scope.countryId == 647 || $scope.countryId == 657 || $scope.countryId == 662 || $scope.countryId == 664 || $scope.countryId == 665 || $scope.countryId == 668 || $scope.countryId == 669 || $scope.countryId == 673 || $scope.countryId == 460 || $scope.countryId == 688 || $scope.countryId == 693 || $scope.countryId == 694 || $scope.countryId == 697 || $scope.countryId == 700 ||
                                                                     $scope.countryId == 706 || $scope.countryId == 718 || $scope.countryId == 719 || $scope.countryId == 724 || $scope.countryId == 727 || $scope.countryId == 728 || $scope.countryId == 1164 || $scope.countryId == 731) {
                $scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords($scope.redmHistoryInfo.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                angular.forEach($scope.redeemHistLst, function (res, key) {
                    res.RedemptionAmount = ((res.RedemptionAmount + "").replace(".", ","));
                });
            } else {
                $scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords($scope.redmHistoryInfo.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);
            }

            //$scope.redeemHistLst = getCurrentPageList.getCurrentPageRecords($scope.redmHistoryInfo.LstRedeemptionHistory, $scope.pager.startIndex, $scope.pager.endIndex);

        }
    }]);
});