//main module of the soltion
angular.module("commonApp", ['customSerivces'])
    .config(['$locationProvider', '$httpProvider', '$controllerProvider',
   function ($locationProvider, $httpProvider, $controllerProvider) {
       $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
       //load all translations setting to main transtion loader.

       //regster providers

   }])
.controller('crController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'lazyscriptLoader', '$interval', '$cookies',
   function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, lazyscriptLoader, $interval, $cookies) {
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
       //IDSuite.cleanid({
       //    RequestId: ug,
       //    EventId: clientId,
       //    ChannelId: clientId,
       //    GeoRestrictionEnabled: false,
       //    FullDataSet: false,
       //    onSuccess: (res) => {
       //       
       //        //handle your response data here
       //        $scope.cleanID = JSON.stringify(res);
       //        saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR);
       //    },
       //    onError: (res) => {
       //        saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR);
       //        //handle any error here
       //    }
       //});
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
           //saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR);
           return true;
       }

       var RVIDNoResponse = function () {
          
           rvId = 'Relevant Fail';
           score = 0;
           totalScore = '0;0,0,0,0,0,0,0,0,0'
           //saveRid(0, 0, 0, 0, 0);
       }


       const MAX_RETRIES = 3;
       const RETRY_DELAY = 200;
       let attempts = 0;

       function verisoul() {
           debugger;
           if ($window.Verisoul && typeof $window.Verisoul.session === 'function') {
               $window.Verisoul.session()
                   .then(function (res) {
                       debugger;
                       $scope.session_id = res.session_id;
                       $scope.saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR);
                   })
           } else if (attempts < MAX_RETRIES) {
               attempts++;
               $timeout(verisoul, RETRY_DELAY);
           } else {
               $scope.saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR);
           }
       };

       $timeout(verisoul, 100);

       $scope.saveRid = function (rvid, profileScore, score, fpfScore, isNew, rcheckR) {
           // post user responses
           if (rcheckR == '') {
               rcheckR = 0;
           }
           //var ipqsJsonFormat = localStorage.getItem('ipqsJson');
           //if (ipqsJsonFormat != null && ipqsJsonFormat != undefined && ipqsJsonFormat != '') {
           //    $scope.ipqsJson = ipqsJsonFormat.replaceAll("&", "||**||").replace("#", "||*||").replace("+", "||***||");
           //}
           httpService.postData('/rg/saverelevantinfo?ug=' + ug + '&score=' + score + '&rvid=' + rvid + '&pfScore=' + profileScore + '&fpfScores=' + fpfScore + '&isNew=' + isNew + '&rchecker=' + rcheckR + '&cid=' + clientId + '&pid=' + pid + '&userId=' + panelistid + '&sessionId=' + $scope.session_id).then(function (data, status) {
            
               //$interval.cancel(poling);
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
       // $scope.saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR)

       //var poling = "";
       //$timeout(function () {
       //    poling = $interval($scope.saveRid(rvid, profileScore, score, fpfScore, isNew, rcheckR), 3000)
       //}, 3000);//polling starts after 3 seconds of page load.
   }])


