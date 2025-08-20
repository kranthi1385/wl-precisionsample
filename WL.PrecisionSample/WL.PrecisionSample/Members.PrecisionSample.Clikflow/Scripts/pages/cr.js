define(['app'], function (app) {
    app.register.controller('crController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'translationsLoadingService', 'lazyscriptLoader', '$cookies',
   function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, translationsLoadingService, lazyscriptLoader, $cookies) {
       //translationsLoadingService.writeNlogService();
       // Reading all Params Stored in View Bag, to be used for RELEVANT API Call.
       var TransID = getQueryParams.getUrlVars()['transid'];
       var qg = getQueryParams.getUrlVars()['qig'];
       var ug = getQueryParams.getUrlVars()['ug'];
       var prjId = getQueryParams.getUrlVars()['project'];
       var source = getQueryParams.getUrlVars()['s'];
       var GeoIp2Res = '';
       var that = {};
       $scope.getUrlParms = function () {
           var Url = window.location.href;
           var vars = {};
           var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
               vars[key] = value;
           });
           return vars;
       }
       var subId = $scope.getUrlParms()['sub_id'];
       var rid = getQueryParams.getUrlVars()['rid'];
       var osId = getQueryParams.getUrlVars()['osid'];
       var cid = getQueryParams.getUrlVars()['cid'];
       var sub = getQueryParams.getUrlVars()['sub'];
       var txid = getQueryParams.getUrlVars()['tx_id'];
       //added the below two params for swagbucks verity score saving
       var vscore = getQueryParams.getUrlVars()['vscore'];
       var vid = getQueryParams.getUrlVars()['vid'];
       var fedrespid = getQueryParams.getUrlVars()['frid'];
       var now = new $window.Date(),
       exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
       var lid = getQueryParams.getUrlVars()['lid'];
       translationsLoadingService.setCurrentUserLanguage(lid);
       if (TransID != '' && TransID != undefined && TransID != null) {
           subId = TransID;
       }
       if (sub != '' && sub != undefined && sub != null) {
           subId = sub;
       }
       if (txid != '' && txid != undefined && txid != null) {
           subId = txid;
       }

       if (source == undefined) {
           source = '';
       }
       $scope.getUrlParms = function () {
           var Url = window.location.href;
           var vars = {};
           var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
               vars[key] = value;
           });
           return vars;
       }
       if (cid == 427) {
           subId = $scope.getUrlParms()['sub_id'];
       }
       if (subId == undefined) {
           subId = '';
       }
       if (rid == undefined) {
           rid = '';
       }
       if (osId == undefined) {
           osId = '';
       }

       if (fedrespid == undefined) {
           fedrespid = '';
       }

       if (cid == undefined || cid == null || cid == '') {
           cid = 0;
       }
       if (vscore == undefined || vscore == null || vscore == '') {
           vscore = 0
       }
       if (vid == undefined || vid == null || vid == '') {
           vid = '';
       }
       var rvid = '';
       var score = '';
       var totalScore = ''; //releventscore + fpfscore
       var fpfScore = '';
       var rvScores = '';
       var uig = '';
       var prjTrafficTypeIds = '';
       var isMobile = 1;
       var deviceMatchedCount = 0;
       var isNew = 0;

       //Device Detection Logic:
       $scope.isDeviceFail = false;
       $scope.desktopsurvey = false;
       $scope.mobilesurvey = false;
       $scope.isMessageShow = 0;
       $scope.showMessage = false;
       $scope.isUsMember = false;
       $scope.isNonUsMember = false;
       $scope.showViewContent = true;
       $scope.mobileNumber = "";
       //Invoke Relevant Methods.
       function callRVIDService() {
           populateInputFields();
           //setTimeout("RVIDNoResponse();", 8000);
           callRVIDNow();
       }

       function populateInputFields() {
           document.getElementById('ClientID').value = '7DAB107F-8B12-4C44-A1E3-B569F6932EEA';
           if (ug != '' && ug != null) {
               document.getElementById('PanelistID').value = ug;
               document.getElementById('SurveyID').value = prjId; //for Registration
           }
           document.getElementById('GeoCodes').value = '1,' + "US";
           document.getElementById('TimePeriod').value = '';
       }
       var getgeodata = function () {
           // $scope.geodata = GeoIp2Res;
           $scope.radiusData = { //search Information object
               geodata: GeoIp2Res,
               UserGuid: ug,
           }
           //httpService.postData("/cr/checkradius", $scope.radiusData).then(function (data) {
           //           if (parseInt(data) != 0) { //if we get question then fetch the question else redirect to next page
           //               $scope.isMessageShow = 1;
           //           }
           //           else {
           //               $scope.isMessageShow = 2;
           //           }
           //       }, function (err) {

           //       });
       }

       //var result = $window.RVIDResponseComplete;
       //$scope.RVIDResponseComplete = result;
       window.RVIDResponseComplete = function () {
           // Client will implement appropriate redirect logic in this function
           // To access the various reponse parameters, use document.getElementById(“fieldName”)
           // Example: var RVID = document.getElementById(“RVid”).value;
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
           //  isNew = document.getElementById('isNew').value;
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
           try {
               $scope.showMessage = false;
               radiusData = { //search Information object
                   geodata: GeoIp2Res,
                   UserGuid: ug,
               }
               var url = "/cr/save?qg=" + qg + "&ug=" + ug + '&prjId=' + prjId + '&rid=' + rid + '&cid=' + cid + '&source=' + source + '&subId=' + subId + '&isNew=' + isNew +
                          '&osId=' + osId + "&rvId=" + rvId + "&tscore=" + totalScore + '&vid=' + vid + '&vscore=' + vscore + '&fedresid=' + fedrespid
               // post user responses
               httpService.postData(url, radiusData, '', 1).then(function (data, status) {
                   //write new nlog file for click start page
                   if (data.lid != '' || data.lid != undefined) {
                       $cookies.put('LangCode', data.lid, {
                           expires: exp,
                           path: '/'
                       });
                       translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
                   }
                   else {
                       translationsLoadingService.setCurrentUserLanguage('en');
                   }
                   if (data != undefined && data != '') { //if we get question then fetch the question else redirect to next page
                       $scope.surveyResponse = data;
                       if (cid == 0) {
                           cid = data.OrgId;
                       }
                       prjTrafficTypeIds = data.SurveyUserTypeIds;
                       if ($scope.surveyResponse.RedirectUrl != null && $scope.surveyResponse.RedirectUrl != "" && $scope.surveyResponse.RedirectUrl.indexOf("5CE933ED-9891-4CD7-8AC6-B529C58C6B55") != -1) {
                           //Find the device of the Member.
                           if ($window.navigator.userAgent.match(/Android/i)
                            || $window.navigator.userAgent.match(/webOS/i)
                            || $window.navigator.userAgent.match(/iPhone/i)
                            || $window.navigator.userAgent.match(/iPad/i)
                            || $window.navigator.userAgent.match(/iPod/i)
                            || $window.navigator.userAgent.match(/BlackBerry/i)
                            || $window.navigator.userAgent.match(/Windows Phone/i)) {
                               //Mobile Device Detected.
                               isMobile = 2;
                           }
                           else {
                               //Non Mobile Device detected
                               isMobile = 3;
                           }
                           //we need to Split Project Level Traffic Type Ids and match with Current User Traffic Type.
                           var trafficTypes = prjTrafficTypeIds.split(";")
                           for (i = 0; i < trafficTypes.length; i++) {
                               //If the Project Both devices then 
                               if (trafficTypes[i] == 1) {
                                   deviceMatchedCount = deviceMatchedCount + 1;
                               }
                               else
                                   if (trafficTypes[i] == isMobile) {
                                       deviceMatchedCount = deviceMatchedCount + 1;
                                   }
                           }
                           //If the Mobile User is on Non Mobile Survey.
                           if (deviceMatchedCount == 0 && isMobile == 2) {
                               if ($scope.surveyResponse.IsStandalone) {
                                   window.location.href = "http://e.opinionetwork.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + $scope.surveyResponse.ActualInvitationGuid + '&ug=' + ug + '&pid=' + prjId + '&cid=' + cid; //redirect to endpage
                               }
                               else {
                                   window.location.href = "http://www.opinionetwork.com/reg/home?ug=" + ug + "&cid=" + cid; //redirect to mobile survey page
                               }
                           }
                           //If Desktop User on a Mobile Survey.
                           if (deviceMatchedCount == 0 && isMobile == 3) {
                               $scope.isDeviceFail = true;
                               $scope.showViewContent = true;
                               if ($scope.surveyResponse.CountyCode.toLowerCase() == "us") {
                                   if ($scope.surveyResponse.IsStandalone) { // check standalone partner
                                       if ($scope.surveyResponse.IsEmailInvitationEnable == true || $scope.surveyResponse.IsSmsInvitation == false) //Standalone partner not having email invitation redirect to endpage. Added 06/15/2016
                                       {
                                           window.location.href = "http://e.opinionetwork.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + $scope.surveyResponse.ActualInvitationGuid + '&ug=' + ug + '&pid=' + prjId + '&cid=' + cid; //redirect to endpage
                                       }

                                       showSms();
                                   }
                                   else {
                                       showSms();
                                   }
                               }
                               else {
                                   //$scope.isNonUsMember = true;
                                   window.location.href = "http://e.opinionetwork.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + $scope.surveyResponse.ActualInvitationGuid + '&ug=' + ug + '&pid=' + prjId + '&cid=' + cid; //redirect to endpage
                               }
                           }
                           //redirect to Next Page, if all the Validations are Correct
                           if (deviceMatchedCount > 0 && $scope.surveyResponse.RedirectUrl == "") {
                               //We need to Rediect to Pre-Prescreener Page
                               window.location.href = "/reg/pii?uig=" + $scope.surveyResponse.UserInvitationId + "&ug=" + ug + "&cid=" + cid +
                                   "&pid=" + $scope.surveyResponse.ProjectId + "&tid=" + $scope.surveyResponse.Targetid + "&usid=" + $scope.surveyResponse.UserId;
                           }
                       }
                           //If any Settings Failed, we need to reidrect the 
                       else if ($scope.surveyResponse.RedirectUrl != "" && $scope.surveyResponse.RedirectUrl != undefined) {
                           window.location.href = $scope.surveyResponse.RedirectUrl + '&pid=' + prjId + '&ug=' + ug + '&cid=' + cid;
                       }
                       else if ($scope.surveyResponse.RedirectUrl == "" || $scope.surveyResponse.RedirectUrl == undefined) {
                           //Redirect to Pre-Prescreener Page.
                           window.location.href = "/reg/pii?uig=" + $scope.surveyResponse.UserInvitationId + "&ug=" + ug + '&cid=' + cid +
                                   "&pid=" + $scope.surveyResponse.ProjectId + "&tid=" + $scope.surveyResponse.Targetid + "&usid=" + $scope.surveyResponse.UserId;
                       }
                   }
               }, function (err) {
                   //Redirect the Member to Error End Page, based on Org we need to redirect the members to respective end pages.
               });
           }
           catch (err) {
               throw err;
           }
       }
       function showSms() {
           //Sms not avaliable to the partner redirect to endpage.
           if ($scope.surveyResponse.IsSmsInvitation) {
               $scope.isUsMember = true;
               // $scope.isNonUsMember = true;  
           }
           else {
               window.location.href = "http://e.opinionetwork.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + $scope.surveyResponse.ActualInvitationGuid + "&project=" + prjId + '&cid=' + cid; //redirect to endpage
               //Response.Redirect(ConfigurationManager.AppSettings["EndPageUrl"].ToString() + "usg=50AD6CC9-9228-496F-B936-7D0E0973E60A&ug=" + UserGuiid.ToString() + "&uig=" + oSurvey.UserInvitationId.ToString() + "&usid=" + UserId + "&project=" + Project);
           }
       }

       $scope.sendsms = function (valid, mobileNo) {
           if (valid) {
               $scope.isMessageShow = 0;
               $scope.showMessage = false;
               $scope.mobileNumber = mobileNo;
               //Send SMS Logic.
               httpService.postData("/cr/sendsms?uig=" + $scope.surveyResponse.ActualInvitationGuid + "&ug=" + ug + '&prjId=' + prjId + "&mobileNum=" + $scope.mobileNumber +
                   '&surveyName=' + $scope.surveyResponse.SurveyName + '&orgId=' + $scope.surveyResponse.OrgId, '', 1).then(function (data) {
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

       //maxmind go ip2 check success callback
       var onSuccess = function (location) {
           GeoIp2Res = JSON.stringify(location)
       };
       //maxmind go ip2 check error callback
       var onError = function (error) {
           GeoIp2Res = "";
       };
       geoip2.city(onSuccess, onError);

       $scope.relevantInitialize = function () {
           try {
               if (qg == undefined && prjId == undefined) {
                   //alert('Invalid url format.')
               }
               else {
                   callRVIDService();
               }
           }
           catch (err) {

           }
       };
   }])
});

