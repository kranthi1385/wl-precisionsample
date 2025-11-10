

function SendRequests() {
    $("#SDLOffers input[type=checkbox]").each(function() {
        if (this.checked) {
            var hiddenId = this.id.replace('checkbox', 'hidden');
            PostRequest($('#' + hiddenId).val());
        }
    });
}

function PostRequest(url) {

    var obj = null;
    var rslt = "";

    //    obj = new ActiveXObject("rcbdyctl.Setting");
    //    rslt = obj.GetIPAddress;


    var fname = document.getElementById('txtFirstName').value;
    var lname = document.getElementById('txtLastName').value;
    var email = document.getElementById('txtEmailAddress').value;
    var address1 = document.getElementById('txtAddress1').value;
    var address2 = document.getElementById('txtAddress2').value;

    var _cou = document.getElementById('ddlCountry');
    var countrycode = _cou.options[_cou.selectedIndex].value;
    var country = _cou.options[_cou.selectedIndex].text;
    if (countrycode = '231') {
        countrycode = 'USA';
    }
    else if (countrycode = '15') {
        countrycode = 'AUS';
    }
    else if (countrycode = '38') {
        countrycode = 'CAN';
    }
    else if (countrycode = '229') {
        countrycode = 'UK';
    }
    var _sta = document.getElementById('ddlState');
    var state = _sta.options[_sta.selectedIndex].value;

    var city = document.getElementById('txtCity').value;
    var zip = document.getElementById('txtZipCode').value;
    var phone = document.getElementById('txtPhoneNumber').value;

    var _gen = document.getElementById('ddlGender');
    var gendercode = _gen.options[_gen.selectedIndex].value;
    var gender = _gen.options[_gen.selectedIndex].text;

    var _mon = document.getElementById('ddlMonth');
    var month = _mon.options[_mon.selectedIndex].value;

    var _day = document.getElementById('ddlDay');
    var day = _day.options[_day.selectedIndex].value;

    var _year = document.getElementById('ddlYear');
    var year = _year.options[_year.selectedIndex].value;

    var _ethn = document.getElementById('ddlEthnicity');
    var ethnicity = _ethn.options[_ethn.selectedIndex].value;

    var password = document.getElementById('txtPassword').value;

    url = url.replace('%%fname%%', fname);
    url = url.replace('%%lname%%', lname);
    url = url.replace('%%email%%', email);
    url = url.replace('%%address1%%', address1);
    url = url.replace('%%address2%%', address2);
    url = url.replace('%%city%%', city);
    url = url.replace('%%state%%', state);
    url = url.replace('%%country1%%', countrycode);
    url = url.replace('%%country2%%', country);
    url = url.replace('%%zip%%', zip);
    url = url.replace('%%mobile1%%', phone);
    url = url.replace('%%gender1%%', gendercode);
    url = url.replace('%%gender2%%', gender);
    url = url.replace('%%dob1%%', month + day + year);
    url = url.replace('%%dob2%%', month + "/" + day + "/" + year);
    url = url.replace('%%dob3%%', month + "-" + day + "-" + year);
    url = url.replace('%%ip%%', rslt);
    url = url.replace('%%ehtnicity%%', ethnicity);

    document.getElementById('hfPostURL').value += url + '*';


    //    test('Get', function() {
    //        jQuery.get(url, function(res) {
    //            if (res.responseText == null) {
    //            }
    //            else {
    //                $('#SDLOffers').html(res.responseText);
    //            }

    //        });

    //    });

}

function CallOffers() {

    //country
    var _cou = document.getElementById('ddlCountry');
    var countrycode = _cou.options[_cou.selectedIndex].value;

    //gender
    var _gen = document.getElementById('ddlGender');
    var gendercode = _gen.options[_gen.selectedIndex].value;

    //age
    var _mon = document.getElementById('ddlMonth');
    var month = _mon.options[_mon.selectedIndex].value;

    var _day = document.getElementById('ddlDay');
    var day = _day.options[_day.selectedIndex].value;

    var _year = document.getElementById('ddlYear');
    var year = _year.options[_year.selectedIndex].value;

    var age = month + "/" + day + "/" + year;

    //        ethnicity
    var _ethn = document.getElementById('ddlEthnicity');
    var ethnicity = _ethn.options[_ethn.selectedIndex].value;

    //    var c = 231;
    //    var g = 'm';
    //    var age = '1/1/1983';
    //    var et = 2;

    var URL = '/Misc/Page1Offers.aspx?c=' + countrycode + '&g=' + gendercode + '&age=' + age + '&et=' + ethnicity;

    //var URL = '/Misc/Page1Offers.aspx?c=' + c + '&g=' + g + '&age=' + age + '&et=' + et;

    $.ajax({
        url: URL,
        type: 'GET',
        success: function(res) {

            $('#SDLOffers').html(res);

        }
    });
}

///Pop UP for PerkPrivacyPolicyUrl///

function Open(PerkPrivacyUrl) {
    var url = PerkPrivacyUrl;
    window.open(url, "PerkPrivacyUrl", "menubar=1,resizable=1,width=1025,scrollbars=1,height=700,");
}

 



