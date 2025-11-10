define(['rgApp'], function (app) {
    app.register.controller("step1Controller", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService',
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

    //get all userdata
    httpService.getData('/Home/GetUserData').then(function (response) {
        $scope.user = response;
    }, function (err) {
        //  $scope.errMsg = true;
    });

    //optintelligence check
    $scope.optIntelligenceCheck = function () {
        validateOpt();
    }

    //user save
    $scope.btnGo_Click = function () {
        if ($("#hfShow").val() == '0') {
            checkflag();
            Delay();
            $timeout(function () { saveUser() }, 3000);
        }
        else {
            saveUser()
        }
    }

    function Delay() {
        debugger;
        return oi_send();
        // sleep(1000);
        return true;
    }

    $scope.submit = function (user) {
        httpService.getData('/Rg/Step1UserDataInsert', user).then(function (response) {
            $scope.user = response;
        }, function (err) {
            //  $scope.errMsg = true;
        });
    }

    function saveUser() {

    }
}])
});