//declare Name of the namespace
var that = {};
var ug = ''; //User Guid
var panelistid = ''; //UserId
var Country = '';
var lpage = '';  //Landing page Name
var s = ''; //User Coming from email or web
var totalscore = ''; //releventscore + fpfscore
var fpfScore = '';
var usid = ''; // userId for pr/sc2 page
var isNew = ''; //Check the Dupliates 
var rvid = 0;
var profileScore = 0;
var Score = 0;
var fpfScore = 0;
var isNew = ''

$(document).ready(function() {
    $('#dvImageLoading').show();
    that.init();
});

that.init = function() {

    ug = getUrlVars()["ug"];
    lpage = getUrlVars()["lpage"];
    panelistid = getUrlVars()["id"];
    Country = getUrlVars()["c"];
    usid = getUrlVars()["usid"];
    rid = getUrlVars()["rid"];
    txid = getUrlVars()["txid"];
    var rdjson = '';
    if (panelistid == '' || panelistid == null)
        panelistid = txid;

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

    // Getdata();
}
// to get URL params
function getUrlVars() {
    var Url = window.location.href.toLowerCase();
    var vars = {};
    var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value) {
        vars[key] = value;
    });
    return vars;
}

function getUrlVarsForMID() {
    var Url = window.location.href;
    var vars = {};
    var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value) {
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
        document.getElementById('SurveyID').value = rid; //for Registration
    }


    document.getElementById('GeoCodes').value = '1,' + Country;
  
    document.getElementById('TimePeriod').value = '';
 
}


function RVIDResponseComplete() {

    // Client will implement appropriate redirect logic in this function
    // To access the various reponse parameters, use document.getElementById(“fieldName”)
    // Example: var RVID = document.getElementById(“RVid”).value;
    document.getElementById('RVIDCompleted').value = "1";
    var rvid = document.getElementById('RVid').value;
    var Score = document.getElementById('Score').value;
    var profileScore = document.getElementById('FraudProfileScore').value;
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



    if (ug != '' && ug != null && lpage != '' && lpage != null) {
        RvidSave(rvid, profileScore, Score, fpfScore, isNew);
    }

}


function RVIDNoResponse() {
    // Client should implement appropriate logic in case response is not received within given time period
    // Make sure that the RVIDResponseComplete() method has not already been executed
    if (document.getElementById('RVIDCompleted').value == "0") {
        $('#dvImageLoading').hide();

        if (ug != '' && ug != null && lpage != '' && lpage != null) {
            window.location.href = '/river/s2.htm?ug=' + ug;
        }
    }
}


// Logic to populate input fields intentionally left outside common library since clients may want to
// implement differently. By default, the input fields will be populated from query strings params.
function callRVIDService() {

    //populateInputFields();
    //setTimeout("RVIDNoResponse();", 5000); // 1000 = 1 second; suggested value 5000
    //callRVIDNow();
    //var url = "https://prod.rtymgt.com/api/v2/respondents/search/50946766-ef1c-4060-b967-7e63161232b4?sn_ud=" + ug + "&sy_nr=" + 110 + "&coordinates=1&wd=1";
    //var xhr = new XMLHttpRequest();
    //xhr.open('GET', url, true);
    //xhr.withCredentials = true;
    //xhr.send(null);
    ////alert('hi1');     
    //xhr.onreadystatechange = (e) => {
    //    if (xhr.readyState == 4) {
    //        // alert('hi2');
    //        //alert(xhr.responseText);
    //        if (xhr.responseText != null && xhr.responseText != "") {
    //            rdjson = xhr.responseText;
    //            RvidSave(rvid, profileScore, Score, fpfScore, isNew, rdjson);
    //            //var url = "/cr/RdjsonInsert?userid=" + usid + "&uid=" + uid + '&json=' + xhr.responseText + "&cid=" + cid + '&uig=' + uig;
    //            // httpService.postData(url).then(function (data) {
    //            //if (data != "") {
    //            //    window.location.href = data;
    //            //} else {
    //            //    window.location.href = "/reg/pii?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&tid=" + tid + "&usid=" + usid;
    //            //}
    //            // }, function (err) {
    //            //Redirect the Member to Error End Page, based on Org we need to redirect the members to respective end pages.
    //            //});
    //        }
    //    }
    //}
    //var token = "https://prod.rtymgt.com/api/v3/respondents/get_token/8f34a44d-9bba-43bb-911b-b4a466cf4b05";
    //var xhr = new XMLHttpRequest();
    //xhr.open('GET', token, true);
    //xhr.withCredentials = true;
    //xhr.send(null);
    //xhr.onreadystatechange = (e) => {
    //    if (xhr.readyState == 4) {
    //        if (e.currentTarget.status == 200) {
    //            if (e.currentTarget.responseText != null && e.currentTarget.responseText != "") {
    //                result = JSON.parse(e.currentTarget.responseText);
    //                token = result.results[0].token;
    //                RvidSave(rvid, profileScore, Score, fpfScore, isNew, token);
    //            }
    //        }
    //        else {
    //            token = null;
    //            RvidSave(rvid, profileScore, Score, fpfScore, isNew, token);
    //        }
    //    }
    //}
    var cleanId = "";
    IDSuite.cleanid({
        RequestId: uig,
        EventId: pid,
        ChannelId: cid,
        GeoRestrictionEnabled: false,
        FullDataSet: false,
        onSuccess: (res) => {
            //handle your response data here
            cleanId = JSON.stringify(res);
            RvidSave(rvid, profileScore, Score, fpfScore, isNew, cleanId);
        },
        onError: (res) => {
            RvidSave(rvid, profileScore, Score, fpfScore, isNew, cleanId);
        }
    });
}

//logic to save the relevant data into database
function RvidSave(rvid, profileScore, Score, fpfScore, isNew, cleanId) {
    $.ajax({
        datatype: 'json',
        type: 'POST',
        url: '/services/RiverService.aspx?Mode=SaveRiddata&ug=' + ug + '&score=' + Score + '&rid=' + rvid + '&pscore=' + profileScore + '&fpfscores=' + fpfScore + '&isnew=' + isNew + '&cleanIDJson=' + cleanId,
        cache: false,
        async: false,
        success: function(pagedata) {

            $('#dvImageLoading').hide();

            window.location.href = '/river/s2.htm?ug=' + ug;
        }
    });
}