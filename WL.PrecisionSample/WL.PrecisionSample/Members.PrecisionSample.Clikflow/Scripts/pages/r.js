define(['app'], function (app) {
    app.register.controller('rController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'translationsLoadingService', '$interval'
   , function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, translationsLoadingService, $interval) {
       var uig = getQueryParams.getUrlVars()['uig'];
       var ug = getQueryParams.getUrlVars()['ug'];
       var pid = getQueryParams.getUrlVars()["pid"];
       var cid = getQueryParams.getUrlVars()["cid"];
       var uid = getQueryParams.getUrlVars()["usrinvid"];
       var cstrId = getQueryParams.getUrlVars()["cstring"];
       $scope.maxNumberofChecks = 2;
       $scope.currentChecks = 0;
       $scope.needtoCheck = true;
       if (pid == undefined) {
           pid = '';
       }
       tid = getQueryParams.getUrlVars()["tid"];
       if (tid == undefined) {
           tid = '';
       }
       usid = getQueryParams.getUrlVars()["usid"];
       if (usid == undefined) {
           usid = '';
       }
       if (cid == undefined) {
           cid = '';
       }
       var GeoIp2Res = '';
       var rvid = '';
       var score = '';
       var totalScore = ''; //releventscore + fpfscore
       var fpfScore = '';
       var rvScores = '';
       var isNew = 0;

       //Invoke Relevant Methods.
       function callRVIDService() {
           populateInputFields();
           callRVIDNow();

       }

       function populateInputFields() {
           document.getElementById('ClientID').value = '7DAB107F-8B12-4C44-A1E3-B569F6932EEA';
           if (ug != '' && ug != null) {
               document.getElementById('PanelistID').value = ug;
               document.getElementById('SurveyID').value = pid; //for Registration
           }
           document.getElementById('GeoCodes').value = '1,' + "US";
           document.getElementById('TimePeriod').value = '';
       }

       window.RVIDResponseComplete = function () {
           document.getElementById('RVIDCompleted').value = "1";
           rvId = document.getElementById('RVid').value;
           score = document.getElementById('Score').value;
           profileScore = document.getElementById('FraudProfileScore').value;
           if (document.getElementById('isNew').value.toLowerCase() == "true") {
               isNew = 1;
           }
           else {
               isNew = 0;
           }
           fpfScore += document.getElementById('FPF1').value + ',';
           fpfScore += document.getElementById('FPF2').value + ',';
           fpfScore += document.getElementById('FPF3').value + ',';
           fpfScore += document.getElementById('FPF4').value + ',';
           fpfScore += document.getElementById('FPF5').value + ',';
           fpfScore += document.getElementById('FPF6').value + ',';
           fpfScore += document.getElementById('FPF7').value + ',';
           fpfScore += document.getElementById('FPF8').value + ',';
           fpfScore += document.getElementById('FPF9').value;
           totalScore += score + ';' + fpfScore + ';' + profileScore;
           saveRid();
           return true;
       }

       var RVIDNoResponse = function () {
           rvId = 'Relevant Fail';
           score = 0;
           totalScore = '0;0,0,0,0,0,0,0,0,0'
           saveRid();
       }

       function saveRid() {
           radiusData = { //search Information object
               geodata: GeoIp2Res,
               UserGuid: ug,
           }
           var url = "/cr/InsertValidations?usid=" + usid + "&uig=" + uig + '&cid=' + cid + '&isNew=' + isNew +
                          "&rvId=" + rvId + "&tscore=" + totalScore
           httpService.postData(url, radiusData, '', 1).then(function (data, status) {
               if (data != null) {
                  
               }
           }, function (err) {
               //Redirect the Member to Error End Page, based on Org we need to redirect the members to respective end pages.
           });
       }


       //maxmind go ip2 check success callback
       var onSuccess = function (location) {
           GeoIp2Res = JSON.stringify(location)
       };

       //maxmind go ip2 check error callback
       var onError = function (error) {
           GeoIp2Res = "";
       };

       try
       {
           geoip2.city(onSuccess, onError);
       }
       catch (err) {
           GeoIp2Res = "";
       }


       $scope.relevantInitialize = function () {
           try {
               callRVIDService();
           }
           catch (err) {
              // window.location.href = "/reg/pii?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid;
           }
       };
   }])
});