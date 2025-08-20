define(['app'], function (app) {
    app.register.controller('vldController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'translationsLoadingService', '$interval', '$cookies',
   function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, translationsLoadingService, $interval, $cookies) {
       var uig = getQueryParams.getUrlVars()['uig'];
       var ug = getQueryParams.getUrlVars()['ug'];
       var pid = getQueryParams.getUrlVars()["pid"];
       var cid = getQueryParams.getUrlVars()["cid"];
       var uid = getQueryParams.getUrlVars()["usrinvid"];
       var cstrId = getQueryParams.getUrlVars()["cstring"];
       var lid = getQueryParams.getUrlVars()["lid"];
       var ipqsJsonFormat = '';
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

       //IDSuite.cleanid({
       //    RequestId: uig,
       //    EventId: pid,
       //    ChannelId: cid,
       //    GeoRestrictionEnabled: false,
       //    FullDataSet: false,
       //    onSuccess: (res) => {
       //        //handle your response data here
       //        $scope.cleanID = JSON.stringify(res);
       //        $scope.RDJsonInsert();
       //    },
       //    onError: (res) => {
       //        
       //        //handle any error here
       //    }
       //});
       var now = new $window.Date(),
       exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
       if (lid != '' && lid != undefined) {
           $cookies.put('LangCode', lid, {
               expires: exp,
               path: '/'
           });
           translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
       }
       else {
           translationsLoadingService.setCurrentUserLanguage('en');
       }
       //debugger;
       //if (typeof Startup !== "undefined") {
       //    Startup.Store('transactionID', uid);
       //    Startup.Store('userID', usid);
       //    Startup.Store('campaignID', pid);
       //    Startup.Store('publisherID', cid);

       //    Startup.AfterResult(function (result) {
       //        debugger;
       //        alert(result);
       //        $scope.IPQS = JSON.stringify(result);
       //    });
       //    Startup.AfterFailure(function (reason) {
       //        alert(reason);
       //        $scope.ipqssuccess = 1;
       //        // user has blocked the second JavaScript call
       //        // can redirect or perform other business logic if JS is not loaded
       //        // window.location.href = "";
       //    });
       //    Startup.Init();
       //}
       //else {
       //    alert(typeof Startup);
       //}

       $scope.maxNumberofChecks = 2;
       $scope.currentChecks = 0;
       $scope.needtoCheck = true;
       //$scope.RDJsonInsert = function () {
       //    debugger;
       //    ipqsJsonFormat = localStorage.getItem('ipqsJson');
       //    if (ipqsJsonFormat != null && ipqsJsonFormat != undefined && ipqsJsonFormat != '') {
       //        $scope.IPQSJson = ipqsJsonFormat.replaceAll("&", "||**||").replace("#", "||*||").replace("+", "||***||");
       //    }
       //    var url = "/cr/RdjsonInsert?userid=" + usid + "&uid=" + uid + "&cid=" + cid + '&uig=' + uig + '&ug=' + ug + '&pid=' + pid + '&cleanIDJson=' + $scope.cleanID + '&ipqsJson=' + $scope.IPQSJson;
       //    httpService.postData(url).then(function (data) {
       //        $interval.cancel(poling);
       //        if (data.RedirectURL != "" && data.RedirectURL != null && data.RedirectURL != undefined) {
       //            window.location.href = data.RedirectURL;
       //        } else {
       //            //if (data.IsGDPRCompliance == false && cid != 541 && cid != 542) {
       //            //    window.location.href = "/reg/priterms?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid;
       //            //}
       //            //else {
       //            window.location.href = "/reg/pii?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid;
       //            //}
       //        }
       //    }, function (err) {
       //        //Redirect the Member to Error End Page, based on Org we need to redirect the members to respective end pages.
       //    });
       //}

       $scope.RDJsonInsert = function () {
           debugger;
           var ipqsJsonFormat = localStorage.getItem('ipqsJson');
           if (ipqsJsonFormat != null && ipqsJsonFormat != undefined && ipqsJsonFormat != '') {
               $scope.IPQSJson = ipqsJsonFormat.replaceAll("&", "||**||").replace("#", "||*||").replace("+", "||***||");
           }
           var dvType = checkDeviceInfo();
           var url = "/cr/RdjsonInsert?userid=" + usid + "&uid=" + uid + "&cid=" + cid + '&uig=' + uig + '&ug=' + ug + '&dvtype=' + dvType + '&sessionId=' + $scope.session_id;
           httpService.postData(url, $scope.IPQSJson).then(function (data) {
               $interval.cancel(poling);
               if (data.RedirectURL != "" && data.RedirectURL != null && data.RedirectURL != undefined) {
                   window.location.href = data.RedirectURL;
               }
           }, function (err) {
               //Redirect the Member to Error End Page, based on Org we need to redirect the members to respective end pages.
           });
       }

       //device check
       function checkDeviceInfo() {
           var regExp = '';
           regExp = new RegExp('Android|webOS|iPhone|iPad|' +
               'BlackBerry|Windows Phone|' +
               'Opera Mini|IEMobile|Mobile',
               'i');

           if (regExp.test(navigator.userAgent))
               return 'mobile'
           else
               return "unknown";
       }

       function check() {
           //alert('check antered');
           //alert($scope.maxNumberofChecks);
           //alert($scope.currentChecks);

           if ($scope.maxNumberofChecks > $scope.currentChecks++) {
               var url = "/cr/check?userId=" + uid + "&cnsid=" + cstrId;
               httpService.getData(url).then(function (res) {
                   if (res.RelevantScore != '' && res.ZipRadius != '') {
                       //alert($scope.currentChecks);
                       $interval.cancel(poling);
                       //alert(res.URL);
                       if (res.URL) {
                           //alert('before reidrect');
                           window.location.href = res.URL;
                       }
                       else {
                           //alert('pii reidrect');
                           //if (res.IsGDPRCompliance == false && cid != 541 && cid != 542) {
                           //    window.location.href = "/reg/priterms?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid;
                           //}
                           //else {
                           window.location.href = "/reg/pii?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid;
                           //}
                       }
                   }
               },
               function () { });
           }
           else {
               $interval.cancel(poling);
               window.location.href = "/reg/pii?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid;
           }

       }
       //alert("hello");

       //maxmind go ip2 check success callback
       //var onSuccess = function (location) {
       //    GeoIp2Res = JSON.stringify(location)
       //};
       //maxmind go ip2 check error callback
       //var onError = function (error) {
       //    GeoIp2Res = "";
       //};
       //geoip2.city(onSuccess, onError);
       const MAX_RETRIES = 3;
       const RETRY_DELAY = 200;
       let attempts = 0;

       function  verisoul () {
           debugger;
           if ($window.Verisoul && typeof $window.Verisoul.session === 'function') {
               $window.Verisoul.session()
                 .then(function (res) {
                     debugger;
                     $scope.session_id = res.session_id;
                 })
           }else if(attempts < MAX_RETRIES){
               attempts++;
               $timeout(verisoul, RETRY_DELAY);
           } else {

           }
       };

       $timeout(verisoul, 100);

       var poling = "";

       $timeout(function () {
           $scope.RDJsonInsert();
           poling = $interval(check, 3000)
       }, 3000);//polling starts after 3 seconds of page load.

   }])
});