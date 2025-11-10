define(['app'], function (app) {
    app.register.controller('vldController', ['$scope', '$rootScope', '$window', '$timeout', 'httpService', 'getQueryParams',
        function ($scope, $rootScope, $window, $timeout, httpService, getQueryParams) {
            function getUrlVars() {
                var Url = window.location.href;
                var vars = {};
                var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }
            // Reading all Params Stored in View Bag, to be used for RELEVANT API Call.
            var uig = getQueryParams.getUrlVars()['uig'];
            var ug = getUrlVars()['ug'];
            var pid = getQueryParams.getUrlVars()['pid'];
            var cid = getQueryParams.getUrlVars()['cid'];
            var extmid = getQueryParams.getUrlVars()['extmid'];
            if (uig == undefined) { uig = ''; }
            if (ug == undefined) { ug = ''; }
            if (pid == undefined) { pid = ''; }
            if (cid == undefined) { cid = ''; }
            if (extmid == undefined) {
                extmid = '';
            }
            //IDSuite.cleanid({
            //    RequestId: uig,
            //    EventId: pid,
            //    ChannelId: cid,
            //    GeoRestrictionEnabled: false,
            //    FullDataSet: false,
            //    onSuccess: (res) => {
            //handle your response data here

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
                            CleanIdDataInsert();
                        })
                } else if (attempts < MAX_RETRIES) {
                    attempts++;
                    $timeout(verisoul, RETRY_DELAY);
                } else {
                    CleanIdDataInsert();
                }
            };

            $timeout(verisoul, 100);

            // Use $timeout to introduce a wait time
            function CleanIdDataInsert() {
                //var ipqsJsonFormat = localStorage.getItem('ipqsJson');
                var json = 'undefined';
                //if (ipqsJsonFormat != null)
                //    $scope.IPQSJson = ipqsJsonFormat.replaceAll("&", "||**||").replace("#", "||*||").replace("+", "||***||");
                //else
                //    $scope.IPQSJson = null;
                //httpService.postData('/e / CleanIDDataInsert ? uig = ' + uig + "&ug=" + ug + "&pid=" + pid + "&extmid=" + extmid + "&cid=" + cid + ' & json=' + json + ' & sessionId=' + $scope.session_id, $scope.IPQSJson).then(function (data) {
                httpService.postData('/e/CleanIDDataInsert?uig=' + uig + "&ug=" + ug + "&pid=" + pid + "&extmid=" + extmid + "&cid=" + cid + '&json=' + json + '&sessionId=' + $scope.session_id).then(function (data) {
                    window.location.href = data + '&pid=' + pid;
                });
            };
            //    },
            //    onError: (res) => {
            //        window.location.href = 'https://ps.opinionetwork.com/prs/psl?ug=' + ug + '&uig=' + uig + '&cid=' + cid + '&pid=' + pid;
            //    }
            //});
        }])
});

