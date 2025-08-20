//main module of the soltion
angular.module("unsubApp", ['pascalprecht.translate', 'customSerivces'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider',
   function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider) {
       $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
       //load all translations setting to main transtion loader.
       $translateProvider.useLoader('$translatePartialLoader', {
           urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
       });
       //add default language
       $translateProvider.preferredLanguage('en');

       //regster providers

   }])
    //terms controller
.controller('unsubController', ['$rootScope', '$scope', 'httpService', 'getQueryParams',
    function ($rootScope, $scope, httpService, getQueryParams) {
        $scope.isRegErrMsg = false;
        // to get URL params
        var ug = getQueryParams.getUrlVars()['ug'];
        if (ug == undefined) {
            ug = '';
        }

        var cid = getQueryParams.getUrlVars()['cid'];
        if (cid == undefined || cid == null || cid == '') {
            cid = 0;
        }
        var em = getQueryParams.getUrlVars()["em"];
        if (em == undefined) {
            em = '';
        }
        $scope.showMsg = false;
        //translationsLoadingService.loadTranslatePagePath("ep");
        if (em != "") {
            httpService.getData('/Home/UnsubUserByEmail?em=' + em).then(function (res) {
                $scope.showMsg = true;
            })
        }
        else {
            if (ug != "") {
                httpService.getData('/Home/GetUserEmail?ug=' + ug).then(function (response) {
                    $scope.user = response;
                    if ($scope.user.EmailAddress != "" && $scope.user.EmailAddress != null) {
                        httpService.postData('/Home/UserEmailDncInsert?EmailAddress=' + $scope.user.EmailAddress + '&cid=' + $scope.user.OrgId + '&refId=' + $scope.user.RefferId).then(function (response) {
                            $scope.userDetails = response;
                            if (ug == "") {
                                $scope.showMsg = true;
                            }
                        }, function (err) {
                            $scope.errMsg = true;
                        });
                        httpService.getData('/Home/UnsubUserDnc?ug=' + ug + '&cid=' + cid).then(function (res) {
                            $scope.Unsub = res;
                            if (ug != "") {
                                $scope.showMsg = true;
                            }
                        }, function (err) {
                            $scope.errMsg = true;
                        });
                    }
                }, function (err) {
                    $scope.errMsg = true;
                });
            }
        }

        $scope.userDetails = {
            EmailAddress: ''
        }
        $scope.unsubClick = function (valid) {
            if (valid) {
                if ($scope.userDetails.EmailAddress != "") {
                    httpService.postData('/Home/unsubUser?EmailAddress=' + $scope.userDetails.EmailAddress).then(function (response) {
                        $scope.userDetails = response;
                        if (response == "accepted") {
                            $scope.showMsg = true;
                        }
                    }, function (err) {
                        $scope.errMsg = true;
                    });
                }
                else {
                    $scope.isRegErrMsg = true;
                }
            }
            else {
                $scope.isRegErrMsg = true;
            }
        }
        //httpService.getData('/Home/GetUserEmail?ug=' + ug).then(function (response) {
        //    $scope.user = response;
        //}, function (err) {
        //    $scope.errMsg = true;
        //});
        //$scope.unsubClick = function (valid) {
        //    if (valid) {
        //        httpService.postData('/Home/unsubUser', $scope.user).then(function (response1) {
        //            $scope.result = response1;
        //        }, function (err) {
        //        });
        //    }
        //    else {
        //        $scope.isRegErrMsg = true;
        //    }
        //}
    }])