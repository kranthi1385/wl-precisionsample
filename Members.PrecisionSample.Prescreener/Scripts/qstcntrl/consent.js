define(['psApp'], function (psApp) {
    psApp.register.controller("consentController", function ($scope, $rootScope, getQueryParams) {
        debugger;
        var lid = getQueryParams.getUrlVars()["lid"];
        if (lid == undefined || lid == null || lid == '') {
            lid = 114282;
        }
        var lan = languagelst.filter(x => x.QuestionId = 1675 && x.OptionId == Number(lid));
        $scope.psPageLoad = true;
        $scope.showBeraError = false;
        debugger;
        $scope.beraData = {
            agreePrivacy: false
        };
        $scope.beraPrivacy = false;
        angular.forEach(lan, function (ln) {
            $rootScope.beraprivacy1 = ln.beraprivacy1;
                $rootScope.beraprivacy2 = ln.beraprivacy2;
                $rootScope.beraprivacy3 = ln.beraprivacy3;
                $rootScope.beraprivacy4 = ln.beraprivacy4;
                $rootScope.beraprivacy5 = ln.beraprivacy5;
                $rootScope.beraprivacy6 = ln.beraprivacy6;
                $rootScope.beraprivacy7 = ln.beraprivacy7;
        });     
	})
});