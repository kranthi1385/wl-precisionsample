$(document).ready(function () {
    var scroll_start = 1;
    var startchange = $('.header');
    var offset = startchange.offset();
    var limiter = 767;
    $(document).scroll(function () {
        scroll_start = $(this).scrollTop();
        if ($(window).width() > limiter) {
            if (scroll_start > offset.top) {
                $('.header').css('background-color', 'rgba(184,39,43,0.9)');
            } else {
                $('.header').css('background-color', 'transparent');
            }
        } else {
            $('.header').css('background-color', 'rgba(184,39,43,1)');
        }
    });


});