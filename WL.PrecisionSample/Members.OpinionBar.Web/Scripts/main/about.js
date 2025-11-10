define(['app'], function (app) {
    app.register.controller('aboutController', ['$rootScope', '$scope', 'httpService', '$cookies', 'translationsLoadingService',
        function ($rootScope, $scope, httpService, $cookies, translationsLoadingService) {
            $scope.flagimg = "/images/flag_usa.png";
            if (($cookies.get('UserLangCode') != undefined || $cookies.get('MainLangCode') != undefined) && ($cookies.get('UserFlagCode') != undefined || $cookies.get('MainFlagCode') != undefined)) {
                translationsLoadingService.setCurrentUserLanguage($cookies.get('UserLangCode') || $cookies.get('MainLangCode'));
              $scope.flagimg = $cookies.get('UserFlagCode') || $cookies.get('MainFlagCode');
               
            }
            else {
                translationsLoadingService.setCurrentUserLanguage('en');
                $scope.flagimg = "/images/flag_usa.png";
            }
            translationsLoadingService.loadTranslatePagePath("abt");
           
           
        }])
})
