var rep = function () {

    /* varible declaration */
    var that = {};
    var _usg = '';
    var _ug = '';
    var _uig = '';
    /* initEvents */
    var initEvents = function () {
        $('#btnYesSuccess').on('click', function () {
            window.location.href = '/river/entry.htm?ug=' + _ug;
        });
        $('#btnNoSuccess').on('click', function () {
            document.formendpage.action = '/river/rfep.aspx?ug=' + _ug;
            document.formendpage.submit();
        });

        $("#btnBookmark").click(function () {
            var bookmarkUrl = this.href;
            var bookmarkTitle = this.title;
            //alert(bookmarkurl);
            if ($.browser.mozilla) // For Mozilla Firefox Bookmark
            {
                window.sidebar.addPanel(bookmarkTitle, bookmarkUrl, "");
            }
            else if ($.browser.msie || $.browser.webkit) // For IE Favorite
            {
                window.external.AddFavorite(bookmarkUrl, bookmarkTitle);
            }
            else if ($.browser.opera) // For Opera Browsers
            {
                $(this).attr("href", bookmarkUrl);
                $(this).attr("title", bookmarkTitle);
                $(this).attr("rel", "sidebar");
                $(this).click();
            }
            else // for other browsers which does not support
            {
                alert('Please hold CTRL+D and click the link to bookmark it in your browser.');
            }
            return false;
        });
    }



    var getUrlVars = function () {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

    _usg = getUrlVars()["usg"];
    _ug = getUrlVars()["ug"];
    _uig = getUrlVars()["uig"];

    var UpdateUserInvitationDetails = function () {
        $.ajax({
            datatype: 'json',
            url: '/services/RiverService.aspx?Mode=UpdateUserInvitationDetails&usg=' + _usg + '&uig=' + _uig,
            cache: false,
            async: false,
            success: function (pagedata) {
                if (pagedata != "") {
                    var _statusvalues = [];
                    _statusvalues = pagedata.split(';');
                    if (_statusvalues.length > 1) {
                        if (_statusvalues[0] == 14666 && _statusvalues[1] != undefined) {
                            window.location.href = 'http://www.we-tell.com/wl/wtlstep2.aspx?ug=' + _statusvalues[1];
                        }
                        else {
                            $('#dvContainer').show();
                            if (_statusvalues[0] != "") {
                                if (parseInt(_statusvalues[1]) == 1) {
                                    //Pixel Type
                                    $('#dvPixel').html(_statusvalues[0]);
                                }
                                else if (parseInt(_statusvalues[1]) == 2) {
                                }
                                $.ajax({
                                    url: '/services/RiverService.aspx?Mode=rivercallback&url=' + encodeURIComponent(_statusvalues[0]) + '&uig=' + _uig + '&ug=' + _ug + '&type=' + _statusvalues[1],
                                    cache: false,
                                    async: false,
                                    success: function (pagedata) {
                                    },
                                    error: function (er) {
                                    }
                                });
                            }
                            // 
                        }
                    }
                    else {
                        $('#dvContainer').show();
                    }
                }
                else {
                    $('#dvContainer').show();
                }
                $('#dvImageLoading').hide();

            },
            error: function () {
                $('#dvContainer').show();
            }
        });
    }

    /* init function */

    that.init = function () {
        initEvents();
        UpdateUserInvitationDetails();
        if (_usg.toUpperCase() == 'F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4' || _usg.toUpperCase() == '67B98BED-9C3F-42AE-BDD3-7E15F9C17F00') {
            $('#dvFailureMessage').show();
        }
        else if (_usg.toUpperCase() == '6AC169C6-DF47-4CD1-8F4D-1311F5C5F163') {
            $('#dvSuccessMessage').show();
        }
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
    rep.init();
});