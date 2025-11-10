var GetLead = function () {
    var that = {};
    var Count = 0;
    var _isValid = false;
    var rid = '';
    var sid = '';
    var phno = '';


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
    rid = getUrlVars()["rid"];
    sid = getUrlVars()["sid"];
    phno = getUrlVars()["pn"];

    //----------------------init function-----------------------------------------------
    var initevents = function () {

    }

    //Check For Validations

    function Vaildations() {
        debugger;
        $('.error').hide();
        var isValid = true;
        var phnReg = /@^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$/;
        //var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;

        if (rid == undefined || rid == '') {
            isValid = false;
            $('#spnRid').show();
            $('#spnRid').text("Please Provide the Rid");
        }
        else {
            $('#spnRid').hide();
        }
        //if (sid == undefined) {
        //    isValid = false;
        //    $('#spnErr').show();
        //    $('#spnErr').text("Please Provide the Sid");
        //}
        //else {
        //    $('#spnErr').hide();
        //}
        if (phno == undefined) {
            isValid = false;
            $('#spnPhno').show();
            $('#spnPhno').text("Please Provide the Phone number");
        }
            //else if (!phnReg.test(phno)) {
            //    $('#spnPhno').show();
            //    $('#spnPhno').text("Please Provide the valid Phone number");
            //    isValid = false;
            //}
        else {
            $('#spnPhno').hide();
        }
        return isValid;
    }

    var InsertLead = function () {
        $('#dvImgLoading').show();
        $.ajax({
            url: '/Services/ALservice.aspx?Mode=insertlead&rid=' + rid + '&sid=' + sid + '&phno=' + phno,
            timeout: 50000,
            datatype: 'json',
            type: 'POST',
            async: false,
            success: function (data) {
                if (data.length == 36) {
                    $('#dvImgLoading').hide();
                    $('.error').hide();
                    $.ajax({
                        url: '/Services/ALservice.aspx?Mode=sendsms&rid=' + rid + '&sid=' + sid + '&phno=' + phno + '&lid=' + data,
                        timeout: 50000,
                        datatype: 'json',
                        type: 'POST',
                        async: false,
                        success: function (data) {
                            if (data == "success") {
                                $('#dvImgLoading').hide();
                                $('#spnPhno').show();
                                $('#spnPhno').css('color', 'white');
                                $('#spnPhno').text("Text message was successfully sent to " + phno);

                            }
                            else {
                                $('#spnPhno').show();
                                $('#spnPhno').css('color', 'red');
                                $('#spnPhno').text("Failed to send SMS to " + phno);
                            }
                        },
                        error: function (error) {
                            $('#dvImgLoading').hide();
                        }
                    });
                }
                else {
                    $('#dvImgLoading').hide();
                    $('#spnPhno').show();
                    $('#spnPhno').css('color', 'red');
                    $('#spnPhno').text("SMS was Already sent to " + phno);
                }
            },
            error: function (error) {
                $('#dvImgLoading').hide();
            }
        });

    }



    that.init = function () {
        if (Vaildations()) {
            debugger;
            InsertLead();
        }
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
    GetLead.init();
});
