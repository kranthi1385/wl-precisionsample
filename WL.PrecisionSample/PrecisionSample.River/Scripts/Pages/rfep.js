var rfep = function() {

    /* varible declaration */
    var that = {};
    var _usg = '';
    var _ug = '';
    var _uig = '';
    /* initEvents */
    var initEvents = function() {
        $('#btnClose').click(function() {
            CloseThis();
        });
    }
    var getUrlVars = function() {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

    _usg = getUrlVars()["usg"];
    if (_usg == undefined) {
        _usg = '';
    }
    _ug = getUrlVars()["ug"];
    if (_ug == undefined) {
        _ug = '';
    }
    _uig = getUrlVars()["uig"];
    if (_uig == undefined) {
        _uig = '';
    }

    /*-----------------------------------Close Function-------------------------------------------------*/
    var CloseThis = function() {
        var win = window.open('', '_self');
        win.close();
        return false;
    }

    /*-----------------------------------Get Url--------------------------------------------------*/
    var GetUrl = function() {
        $.ajax({
            datatype: 'json',
            url: '/services/RiverService.aspx?Mode=GetProjectDetails&ug=' + _ug + '&rn=' + Math.random(),
            cache: false,
            async: false,
            success: function(pagedata) {
                $('#dvImageLoading').hide();
                if (pagedata.indexOf("rfep.htm") >= 0) {
                }
                else {
                    window.location.href = '/River/top10.htm?ug=' + _ug;
                }
            }
        });
    }
    /* init function */

    that.init = function() {
        initEvents();
        GetUrl();

    }

    return that;
} ();

$(document).ready(function() {
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
        $('.cp-bg').animate(1000, function() {
            $(this).css('background-image', 'url(' + preloadArr[currImg++ % preloadArr.length].src + ' )');
        }).animate(1000);
    }
    if ($.browser.msie) {
        if ($.browser.version <= 8.0) {

            $("#lnkMainCss").attr("href", "/css/river-mainIE.css");
        }
    }
    rfep.init();
});