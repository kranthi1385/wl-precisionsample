define(['app'], function (app) {
    app.register.controller('surveyPController', ['$rootScope', '$scope', '$window', 'translationsLoadingService', 'httpService', 'pagerService', 'getCurrentPageList',
    function ($rootScope, $scope, $window, translationsLoadingService, httpService, pagerService, getCurrentPageList) {
        translationsLoadingService.loadTranslatePagePath("mr");
        $scope.showSurveyHistory = false;
        //add header for each  request to identify AntiForgeryToken
        $scope.getCurrentDomailDetails = function () {
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
        }
        $scope.getCurrentDomailDetails();
        //to get user details
        httpService.getData('/hm/GetUserDetails').then(function (response) {
            $scope.user = response;
            GetSurveyHistory();
        })
        //Accordian tabs click event
        $scope.tabClick = function (index) {
            $scope.tabIndex = index;

        }
        // ----------------------------------------------Get survey history ---------------------------------------------------------------


        function GetSurveyHistory() {
            $scope.suvHisLst = [];
            httpService.getData('/Reg/GetSurveyHistory?ug=' + $scope.user.UserGuid + '&cid=' + $scope.user.OrgId).then(function (data) {
                $scope.surveyHistory = data;
                if (data == "") {
                    $scope.showSurveyHistory = true;
                }
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
    }
    ])
})