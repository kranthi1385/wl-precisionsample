define(['app'], function (app) {
    app.register.controller('smsController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'translationsLoadingService',
   function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, translationsLoadingService) {
       var uig = getQueryParams.getUrlVars()['uig'];
       var ug = getQueryParams.getUrlVars()['ug'];
       var pid = getQueryParams.getUrlVars()["pid"];
       var cid = getQueryParams.getUrlVars()["cid"];
       var sn = getQueryParams.getUrlVars()["sn"];
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
       translationsLoadingService.setCurrentUserLanguage('en');
       $scope.isDeviceFail = false;
       $scope.desktopsurvey = false;
       $scope.mobilesurvey = false;
       $scope.isMessageShow = 0;
       $scope.showMessage = false;
       $scope.isUsMember = false;
       $scope.isNonUsMember = false;
       $scope.showViewContent = true;
       $scope.mobileNumber = "";

       $scope.Nothankyou = function() {
           var url = "https://e.reachcollective.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=XXXXX"
           $scope.rplurl = url.replace("XXXXX", uig);
           window.location.href = $scope.rplurl;
       }

       $scope.sendsms = function (valid, mobileNo) {
           if (valid) {
               $scope.isMessageShow = 0;
               $scope.showMessage = false;
               $scope.mobileNumber = mobileNo;
               //Send SMS Logic.
               httpService.postData("/cr/sendsms?uig=" + uig + "&ug=" + ug + '&prjId=' + pid + "&mobileNum=" + $scope.mobileNumber +
                   '&surveyName=' + sn + '&orgId=' + cid, '', 1).then(function (data) {
                       if (parseInt(data) != 0) { //if we get question then fetch the question else redirect to next page
                           $scope.isMessageShow = 1;
                       }
                       else {
                           $scope.isMessageShow = 2;
                       }
                   }, function (err) {

                   });
           }
           else {
               $scope.showMessage = true;
           }
       }
   }])
});