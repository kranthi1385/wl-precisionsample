define(['rgApp'], function (app) {
    app.register.controller("step2Controller", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService',
function ($scope, $http, $window, $location, $rootScope, $timeout, httpService, translationsLoadingService) {
    //load current login json file
    translationsLoadingService.loadTranslatePagePath("step2");
    //set optIntelligence value
    function checkflag() {
        if ($("#hfShow").val() == 0) {
            setvalue(1);
        }
        function setvalue(value) {
            $('input[name=OI_button]').val(value);
            document.getElementById('hfvalue').value = value;
            return true;
        }
    }

    //load optintelligence offers
    function validateOpt() {
        if ($scope.user.CountryCode != "" && $scope.user.Gender != null && $scope.user.Day != null && $scope.user.Month != null && $scope.user.Year != null) {
            oi().bind_event();
            document.getElementById("divShow").style.display = 'block';
        }
        else {
            document.getElementById("divShow").style.display = 'none';
        }
    }
    //beind years
    var d = new Date();
    var year = d.getFullYear();
    var yearLst = [];
    for (var i = 13; i < 100; i++) {
        yearLst.push({ key: year - i, value: year - i });

    }
    $scope.year = yearLst;
    //get all avaliable ethnicities
    debugger;
    httpService.getData('/Common/GetEthnicityList').then(function (response) {
        $scope.ethnicityLst = response;
    }, function (err) {
        // $scope.errMsg = true;
    });
    //get all userdata
    httpService.getData('/Home/GetUserData').then(function (response) {
        $scope.user = response;
    }, function (err) {
        //  $scope.errMsg = true;
    });

    //get all avaliables states
    debugger;
    $scope.countryByStates = function (CountryCode) {
        httpService.getData('/Common/GetStates?Cid=' + CountryCode).then(function (response) {
            $scope.states = response;
        }, function (err) {
            // $scope.errMsg = true;
        });
        validateOpt();
    }
    //optintelligence check
    $scope.optIntelligenceCheck = function () {
        validateOpt();
    }

    $scope.submit = function (user) {
        httpService.postData('/Rg/saveUser', user).then(function (response) {
            $scope.result = response;
        }, function (err) {
            //  $scope.errMsg = true;
        });
    }

    //user save
    //$scope.Submit = function () {
    //    if ($("#hfShow").val() == '0') {
    //        checkflag();
    //        Delay();
    //        $timeout(function () { saveUser() }, 3000);
    //    }
    //    else {
    //        saveUser()
    //    }
    //}

    function Delay() {
        //return oi_send();
        // sleep(1000);
        return true;
    }

    //function sleep(ms) {
    //    var dt = new date();
    //    dt.settime(dt.gettime() + ms);
    //    while (new date().gettime() < dt.gettime());
    //}

    
}])
});