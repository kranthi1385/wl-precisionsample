define(['app'], function (app) {
    app.register.controller('cvController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'translationsLoadingService',
   function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, translationsLoadingService) {
       // translationsLoadingService.writeNlogService();
       //get query params
       var uig = getQueryParams.getUrlVars()['uig'];
       var ug = getQueryParams.getUrlVars()['ug'];
       var pid = getQueryParams.getUrlVars()["pid"];
       var cid = getQueryParams.getUrlVars()["cid"];
       var sr = getQueryParams.getUrlVars()["sr"];
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
       if (sr == undefined) {
           sr = '';
       }
       var cc = getQueryParams.getUrlVars()["cc"];
       if (cc == undefined) {
           cc = '';
       }
       //check verity and get new user invitation guid
       httpService.getData('/cv/veritycheck?uig=' + uig + "&ug=" + ug +
                    "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + "&cid=" + cid, 3).then(function (data) {
                        // alert(data);


                        //If the Member failed Verity 5 then we should redirect to end page.
                        if (data.RedirectUrl != "" && data.RedirectUrl != undefined) {
                            window.location.href = data.RedirectUrl;
                        }

                        else {
                            window.location.href = '/cv/v6?ug=' + ug + '&uig=' + data.UserInvitationId + '&cid=' + cid +
                        "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + '&sr=' + sr + '&cc=' + cc;
                        }
                        // get user basic profile questions

                    });
   }])
});

