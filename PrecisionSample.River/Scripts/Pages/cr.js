//declare Name of the namespace
var that = {};
var ug = ''; //User Guid
var panelistid = ''; //UserId
var Country = '';
var lpage = '';  //Landing page Name
var mid = ''; //External Member Id
var pid = ''; //External Member Quota ID
var project = ''; //ProjectId
var sid = ''; //SurveyInvitationGuid
var qid = ''; //QuotaGroupGuid
var s = ''; //User Coming from email or web
var totalscore = ''; //releventscore + fpfscore
var fpfScore = '';
var Score = 0; //set to 0 for research defender
var rvid = ''; //set to 0 for research defender
var profileScore = 0; //set to 0 for research defender
var usid = ''; // userId for pr/sc2 page
var isNew = ''; //Check the Dupliates
var token = ''; //research defender json
var cleanID = ''; // clean id json
$(document).ready(function () {
    $('#dvImageLoading').show();
    that.init();
});

that.init = function () {

    ug = getUrlVars()["ug"];
    lpage = getUrlVars()["lpage"];
    panelistid = getUrlVars()["id"];
    Country = getUrlVars()["c"];
    mid = getUrlVarsForMID()["mid"];
    pid = getUrlVars()["pid"];
    project = getUrlVars()["project"];
    sid = getUrlVars()["sid"];
    qid = getUrlVars()["qid"];
    s = getUrlVars()["s"];
    usid = getUrlVars()["usid"];

    if (lpage == undefined) {
        lpage = '';
    }
    if (ug == undefined) {
        ug = '';
    }
    if (panelistid == undefined) {
        panelistid = '';
    }
    if (Country == undefined) {
        Country = '';
    }
    if (pid == undefined) {
        pid = '';
    }
    if (project == undefined) {
        project = '';
    }
    if (mid == undefined) {
        mid = '';
    }
    if (sid == undefined) {
        sid = '';
    }
    if (qid == undefined) {
        qid = '';
    }
    if (s == undefined) {
        s = '';
    }
    if (usid == undefined) {
        usid = '';
    }
    //$('#btnSubmit').click(function () {
    //    var captcha_response = grecaptcha.getResponse();
    //    if (captcha_response.length == 0) {
    //        // Captcha is not Passed
    //        $("#lblResult").show();
    //    }
    //    else {
    //        $("#lblResult").hide();
    //        // Captcha is Passed
    //        GetSurveyURl();
    //    }

    //});
    // Getdata();
}
// to get URL params
function getUrlVars() {
    var Url = window.location.href.toLowerCase();
    var vars = {};
    var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}

function getUrlVarsForMID() {
    var Url = window.location.href;
    var vars = {};
    var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}

//

function getQueryStringParam(param) {
    querySt = window.location.search.substring(1);
    queryStringArray = querySt.split("&");
    for (i = 0; i < queryStringArray.length; i++) {
        ft = queryStringArray[i].split("=");
        if (ft[0].toLowerCase() == param.toLowerCase()) {
            return ft[1];
        } // end if
    } // end for
    return ""; // not found in query string params
} // end function


function populateInputFields() {

    document.getElementById('ClientID').value = '7DAB107F-8B12-4C44-A1E3-B569F6932EEA';
    if (panelistid != '' && panelistid != null) {
        document.getElementById('PanelistID').value = panelistid;
        document.getElementById('SurveyID').value = 1; //for Registration
    }
    else if (mid != '' && mid != null && pid != '' && pid != null) { //For External Members
        document.getElementById('PanelistID').value = mid;
        document.getElementById('SurveyID').value = pid;
    }
    else if (usid != '' && usid != null && project != '' && project != null) { //For Internal Members Survey Click Page.
        document.getElementById('PanelistID').value = usid;
        document.getElementById('SurveyID').value = project;
    }

    document.getElementById('GeoCodes').value = '1,' + Country;
    document.getElementById('CID').value = '';
    document.getElementById('TID').value = '';
    document.getElementById('TimePeriod').value = '';
    document.getElementById('PropertyList').value = '';
}


function RVIDResponseComplete() {

    // Client will implement appropriate redirect logic in this function
    // To access the various reponse parameters, use document.getElementById(“fieldName”)
    // Example: var RVID = document.getElementById(“RVid”).value;
    document.getElementById('RVIDCompleted').value = "1";
    rvid = document.getElementById('RVid').value;
    Score = document.getElementById('Score').value;
    profileScore = document.getElementById('FraudProfileScore').value;
    isNew = document.getElementById('isNew').value;
    fpfScore += document.getElementById('FPF1').value + ',';
    fpfScore += document.getElementById('FPF2').value + ',';
    fpfScore += document.getElementById('FPF3').value + ',';
    fpfScore += document.getElementById('FPF4').value + ',';
    fpfScore += document.getElementById('FPF5').value + ',';
    fpfScore += document.getElementById('FPF6').value + ',';
    fpfScore += document.getElementById('FPF7').value + ',';
    fpfScore += document.getElementById('FPF8').value + ',';
    fpfScore += document.getElementById('FPF9').value;
    totalscore += Score + ';' + fpfScore;
    $('#dvImageLoading').hide();
    if (ug != '' && ug != null && project != '' && project != null && qid != '' && qid != null) {
        InsertUserInvitation();
        // $('#dvContent').show();
    }
    else {
        window.location.href = '/river/rep.htm?ug=' + ug + '&usg=F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4' + '&uig=00000000-0000-0000-0000-000000000000';
    }
}
//Rdjson
//function RDService() {
//    //var url = "https://prod.rtymgt.com/api/v2/respondents/search/50946766-ef1c-4060-b967-7e63161232b4?sn_ud=" + ug + "&sy_nr=" + project + "&rt_sr_pd=" + qid + "&coordinates=1&wd=1";
//    //var xhr = new XMLHttpRequest();
//    //xhr.open('GET', url, true);
//    //xhr.withCredentials = true;
//    //xhr.send(null);
//    //alert('hi1');     
//    //xhr.onreadystatechange = (e) => {
//    //    if (xhr.readyState == 4) {
//    //        rdJson = xhr.responseText;
//    //        InsertUserInvitation();
//    //    }
//    //}
//    var passParam = "";
//    var geoLonLat = "";
//    var token = "https://prod.rtymgt.com/api/v3/respondents/get_token/8f34a44d-9bba-43bb-911b-b4a466cf4b05";
//    var xhr = new XMLHttpRequest();
//    xhr.open('GET', token, true);
//    xhr.withCredentials = true;
//    xhr.send(null);
//    xhr.onreadystatechange = (e) => {
//        if (xhr.readyState == 4) {
//            if (e.currentTarget.status == 200) {
//                if (e.currentTarget.responseText != null && e.currentTarget.responseText != "") {
//                    result = JSON.parse(e.currentTarget.responseText);
//                    token = result.results[0].token;
//                    debugger;
//                    let stateCheck = setInterval(() => {
//                        passParam = localStorage.getItem('passParam');
//                        geoLonLat = localStorage.getItem('geoLonLat');
//                        clearInterval(stateCheck);
//                        InsertUserInvitation(token, passParam, geoLonLat);
//                    }, 1000)
//                }
//            }
//            else {
//                token = null;
//                InsertUserInvitation(token, passParam, geoLonLat);
//            }
//        }
//    }
//}

function RVIDNoResponse() {
    // Client should implement appropriate logic in case response is not received within given time period
    // Make sure that the RVIDResponseComplete() method has not already been executed
    //if (document.getElementById('RVIDCompleted').value == "0") {

    $('#dvImageLoading').hide();
    if (ug != '' && ug != null && project != '' && project != null && qid != '' && qid != null) {
        //IDSuite.cleanid({
        //    RequestId: uig,
        //    EventId: pid,
        //    ChannelId: cid,
        //    GeoRestrictionEnabled: false,
        //    FullDataSet: false,
        //    onSuccess: (res) => {
        //        debugger;
        //        //handle your response data here
        //        cleanID = JSON.stringify(res);
        //        InsertUserInvitation(cleanID);
        //        //setTimeout("RDService();", 3000); // 1000 = 1 second; suggested value 5000
        //    },
        //    onError: (res) => {
        //        InsertUserInvitation(cleanID);
        //        //setTimeout("RDService();", 3000); // 1000 = 1 second; suggested value 5000
        //    }
        //});
        InsertUserInvitation(cleanID);
    }
    else {
        window.location.href = '/river/rep.htm?ug=' + ug + '&usg=F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4' + '&uig=00000000-0000-0000-0000-000000000000';
    }
    //}
}


// Logic to populate input fields intentionally left outside common library since clients may want to
// implement differently. By default, the input fields will be populated from query strings params.
function callRVIDService() {

    // populateInputFields();
    //setTimeout("RVIDNoResponse();", 5000); // 1000 = 1 second; suggested value 5000
    setTimeout("RDService();", 5000);
    //callRVIDNow();
}

var InsertUserInvitation = function (cleanID) {
    $.ajax({
        datatype: 'json',
        type: 'POST',
        url: '/services/RiverService.aspx?Mode=InsertUserInvitation&ug=' + ug + '&score=' + Score + '&rid=' + rvid + '&pscore=' + profileScore + '&fpfscores=' + fpfScore + '&project=' + project + '&qgid=' + qid + '&isNew=' + isNew + '&cleanIDJson=' + cleanID,
        success: function (pagedata) {
            $('#dvImageLoading').hide();
            window.location.href = pagedata;
        }
    });
}

