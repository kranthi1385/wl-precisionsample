define(['app'], function (app) {
    app.register.controller('cvqController', ['$rootScope', '$scope', '$window', '$timeout', 'httpService', 'getQueryParams', 'translationsLoadingService', '$cookies',
   function ($rootScope, $scope, $window, $timeout, httpService, getQueryParams, translationsLoadingService, $cookies) {
       // translationsLoadingService.writeNlogService();
       //get query params
       var uig = getQueryParams.getUrlVars()['uig'];
       var ug = getQueryParams.getUrlVars()['ug'];

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
       if (dvtype == undefined) {
           dvtype = '';
       }
       var qoptLst = { //dummy object 
           OptionText: ''
       }
       if ($cookies.get('LangCode') != '' || $cookies.get('LangCode') != undefined) {
           translationsLoadingService.setCurrentUserLanguage($cookies.get('LangCode'));
       }
       else {
           translationsLoadingService.setCurrentUserLanguage('en');
       }
       $scope.showViewContent = false;
       $scope.showErrMsg = false;
       //check verity and get new user invitation guid
       httpService.getData('/cv/getverityquestions?uig=' + uig + "&ug=" + ug + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid + "&cid=" + cid + "&dvtype=" + dvtype, 3).then(function (data) {
           debugger;
           //  alert(data[0]);
           if (data != null) {
               console.log(data[0]);
               // get user basic profile questions
               if (data[0].RedirectUrl != "" && data[0].RedirectUrl != undefined) {
                   window.location.href = data[0].RedirectUrl;
               }
               else {
                   $scope.questions = data;
                   uig = data[0].UserInvitationGuid;
                   //If we Have Challenge Questions, then only we need to render the Questions.
                   if (data[0].QuestionText != "" && data[0].QuestionText != undefined) {
                       $scope.showPage = true;
                       $scope.showViewContent = true;
                       angular.forEach($scope.questions, function (qst, index) {
                           var optlst = []; // options list array
                           var dlst = []; // dummay options list for object construction
                           optlst = qst.OptionText.split(','); // split options by comma seperator
                           for (var r in optlst) {
                               dlst.push(angular.copy(qoptLst))
                           }
                           for (var p in optlst) {
                               dlst[p].OptionText = optlst[p];
                           }
                           if (optlst.length > 0) { //split options list insert into original object list
                               qst.OptionList = dlst;
                           }
                       });
                   }
                   else {
                       //Means, if the Member fails any Verity6 Rules like , no B2B quesitons.
                       if (data[0].RedirectUrl != "" && data[0].RedirectUrl != undefined) {
                           //Redirect to End Page.
                           window.location.href = data[0].RedirectUrl;
                       }
                       else {
                           if (uig == ug)
                               window.location.href = 'http://dev.affiliate.endlinks.com/e/interstitial?ug=' + ug + '&uig=' + uig + '&sr=' + sr + '&cid=' + cid + '&cc=' + cc + '&fc=n&pid=' + pid;
                               //window.location.href = 'http://dev.prs.com/prs/psl?ug=' + ug + '&uig=' + uig + '&cid=' + cid;
                           else
                               window.location.href = "http://dev.prs.com/prs/umq" + '?ug=' + ug + '&uig=' + uig + '&pid=' + pid + '&cid=' + cid;
                       }

                   }
               }
           }
           else {
               if (uig == ug)
                   window.location.href = 'http://dev.affiliate.endlinks.com/e/interstitial?ug=' + ug + '&uig=' + uig + '&sr=' + sr + '&cid=' + cid + '&cc=' + cc + '&fc=n&pid=' + pid;
               else
                   window.location.href = "http://dev.prs.com/prs/umq" + '?ug=' + ug + '&uig=' + uig + '&pid=' + pid + '&cid=' + cid;
           }
       });
       //checkbox click
       $scope.chkOptions = function (qst, opt) {
           qst.AnswerText = opt.OptionText
       }
       //save options
       $scope.save = function (isValid) {
           if (isValid) {
               $scope.showErrMsg = false;

               //We need to Handle this for Verity 6 Required Proejct and need to do all Validations.
               httpService.postData("/cv/saveverityquestions?uig=" + uig + "&ug=" + ug + '&cid=' + cid + "&dvtype=" + dvtype, $scope.questions, 3).then(function (data) {
                   if (data.RedirectUrl != "" && data.RedirectUrl != undefined) {
                       window.location.href = data.RedirectUrl + '&ug=' + ug + '&pid=' + pid + '&cid=' + cid; // redirect
                   }
                   else {
                       // we need to redirect to the Mobile Prescreener Page.
                       if (uig == ug)
                           window.location.href = 'http://dev.affiliate.endlinks.com/e/interstitial?ug=' + ug + '&uig=' + uig + '&sr=' + sr + '&cid=' + cid + '&cc=' + cc + '&fc=n&pid=' + pid;
                       else
                           window.Location.hef = "http://dev.prs.com/prs/umq" + '?ug=' + ug + '&uig=' + data.UserInvitationGuid + '&pid=' + pid + '&cid=' + cid;
                   }
               }, function (err) {

               });
           }
           else {
               $scope.showErrMsg = true; //show error message
           }
       }

       //skip verity enchance questions
       $scope.skip = function () {
           httpService.getData('/cv/skipverityquestions?uig=' + uig + "&ug=" + ug + "&cid=" + cid, 3).then(function (data) {   // get user basic profile questions

           });
       }
   }])
});

