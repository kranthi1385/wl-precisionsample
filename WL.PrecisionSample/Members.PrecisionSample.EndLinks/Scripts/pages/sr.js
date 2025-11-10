define(['app'], function (app) {
    debugger;
    app.register.controller('EController', ['$rootScope', '$scope', 'httpService', 'getQueryParams', '$timeout',
    function ($rootScope, $scope, httpService, getQueryParams, $timeout) {
        $scope.ReplacestripGuid = '';
        $scope.Message = "";
        $scope.timermessage = "";
        $scope.pixel = "";

        $timeout(function () {
        }, 5000);


        //FED Response ID
        FedProjectId = getQueryParams.getUrlVars()["fpid"];
        if (FedProjectId == undefined) {
            FedProjectId = '';
        }

        //FED COST
        cost = getQueryParams.getUrlVars()["cost"];
        if (cost == undefined) {
            cost = '';
        }

        //Redirect to FED Param
        redirect = getQueryParams.getUrlVars()["r"];
        if (redirect == undefined) {
            redirect = 'y';
        }


        //handle Survey Status GUID
        var SurveyStatusGuid = getQueryParams.getUrlVars()["usg"];
        if (SurveyStatusGuid != undefined && SurveyStatusGuid != null) {
            if (SurveyStatusGuid.length > 36) {
                SurveyStatusGuid = SurveyStatusGuid.replace("0000", "");
            }

        }
        else if (SurveyStatusGuid.length == 32) {
            SurveyStatusGuid = ReplacestripGuid(SurveyStatusGuid);
        }

        //Handle User Invitatio GUID
        var SurveyInvitationGuid = getQueryParams.getUrlVars()["uig"];
        if (SurveyInvitationGuid != undefined && SurveyInvitationGuid != null) {
            if (SurveyInvitationGuid.length > 36) {
                SurveyInvitationGuid = SurveyInvitationGuid.replace("0000", "");
            }

        }
        else if (SurveyInvitationGuid.length == 32) {
            SurveyInvitationGuid = ReplacestripGuid(SurveyInvitationGuid);
        }

        else if (SurveyInvitationGuid.length < 32) {
            //Means if the Clients passed User Invitation ID instead of GUID.
            httpService.getData('/')
        }

        //Function to replace the Strip Guid to convert Proper Guid.
        var ReplacestripGuid = function (guid) {
            guid = guid.slice(0, 8), "-", guid.slice("-").join('');
            guid = guid.slice(0, 14), "-", guid.slice("-").join('');
            guid = guid.slice(0, 19), "-", guid.slice("-").join('');
            guid = guid.slice(0, 24), "-", guid.slice("-").join('');
            return guid;
        }

        var Redirect2Fed = function (redirecturl) {
            if (redirecturl.length > 0 && redirecturl != undefined) {
                window.location.href = redirecturl;
            }
            else {
                //Show a Message.
                $scope.Message = "Thank you for participating! Unfortunately the survey you''re attempting to access has been closed.";
            }
        }
        if (SurveyInvitationGuid != undefined && SurveyStatusGuid != undefined) {

            if (document.referrer != undefined && document.referrer != "") {
                //EndController Call to  update the Prelim status and 
                httpService.getData('/End/GetSurveyInvitationStatus?usg=' + SurveyStatusGuid + "&uig=" + SurveyInvitationGuid + "&cost=" + cost + "&fpid=" + FedProjectId + "&r=" + redirect).then(function (dataResponse) {
                    $scope.data = dataResponse;

                    //If the Member is River Member
                    if (data.IsRiver == 1) {
                        // We have the custom End Pages for River.
                        window.location = data.MemberUrl + '/river/rep.htm?usg=' + SurveyStatusGuid + '&uig=' + SurveyInvitationGuid;
                    }
                    else {
                        //Check If the Member is External Member.
                        if (data.TartgetTypeId == 2) {
                            if (data.UserInvitationStatusId != 1 && FedResposneId != undefined && FedResposneId != "") {
                                Redirect2Fed(data.FedRedirectURl);
                            }
                            else {
                                if (data.ProjectStatusId == 2 && data.PostbackURL != undefined &&
                                      (data.UserInvitationStatusId == 1 || data.UserInvitationStatusId == 2 || data.UserInvitationStatusId == 4)) {

                                    if (data.PixelTypeId == 2) {
                                        httpService.getData(data.PostbackURL).then(function (dataResponse) {
                                        });
                                    }
                                }

                                // If the Post back is Pixel Type and member has completed the survey.
                                if (data.ExtRedirectUrl != undefined) {
                                    if (data.PostbackURL != undefined && data.PixelTypeId == 1 && data.UserInvitationStatusId == 1) {
                                        //Fire Pixel. and we need to wait for 5sec.to fire the pxiel

                                        $scope.timermessage = 'You will be redirected in 5.4.3.2.1 seconds.';
                                        //To Fire Pixel.
                                        $scope.pixel = data.PostbackURL;
                                        $timeout(); // To Wait until 5 secs to fire pixels.
                                        window.location.href = data.ExtRedirectUrl; //then redirect the member to external redirect url.
                                    }
                                    else {
                                        window.location.href = data.ExtRedirectUrl;
                                    }
                                }
                                else {

                                    if (data.UserInvitationStatusId == 1) {
                                        //Diaply Message.
                                        $scope.Message = "Congratulations!  You’ve successfully completed this survey and will receive your reward.";
                                    }
                                    else if (data.UserInvitationStatusId == 2) {
                                        $scope.Message = "Thank you for participating!  Unfortunately you didn’t qualify to complete this survey.";
                                    }
                                    else if (data.UserInvitationStatusId == 4) {
                                        $scope.Message = "Sorry! the Quota for this survey was full";
                                    }
                                    else {
                                        $scope.Message = "Thank you for participating!  Unfortunately you didn’t qualify to complete this survey.";
                                    }
                                }


                            }

                        }
                            //If the Member is External Registraion Type.
                        else if (data.TartgetTypeId == 4) {

                            if (data.ProjectStatusId == 2) {

                                if (data.PostbackURL != undefined && (data.UserInvitationStatusId == 1 || data.UserInvitationStatusId == 2 || data.UserInvitationStatusId == 4)) {
                                    if (data.PixelTypeId == 1) //For Pixels. 
                                    {
                                        //Image / IFRAME pixel.
                                        httpService.getData(data.postbacktext).then(function (dataResponse) {
                                        });

                                        //We need try catch here.
                                    }
                                    else if (data.PixelTypeId == 2) //For Call backs
                                    {
                                        httpService.getData(data.PostbackURL).then(function (dataResponse) {
                                        });
                                    }
                                }

                                //Last Step for External Registration.
                                window.location.href = data.ExtRedirectUrl
                            }
                            else {
                                $scope.Message = "Thank you for participating! Unfortunately the survey you''re attempting to access has been closed.";

                            }
                        }

                        //If the Member is Demographics/Listed .so for all SDL/WL/API/Widget
                        else {
                            //If we have Fed Response Id.
                            if (data.UserInvitationStatusId != 1 && FedResposneId != undefined && FedResposneId != "") {
                                Redirect2Fed(data.FedRedirectURl);
                            }
                            else {
                                if(data.IsTop10Enable)
                                {
                                    if(data.IsTop10CompleteCheck)
                                    {

                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }

                    

                    }
                }

               )
            }
            else {

                //If the Session is Invlaid mostly, if the member did not reidrected from End client and borswe starightaway.

            }
        }
            //Invalid or Missing Status GUID/ Invitation GUID is found.
        else {

            //We need to show a message.
        }
    }]);
})