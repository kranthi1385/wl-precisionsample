var GetPartner = function () {
    var that = {};
    ViewModel = ko.observableArray();
    var Count = 0;
    var statelist = [];
    var countrylist = [];
    var Ethinicity = [];
    var YearList = [];
    var userid = '';
    var day = '';
    var month = '';
    var year = '';
    var CountryId = '';
    var GeoIp2Res = '';
    // to get URL params
    function getUrlVars() {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }
    //Get query parmas value referrer id
    ug = getUrlVars()["ug"];
    if (ug == undefined) {
        ug = ''
    }

    //Get query parmas value referrer id
    rid = getUrlVars()["rid"];
    if (rid == undefined || rid == '') {
        _refercheck = 1;
        rid = 14402
    }

    sid = getUrlVars()["sid"];
    if (sid == undefined) {
        sid = ''
    }

    txid = getUrlVars()["txid"];
    if (txid == undefined) {
        txid = ''
    }

    //----------------------init function-----------------------------------------------
    var initevents = function () {
        //button click event
        $("#imgButton").click(function () {
            SaveDetails();
        });
    }

    //Bind ViewModel
    var BindData = function () {
        $.ajax({
            url: '/services/RiverService.aspx?Mode=BindData',
            timeout: 50000,
            datatype: 'json',
            type: 'GET',
            async: false,
            success: function (data) {
                if (data != null && data != '') {
                    ViewModel = ko.mapping.fromJS(data);
                    ko.applyBindings(ViewModel, document.getElementById("border"));
                }
            }

        });
    }

    //Load Year DropDown Dynamically
    function LoadYear() {
        var currentTime = new Date();
        var currentYear = currentTime.getFullYear();
        $('#ddlYear').append("<option value=-1>--</option>");
        for (var i = 13; i < 100; i++) {
            $('#ddlYear').append("<option value='" + (currentYear - i) + "'>" + (currentYear - i) + "</option>");
            YearList.push(currentYear - i);
        }

    }
    //Load country and states
    function LoadCountrysAndStates() {
        $.ajax({
            url: '/services/RiverService.aspx?Mode=GetCountryandStates',
            timeout: 50000,
            datatype: 'json',
            type: 'GET',
            async: false,
            success: function (data) {
                if (data == null) {
                }
                else {
                    countrylist = data.CountryList;
                    statelist = data.StateList;
                    $('#ddlCountry').empty();
                    $('#ddlCountry').append("<option value=-1>--Select--</option>");
                    for (var i = 0; i < data.CountryList.length; i++) {
                        $("#ddlCountry").append("<option value='" + data.CountryList[i].CountryId + "'>" + data.CountryList[i].CountryName + "</option>");
                    }
                }
            }

        });
    }

    //ethnicity bind

    function BindEthinicity() {
        $.ajax({
            url: '/services/RiverService.aspx?Mode=GetEthinicity',
            timeout: 5000,
            datatype: 'json',
            type: 'POST',
            async: false,
            success: function (data) {
                if (data == null || data == "") {
                }
                else {
                    Ethinicity = data;
                    $('#ddlEthnicity').empty();
                    $('#ddlEthnicity').append("<option value=-1>--Select--</option>");
                    for (var i = 0; i < data.length; i++) {
                        $("#ddlEthnicity").append("<option value='" + data[i].EthnicityId + "'>" + data[i].EthnicityType + "</option>");

                    }

                }
            }
        });
    }

    //change function for dropdown

    function DropDownChange() {
        $('#ddlStates').empty();
        var CountryId = $('#ddlCountry option:selected').val();
        $('#ddlStates').append("<option value=-1>-Select-</option>");
        for (var i = 0; i < statelist.length; i++) {
            if (CountryId == statelist[i].CountryId) {
                $("#ddlStates").append("<option value='" + statelist[i].StateId + "'>" + statelist[i].StateName + "</option>");
            }

        }
    }

    //Check For Validations

    function Vaildations() {
        var sum = 0;
        $('.error').hide();
        var isValid = true;

        var Country = $("#ddlCountry option:selected").val();

        var Gender = $("#ddlGender option:selected").val();
        var Month = $("#ddlMonth option:selected").val();
        var Day = $("#ddlDay option:selected").val();
        var Year = $("#ddlYear option:selected").val();
        var Eaddress = $('#txtEmail').val();
        var Zipcode = $('#txtZip').val();
        var Ethnicity = $("#ddlEthnicity option:selected").val();
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;

        if ((Eaddress == '' || Eaddress == null) && rid != 14420 && rid != 14666) {
            isValid = false;
            $('#txtEmail').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
        }
        else if ((!emailReg.test(Eaddress)) && rid != 14420 && rid != 14666) {
            $("#txtEmail").after('<span style="color:Red;font-family: Verdana;font-size: 12px;" class="error">&nbsp;Invalid email address.</span>');
            isValid = false;
        }
        if (Country == -1) {
            isValid = false;
            $('#ddlCountry').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
        }

        if (Zipcode == '' || Zipcode == null) {
            isValid = false;
            $('#txtZip').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
        }
        if (Gender == -1) {
            isValid = false;
            $('#ddlGender').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
        }

        if (Month == '' || Month == null) {
            if (sum == 0) {
                isValid = false;
                $('#ddlYear').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
                sum = 1;
            }
        }
        if (Month == -1) {
            if (sum == 0) {
                isValid = false;
                $('#ddlYear').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
                sum = 1;
            }
        }
        if (Day == '' || Day == null) {
            if (sum == 0) {
                isValid = false;
                $('#ddlYear').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
                sum = 1;
            }
        }
        if (Day == -1) {
            if (sum == 0) {
                isValid = false;
                $('#ddlYear').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
                sum = 1;
            }
        }
        if (Year == '' || Year == null || Year == 'undefined') {
            if (sum == 0) {
                isValid = false;
                $('#ddlYear').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
                sum = 1;
            }
        }
        if (Year == -1) {
            if (sum == 0) {
                isValid = false;
                $('#ddlYear').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
            }
            sum = 1;
        }
        if (Ethnicity == -1) {
            isValid = false;
            $('#ddlEthnicity').after('<span style="color:red;padding-left:10px;font-family: Verdana;font-size: 12px;" class="error">Required</span>');
        }
        return isValid;

    }

    //get member reg details
    function GetMemberDetails() {
        $.ajax({
            url: '/services/RiverService.aspx?Mode=GetMemberDetails&ug=' + ug,
            timeout: 50000,
            datatype: 'json',
            type: 'POST',
            async: false,
            success: function (data) {
                if (data == null) {
                }
                else {
                    if (data.EmailAddress != '' && data.CountryCode != '' && data.Gender != '' && data.Dob != '' && data.ZipCode != '' && data.EthnicityId != 0 && data.EthnicityId != -1 && data.Address1 == "0") {
                        window.location.href = '/river/top10.htm?ug=' + ug;
                    }
                    else {
                        if (data.Address1 == "0" || data.Address1 == "1") {
                            data.Address1 = "";
                        }
                        ViewModel = ko.mapping.fromJS(data);
                        var date = data.Dob;
                        var splitdate = date.split(' ');
                        var dateparts = splitdate[0].split('/');
                        if (dateparts[2] != "1900") {
                            ViewModel.Year = dateparts[2];
                            ViewModel.Month = dateparts[0];
                            ViewModel.Day = dateparts[1];
                        }
                        ViewModel.ZipCode = "";
                        ko.applyBindings(ViewModel, document.getElementById("border"));
                    }
                }
            }

        });

    }

    //Save member alldetails
    var SaveDetails = function () {
        $('#dvImageLoading').show();
        //added google captcha on 14-6-2017
        var captcha_response = grecaptcha.getResponse();
        if (captcha_response.length == 0) {
            // Captcha is not Passed
            $("#lblResult").show();
            $('#dvImageLoading').hide();
        }
        else {
            var date = $("#ddlMonth option:selected").val() + '/' + $("#ddlDay option:selected").val() + '/' + $("#ddlYear option:selected").val();
            if (Vaildations()) {
                ViewModel.GeoIp2Content = GeoIp2Res;
                $.ajax({
                    url: '/services/RiverService.aspx?Mode=SaveUserDetails&date=' + date,
                    timeout: 50000,
                    datatype: 'json',
                    type: 'POST',
                    async: false,
                    data: ko.toJSON(ViewModel),
                    success: function (data) {
                        if (data != null && data != '') {
                            $('#dvImageLoading').hide();
                            window.location.href = '/River/top10.htm?ug=' + data;
                        }
                        else {
                            $('#dvImageLoading').hide();
                        }
                    },
                    error: function (error) {
                        $('#dvImageLoading').hide();
                    }
                });

            }
            else {
                $('#dvImageLoading').hide();
            }
        }
    }

    // insert clicks
    var insertclicks = function () {
        $.ajax({
            url: '/services/RiverService.aspx?Mode=InsertClickes&rid=' + rid + '&sid=' + sid + '&txid=' + txid,
            timeout: 50000,
            datatype: 'json',
            type: 'POST',
            async: false,
            success: function (data) {
            },
            error: function (er) {
            }
        });
    }

    //inser user
    var InsertUser = function () {

        $.ajax({
            url: '/services/RiverService.aspx?Mode=FBInsertUserDetails&rid=' + rid + '&sid=' + sid + '&txid=' + txid,
            timeout: 50000,
            datatype: 'json',
            type: 'POST',
            async: false,

            success: function (data) {
                if (data != null && data != '') {
                    ug = data.split(';')[0];
                }
            }
        });
    }


    //maxmind go ip2 check success callback
    var onSuccess = function (location) {
        GeoIp2Res = JSON.stringify(location)

    };

    //maxmind go ip2 check error callback
    var onError = function (error) {
        GeoIp2Res = "";
    };

    that.init = function () {

        geoip2.city(onSuccess, onError);

        /* Change done on 10/12/2015 By Madhavi */

        if (rid != '') {

            if (rid == 14420 || rid == 14666) {
                insertclicks();
                $("#dvEmailAddress").hide();
                InsertUser();
            }

        }



        /* ************************************* */

        LoadCountrysAndStates();
        LoadYear();
        BindEthinicity();
        //dropdown change function
        $('#ddlCountry').change(function (e) {
            DropDownChange()
        });
        if (ug != '') {
            GetMemberDetails();
        }
        else {
            BindData();
        }
        initevents();

    }
    return that;
}();
$(document).ready(function () {
    var imgArr = new Array( // relative paths of images
    '/Images/background-1.jpg',
    '/Images/background-2.jpg',
    '/Images/background-3.jpg',
    '/Images/background-4.jpg'
    );
    var preloadArr = new Array();
    var i; /* preload images */
    for (i = 0; i < imgArr.length; i++) {
        preloadArr[i] = new Image();
        preloadArr[i].src = imgArr[i];
    }
    var currImg = 1;
    var intID = setInterval(changeImg, 6000); /* image rotator */function changeImg() {
        $('.cp-bg').animate(1000, function () {
            $(this).css('background-image', 'url(' + preloadArr[currImg++ % preloadArr.length].src + ' )');
        }).animate(1000);
    }
    if ($.browser.msie) {
        if ($.browser.version <= 8.0) {

            $("#lnkMainCss").attr("href", "/css/river-mainIE.css");
        }
    }
    GetPartner.init();
});