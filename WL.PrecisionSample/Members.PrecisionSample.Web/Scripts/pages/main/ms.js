define(['app'], function (app) {
    app.register.controller('MsController', ['$rootScope', '$scope', 'translationsLoadingService', 'httpService',
        function ($rootScope, $scope, translationsLoadingService, httpService) {
            $scope.congrats = false;
            $scope.thankYou = false;
            $scope.weAreSorry = false;
            $scope.sorryQuota = false;
            $scope.youTerminated = false;
            $scope.alredyParticipated = false;
            $scope.thankParticipated = false;
            $scope.offersNotExists = false;
            $scope.showSurveys = true;
            $scope.dOIRecruit = false;
            $scope.message = 0;
            // to get URL params
            function getUrlVars() {
                var Url = window.location.href;
                var vars = {};
                var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }
            //Get query parmas value referrer id
            $scope.usg = getUrlVars()["usg"];
            if ($scope.usg == undefined) {
                $scope.usg = ''
            }
            $scope.cid = getUrlVars()["cid"];
            if ($scope.cid == undefined) {
                $scope.cid = ''
            }
            if ($scope.usg.toLowerCase() == "6ac169c6-df47-4cd1-8f4d-1311f5c5f163" || $scope.usg.toLowerCase() == "181cf682-614e-46ec-9716-816af9dfe43d" || $scope.usg.toLowerCase() == "167944ad-051f-48e2-b458-184a27c27ece" || $scope.usg.toLowerCase() == "e999a83c-f5c0-4cde-bee1-6557b6fa001f") {
                $scope.congrats = true;
            }
            else if ($scope.usg.toLowerCase() == "f6a6b754-4cf8-41bb-b9aa-5b97c412b1f4" || $scope.usg.toLowerCase() == "2b9038b6-db53-429a-8854-7bb83338b2d4") {
                if ($scope.cid == 385) {
                    $scope.prescreenerterm = true;
                }
                else {
                    $scope.thankYou = true;
                }
            }
            else if ($scope.usg.toLowerCase() == "C1F0F26D-FE64-4A7F-A2A3-69379A0FEA91") {
                if ($scope.cid == 38) {
                    $scope.psoqs = true;
                }
                else {
                    $scope.psoqthankYou = true;
                }
            }
            else if ($scope.usg.toLowerCase() == "ec9ad2bb-a92b-4781-87c1-5d3b505f6cd3") {
                $scope.weAreSorry = true;
            }
            else if ($scope.usg.toLowerCase() == "67b98bed-9c3f-42ae-bdd3-7e15f9c17f00") {
                if ($scope.cid == 38) {
                    $scope.QuotaFull = true;
                }
                else {
                    $scope.sorryQuota = true;
                }
            }
            else if ($scope.usg.toLowerCase() == "d5f04cf6-50ab-4617-9b0f-95b23a07488c" || $scope.usg.toLowerCase() == "b75a1590-2786-45f9-a5e3-656ae1c13724") {
                $scope.thankYou = true;
            }
            else if ($scope.usg.toLowerCase() == "664b50cb-e1e7-40cc-b2eb-a94e1d54228f") {
                $scope.alredyParticipated = true;
            }
            else if ($scope.usg.toLowerCase() == "a24bc10d-1eeb-4a1a-83fd-8789180631ef") {
                $scope.thankParticipated = true;
            }
            else if ($scope.usg == "2BC664BA-94DD-41E8-B7E1-251A90105119") {
                $scope.dOIRecruit = true;
            }
            else if ($scope.usg != "") {
                $scope.offersNotExists = true;
            }
            translationsLoadingService.loadTranslatePagePath("ms");
            $scope.surveys = []; //Get Surveys Data
            httpService.getData('/Ms/GetSurveysList').then(function (response) {
                if (response != '') {
                    if (response[0].ProjectId == 0) {
                        $scope.showSurveys = false;
                    }
                    $scope.surveys = response;
                }
                else {
                    $scope.showSurveys = false;
                    $scope.message = 1;
                }
            }, function (err) {
                $scope.errMgs = true;
            });

            $scope.refreshSurveys = function () {
                httpService.getData('/Ms/GetSurveysList').then(function (response) {
                    if (response != '') {
                        if (response[0].ProjectId == 0) {
                            $scope.showSurveys = false;
                        }
                        $scope.surveys = response;
                    }
                    else {
                        $scope.showSurveys = false;
                        $scope.message = 1;
                    }
                }, function (err) {
                    $scope.errMgs = true;
                });
            }

            //window .open
            $scope.openWindow = function () {
                //$window.open('PartialViews/p2sampleentry.html?uid="+ userid.ToString() +"&prjid="+ obj.surveys[i].project_id +"&URL="+ obj.surveys[i].entry_link + "','','scrollbars,resizable,screeny=top,width=950,height=800');
            }
            $scope.openWindow = function () {
                //$window.open('PartialViews/p2sampleentry.html?uid="+ userid.ToString() +"&prjid="+ obj.surveys[i].project_id +"&URL="+ obj.surveys[i].entry_link + "','','scrollbars,resizable,screeny=top,width=950,height=800');
            }
            $scope.popup = function (url) {
                var width = 700;
                var height = 550;
                var left = (screen.width - width) / 2;
                var top = (screen.height - height) / 2;
                var params = 'width=' + width + ', height=' + height;
                params += ', top=' + top + ', left=' + left;
                params += ', directories=no';
                params += ', location=no';
                params += ', menubar=no';
                params += ', resizable=no';
                params += ', scrollbars=yes';
                params += ', status=no';
                params += ', toolbar=no';
                newwin = window.open(url, 'windowname5', params);
                setTimeout(() => { this.refreshSurveys(); }, 5000);
                debugger;
                if (window.focus) { newwin.focus() }
                return false;
            }
        }]);
});