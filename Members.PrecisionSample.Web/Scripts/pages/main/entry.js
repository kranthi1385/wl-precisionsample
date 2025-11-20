define(['app'], function (app) {
    app.register.controller('regController', ['$rootScope', '$scope', 'httpService', 'getQueryParams', 'translationsLoadingService', '$cookies',
    function ($rootScope, $scope, httpService, getQueryParams, translationsLoadingService, $cookies) { //controller
        debugger;
        mid = getQueryParams.getUrlVars()["mid"];
        if (mid == undefined) {
            mid = '';
        }
        key = getQueryParams.getUrlVars()["key"];
        if (key == undefined) {
            key = "";
        }
        project = getQueryParams.getUrlVars()["project"];
        if (project == undefined) {
            project = "";
        }
        sub_id = getQueryParams.getUrlVars()["sub_id"];
        $scope.getUrlParms = function () {
            var Url = window.location.href;
            var vars = {};
            var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                vars[key] = value;
            });
            return vars;
        }
        if (key == "cf27932e-660e-439b-bc94-f5368e4fca31" || key == "dd7af6a4-0759-422e-9ac6-01edfdde112b" || key == "3370f796-3197-4f9e-926c-36f1cdabb542" || key == "b5404da5-60dd-4fa5-ad14-6325985c7fbf" || key == "fe8e65e4-0408-4852-a563-455b524dbb5c") {
            sub_id = $scope.getUrlParms()['sub_id'];
        }
        if (sub_id == undefined) {
            sub_id = "";
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
        httpService.getData('/survey/RedirectAPI?ug=' + mid + "&project=" + project + "&subid=" + sub_id + "&key=" + key, 2).then(function (dataResponse) {
            if (dataResponse[0].QuestionId == "-1") {
                window.location = dataResponse[0].ProjectURL;
            }
            if (dataResponse[0].QuestionId != "-1") { //pending question count is 0  then redirect to next page
                $scope.question = dataResponse[0]; // insert question  
                $scope.qustid = dataResponse[0].QuestionId;
                $scope.optlst = dataResponse[0].Options;
                $scope.showViewContent = true;
            }
        });

        $scope.Save = function (valid) { // save user responses
            $scope.showloading = true;
            var answer = "";
            var qstId = "";
            var optId = 0;
            if (valid) { //check page valid or not
                //for (var q in $scope.question) {
                if ($scope.question.QuestionTypeId == 1) { // check question type
                    if ($scope.question.QuestionId == 1636) { // check questionid 
                        // combine month and day and year options
                        var year = $scope.question.Year;
                        var month = $scope.question.Month;
                        var day = $scope.question.Day
                        var d = new Date(year, month, day);
                        //if (d.getFullYear() == year && d.getMonth() == month && d.getDate() == day) {
                        answer = $scope.question.Month + '/' + $scope.question.Day + '/' + $scope.question.Year
                        qstId = $scope.question.QuestionId;
                    }
                    else {// remaining responses
                        answer = $scope.question.AnswerText
                        qstId = $scope.question.QuestionId;
                    }
                }
                else {
                    // user response for remaining question typeids except 1
                    answer = $scope.question.AnswerText;
                    qstId = $scope.question.QuestionId;
                    if (qstId == 1634 || qstId == 1640 || qstId == 1638) {
                        optId = parseInt(answer)
                    }
                }
                if ($scope.question.QuestionId == 4) {
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
                //}
                if (answer != "") { // check reasones 
                    $scope.showMessage = false;
                    $scope.optlst = [];
                    //post user responses
                    httpService.getData("/survey/SavePiiData?ug=" + mid + "&project=" + project + "&subid=" + sub_id + "&key=" + key + "&qid=" + qstId + "&otext=" + answer + "&optId=" + optId + "&cid=" + $scope.question.ClientId +
                        "&zip=" + $scope.piiQuestions.ZIPCode, '', 2).then(function (data, status) {
                            if (data[0].QuestionId == "-1") {
                                window.location = data[0].ProjectURL;
                            }
                            if (data[0].QuestionId != "-1") { //if we get question then fetch the question else redirect to next page
                                $scope.question = data[0];
                                $scope.qustid = data[0].QuestionId;
                                $scope.optlst = data[0].Options;
                                answer = "";
                                $scope.errorshow = false;
                                $scope.showloading = false;
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



