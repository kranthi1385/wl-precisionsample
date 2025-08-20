
define(['psApp'], function (psApp) {
    psApp.register.controller('messageController', ['$rootScope', '$scope', '$http', '$window', 'httpService', 'getQueryParams', 'translationsLoadingService',
        function ($rootScope, $scope, $http, $window, httpService, getQueryParams, translationsLoadingService) {
            translationsLoadingService.writeNlogService();
            var ug = getQueryParams.getUrlVars()['ug'];
            if (ug == undefined) {
                ug = '';
            }
            var cid = getQueryParams.getUrlVars()['cid'];
            if (cid == undefined) {
                cid = '';
            }
            var uig = getQueryParams.getUrlVars()['uig'];
            if (uig == undefined) {
                uig = '';
            }
            var pid = getQueryParams.getUrlVars()['pid'];
            if (pid == undefined) {
                pid = '';
            }
            $scope.internalMemberMsg = false;
            $scope.externalMemberMsg = false;
            $scope.surveyName = '';
            $scope.surveyLength = '';
            var lan = languagelst;

            $scope.surveyCompleteReward = '';
            //get selected languages
            function getSelectedProjectLanguages() {
                debugger;
                httpService.getData('/prs/getporjectlanguages?ug=' + ug + '&cid=' + cid + '&uig=' + uig).then(function (data) {   // get user basic profile questions
                    debugger;
                    if (ug != uig) {
                        uig = data.UserInvitationGuid;//update current userinvitaitonguid from new userinvitationguid
                    }
                    //$rootScope.psPageLoad = true;
                    //$scope.showViewContent = true;
                    //$rootScope.isShowFooter = true;
                    //Show the Language Selection page.
                    $scope.langQst = data;
                    angular.forEach(lan, function (ln) {
                        if (data.SelectedOptions[0].OptionId == ln.OptionId) {
                            //$scope.buttontext = ln.OptionText;
                            //$rootScope.footertex = ln.footer;
                            //$rootScope.privacy = ln.privacy;
                            //$rootScope.powerby = ln.powerby;
                            $scope.thankutext = ln.thankutext;
                            $scope.SurveyName = ln.SurveyName;
                            $scope.SurveyLength = ln.SurveyLength;
                            $scope.SurveyReward = ln.SurveyReward;
                            $scope.takethisSurvey = ln.takethisSurvey;
                            $scope.qualifySurvey = ln.qualifySurvey;
                            $scope.close = ln.close;
                            $scope.nothanku = ln.nothanku;
                            $scope.yestakeme = ln.yestakeme;
                        }
                    });
                });
            }
            getSelectedProjectLanguages();
            document.getElementsByClassName('preloader')[0].style.visibility = 'hidden';
            if (ug == uig) {
                $scope.internalMemberMsg = false;
                $scope.externalMemberMsg = true;
                $rootScope.psPageLoad = true;
                $rootScope.isShowQuestions = true;
                $rootScope.isShowFooter = false;
            }
            else {
                $scope.internalMemberMsg = true;
                $scope.externalMemberMsg = false;
                var url = "";
                url = '/prs/getprojectrewarddetails?ug=' + ug + '&uig=' + uig + '&cid=' + cid;
                httpService.getData(url).then(function (response) {
                    if (response.ProjectId > 0 && response.ProjectId != null && response.ProjectId != "") {
                        debugger;
                        $rootScope.psPageLoad = true;
                        $rootScope.isShowQuestions = true;
                        $rootScope.isShowFooter = true;
                        $scope.surveyName = response.SurveyName;
                        $scope.surveyLength = (response.SurveyLength + ' minutes');
                        $scope.surveyCompleteReward = response.SurveyCompletereward;
                        $scope.orgTypeId = response.OrganizationTypeId;
                        $scope.enableRecaptcha = response.EnableRecaptcha;
                        $scope.memberUrl = response.MemberUrl;
                        $scope.prjId = response.ProjectId;
                        //here we need to get the Original GUID by passing Step6.
                        $scope.UserInvitationGuid = response.User2PerkGuid;
                    }
                    else {
                        $window.location.href = "https://e.opinionetwork.com/e/psr?usg=2B9038B6-DB53-429A-8854-7BB83338B2D4&ug=" + ug + '&cid=' + cid + "&uig=" + $scope.UserInvitationGuid + '&pid=' + pid;
                    }
                });

                $scope.close = function () {
                    window.opener = self;
                    window.close();
                }
            }
            $scope.successClick = function () {

                //Based on the 
                window.location.href = "https://e.opinionetwork.com/e/interstitial?ug=" + ug + "&uig=" + uig + "&sr=" + $scope.enableRecaptcha + "&cid=" + cid + '&fc=n&pid=' + pid;
            }
            $scope.noThanks = function () {
                $window.location.href = "https://e.opinionetwork.com/e/psr?usg=FA27DD1D-CA1D-478B-A66A-93C589361D34&ug=" + ug + '&cid=' + cid + "&uig=" + $scope.UserInvitationGuid + '&pid=' + pid;
            }
        }])
});