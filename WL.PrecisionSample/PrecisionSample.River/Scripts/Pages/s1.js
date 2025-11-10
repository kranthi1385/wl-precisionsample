var GetMember = function () {
    var that = {};
    ViewModel = ko.observableArray();
    var Count = 0;
    var _isEmailXverify = false;
    var _isValid = false;
    var _emailAddress = '';
    var _refercheck = 0;
    // to get URL params
    function getUrlVars() {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

    function getUrlVars1() {
        var Url = window.location.href;
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }
    //Get query parmas value referrer id
    rid = getUrlVars()["rid"];

    if (rid != '' && rid != undefined) {
        if (rid.indexOf("?fn=") != -1) {
            rid = rid.replace("?fn=", "");
        }

    }


    if (rid == undefined || rid == '') {
        _refercheck = 1;
        rid = 14402
    }


    sid = getUrlVars()["sid"];

    if (sid != '' && sid != undefined) {
        if (sid.indexOf("?fn=") != -1) {
            sid = sid.replace("?fn=", "");
        }
        if (sid == undefined) {
            sid = ''
        }
    }

    //Need to Save the trasnsaction ID of each survey Click/Registraion added by sandy on 2/14/2107
    transactionid = getUrlVars1()["trans_id"]
    if (transactionid != '' && transactionid != undefined) {

        if (transactionid.indexOf("?fn=") != -1) {
            transactionid = transactionid.replace("?fn=", "");
        }
        if (transactionid == undefined) {
            transactionid = ''
        }
    }

    txid = getUrlVars1()["txid"];
    if (txid != '' && txid != undefined) {

        if (txid.indexOf("?fn=") != -1) {
            txid = txid.replace("?fn=", "");
        }
        if (txid == undefined) {
            txid = ''
        }
    }


    var insertclicks = function () {
        $.ajax({
            url: '/services/RiverService.aspx?Mode=InsertClickes&rid=' + rid + '&sid=' + sid + '&txid=' + txid + '&trans_id=' + transactionid,
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


    //----------------------init function-----------------------------------------------
    var initevents = function () {
        //button click event
        $("#btnSave").click(function () {
            SaveDetails();
        });
        $("#txtEmail").blur(function () {
            //debugger;
            CheckEmailAddress();
        });
    }


    //------- check email address via x-verify ---------
    function CheckEmailAddress() {
        // debugger;
        $('.xverifyerror').hide();
        $('#dvLoading').show();
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        var Ea = $("#txtEmail").val();
        if (emailReg.test(Ea)) {
            $.ajax({
                url: '/services/RiverService.aspx?Mode=CheckEmailAddress&ea=' + Ea,
                timeout: 50000,
                datatype: 'json',
                type: 'POST',
                async: false,
                success: function (data) {
                    debugger;
                    if (data == 'accepted') {
                        _isEmailXverify = true;
                        $("#txtEmail").after('<span style="color:green;font-family: Verdana;font-size: 12px;" class="xverifyerror">&nbsp;Email address verified.</span>');
                    }
                    else if (data == 'rejected') {
                        $("#txtEmail").after('<span style="color:red;font-family: Verdana;font-size: 12px;" class="xverifyerror">&nbsp;Email address does not exist.</span>');
                    }
                    else if (data == 'existed') {
                        $("#txtEmail").after('<span style="color:red;font-family: Verdana;font-size: 12px;" class="xverifyerror">&nbsp;Email address already exist.</span>');
                    }

                    else {
                    }
                },
                error: function (data) {
                }
            });
        }
        else {
            $("#txtEmail").after('<span style="color:red;font-family: Verdana;font-size: 12px;" class="xverifyerror">&nbsp;Invalid email address.</span>');
        }
    }

    //Check For Validations

    function Vaildations() {
        var sum = 0;
        $('.error').hide();
        var isValid = true;
        var eAddress = $('#txtEmail').val();
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;

        if (eAddress == '' || eAddress == null) {
            isValid = false;
            //            $('#spnEmailErr').show();
            //            $('#spnEmailErr').text('Required');
            $("#txtEmail").after('<span style="color:Red;font-family: Verdana;font-size: 12px;" class="error errors1">&nbsp;Required</span>');
        }
        else if (!emailReg.test(eAddress)) {
            //            $('#spnEmailErr').show();
            //            $('#spnEmailErr').show('Invalid emailaddress.');
            $("#txtEmail").after('<span style="color:Red;font-family: Verdana;font-size: 12px;" class="error errors1">&nbsp;Invalid emailaddress.</span>');
            isValid = false;
        }

        return isValid;
    }

    //Insert member alldetails
    var SaveDetails = function () {
        $('#dvImgLoading').show();
        $('#dvError').hide();
        $('.error').hide();
        var Email = $('#txtEmail').val();
        if (_isEmailXverify) {
            $('.xverifyerror').hide();
            if (Vaildations()) {
                if (EmailExistsCheck(Email)) {
                    $.ajax({
                        url: '/services/RiverService.aspx?Mode=FBInsertUserDetails&Email=' + Email + '&rid=' + rid + '&sid=' + sid + '&txid=' + txid + '&trans_id=' + transactionid,
                        timeout: 50000,
                        datatype: 'json',
                        type: 'POST',
                        async: false,
                        data: ko.toJSON(ViewModel),
                        success: function (data) {
                            if (data != null && data != '') {
                                $('#dvError').hide();
                                $('#dvImgLoading').hide();
                                if (rid == 14666) {
                                    //we need to Skip Relevant For We-tell SMS Campaign.
                                    window.location.href = '/river/s2.htm?ug=' + data.split(';')[0];
                                }
                                else {
                                    // we are not doing relevant for River members on the Registration instead we are doing it on Survey Click,
                                    //window.location.href = '/River/relevant.htm?ug=' + data.split(';')[0] + '&lpage=home' + '&id=' + Email + "c=" + data.split(';')[1] + '&rid=' + rid + '&txid=' + txid;
                                    window.location.href = '/river/s2.htm?ug=' + data.split(';')[0];
                                }
                            }
                            else {
                                // $('#dvError').show();
                                $('#dvImgLoading').hide();
                            }
                        },
                        error: function (error) {
                            //                        $('#dvError').show();
                            //                        $('#dvError').html(' Email Address already exists!');
                            $('#dvImgLoading').hide();
                        }
                    });
                }
                else {
                    $('#dvImgLoading').hide();
                }
            }
            else {
                $('#dvImgLoading').hide();
                //$('#txtEmail').after("<span style='color:red;margin-left:2px;font-size: 12px;' class='error'>Required</span>"); ;
            }
        }
        else {
            if (Email == "") {
                $('.xverifyerror').hide();
                $("#txtEmail").after('<span style="color:red;font-family: Verdana;font-size: 12px;" class="xverifyerror">&nbsp;Invalid email address.</span>');
            }

        }
    }

    var EmailExistsCheck = function (_email) {
        $('#dvImgLoading').show();

        $.ajax({
            url: '/services/RiverService.aspx?Mode=MemberExistence&Email=' + _email + '&rid=' + rid + '&sid=' + sid + '&txid=' + txid + '&trans_id=' + transactionid,
            timeout: 50000,
            datatype: 'json',
            type: 'POST',
            async: false,
            data: ko.toJSON(ViewModel),
            success: function (data) {
                if (data != null && data != '') {
                    $('#dvImgLoading').hide();
                    window.location.href = '/river/s2.htm?ug=' + data;
                    //  window.location.href = '/river/top10.htm?ug=' + data;
                }
                else {
                    _isValid = true;
                    $('#dvError').hide();
                    $('#dvImgLoading').hide();
                }
            },
            error: function (error) {
                $('#dvImgLoading').hide();
            }
        });

        return _isValid;
    }



    that.init = function () {
        $('#dvImgLoading').show();

        if (rid != '' && rid != undefined) {
            insertclicks();
        }

        initevents();
        //GetEmailFromCookie();
        $('#dvError').hide();
        if (document.cookie && document.cookie != '') {

            var split = document.cookie.split(';');
            for (var i = 0; i < split.length; i++) {
                var spilt_name = split[i].replace('&', '=');
                var name_value = spilt_name.split('=');
                // var referrer_name = spilt_name[1].split('=')
                if (name_value[0] == "FandFAccountName" || name_value[0] == " FandFAccountName") {
                    _emailAddress = name_value[2];
                    if (_refercheck == 1) {
                        rid = name_value[4]
                    }

                }
            }

        }

        if (_emailAddress != '' && _emailAddress != null && _emailAddress != "") {
            //      jQuery('#dvLogin').css('display','inline');
            if (rid != '' && rid != undefined && sid != '' && sid != undefined && transactionid != '' && transactionid != undefined && txid != '' && txid != undefined) {
                EmailExistsCheck(_emailAddress);
            }
        }
        else {
            jQuery('#dvLogin').css('display', 'inline');
        }
        $('#dvImgLoading').hide();
    }
    return that;
}();

$(document).ready(function () {
    //    $('.cp-bg').height($(document).height());
    var imgArr = new Array( // relative paths of images
    '/Images/background-1.jpg',
    '/Images/background-2.jpg',
    '/Images/background-3.jpg',
    '/Images/background-4.jpg'
    );

    function parseUrl(url) {
        var a = document.createElement('a');
        a.href = url;
        return a;
    }

    //var search = window.location.href.search;
    //var searchText = document.location.href.split('?')[2]; // removes the leading '?'



    //if (searchText != undefined && searchText.length < 4) {
    //var url = window.location.href.replace('?fn=', '');
    //alert(url);
    // window.location.href = url;
    //}

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
    GetMember.init();
});