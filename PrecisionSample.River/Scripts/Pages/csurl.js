var redirectsurvey = function () {
    var that = {};
    var getUrlVars = function () {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

    var ug = getUrlVars()["ug"];
    var uig = getUrlVars()["uig"];
    if (ug == undefined) {
        ug = '';
    }
    if (uig == undefined) {
        uig = '';
    }
    //Get SurveyUrl
    var GetSurveyURl = function () {
        $.ajax({
            datatype: 'json',
            type: 'POST',
            url: '/services/RiverService.aspx?Mode=GetSurveyUrl&ug=' + ug + '&uig=' + uig,
            success: function (pagedata) {
                $('#dvImageLoading').hide();
                if (pagedata != "" ) {
                    window.location.href = pagedata;
                }
                else {
                    window.location.href = '/river/rep.htm?ug=' + ug + '&usg=F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4' + '&uig=' + uig;
                }

            }
        });
    }

    that.init = function () {
        $('#btnSubmit').click(function () {
            $('#dvImageLoading').show();
            var captcha_response = grecaptcha.getResponse();
            if (captcha_response.length == 0) {
                // Captcha is not Passed
                $("#lblResult").show();
                $('#dvImageLoading').hide();
            }
            else {
                $("#lblResult").hide();
                // Captcha is Passed
                GetSurveyURl();
            }

        });

    }
    return that;
}();
$(document).ready(function () {
    redirectsurvey.init();
});