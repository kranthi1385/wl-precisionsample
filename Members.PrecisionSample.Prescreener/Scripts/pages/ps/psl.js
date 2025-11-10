
define(['psApp'], function (psApp) {
    psApp.register.controller("psLanguageController", ['$rootScope','$timeout', '$scope', '$http', '$window', 'httpService', 'getQueryParams', 'translationsLoadingService',
    function ($rootScope,$timeout, $scope, $http, $window, httpService, getQueryParams, translationsLoadingService) {
        //rad query param values
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
        var usid = getQueryParams.getUrlVars()['usid'];
        if (usid == undefined || usid == "undefined" || usid == null || usid == '') {
            usid = '';
        }
        var pid = getQueryParams.getUrlVars()["pid"];
        if (pid == undefined || pid == "undefined" || pid == null || pid == '') {
            pid = '';
        }
        var conid = getQueryParams.getUrlVars()["conid"];
        if (conid == undefined || conid == "undefined" || conid == null || conid == '') {
            conid = '';
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
        var isStandalonePartner = '';
        var lan = languagelst;
        $scope.submitValidations = false;
        $scope.showViewContent = false;
        $scope.isBeraProject = false;
        $scope.beraData = {
            agreePrivacy: false
        };
        $scope.beraPrivacy = false;
        $scope.agreePrivacy = false;
        $scope.showBeraError = false;
        //get project selected languages
        function getSelectedProjectLanguages() {
            httpService.getData('/prs/getporjectlanguages?ug=' + ug + '&cid=' + cid + '&uig=' + uig).then(function (data) {   // get user basic profile questions
                $rootScope.isShowQuestions = true;
                $scope.isBeraProject = data.OrgId == 7191;
                $scope.isIpsosProject = data.SelectedOptions[0].IsIpsosProject;
                $rootScope.isShowFooter = !$scope.isBeraProject;
                $rootScope.isShowOrgLogo = $scope.isBeraProject ? false : $rootScope.isShowOrgLogo;
                $rootScope.isShowBeraHeader = !$rootScope.isShowOrgLogo;
                $scope.zip = data.SelectedOptions[0].ZIPCode;
                if (ug != uig) {
                    uig = data.UserInvitationGuid;//update current userinvitaitonguid from new userinvitationguid
                }
                if ($scope.isIpsosProject == true) {
                    if (data.SelectedOptions.length == 1 & data.SelectedOptions[0].OptionId == 114282) {
                        //Show the Step1 Penidng Question.
                        window.location.href = '/prs/zip?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + data.SelectedOptions[0].OptionId + '&conid=' + conid + '&zip=' + $scope.zip;
                    }
                    else if (data.SelectedOptions.length == 1 & data.SelectedOptions[0].OptionId != 114282) {
                        window.location.href = '/prs/zip?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + data.SelectedOptions[0].OptionId + '&conid=' + conid + '&zip=' + $scope.zip;
                    }
                    else {
                        $rootScope.psPageLoad = true;
                        $scope.showViewContent = true;
                        $rootScope.isShowFooter = !$scope.isBeraProject;
                        //Show the Language Selection page.
                        $scope.langQst = data;
                        $scope.LanguageHeader = $scope.langQst.SelectedOptions[0].LanguageHeader.replace(/\//g, '<br/><br/>');      
                        angular.forEach(lan, function (ln) {                            
                            if (data.SelectedOptions[0].OptionId == ln.OptionId) {                                
                                $scope.buttontext = ln.OptionText;
                                $rootScope.footertex = ln.footer;
                                $rootScope.privacy = ln.privacy;
                                $rootScope.powerby = ln.powerby;
                                $rootScope.beraprivacy1 = ln.beraprivacy1;
                                $rootScope.beraprivacy2 = ln.beraprivacy2;  
                                $rootScope.beraprivacy3 = ln.beraprivacy3;
                                $rootScope.beraprivacy4 = ln.beraprivacy4;
                                $rootScope.beraprivacy5 = ln.beraprivacy5;
                                $rootScope.beraprivacy6 = ln.beraprivacy6;
                                $rootScope.beraprivacy7 = ln.beraprivacy7;
                            }
                        });
                    }
                }
                else {
                    if (data.SelectedOptions.length > 0) {
                        //   if (data.SelectedOptions[0].PendingQuestionCount > 0) {
                        //Only English language is Selected in the Pre-Screener.
                        if (data.SelectedOptions.length == 1 & data.SelectedOptions[0].OptionId == 114282) {
                            //Show the Step1 Penidng Question.
                            if ($scope.isBeraProject) {
                                httpService.getData('/prs/saveprescreenerlanguage?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&langid=' + data.SelectedOptions[0].OptionId).then(function (data) {
                                    if (data == 'Success') {
                                        window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&conid=' + conid;
                                    } else {
                                        getSelectedProjectLanguages();
                                    }
                                });
                            }
                            else
                            {
                                window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&conid=' + conid;
                            }
                        }
                        else if (data.SelectedOptions.length == 1 & data.SelectedOptions[0].OptionId != 114282) {
                            if ($scope.isBeraProject) {
                                httpService.getData('/prs/saveprescreenerlanguage?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&langid=' + data.SelectedOptions[0].OptionId).then(function (data) {
                                    if (data == 'Success') {
                                        window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + data.SelectedOptions[0].OptionId + '&conid=' + conid;
                                    } else {
                                        getSelectedProjectLanguages();
                                    }
                                });
                            }
                            else {
                                window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + data.SelectedOptions[0].OptionId + '&conid=' + conid;
                            }
                        }
                        else {
                            $rootScope.psPageLoad = true;
                            $scope.showViewContent = true;
                            $rootScope.isShowFooter = !$scope.isBeraProject;
                            //Show the Language Selection page.
                            $scope.langQst = data;
                            $scope.LanguageHeader = $scope.langQst.SelectedOptions[0].LanguageHeader.replace(/\//g, '<br/><br/>');      
                            angular.forEach(lan, function (ln) {
                                if (data.SelectedOptions[0].OptionId == ln.OptionId) {                                   
                                    $scope.buttontext = ln.OptionText;
                                    $rootScope.footertex = ln.footer;
                                    $rootScope.privacy = ln.privacy;
                                    $rootScope.powerby = ln.powerby;
                                    $rootScope.beraprivacy1 = ln.beraprivacy1;
                                    $rootScope.beraprivacy2 = ln.beraprivacy2;
                                    $rootScope.beraprivacy3 = ln.beraprivacy3;
                                    $rootScope.beraprivacy4 = ln.beraprivacy4;
                                    $rootScope.beraprivacy5 = ln.beraprivacy5;
                                    $rootScope.beraprivacy6 = ln.beraprivacy6;
                                    $rootScope.beraprivacy7 = ln.beraprivacy7;
                                }
                            });
                        }
                        // }
                        //else {

                        //    //if member belongs to river and pending question count is zero then redirect to river survey url page
                        //    if (data.SelectedOptions[0].PendingQuestionCount == 0 && river == ug) {
                        //        window.location.href = 'http://opt.opinionetwork.com/misc/csurl.html?ug=' + ug + '&uig=' + uig;
                        //        return true;
                        //    }
                        //    //Means Member do not math the basic demogrpahics.
                        //    if (data.SelectedOptions[0].PendingQuestionCount == -1) {
                        //        //if member belongs to river and pending question is -1 the  redirect ot ennd page
                        //        if (river == ug) {
                        //            window.location.href = "http://sdl.precisionsample.com/e/psr?usg=2B9038B6-DB53-429A-8854-7BB83338B2D4&ug=" + ug + "&uig=" + uig;
                        //            return true;
                        //        }
                        //        //window.location.href = '/ps/rep.html?ug=' + ug + '&uig=' + uig;
                        //        if (ug == uig) {
                        //            window.location.href = '/psr/rep?ug=' + ug + '&uig=' + uig + "&project=" + pid;
                        //        }
                        //        else {
                        //            // getendpageurlfororganization(); //Will decide the Home Page URL & Stand Alone Check.\
                        //            if ($rootScope.isStandalonePartner == 't') {
                        //                // $.session.clear();
                        //                window.location.href = "http://sdl.precisionsample.com/e/psr?usg=2B9038B6-DB53-429A-8854-7BB83338B2D4&ug=" + ug + "&uig=" + uig + "&project=" + pid;
                        //            }
                        //            else {

                        //            }
                        //        }
                        //    }
                        //    else {
                        //        //To get the Survey URL.
                        //        if (data.SelectedOptions[0].Source == "p" || data.SelectedOptions[0].Source == "t") {
                        //            //getendpageurlfororganization();
                        //            //$.session.clear();
                        //            //Will decide the Home Page URL & Stand Alone Check.
                        //            if ($rootScope.isStandalonePartner == 't') {
                        //                window.location.href = "http://sdl.precisionsample.com/e/psr?usg=2B9038B6-DB53-429A-8854-7BB83338B2D4&ug=" + ug + "&uig=" + uig + "&project=" + pid;
                        //            }
                        //            else {
                        //                window.location.href = '/psr/rep?ug=' + ug + '&uig=' + uig + "&project=" + pid;
                        //            }
                        //        }
                        //        else {
                        //            // if member belongs to projects api
                        //            if (data.SelectedOptions[0].PrjAPIFlag == 1) {
                        //                //check firstname,latname,emailaddress
                        //                if (_userinfo[0] != "" && _userinfo[1] != "" && _userinfo[2] != "") {
                        //                    $.session.clear();
                        //                    window.location.href = 'http://sdl.precisionsample.com/e/csurl?ug=' + ug + '&uig=' + uig + '&sr=';
                        //                }
                        //                else {
                        //                    window.location.href = '/ps/prjapibp.html?ug=' + ug + '&uig=' + uig;
                        //                }
                        //            }
                        //            else {
                        //                window.location.href = 'http://sdl.precisionsample.com/e/csurl?ug=' + ug + '&uig=' + uig + '&sr=';
                        //            }
                        //        }

                        //    }
                        //}
                    }
                    else {
                        // if no prescreener questions for this project
                        if ($scope.isBeraProject) {
                            httpService.getData('/prs/saveprescreenerlanguage?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&langid=' + 114282).then(function (data) {
                                if (data == 'Success') {
                                    window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&conid=' + conid;
                                } else {
                                    getSelectedProjectLanguages();
                                }
                            });
                        }
                        else
                        {
                            window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + "&pid=" + pid + '&river=' + river + '&pstest=' + pstest + '&conid=' + conid;
                        }
                    }
                }
            });
        }

        //set organization details into cookie
        function getorgnizationlogo(orglogo, projectApi, userInfo) {
            if (ug == uig) {
                $rootScope.orglogo = "";
            }
            if (projectApi == 1) {
                ////$cookies.put('prjAPI', projectApi);
                //userInfo = userInfo.split('|')
                //$cookies.put('userInfo', userInfo);
            }
            else {
                //$cookies.put('prjAPI', 0);
            }
        }
        //get selected languages
        getSelectedProjectLanguages();

        $scope.radiobtnvalidation = -1;

        $scope.rdClick = function ($event, optid, index) {
            $scope.radiobtnvalidation = index;
            $scope.langQst.OptionId = optid;
        };

        $timeout(function () {
            applyEqualHeight();
            window.addEventListener('resize', applyEqualHeight); // Optional: for responsive design
        }, 0);

        function applyEqualHeight() {
            const boxes = document.querySelectorAll('.dvChkbox.dvstyle');
            let maxHeight = 0;

            boxes.forEach(box => {
                box.style.height = 'auto'; // Reset to natural height
                if (box.offsetHeight > maxHeight) {
                    maxHeight = box.offsetHeight;
                }
            });

            boxes.forEach(box => {
                box.style.height = `${maxHeight}px`;
            });
        }

        //contine click
        $scope.save = function (valid) {
            if (valid) {
                if ($scope.IsIpsosProject == true) {
                    window.location.href = '/prs/zip?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + $scope.langQst.OptionId + '&conid=' + conid + '&zip=' + $scope.zip;
                }               
                else if($scope.isBeraProject)
                {                  
                    httpService.getData('/prs/saveprescreenerlanguage?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&langid=' + $scope.langQst.OptionId).then(function (data) {
                        if (data == 'Success') {
                            window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + $scope.langQst.OptionId + '&conid=' + conid;
                        } else {
                            getSelectedProjectLanguages();
                        }
                    });                                  
                }
                else
                {
                    window.location.href = '/prs/prsqst?ug=' + ug + '&cid=' + cid + '&uig=' + uig + '&pid=' + pid + '&river=' + river + '&pstest=' + pstest + '&lid=' + $scope.langQst.OptionId + '&conid=' + conid;
                }
            }
            else
            {
                $scope.submitValidations = true;
            }
        }
    }]);
});

