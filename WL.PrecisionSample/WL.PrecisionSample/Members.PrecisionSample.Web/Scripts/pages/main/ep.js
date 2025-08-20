define(['app'], function (app) {
    app.register.controller('accountController', ['$rootScope', '$scope', 'httpService', 'translationsLoadingService',
    function ($rootScope, $scope, httpService, translationsLoadingService) {
        translationsLoadingService.loadTranslatePagePath("ep");
        $scope.submitMsg = false;
        //beind years
        var d = new Date();
        var year = d.getFullYear();
        var yearLst = [];
        for (var i = 13; i < 100; i++) {
            yearLst.push({ key: year - i, value: year - i });
        }
        $scope.year = yearLst;
        $scope.stateslist = [];
        //get all avaliable ethnicities
        httpService.getData('/Common/GetEthnicityList').then(function (response) {
            $scope.ethnicityLst = response;
        }, function (err) {
            // $scope.errMsg = true;
        });

        $scope.countryByStates = function (cc) {
            $scope.states = [];
            for (var i = 0; i < $scope.stateslist.length; i++) {
                if (cc == $scope.stateslist[i].CountryId) {
                    $scope.states.push($scope.stateslist[i]);

                }
            }
        }
        //get all states avalibles
        $scope.getUserDetails = function () {
            httpService.getData('/Ep/GetUserData').then(function (response) {
                $scope.user = response;
                httpService.getData('/Common/GetCountrysAndStates').then(function (res) {
                    $scope.countries = res.CountryList;
                    $scope.stateslist = res.StateList;
                    if ($scope.user.CountryId != "") {
                        $scope.countryByStates($scope.user.CountryId);
                    }
                }, function (err) {
                    // $scope.errMsg = true;
                });

            }, function (err) {
                $scope.errMsg = true;
            });
        }
        $scope.getUserDetails();
        $scope.submit = function (user) {
            httpService.postData('/Ep/saveUser', user).then(function (response) {
                $scope.result = response;
                if (response != "") {
                    $scope.submitMsg = true;
                    $scope.cancelMsg = false;
                    $scope.errMsg = false;
                }
            }, function (err) {
                $scope.errMsg = true;
            });
        }
        $scope.cancel = function () {
            $scope.getUserDetails();
            httpService.postData('/Ep/DeleteUserData?SubId3=' + $scope.user.SubId3).then(function (response) {
                $scope.result = response;
                if (response == "") {
                    $scope.cancelMsg = true;
                    $scope.submitMsg = false;
                    $scope.errMsg = false;
                }
            }, function (err) {
                $scope.errMsg = true;
            });
        }
    }]);
});