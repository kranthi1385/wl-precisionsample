
//main module of the soltion
angular.module("olpApp", ['pascalprecht.translate', 'customSerivces', 'vcRecaptcha'])
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
  //login controller section
   .controller("olpCtrl", ['$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', 'vcRecaptchaService', 'getQueryParams',
function ($scope, $http, $window, $location, $rootScope, $timeout, httpService, translationsLoadingService, vcRecaptchaService, getQueryParams) {
    //load current login json file
    translationsLoadingService.setCurrentUserLanguage("en");
    translationsLoadingService.loadTranslatePagePath("olp");
    $scope.step2 = "/PartialViews/ucl/step2.html";
    //get all avaliable ethnicities
    httpService.getData('/Common/GetEthnicityList').then(function (response) {
        $scope.ethnicityLst = response;
    }, function (err) {
        // $scope.errMsg = true;
    });

    httpService.getData('/Common/GetCountrysAndStates').then(function (response) {
        $scope.countries = response.CountryList;
        $rootScope.stateslist = response.StateList;
    }, function (err) {
        // $scope.errMsg = true;
    });

    var sleep = function (ms) {
        var dt = new Date();
        dt.settime((new Date()).getTime() + ms);
        while (new date().gettime() < (new Date()).getTime());
    }
    function Delay() {
        return oi_send();
        sleep(1000);
        return true;
    }
    // External member details
    //httpService.getData('/Rg/ExternalMemberByIdGet?extMemGuid=' + getQueryParams.getUrlVars()["mid"]).then(function (response) {
    //    alert(response);

    //}, function (err) {
    //});
    if (getQueryParams.getUrlVars()["st"] == 's') {
        $scope.msg = "Success!";
        $scope.sdlmsg = "Thank you for completing this survey.  Please complete the form below to join our research panel and claim your $5.00 reward.  In addition to future surveys about your vehicle you’ll also be invited to participate in additional surveys on products and services you use every day paying up to $30 each!";
        if (getQueryParams.getUrlVars()["rid"] == 368 || getQueryParams.getUrlVars()["rid"] == 369) {
            // Get tracking List
        }
    }
    else {
        $scope.msg = "";
        $scope.sdlmsg = "";
    }
    $scope.submit = function (valid) {
        if (valid) {
            $scope.showLoginError = false;
            $scope.showStep1ErrMsg = false;
            if ($scope.user.EmailAddress != "") {
                httpService.getData('/Home/vldEmail?email=' + $scope.user.EmailAddress).then(function (response) {
                    if (response == "accepted") {
                        httpService.postData('/Rg/RewardAndUserInsert', $scope.user).then(function (response) {
                            alert('saved');
                        }, function (err) {
                        });
                    }
                    else {
                        $scope.emailXVerify = false;
                        $scope.xVerifyMessage = "Email Address not exist."
                    }
                }, function (err) {
                });
            }
        }
        else {
            $scope.showStep1ErrMsg = true;
        }
    }
    //validate Password
    $scope.validatePswd = function () {
        if ($scope.user.Password == $scope.user.Cpassword) {
            $scope.xVerifyPassword = ""
        }
        else {
            $scope.xVerifyPassword = "Password not match"
        }
    }
    //validate email through xverify
    $scope.validateEmail = function () {
        $scope.showStep1ErrMsg = false;
        $scope.emailXVerify = -1;
        $scope.xVerifyMessage = '';
        if ($scope.user.EmailAddress != "") {
            httpService.getData('/Home/vldEmail?email=' + $scope.user.EmailAddress).then(function (response) {
                //var message = xVerify();
                if (response == "accepted") {
                    $scope.emailXVerify = 1;
                    $scope.xVerifyMessage = "Verified."
                }
                else {
                    $scope.emailXVerify = 0;
                    $scope.xVerifyMessage = "Email Address not exist."
                }
            }, function (err) {
            });
        }

    }

    ////get all userdata
    //httpService.getData('/Home/GetUserData').then(function (response) {
    //    $scope.user = response;
    //}, function (err) {
    //    //  $scope.errMsg = true;
    //});
    //get all avaliables states
    $scope.countryByStates = function (cc) {
        $scope.states = [];
        for (var i = 0; i < $rootScope.stateslist.length; i++) {
            if (cc == $rootScope.stateslist[i].CountryId) {
                $scope.states.push($rootScope.stateslist[i]);

            }
        }
        validateOpt();
    }


}])
