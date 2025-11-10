define(['app'], function (app) {
    app.register.controller('downlineController', ['$rootScope', '$scope', '$window', 'translationsLoadingService', 'httpService', 'pagerService', 'getCurrentPageList',
        function ($rootScope, $scope, $window, translationsLoadingService, httpService, pagerService, getCurrentPageList) {
            //Language translations
            translationsLoadingService.loadTranslatePagePath("rf");
            $scope.network = true;//for expand and collapse friend list
            $scope.downlineLst = [];
            $scope.downline = [];
            $scope.totalItems = 0; //total page item count
            $scope.pager = {}; // pagging array
            $scope.tabIndex = 0;//default tab index
            /*-------------- Get members friend information ------------------ */
            httpService.getData('/Rf/FriendInformation', "").then(function (response) {
                debugger;
                $scope.downlineLst = [];
                $scope.downline = response;
                $scope.totalItems = response.length;
                if ($scope.totalItems > 0) {
                    $scope.pager = pagerService.getPager($scope.totalItems, 1);// set pager value and index 
                    //using service get current page list 
                    $scope.downlineLst = getCurrentPageList.getCurrentPageRecords($scope.downline, $scope.pager.startIndex, $scope.pager.endIndex);
                }
                else {
                    $scope.downlineLst = response;
                }
            }, function (err) {
                $scope.errMsg = true;
            })

            //Pagging click event
            $scope.setPage = function (page) { // page  click count  
                if (page < 1 || page > $scope.pager.totalPages) {  //current page  count lessthan 1 or more than page count  then retu
                    return;
                }
                // get pager object from service
                $scope.pager = pagerService.getPager($scope.totalItems, page, $scope.pager.startPage, $scope.pager.endPage); // set pager value and index 
                $scope.downlineLst = [];
                //using service get current page list 
                $scope.downlineLst = getCurrentPageList.getCurrentPageRecords($scope.downline, $scope.pager.startIndex, $scope.pager.endIndex);
            }

            //get user details
            httpService.getData('/hm/GetUserDetails', "").then(function (response) {
                $scope.user = response;
            }, function (err) {
            });
            //get current domain details
            httpService.getData('/Common/GetCurrentDomainDetails').then(function (res) {
                $scope.orgDetails = res;
            }, function (err) {
            });
            //get friends invited list
            httpService.getData('/Rf/FriendList', "").then(function (response) {
                $scope.friendsList = response;
            }, function (err) {
                $scope.errMsg = true;
            })
            //expand and collapse friend list
            $scope.expand = function () {
                console.log("network")
            }
            //opens new window to login to facebook
            $scope.openWindow = function () {
                $window.open('http://www.facebook.com', 'Facebook', 'width=800,height=600');
            }
            //opens new window to login to twitter
            $scope.windowOpen = function () {
                $window.open('http://www.twitter.com', 'Twitter', 'width=800,height=600');
            }
            $scope.surveyDownline = function () {
                window.location.href = $scope.orgDetails.MemberUrl + '/e/s?fid=' + $scope.user.UserId;
            }
        }]);
});