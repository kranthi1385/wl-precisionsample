//main module of the soltion
angular.module("lbMsgApp", ['pascalprecht.translate', 'customSerivces'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider',
   function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider) {

       ////load all translations setting to main transtion loader.
       // $translateProvider.useLoader('$translatePartialLoader', {
       // urlTemplate: '/Scripts/i18n/{lang}/{part}.json'
       // });
       ////add default language
       //$translateProvider.preferredLanguage(cc);
       //regster providers

   }])
 .controller('lbmsgController', ['$rootScope', '$scope', '$http', '$window', 'httpService', 'getQueryParams', 'translationsLoadingService',
function ($rootScope, $scope, $http, $window, httpService, getQueryParams, translationsLoadingService) {
   
    $scope.ImageLoading = false;
    $scope.pc = getQueryParams.getUrlVars()["pc"];
    $scope.ug = getQueryParams.getUrlVars()["ug"];
    $scope.into = getQueryParams.getUrlVars()["into"];
    var sid = getQueryParams.getUrlVars()["sid"] //user status guid added 06/13/2016
    if (sid == undefined) {
        sid = '';
    }
    $scope.pid = getQueryParams.getUrlVars()["prjid"];
    if ($scope.pid == undefined) {
        $scope.pid = '';
    }
    $scope.rid = getQueryParams.getUrlVars()["rid"];
    if ($scope.rid == undefined) {
        $scope.rid = '';
    }
    if ($scope.ug == undefined) {
        $scope.ug = '';
    }
    if ($scope.into == undefined) {
        $scope.into = '';
    }
    if ($scope.pc == undefined) {
        $scope.pc = '';
    }
    else {
        $scope.pc = $scope.pc.replace($scope.ug, "");
    }

    $scope.extid = getQueryParams.getUrlVars()["extid"];
    $scope.txid = getQueryParams.getUrlVars()["txid"]
    if ($scope.extid == undefined) {
        $scope.extid = '';
    }
    if ($scope.extid == '') {
        $scope.extid = $scope.txid;
    }

    if ($scope.pc != '' && $scope.pc != undefined && $scope.pc != 'na' && $scope.ug != '' && $scope.ug != undefined) {
        var pc = $scope.pc;
        var ug = $scope.ug;
        httpService.getData('/Cor/GetScript?ug=' + ug).then(function (res) {


            if (res != "") {
                var scriptPixel = res;
                for (i = 0 ; i < scriptPixel.length; i++) {
                    if (scriptPixel[i].pixeltype == 'J') {
                        var script = document.createElement("script");
                        script.type = "text/javascript";
                        script.async = true;// it is true by default, but we are jsut applying it.
                        if (scriptPixel[i].src != '') {
                            script.src = scriptPixel[i].src.replace("%%client_cpi%%", $scope.pc).replace("%%external_member_id%%", $scope.extid);
                        }
                        else if (scriptPixel[i].attributrValue != '') {
                            script.innerHTML = scriptPixel[i].attributrValue
                        }
                        document.body.appendChild(script);
                    }
                    else if (scriptPixel[i].pixeltype == 'I') {
                        var imgsfpixel = document.createElement('img');
                        imgsfpixel.height = "1px";
                        imgsfpixel.width = "1px";
                        imgsfpixel.alt = "Ipixel";
                        if (scriptPixel[i].src != '') {
                            imgsfpixel.src = scriptPixel[i].src.replace("%%client_cpi%%", $scope.pc).replace("%%external_member_id%%", $scope.extid);
                        }
                        else if (scriptPixel[i].attributrValue != '') {
                            imgsfpixel.innerHTML = scriptPixel[i].attributrValue
                        }
                        document.getElementsByTagName('head')[0].appendChild(imgsfpixel);
                    }

                }
            }
            else {
                $scope.isShowLoginFailErrmsg = true;
            }
        }, function (err) {
            //  $scope.errMsg = true;
        });
    }
    //if (($scope.into == 'n' && prjid > 0)) {
    //    window.location.href = '/cor/surveygetbyprjid?rid=' + rid + '&prjid=' + prjid;
    //}

    //$scope.thankyou = function() {
    //    if (($scope.into == 'y' ||  $scope.into == '')|| ($scope.into == 'n' && prjid == 0)) {
    //        window.location.href = '/cor/ps2?rid=' + rid + '&extid=' + extid + '&prjid=' + prjid;
    //    }
    //    else {
    //        window.location.href = '/cor/surveygetbyprjid?rid=' + rid + '&prjid=' + prjid;

    //    }
    //}
    $scope.thankyou = function (SkiptoSurvey) {
        $scope.ImageLoading = true;
        if (SkiptoSurvey == "1") {
            httpService.getData('/Cor/GetSurveyURL?pid=' + $scope.pid + '&rid=' + $scope.rid + '&extid=' + $scope.extid).then(function (res) {
                window.location.href = res;
            });
        }
        else {
            window.location.href = '/cor/ps2?rid=' + $scope.rid + '&extid=' + $scope.extid + '&prjid=' + $scope.pid + '&isClick=1&sid=' + sid;
        }
    }
    //privacy click
    $scope.privacyClickOB = function () {
        $window.open('https://www.opinionbar.com/Footer/Privacy', 'OB-Privacy', 'width=800,height=800')
    }
    //terms click
    $scope.termsClickOB = function () {
        $window.open('https://www.opinionbar.com/Footer/TC', 'OB-Terms', 'width=800,height=800')
    }

    //Conversant privacy
    $scope.privacyClickCon = function () {
        $window.open('https://wl.conversant.pro/login/p', 'Conversant-Privacy', 'width=800,height=800')
    }
    //Conversant terms
    $scope.termsClickCon = function () {
        $window.open('https://wl.conversant.pro/login/t', 'Conversant-Terms', 'width=800,height=800')
    }

}]);
