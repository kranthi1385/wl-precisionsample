//main module of the soltion
angular.module("commonApp", ['customSerivces'])
    .config(['$locationProvider', '$httpProvider', '$controllerProvider',
   function ($locationProvider, $httpProvider, $controllerProvider) {
       $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
       //load all translations setting to main transtion loader.

       //regster providers

   }])
.controller('crController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'lazyscriptLoader', '$cookies',
   function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, lazyscriptLoader, $cookies) {
       //Google cookie
       $scope.acceptCookie = $cookies.get('google_fb_cookie_aacept');
       if ($scope.acceptCookie == 1) {
           (function (w, d, s, l, i) {
               w[l] = w[l] || []; w[l].push({
                   'gtm.start':
                   new Date().getTime(), event: 'gtm.js'
               }); var f = d.getElementsByTagName(s)[0],
               j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
               'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
           })(window, document, 'script', 'dataLayer', 'GTM-N9DDS62')
       }
       // Reading all Params Stored in View Bag, to be used for RELEVANT API Call.
       var ug = getQueryParams.getUrlVars()["ug"];
       var lpage = getQueryParams.getUrlVars()["lpage"];
       var panelistid = getQueryParams.getUrlVars()["id"];
       var Country = getQueryParams.getUrlVars()["c"];
       var mid = getQueryParams.getUrlVars()["mid"];
       var pid = getQueryParams.getUrlVars()["pid"];
       var project = getQueryParams.getUrlVars()["project"];
       var sid = getQueryParams.getUrlVars()["sid"];
       var qid = getQueryParams.getUrlVars()["qid"];
       var s = getQueryParams.getUrlVars()["s"];
       var usid = getQueryParams.getUrlVars()["usid"];
       var rcheckR = getQueryParams.getUrlVars()["rcheckr"];
       var erl = getQueryParams.getUrlVars()["RL"];
       var zipcode = getQueryParams.getUrlVars()["z"];
       var dob = getQueryParams.getUrlVars()['dob'];
       var gender = getQueryParams.getUrlVars()['g'];
       var ethnicity = getQueryParams.getUrlVars()['e'];
       var osid = getQueryParams.getUrlVars()["osid"]
       var erm = getQueryParams.getUrlVars()["RM"];
       var refid = getQueryParams.getUrlVars()['refid']
       var clientId = getQueryParams.getUrlVars()['cid'];
       var rvid = 0;
       var score = 0;
       var profileScore = 0;
       var fpfScore = 0;
       var totalscore = 0;
       var rdjson = '';
       var isNew = 0;
       if (lpage == undefined) {
           lpage = '';
       }
       if (ug == undefined) {
           ug = '';
       }
       if (panelistid == undefined) {
           panelistid = '';
       }
       if (Country == undefined) {
           Country = '';
       }
       if (pid == undefined) {
           pid = '';
       }
       if (project == undefined) {
           project = '';
       }
       if (mid == undefined) {
           mid = '';
       }
       if (sid == undefined) {
           sid = '';
       }
       if (qid == undefined) {
           qid = '';
       }
       if (s == undefined) {
           s = '';
       }
       if (usid == undefined) {
           usid = '';
       }
       if (osid == undefined) {
           osid = '';
       }
       if (refid == undefined) {
           refid = '';
       }
       //Invoke Relevant Methods.
       function callRVIDService() {
           populateInputFields();
           //$timeout(RVIDNoResponse, 20000); // 1000 = 1 second; suggested value 5000
           callRVIDNow();
       }

       function populateInputFields() {
           document.getElementById('ClientID').value = '7DAB107F-8B12-4C44-A1E3-B569F6932EEA';
           if (panelistid != '' && panelistid != null) {
               document.getElementById('PanelistID').value = panelistid;
               document.getElementById('SurveyID').value = 1; //for Registration
           }
           else if (mid != '' && mid != null && pid != '' && pid != null) { //For External Members
               document.getElementById('PanelistID').value = mid;
               document.getElementById('SurveyID').value = pid;
           }
           else if (usid != '' && usid != null && project != '' && project != null) { //For Internal Members Survey Click Page.
               document.getElementById('PanelistID').value = usid;
               document.getElementById('SurveyID').value = project;
           }

           document.getElementById('GeoCodes').value = '1,' + Country;
           //document.getElementById('CID').value = '';
           //document.getElementById('TID').value = '';
           document.getElementById('TimePeriod').value = '';
           //document.getElementById('PropertyList').value = '';
       }
       //var result = $window.RVIDResponseComplete;
       //$scope.RVIDResponseComplete = result;
       window.RVIDResponseComplete = function () {
           // Client will implement appropriate redirect logic in this function
           // To access the various reponse parameters, use document.getElementById(“fieldName”)
           // Example: var RVID = document.getElementById(“RVid”).value;
           document.getElementById('RVIDCompleted').value = "1";
           rvid = document.getElementById('RVid').value;
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
           totalscore += score + ';' + fpfScore;
           saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR);
           return true;
       }

       var RVIDNoResponse = function () {
           debugger;
           rvId = 'Relevant Fail';
           score = 0;
           totalScore = '0;0,0,0,0,0,0,0,0,0'
           saveRid(0, 0, 0, 0, 0);
       }

       function saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR, token) {
           // post user responses
           if (rcheckR == '') {
               rcheckR = 0;
           }
           httpService.postData('/rg/saverelevantinfo?ug=' + ug + '&score=' + score + '&rvid=' + rvid + '&pfScore=' + profileScore + '&fpfScores=' + fpfScore + '&isNew=' + isNew + '&rchecker=' + rcheckR + '&cid=' + clientId + '&token=' + token).then(function (data, status) {
               debugger;
               if (score > 75 || score == -1) {
                   if (parseInt(refid) == 14723) {
                       window.location.href = '/Ms/Surveys?ug=' + ug + '&lpage=' + lpage + '&confirm=f';
                   }
                   else {
                       window.location.href = "/rg/top10?ug=" + ug + '&cid=' + clientId;
                       //window.location.href = '/rg/OptPage3?ug=' + ug + '&cid=' + clientId;
                       // + '&lpage=' + lpage + '&confirm=f'
                   }
               }
               else {
                   window.location.href = "/rg/top10?ug=" + ug + '&cid=' + clientId;
               }
           }, function (err) {

               //Redirect the Member to Error End Page, based on Org we need to redirect the members to respective end pages.
           });
       }



       $scope.relevantInitialize = function () {
           // lazyscriptLoader.load(['http://relevantid.imperium.com/RVIDWrapperAjax2.js']).then(function () {
           //callRVIDService();
           //Rdjson
           debugger;
           $scope.getToken = function () {
               debugger;
               var token = "https://prod.rtymgt.com/api/v3/respondents/get_token/8f34a44d-9bba-43bb-911b-b4a466cf4b05";
               var xhr = new XMLHttpRequest();
               xhr.open('GET', token, true);
               xhr.withCredentials = true;
               xhr.send(null);
               xhr.onreadystatechange = (e) => {
                   if (xhr.readyState == 4) {
                       if (e.currentTarget.status == 200) {
                           if (e.currentTarget.responseText != null && e.currentTarget.responseText != "") {
                               result = JSON.parse(e.currentTarget.responseText);
                               token = result.results[0].token;
                               saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR, token);
                           }
                       }
                       else {
                           token = null;
                           saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR, token);
                       }
                   }
               }

           };
           $scope.getToken();
           //})
           //callRVIDService()
           // check if there is query in url
           // and fire search in case its value is not empty
       };
   }])


