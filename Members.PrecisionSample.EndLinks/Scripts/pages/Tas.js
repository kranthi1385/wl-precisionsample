app.register.controller('TasController', ['$rootScope', '$scope', 'httpService', 'getQueryParams', 'translationsLoadingService',
function ($rootScope, $scope, httpService, getQueryParams, translationsLoadingService) {
    translationsLoadingService.writeNlogService();
    ug = getQueryParams.getUrlVars()["ug"];
    if (ug == undefined) {
        ug = '';
    }

    usg = getQueryParams.getUrlVars()["usg"];
    if (usg == undefined) {
        usg = '';
    }


    uig = getQueryParams.getUrlVars()["uig"];
    if (uig == undefined) {
        uig = '';
    }


    usid = getQueryParams.getUrlVars()["usid"];
    if (usid == undefined) {
        usid = "";
    }
    pid = getQueryParams.getUrlVars()["project"];
    if (pid == undefined) {
        pid = "";
    }

    //if (ug != undefined && uig != undefined) {
    //    httpService.getData('/E/TakeAnotherSurvey?ug=' + ug + "&uid=" + uig).then(function (dataResponse) {
    //        $scope.data = dataResponse;
    //        if (data.SurveyUrl != "") {
    //            $scope.name = data.SurveyName;
    //            $scope.length = data.SurveyLength;
    //            $scope.rewards = data.SurveyCompletereward;
    //            _surveyUrl = data.SurveyUrl;
    //        }
    //        else {

    //        }
    //    }

    //)
    //}
    //Take Another Survey Button Click 
    //$scope.takeme = function () {
    //    if (_surveyUrl != '' && _surveyUrl != undefined) {
    //        //Redirect the Member to Next top 1 Survey Click page URL.
    //        window.location.href = _surveyUrl;
    //    }
    //    else {
    //        window.location.href = "/mem/ms?ug=" + ug + "&usg=" + usg + "&uig=" + uig + "&usid=" + usid + "&pid=" + pid;
    //    }
    //}
    ////No Thank you click page.
    //$scope.thankyou = function () {
    //    window.location.href = "/mem/ms?ug=" + ug + "&usg=" + usg + "&uig=" + uig + "&usid=" + usid + "&pid=" + pid;
    //}
}]);
