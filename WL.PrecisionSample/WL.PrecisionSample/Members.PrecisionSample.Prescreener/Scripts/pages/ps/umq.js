define(['psApp'], function (psApp) {
    psApp.register.controller("userMobilePgController", ['$rootScope', '$scope', '$http', '$window', 'httpService', 'getQueryParams', 'translationsLoadingService',
    function ($rootScope, $scope, $http, $window, httpService, getQueryParams, translationsLoadingService) {
        debugger;
        translationsLoadingService.writeNlogService();
        //get query params
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
        var usid = getQueryParams.getUrlVars()['usid'];
        if (usid == undefined || usid == "undefined" || usid == null || usid == '') {
            usid = '';
        }
        var pid = getQueryParams.getUrlVars()["pid"];
        if (pid == undefined || pid == "undefined" || pid == null || pid == '') {
            pid = '';
        }
        var river = getQueryParams.getUrlVars()["isr"]; //river identification 
        if (river == undefined || river == "undefined" || river == null || river == '') {
            river = '';
        }
        var pstest = getQueryParams.getUrlVars()["pstest"];
        if (pstest == undefined) {
            pstest = 0;
        }
        else {
            if (parseInt(pstest) == 1) {
            }
            else {
                pstest = 0;
            }
        }

        $scope.showViewContent = false;
        // to get URL params
        var qst = {
            ChallengeId: 1,
            QuestionText: 'Are you interested in taking surveys on your mobile phone?',
            OptionText: 'Yes,No',
            AnswerText: ''
        }
        var qoptlst =
            {
                OptionText: ''
            }
        var rdurl = "";
        //get device info
        function checkDeviceInfo() {
            var regExp = '';
            regExp = new RegExp('Android|webOS|iPhone|iPad|' +
                        'BlackBerry|Windows Phone|' +
                        'Opera Mini|IEMobile|Mobile',
                       'i');

            if (regExp.test(navigator.userAgent))
                return 'mobile'
            else
                return "unknown";
        }
        //check devce type 
        var getDeviceInfo = function () {
            var dvType = checkDeviceInfo();           
            if (dvType != "unknown" && dvType != '') {
                rdurl = "https://sb.scorecardresearch.com/p?c1=14&c2=22118396&c3=" + ug + "&cid=" + cid + "&c4=s&cj=1&c5=1&c12=&r=" + encodeURIComponent("https://dev.prs.com/prs/pp?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&usid=" + usid + "&pid=" + pid);
            }
            else {
                rdurl = "/prs/pp?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid;
            }
            if (cid == 110) {//Added on 1/11/2019 by Giri to skip 2 profile questions for River
                rdurl = "/prs/psl?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid;
            }
        }
        //check mobile number 
        httpService.getData("/prs/checkusermobilenumber?ug=" + ug + "&cid=" + cid + "&uig=" + uig).then(function (data) {
            if (data != null) {
                document.cookie = "orgLogo=" + data.OrgLogo;
                if (ug != uig) {
                    uig = data.UserInvitationId;//update current userinvitaitonguid from new userinvitationguid
                }
                getDeviceInfo();//g
                if (data.IsrequireVerityValidatedMembers) { //Verity Required Members
                    if (data.IsMobileNumberExist == 0) { //Mobile Not exist
                        $rootScope.psPageLoad = true;
                        $scope.showViewContent = true;
                        $rootScope.isShowFooter = true;
                        httpService.getData("/prs/mobilequestion").then(function (question) {
                            var qstResp = question;
                            if (qstResp != null) {
                                qstResp[0].QuestionText = qst.QuestionText; // push static questions into object
                                qstResp[0].OptionText = qst.OptionText;
                                for (var q in qstResp) {
                                    var optLst = []; // options list array
                                    var dLst = []; // dummay options list for object construction
                                    optLst = qstResp[q].OptionText.split(',') // split options by comma seperator
                                    for (var r in optLst) {
                                        dLst.push(angular.copy(qoptlst))
                                    }
                                    for (var p in optLst) {
                                        dLst[p].OptionText = optLst[p];
                                    }
                                    if (optLst.length > 0) { //split options list insert into original object list
                                        qstResp[q].OptionList = dLst;
                                    }

                                }
                                $scope.questions = qstResp;
                            } /*end second response*/
                            else {
                                window.location.href = rdurl;
                            }
                        });
                    } /*end first response*/
                    else {
                        window.location.href = rdurl;
                    }
                }
                else {
                    window.location.href = rdurl;
                }
            }
            else {
                window.location.href = rdurl;
            }
        });
        $scope.selectedIndex = -1;
        $scope.showmbNumber = false;
        $scope.submitValidations = false;
        //radio button click
        $scope.rdClick = function ($event, value, index) {
            $scope.selectedIndex = index;
            if (value.toLowerCase() == 'yes') {
                $scope.showmbNumber = true;
                $scope.submitValidations = false;
            }
            else {
                $scope.showmbNumber = false;
                $scope.submitValidations = false;

            }
            $scope.questions[0].OptionText = value;
        }
        $scope.mobileNumber = '';
        //update mobile number change event
        $scope.updateMobileNumber = function (phNo) {
            if (phNo != undefined) {
                $scope.mobileNumber = phNo;
            }
        }
        //sumbit click event
        $scope.submitted = function (validate) {
            if ($scope.showmbNumber && validate == false) { //user select yes options means (he is agree to provide mobile number)
                if ($scope.selectedIndex == -1) {
                    $scope.submitValidations = true;
                }
                else {
                    $scope.submitValidations = false;
                }
            }
            else {
                if (validate) {
                    httpService.postData('/prs/UpdateMobileNumber?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&phNumber=' + $scope.mobileNumber).then(function (data) {
                        window.location.href = rdurl;
                    });
                }
                else {
                    $scope.submitValidations = true;
                }
            }

        }
    }]);
});

