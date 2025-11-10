define(['gmApp'], function (gmApp) {
    gmApp.register.controller("userGmStartController", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'loadQuestionFilter', 'questionService', 'specialQuestionsService', 'getQueryParams', 'translationsLoadingService',
    function ($rootScope, $scope, $http, $window, httpService, loadQuestionFilter, questionService, specialQuestionsService, getQueryParams, translationsLoadingService) {
        $scope.startPage = false;
        var lid = getQueryParams.getUrlVars()['lid'];
        if (lid == undefined) {
            lid = '';
        }
        httpService.getData('/lead/GetStep2Details?lid=' + lid).then(function (res) {
            $scope.startPage = true;
            $scope.userLead = res;
        });
        $scope.save = function () {
            window.location.href = '/lead/q?lid=' + lid;
        }
        //privacy click
        $scope.privacyClick = function () {
            $window.open('http://marketplace-insider.com/privacy.html', 'marketplace-insider-Privacy', 'width=800,height=800')
        }
        //terms Click
        $scope.termsClick = function () {
            $window.open('http://marketplace-insider.com/terms.html', 'marketplace-insider-Privacy', 'width=800,height=800')
        }
    }]);
});