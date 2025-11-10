define(['app'], function (app) {
    app.register.controller('obProfileController', ['$rootScope', '$scope', 'httpService', '$cookies', 'translationsLoadingService',
    function ($rootScope, $scope, httpService, $cookies, translationsLoadingService) {
        translationsLoadingService.loadTranslatePagePath("pr");
        //get user data
        httpService.getData('/Ep/GetUserData').then(function (response) {
            $scope.user = response;
            $scope.ug = $scope.user.UserGuid;
        }, function (err) {
            $scope.errMsg = true;
        });
        //get all states avalibles
        httpService.getData('/pr/GetAllProfiles').then(function (response3) {
            $scope.profiles = response3;
        }, function (err) {

        });

        $scope.popup = function (url) {
            url = url + "&ug=" + $scope.user.UserGuid
            var width = 990;
            var height = 600;
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
            if (window.focus) { newwin.focus() }
            return false;
        }
    }]);
});