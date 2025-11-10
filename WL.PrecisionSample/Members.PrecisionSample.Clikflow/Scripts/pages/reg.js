define(['app'], function (app) {
    app.register.controller('regController', ['$rootScope', '$scope', 'httpService', 'getQueryParams', 'translationsLoadingService', '$cookies',
    function ($rootScope, $scope, httpService, getQueryParams, translationsLoadingService, $cookies) { //controller
        debugger;
        // translationsLoadingService.writeNlogService();
        //Get query parmas value user guid
        var sid = getQueryParams.getUrlVars()["uig"];      
        if (sid == undefined) {
            sid = '';
        }
        var ug = getQueryParams.getUrlVars()["ug"];
        if (ug == undefined) {
            ug = '';
        }
        var pid = getQueryParams.getUrlVars()["pid"];
        if (pid == undefined) {
            pid = '';
        }
        var tid = getQueryParams.getUrlVars()["tid"];
        if (tid == undefined) {
            tid = '';
        }
        var usid = getQueryParams.getUrlVars()["usid"];
        if (usid == undefined) {
            usid = '';
        }
        var cid = getQueryParams.getUrlVars()["cid"];
        if (cid == undefined) {
            cid = '';
        }
        var sr = getQueryParams.getUrlVars()["sr"];
        if (sr == undefined) {
            sr = '';
        }
        var cc = getQueryParams.getUrlVars()["cc"];
        if (cc == undefined) {
            cc = '';
        }
        var dvtype = getQueryParams.getUrlVars()["dvtype"];
        if (dvtype == undefined) {
            dvtype = '';
        }
        if ($cookies.get('LangCode') != '' && $cookies.get('LangCode') != undefined) {
            translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
        }
        else {
            translationsLoadingService.setCurrentUserLanguage('en');
        }
        $scope.Yearsarr = []; //total year array
        $scope.imgUrl = ""; //image url
        $scope.showViewContent = false; // page load  body show or hide flag
        $scope.showMessage = false; // error message by show or hide flag
        $scope.showloading = true; // loading iamge flag
        $scope.year = new Date().getFullYear(); // current year flag
        $scope.qustid = 0;
        $scope.is_page_load = 0;
        ipl = $scope.is_page_load

        $scope.UserInvitationGuid = "";
        $scope.piiQuestions = {
            Address1: '',
            Address2: '',
            City: '',
            ZIPCode: ''
        }
        //  questionid 
        //userServices.getclientdetails().then(function (clientResponse) { // get clinet inforamation
        //    if (clientResponse != "") {
        //        $scope.clinentInfo = clientResponse.data;
        //        $scope.imgUrl = $scope.clinentInfo.MemberUrl + '/' + $scope.clinentInfo.OrgLogo//clientResponse.data.OrgLogo;
        //    }

        //});
        httpService.getData('/reg/GetQuestion?uig=' + sid + "&ug=" + ug + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + "&cid=" + cid + "&dvtype=" + dvtype, 2).then(function (dataResponse) {   // get user basic profile questions
            if (dataResponse.Questions[0].QuestionId == undefined) {
                if (ug == sid) {
                    window.location = '/cv/v5?ug=' + ug + '&cid=' + cid + '&uig=' + dataResponse.Questions[0].UserInvitationGuid +
                                            "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + '&sr=' + sr + '&cc=' + cc;
                } else {
                    window.location = dataResponse.RedirectUrl;
                }
            }
            $scope.UserInvitationGuid = dataResponse.Questions[0].UserInvitationGuid;
            if (dataResponse.Questions[0].QuestionId != "-1") { //pending question count is 0  then redirect to next page
                $scope.question = dataResponse.Questions; // insert question  
                $scope.qustid = dataResponse.Questions[0].QuestionId;
                $scope.optlst = dataResponse.Questions[0].Options;
                $scope.showViewContent = true;
            }
            else {
                // alert(dataResponse[0].UserInvitationGuid);
                if (dataResponse.Questions[0].VerityScore == -2 && dataResponse.Questions[0].VerityRequired == true) {
                    //Here we call Verity 5 API 
                    httpService.getData('/reg/SaveVerity?ug=' + ug + '&cid=' + cid + "&uig=" + dataResponse.Questions[0].UserInvitationGuid, 2).then(function (dataResponse) {
                    });
                }
                else {
                    $scope.showloading = false; // Raju Do we Need this  ???
                }
                if (ug == sid) {
                    window.location = '/cv/v5?ug=' + ug + '&cid=' + cid + '&uig=' + dataResponse.Questions[0].UserInvitationGuid +
                                            "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + '&sr=' + sr + '&cc=' + cc;
                } else {
                    window.location = dataResponse.RedirectUrl;
                }
            }
        });
        $scope.Save = function (valid) { // save user responses
            $scope.showloading = true;
            var answer = "";
            var qstId = "";
            var optId = 0;
            if (valid) { //check page valid or not
                for (var q in $scope.question) {
                    if ($scope.question[q].QuestionTypeId == 1) { // check question type
                        if ($scope.question[q].QuestionId == 1) { // check question id
                            // if questionid 1 combine both first name last name
                            answer = $scope.question[q].AnswerText + ',' + $scope.question[q].LastName;
                            qstId = $scope.question[q].QuestionId;
                        }
                        else if ($scope.question[q].QuestionId == 1636) { // check questionid 
                            // combine month and day and year options
                            var year = $scope.question[q].Year;
                            var month = $scope.question[q].Month;
                            var day = $scope.question[q].Day
                            var d = new Date(year, month, day);
                            //if (d.getFullYear() == year && d.getMonth() == month && d.getDate() == day) {
                            answer = $scope.question[q].Month + '/' + $scope.question[q].Day + '/' + $scope.question[q].Year
                            qstId = $scope.question[q].QuestionId;
                            //}
                            //else {
                            //    $scope.showMessage = true;
                            //}

                        }
                        else {// remaining responses
                            answer = $scope.question[q].AnswerText
                            qstId = $scope.question[q].QuestionId;
                        }
                    }
                    else {
                        // user response for remaining question typeids except 1
                        answer = $scope.question[q].AnswerText;
                        qstId = $scope.question[q].QuestionId;
                        if (qstId == 1634 || qstId == 1640 || qstId == 1638) {
                            optId = parseInt(answer)
                        }
                    }
                    if ($scope.question[q].QuestionId == 4) {
                        answer = $scope.question[q].AnswerText;
                        qstId = 1640;
                        if ($scope.question[q].Options.length > 0 && answer != null) {
                            optId = parseInt(answer)
                        }
                        else {
                            optId = 0;
                            answer = "0";
                        }
                    }
                }
                if (answer != "") { // check reasones 
                    $scope.showMessage = false;
                    $scope.optlst = [];
                    //post user responses
                    httpService.postData("/reg/SaveResponse?Uig=" + sid + "&qid=" + qstId + "&otext=" + answer + "&Ug=" + ug + "&optId=" + optId + "&cid=" + cid + "&usid=" + usid +
                        "&address1=" + $scope.piiQuestions.Address1 + "&address2=" + $scope.piiQuestions.Address2 + "&city=" + $scope.piiQuestions.City + "&zip=" + $scope.piiQuestions.ZIPCode + "&dvtype=" + dvtype, '', 2).then(function (data, status) {
                            if (data.Questions[0].QuestionId == undefined) {
                                if (ug == sid) {
                                    window.location = '/cv/v5?ug=' + ug + '&cid=' + cid + '&uig=' + data.Questions[0].UserInvitationGuid +
                                                            "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + '&sr=' + sr + '&cc=' + cc;
                                } else {
                                    window.location = data.RedirectUrl;
                                }
                            }
                            if (data.Questions[0].QuestionId != "-1") { //if we get question then fetch the question else redirect to next page
                                $scope.question = data.Questions;
                                $scope.qustid = data.Questions[0].QuestionId;
                                $scope.optlst = data.Questions[0].Options;
                                answer = "";
                                $scope.errorshow = false;
                                $scope.showloading = false;
                            }
                            else {
                                if (data.Questions[0].VerityScore == -2 && data.Questions[0].VerityRequired == true) {
                                    httpService.getData('/reg/SaveVerity?Ug=' + ug + "&Uig=" + sid + "&cid=" + cid, 2).then(function (dataResponse) {
                                    });
                                }
                                else {
                                    $scope.showloading = false;
                                }
                                if (ug == sid) {
                                    window.location = '/cv/v5?ug=' + ug + '&cid=' + cid + '&uig=' + data.Questions[0].UserInvitationGuid +
                                                            "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + '&sr=' + sr + '&cc=' + cc;
                                } else {
                                    window.location = data.RedirectUrl;
                                }
                            }
                        }, function (err) {

                        });
                }
            }
            else {
                // if member not slected show validations
                $scope.showMessage = true;
                $scope.showloading = false;
                answer = "";
            }
        }

        //bind all years
        $scope.Years = function () {
            var d = new Date();
            $scope.currentYear = d.getFullYear();
            var years = [];
            for (var i = 13; i < 100; i++) {
                $scope.Yearsarr.push($scope.currentYear - i);
            }
        }
        //privacy policy link clikc open  popup
        $scope.privacy = function () {
            urlPrivacy = '/Privacy.htm';
            var width = 700;
            var height = 700;
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
            newwin = window.open(urlPrivacy, 'windowname5', params);
            if (window.focus) { newwin.focus() }
            return false;
        }
        //privacy policy link clikc open  popup
        $scope.terms = function () {
            urlPrivacy = '/Terms.html';
            var width = 700;
            var height = 700;
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
            newwin = window.open(urlPrivacy, 'windowname5', params);
            if (window.focus) { newwin.focus() }
            return false;
        }
    }]);
});
//// Service method for get data and post methods



