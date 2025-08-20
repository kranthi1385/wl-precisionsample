
var app = angular.module("relApp", ['pascalprecht.translate', 'customSerivces'])
   //config all required providers.(ex: color palette, routing, register all providers
   .config(['$locationProvider', '$httpProvider', '$controllerProvider', '$translateProvider',
function ($locationProvider, $httpProvider, $controllerProvider, $translateProvider) {

    //add header for each  request to identify AntiForgeryToken
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    //load all translations setting to main transtion loader.
    $translateProvider.useLoader('$translatePartialLoader', {
        urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
    });
    //add default language
    $translateProvider.preferredLanguage('en');

    //regster providers

}])

app.controller("relController", ['$rootScope', '$scope', '$http', '$window', '$location', '$rootScope', '$timeout', 'httpService', 'translationsLoadingService', 'pagerService', 'getCurrentPageList', 'lazyscriptLoader', '$interval',
function ($rootScope, $scope, $http, $window, $location, $rootScop, $timeout, httpService, translationsLoadingService, pagerService, getCurrentPageList, lazyscriptLoader, $interval) {
    //declare Name of the namespace
    var that = {};
    var ug = ''; //User Guid
    var panelistid = '';
    var Country = '';
    var Score = 0;
    var profileScore = 0;
    var fpfScore = 0;
    var TotalScore = '';
    var rvid = '-1';
    var rid = '-1';

    ug = getUrlVars()["ug"];
    panelistid = getUrlVars()["uid"];
    rid = getUrlVars()["rid"];
    Country = getUrlVars()["c"];
    clientid = getUrlVars()["cid"];
    if (Country == undefined) {
        Country = '';
    }
    // to get URL params
    function getUrlVars() {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }
    //

    function getQueryStringParam(param) {
        querySt = window.location.search.substring(1);
        queryStringArray = querySt.split("&");
        for (i = 0; i < queryStringArray.length; i++) {
            ft = queryStringArray[i].split("=");
            if (ft[0].toLowerCase() == param.toLowerCase()) {
                return ft[1];
            } // end if
        } // end for
        return ""; // not found in query string params
    } // end function



    function populateInputFields() {
        document.getElementById('ClientID').value = '7DAB107F-8B12-4C44-A1E3-B569F6932EEA';
        if (panelistid != '' && panelistid != null) {
            document.getElementById('PanelistID').value = panelistid;
        }
        document.getElementById('SurveyID').value = rid; //New api partner 
        document.getElementById('GeoCodes').value = '1,' + Country;
        //document.getElementById('CID').value = '';
        //document.getElementById('TID').value = '';
        document.getElementById('TimePeriod').value = '';
        //document.getElementById('PropertyList').value = '';
    }


    window.RVIDResponseComplete = function () {

        // Client will implement appropriate redirect logic in this function
        // To access the various reponse parameters, use document.getElementById(“fieldName”)
        // Example: var RVID = document.getElementById(“RVid”).value;
        document.getElementById('RVIDCompleted').value = "1";
        rvid = document.getElementById('RVid').value;
        Score = document.getElementById('Score').value;
        profileScore = document.getElementById('FraudProfileScore').value;
        fpfScore += document.getElementById('FPF1').value + ',';
        fpfScore += document.getElementById('FPF2').value + ',';
        fpfScore += document.getElementById('FPF3').value + ',';
        fpfScore += document.getElementById('FPF4').value + ',';
        fpfScore += document.getElementById('FPF5').value + ',';
        fpfScore += document.getElementById('FPF6').value + ',';
        fpfScore += document.getElementById('FPF7').value + ',';
        fpfScore += document.getElementById('FPF8').value + ',';
        fpfScore += document.getElementById('FPF9').value;
        TotalScore += Score + ';' + fpfScore;
        if (ug != '' && ug != null) {

            httpService.getData('/Reg/RelevantUpdate?ug=' + ug + '&pfscore=' + profileScore + '&ts=' + TotalScore +
                '&c=' + Country + '&rvid=' + rvid + '&cid=' + clientid + '&rdjson=' + $scope.rdjson).then(function (response) {
                    if (response != 0) {
                        window.location.href = '/Reg/Home?ug=' + ug + '&cid=' + clientid;
                    }
                }, function (err) {
                    alert("An error has occured, Sorry for the inconvenience.\n");
                });
        }
    }


    function RVIDNoResponse() {

        // Client should implement appropriate logic in case response is not received within given time period
        // Make sure that the RVIDResponseComplete() method has not already been executed
        if (document.getElementById('RVIDCompleted').value == "0") {
            if (ug != null && ug != '') {

                httpService.getData('/Reg/RelevantUpdate?ug=' + ug + '&pfscore=' + 0 + '&ts=' + TotalScore + '&c=' + Country + '&rvid=' + rvid).then(function (response) {
                    if (response != 0) {
                        window.location.href = '/Reg/Home?ug=' + ug + '&cid=' + clientid;
                    }
                }, function (err) {
                    alert("An error has occured, Sorry for the inconvenience.\n");
                });

            }
        }
    }


    // Logic to populate input fields intentionally left outside common library since clients may want to
    // implement differently. By default, the input fields will be populated from query strings params.
    function callRVIDService() {

        populateInputFields();
        // setTimeout("RVIDNoResponse();", 5000); // 1000 = 1 second; suggested value 5000
        callRVIDNow();
    }

    //Clean ID data
    //IDSuite.cleanid({
    //    RequestId: ug,
    //    EventId: clientid,
    //    ChannelId: clientid,
    //    GeoRestrictionEnabled: false,
    //    FullDataSet: false,
    //    onSuccess: (res) => {
    //        debugger;
    //        //handle your response data here
    //        $scope.cleanID = JSON.stringify(res);
    //        $scope.relevantInitialize();
    //    },
    //    onError: (res) => {
    //        $scope.relevantInitialize();
    //        //handle any error here
    //    }
    //});

    $scope.relevantInitialize = function () {
        //callRVIDService();
        //Rdjson
        var ipqsJsonFormat = localStorage.getItem('ipqsJson');
        if (ipqsJsonFormat != null && ipqsJsonFormat != undefined && ipqsJsonFormat != '') {
            $scope.IPQSJson = ipqsJsonFormat.replaceAll("&", "||**||").replace("#", "||*||").replace("+", "||***||");
        }
        httpService.getData('/Reg/RelevantUpdate?ug=' + ug + '&pfscore=' + profileScore + '&ts=' + TotalScore +
            '&c=' + Country + '&rvid=' + rvid + '&cid=' + clientid + '&ipqsJson=' + $scope.IPQSJson).then(function (response) {
                $interval.cancel(poling);
                if (response != 0) {
                    window.location.href = '/Reg/Home?ug=' + ug + '&cid=' + clientid;
                }
            }, function (err) {
                alert("An error has occured, Sorry for the inconvenience.\n");
            });
    }

    var poling = "";
    $timeout(function () {
        poling = $interval($scope.relevantInitialize(), 3000)
    }, 3000);//polling starts after 3 seconds of page load.
}]);