define(['app'], function (app) {
    debugger;
    app.register.controller('regController', ['$rootScope', '$scope', 'httpService', 'getQueryParams',
    function ($rootScope, $scope, httpService, getQueryParams) {
        $scope.ReplacestripGuid = '';
        FedResposneId = getQueryParams.getUrlVars()["fpid"];
        if (FedResposneId == undefined) {
            FedResposneId = '';
        }

        cost = getQueryParams.getUrlVars()["cost"];
        if (cost == undefined) {
            cost = '';
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

        if (SurveyInvitationGuid != undefined && SurveyStatusGuid != undefined) {
            //EndController Call to  update the Prelim status and 
            httpService.getData('/End/GetSurveyInvitationStatus?usg=' + SurveyStatusGuid + "&uig=" + SurveyInvitationGuid + "&cost=" + cost + "&fpid=" + FedResposneId).then(function (dataResponse) {
                $scope.data = dataResponse;

                if (data.IsRiver == 1) {
                    // We have the custom End Pages for River.
                    window.location = data.MemberUrl + '/river/rep.htm?usg=' + SurveyStatusGuid + '&uig=' + SurveyInvitationGuid;
                }
                    //If the Member is not an External Members, means ( May be a SDL/API/WL Member).
                else {
                    if (data.QuotaType == 1) {
                        if (data.Organizationtype == 'wl')//For all the WL Sites
                        {
                            if (data.ProjectStatusId == 2) {
                                window.location(data.MemberUrl + '/e/psr?usg=' + userstatusguid + "&uig=" + SurveyInvitationGuid + "&eid=" + data.FedresponseId + "&ug=" + data.UserGuid + "&status=" + data.UserInvitationStatusId + "&uid=" + data.UserId + "&project=" + data.ProjectId + "&r=" + Redirect);
                            }
                            else {
                                if (data.IsTop10CompleteCheck) {
                                    window.location(data.MemberUrl + '/mem/ms?usg=offernotexists&ug=' + data.UserGuid + "&uig=" + SurveyInvitationGuid + "&usid=" + data.UserId + "&project=" + data.ProjectId);
                                }
                                else {
                                    window.location(data.MemberUrl + 'EndPageTOp10PageUrl?ug=' + data.UserGuid + "&uig=" + SurveyInvitationGuid + "&usid=" + data.UserId.ToString() + "&project=" + data.ProjectId + "&usg=" + _userstatusguid);
                                }

                            }
                        }

                        else if (data.Organizationtype == "api" || data.Organizationtype == "Social") //For all API  &   --> Social  added on 1/22/2016 by sandy .
                        {
                            if (Redirect == "y" && data.UserInvitationStatusId != 1 && data.FedresponseId != Guid.Empty) {
                                RedirectToFedPages(oSurveys);
                            }
                            else {
                                if (data.ProjectStatusId == 2) {
                                    //Show TOp10page after Endpage. Added 06/13/2016
                                    if (data.IsTop10Enable) {
                                        if (data.IsTop10CompleteCheck) {
                                            window.location(data.MemberUrl + '/partner/newpsr?usg=' + _userstatusguid + "&uig=" + SurveyInvitationGuid + "&ug=" + data.UserGuid + "&usid=" + data.UserId + "&project=" + data.ProjectId);
                                        }
                                        else {
                                            window.location(data.MemberUrl + 'ApiPartnerTop10PageUrl?usg=' + _userstatusguid + "&uig=" + SurveyInvitationGuid + "&ug=" + data.UserGuid + "&usid=" + data.UserId + "&project=" + data.ProjectId);
                                        }
                                    }
                                    else {
                                        window.location(data.MemberUrl + "/partner/newpsr?usg=" + _userstatusguid + "&uig=" + SurveyInvitationGuid + "&ug=" + data.UserGuid + "&usid=" + data.UserId + "&project=" + data.ProjectId);
                                    }
                                }
                                else {
                                    //Show TOp10page after Endpage. Added 06/13/2016
                                    if (data.IsTop10Enable) {
                                        if (data.IsTop10CompleteCheck) {
                                            window.location(data.MemberUrl + '/partner/newpsr?usg=' + _userstatusguid + "&uig=" + SurveyInvitationGuid + "&ug=" + data.UserGuid + "&usid=" + data.UserId + "&project=" + data.ProjectId);
                                        }
                                        else {
                                            window.location(data.MemberUrl + 'ApiPartnerTop10PageUrl?usg=' + _userstatusguid + "&uig=" + SurveyInvitationGuid.ToString() + "&ug=" + oSurveys.UserGuid.ToString() + "&usid=" + oSurveys.UserId.ToString() + "&project=" + oSurveys.ProjectId);
                                        }
                                    }
                                    else {
                                        window.location(data.MemberUrl + '/partner/newpsr?usg=' + _userstatusguid + "&uig=" + SurveyInvitationGuid + "&ug=" + data.UserGuid + "&usid=" + data.UserId + "&project=" + data.ProjectId);
                                    }
                                    //Response.Redirect(oSurveys.MemberUrl + "/partner/newpsr.aspx?usg=F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4&uig=" + SurveyInvitationGuid.ToString() + "&ug=" + oSurveys.UserGuid.ToString());

                                }

                            }
                        }
                    }
                }
            }
            )
        }
    }]);
})