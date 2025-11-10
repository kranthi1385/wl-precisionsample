
define(['app'], function (app) {
    app.register.controller('extController', ['$scope', '$rootScope', '$window', '$timeout', 'httpService', 'getQueryParams',
   function ($scope, $rootScope, $window, $timeout, httpService, getQueryParams) {
     
       function getUrlVars() {
           var Url = window.location.href;
           var vars = {};
           var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
               vars[key] = value;
           });
           return vars;
       }

       // Reading all Params Stored in View Bag, to be used for RELEVANT API Call.
       var qg = getQueryParams.getUrlVars()['qig'];
       var mid = getUrlVars()['mid'];
       var pid = getQueryParams.getUrlVars()['pid'];
       var prjId = getQueryParams.getUrlVars()['prjid'];
       var source = getQueryParams.getUrlVars()['s'];
       var subId = getQueryParams.getUrlVars()['sub_id'];
       var rid = getQueryParams.getUrlVars()['rid'];
       var osId = getQueryParams.getUrlVars()['osId']
       var fedrespid = getQueryParams.getUrlVars()['frid'];
       var ecost = getQueryParams.getUrlVars()['ecost'];
       var e_rm = getQueryParams.getUrlVars()['rm'];
       var e_rl = getQueryParams.getUrlVars()['rl'];
       var pstest = getQueryParams.getUrlVars()['pstest'];       
       if (e_rm == undefined) { e_rm = ''; }
       if (e_rl == undefined) { e_rl = ''; }
       if (ecost == undefined) { ecost = 0; }
       if (qg == undefined) { qg = ''; }
       if (mid == undefined) { mid = ''; }
       if (pid == undefined) { pid = ''; }
       if (prjId == undefined) { prjId = ''; }
       if (source == undefined) {
           source = '';
       }
       if (subId == undefined) {
           subId = '';
       }
       if (rid == undefined) {
           rid = '';
       }
       if (osId == undefined) {
           osId = '';
       }

       if (fedrespid == undefined) {
           fedrespid = '';
       }
       var rvid = '';
       var score = '';
       var totalScore = ''; //releventscore + fpfscore
       var fpfScore = '';
       var rvScores = '';
       var uig = '';
       var prjTrafficTypeIds = '';
       var isMobile = 1;
       var deviceMatchedCount = 0;
       var isNew = 0;
       //Device Detection Logic:
       $scope.isDeviceFail = false;
       $scope.desktopsurvey = false;
       $scope.mobilesurvey = false;
       $scope.isMessageShow = 0;
       $scope.showMessage = false;
       $scope.isUsMember = false;
       $scope.isNonUsMember = false;
       $scope.showViewContent = false;
       $scope.mobileNumber = "";
       $scope.IsDupe = "";
       $scope.ExternalMemberId = "";
       $scope.ProjectId = "";
       $scope.OrgId = "";
       $scope.ExternalMemberGuid = "";

       //Invoke Relevant Methods.
       function callRVIDService() {
           populateInputFields();
           //$timeout(RVIDNoResponse, 20000); // 1000 = 1 second; suggested value 5000
           callRVIDNow();
       }

       //Insert External Member 
       function ExtInsert() {
         
           httpService.getData('/e/ExtInsert?mid=' + mid + "&pid=" + pid).then(function (response) {
             
               $scope.ExtMemdata = response;
           
               $scope.IsDupe = $scope.ExtMemdata.IsDupe;
               $scope.ExternalMemberId = $scope.ExtMemdata.ExternalMemberId;
               $scope.ProjectId = $scope.ExtMemdata.ProjectId;
               $scope.OrgId = $scope.ExtMemdata.OrgId;
               $scope.ExternalMemberGuid = $scope.ExtMemdata.ExternalMemberGuid;
               callRVIDService();
           }, function (err) {
               // $scope.errMsg = true;
           });
       }

       function populateInputFields() {
           document.getElementById('ClientID').value = '7DAB107F-8B12-4C44-A1E3-B569F6932EEA';
           if (mid != '' && mid != null) {
               document.getElementById('PanelistID').value = mid;
               document.getElementById('SurveyID').value = pid; //for Registration
           }
           else if (fedrespid != '') {
               document.getElementById('PanelistID').value = fedrespid;
               document.getElementById('SurveyID').value = pid; //for Registration
           }
           document.getElementById('GeoCodes').value = '1,' + "US";
           document.getElementById('TimePeriod').value = '';
       }
       //var result = $window.RVIDResponseComplete;
       //$scope.RVIDResponseComplete = result;
       window.RVIDResponseComplete = function () {
           // Client will implement appropriate redirect logic in this function
           // To access the various reponse parameters, use document.getElementById(“fieldName”)
           // Example: var RVID = document.getElementById(“RVid”).value;
           document.getElementById('RVIDCompleted').value = "1";
           rvId = document.getElementById('RVid').value;
           score = document.getElementById('Score').value;
           profileScore = document.getElementById('FraudProfileScore').value;
           if (document.getElementById('isNew').value.toLowerCase() == "true") {
               isNew = 1;
           }
           else {
               isNew = 0;
           }
           //  isNew = document.getElementById('isNew').value;
           fpfScore += document.getElementById('FPF1').value + ',';
           fpfScore += document.getElementById('FPF2').value + ',';
           fpfScore += document.getElementById('FPF3').value + ',';
           fpfScore += document.getElementById('FPF4').value + ',';
           fpfScore += document.getElementById('FPF5').value + ',';
           fpfScore += document.getElementById('FPF6').value + ',';
           fpfScore += document.getElementById('FPF7').value + ',';
           fpfScore += document.getElementById('FPF8').value + ',';
           fpfScore += document.getElementById('FPF9').value;
           totalScore += score + ';' + fpfScore + ';' + profileScore;
           saveRid();
           return true;
       }

      
       var RVIDNoResponse = function () {
       
           rvId = 'Relevant Fail';
           score = 0;
           totalScore = '0;0,0,0,0,0,0,0,0,0'
           saveRid();
       }

       function saveRid() {
       
           $scope.showMessage = false;
           // post user responses
           if (mid != undefined || pid != undefined) {
               httpService.postData("/e/ExtUpdate?qg=" + qg + "&mid=" + mid + '&pid=' + pid + '&rid=' + rid + '&source=' + source + '&subId=' + subId + '&isNew=' + isNew +
                          '&osId=' + osId + "&rvId=" + rvId + "&tscore=" + totalScore + '&frid=' + fedrespid + '&ecost=' + ecost + '&e_rm=' + e_rm + '&e_rl=' + e_rl + '&is_dupe=' + $scope.IsDupe + '&external_member_id=' + $scope.ExternalMemberId + "&project_id=" + $scope.ProjectId +"&org_id="+$scope.OrgId + "&external_member_guid=" + $scope.ExternalMemberGuid).then(function (data, status) {
                              if (data != undefined && data != '') { //if we get question then fetch the question else redirect to next page
                                  //debugger;
                                
                                  $scope.surveyResponse = data;
                                  if ($scope.surveyResponse.ProjectStatusId == 2) {
                                      if ($scope.surveyResponse.TargetTypeId == 4) {
                                          window.location.href = "https://www.surveydownline.com/er/1.aspx?mid=" + mid + "pid=" + pid;
                                      }
                                      else {
                                          // $scope.rdUrl = data.Redirecturl;
                                          // uig = data.InvitationGuid;
                                          prjTrafficTypeIds = data.SurveyUserTypeIds;
                                          if ($scope.surveyResponse.RedirectUrl == "") {
                                              //Find the device of the Member.
                                              if ($window.navigator.userAgent.match(/Android/i)
                                               || $window.navigator.userAgent.match(/webOS/i)
                                               || $window.navigator.userAgent.match(/iPhone/i)
                                               || $window.navigator.userAgent.match(/iPad/i)
                                               || $window.navigator.userAgent.match(/iPod/i)
                                               || $window.navigator.userAgent.match(/BlackBerry/i)
                                               || $window.navigator.userAgent.match(/Windows Phone/i)) {
                                                  //Mobile Device Detected.
                                                  isMobile = 2;
                                              }
                                              else {
                                                  //Non Mobile Device detected
                                                  isMobile = 3;
                                              }
                                              //we need to Split Project Level Traffic Type Ids and match with Current User Traffic Type.
                                              var trafficTypes = prjTrafficTypeIds.split(";")
                                              for (i = 0; i < trafficTypes.length; i++) {
                                                  //If the Project Both devices then 
                                                  if (trafficTypes[i] == 1) {
                                                      deviceMatchedCount = deviceMatchedCount + 1;
                                                  }
                                                  else
                                                      if (trafficTypes[i] == isMobile) {
                                                          deviceMatchedCount = deviceMatchedCount + 1;
                                                      }
                                              }
                                              //If the Mobile User is on Non Mobile Survey.
                                              if (deviceMatchedCount == 1 && isMobile == 2) {
                                                  if ($scope.surveyResponse.IsStandalone) {
                                                      window.location.href = "https://e.reachcollective.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + $scope.surveyResponse.ActualInvitationGuid + '&ug=' + ug + '&pid=' + prjId + '&cid=' + cid; //redirect to endpage 
                                                  }
                                                  else {
                                                      window.location.href = "widget.reachcollective.com/reg/home?ug=" + ug + "&cid=" + cid; //redirect to mobile survey page
                                                  }
                                              }

                                              //If Desktop User on a Mobile Survey.
                                              if (deviceMatchedCount == 1 && isMobile == 3) {
                                                  $scope.isDeviceFail = true;
                                                  $scope.showViewContent = true;
                                                  if ($scope.surveyResponse.CountyId == "231") {
                                                      if ($scope.surveyResponse.IsStandalone) { // check standalone partner
                                                          if ($scope.surveyResponse.IsEmailInvitationEnable == true || $scope.surveyResponse.IsSmsInvitation == false) //Standalone partner not having email invitation redirect to endpage. Added 06/15/2016
                                                          {
                                                              window.location.href = "https://e.reachcollective.com/e/psr?usg=50AD6CC9-9228-496F-B936-7D0E0973E60A&uig=" + $scope.surveyResponse.ActualInvitationGuid + '&ug=' + ug + '&pid=' + prjId + '&cid=' + cid; //redirect to endpage
                                                          }
                                                          showSms();
                                                      }
                                                      else {
                                                          showSms();
                                                      }
                                                  }
                                                  else {
                                                      $scope.isNonUsMember = true;
                                                  }
                                              }
                                          }
                                              //redirect to Next Page, if all the Validations are Correct
                                          else if (deviceMatchedCount == 0 && $scope.surveyResponse.RedirectUrl != "") {
                                              window.location.href = $scope.surveyResponse.RedirectUrl + '&project=' + $scope.surveyResponse.ProjectId;
                                          }
                                      }
                                  }
                                  else {
                                      if ($scope.surveyResponse.ExternalPartnerID == 231) {
                                          window.location.href = "https://e.reachcollective.com/e/psr?usg=F80A29B3-D4AD-42FD-82E5-53A6B47F9DB9&uig=" + $scope.surveyResponse.memberGuid; //redirect to endpage
                                      }
                                      if ($scope.surveyResponse.FedResponseID != "" && $scope.surveyResponse.FedResponseID != undefined) {
                                          window.location.href = "https://e.reachcollective.com/e/psr?usg=F80A29B3-D4AD-42FD-82E5-53A6B47F9DB9&uig=" + $scope.surveyResponse.ActualInvitationGuid + '&ug=' + ug + '&pid=' + prjId + '&cid=' + cid; //redirect to endpage 
                                      }
                                      else {
                                          $scope.isMessageShow = 4;
                                      }
                                  }
                              }
                          }, function (err) {

                              //Redirect the Member to Error End Page, based on Org we need to redirect the members to respective end pages.
                          });
           }
           else {
               $scope.isMessageShow = 3;
           }
       }
       function showSms() {
           //Sms not avaliable to the partner redirect to endpage
           if (!$scope.surveyResponse.IsSmsInvitation) {
               $scope.isUsMember = true;
               // $scope.isNonUsMember = true;
           }
           else {
               window.location.href = ""; //redirect to endpage
               //Response.Redirect(ConfigurationManager.AppSettings["EndPageUrl"].ToString() + "usg=50AD6CC9-9228-496F-B936-7D0E0973E60A&ug=" + UserGuiid.ToString() + "&uig=" + oSurvey.UserInvitationId.ToString() + "&usid=" + UserId + "&project=" + Project);
           }
       }
       $scope.sendsms = function (valid, mobileNo) {
           if (valid) {
               $scope.isMessageShow = 0;
               $scope.showMessage = false;
               $scope.mobileNumber = mobileNo;
               //Send SMS Logic.
               httpService.postData("/e/sendsms?uig=" + $scope.surveyResponse.UserInvitationId + "&mid=" + mid + '&pid=' + pid + "&mobileNum=" + $scope.mobileNumber +
                   '&surveyName=' + $scope.surveyResponse.SurveyName + '&orgId=' + $scope.surveyResponse.OrgId).then(function (data) {
                       if (parseInt(data) != 0) { //if we get question then fetch the question else redirect to next page
                           $scope.isMessageShow = 1;
                       }
                       else {
                           $scope.isMessageShow = 2;
                       }
                   }, function (err) {

                   });

           }
           else {
               $scope.showMessage = true;
           }
       }

       $scope.relevantInitialize = function () {
       
           ExtInsert();
       };
   }])
});

