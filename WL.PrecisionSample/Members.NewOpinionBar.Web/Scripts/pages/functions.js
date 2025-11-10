
$(document).ajaxStart(function () {
    $("body").addClass("loading");
});
$(document).ajaxStop(function () {
    $("body").removeClass("loading");
});

var userEdited = false;

$(document).ready(function () {

    $("#datepicker").datepicker({
        dateFormat: 'dd/mm/yy', changeYear: true, yearRange: "-100:-18"
    });
    $("#datepicker").attr('readOnly', 'true');
    var limiter = 767;
    $(document).scroll(function () {
        if ($(window).width() > limiter) {
            if ($(window).scrollTop() != 0) {
                $('.header').css('background-color', 'rgba(184,39,43,0.9)');
            } else {
                $('.header').css('background-color', 'transparent');
            }
        } else {
            $('.header').css('background-color', 'rgba(184,39,43,1)');
        }
    });

    $(document).on("click", ".sign_up_now_btn", function () {
        setTimeout(
          function () {
              $(".email").css("height", $(".register-form").height()-30);
          }, 20);   
    });

    //$(document).on('submit', 'form.register-form', function (evt) {
    //    evt.preventDefault();
    //    $.ajax({
    //        url: this.action,
    //        type: this.method,
    //        data: $(this).serialize(),
    //        success: function (result) {
    //            if (result.Value) {
    //                /*var url = "/Home/Profile";
    //                window.location.href = url;*/
    //                $('.sign_up_div_bg').slideUp();
    //                $('html, body').animate({ scrollTop: 0 }, 'fast');
    //                $("#Country").selectBoxIt({
    //                    theme: "filter-white",
    //                    autoWidth: false,
    //                });
    //                $('#popup5').fadeIn($(this).data());
    //                if ($(window).width() < limiter) {
    //                    $('.overflowcontainer').css({
    //                        'height': '100%',
    //                        'width': '100%',
    //                        'overflow': 'hidden',
    //                        'position': 'fixed'
    //                    });
    //                };
    //            }
    //            else {
    //                $(".sign_up_div_bg").children().html(result.View);
    //                $("#gender").selectBoxIt({
    //                    theme: "filter-white",
    //                    autoWidth: false,
    //                });
    //                $("#Country").selectBoxIt({
    //                    theme: "filter-white",
    //                    autoWidth: false,
    //                });
    //                //$("select").selectBoxIt({ 'autoWidth': false });
    //                $("#datepicker").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true, yearRange: "-100:-18" });
    //                $("#datepicker").attr('readOnly', 'true');
    //            }
    //        }
    //    });
    //    return false;
    //});

    $(document).on('submit', 'form.login-form', function (evt) {
        evt.preventDefault();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.Value == 'login') {
                    $(".sign_in_div").html(result.View);
                }
                else {
                    var url = "/Home/Profile";
                    if (result == "../Account/Delete")
                    {
                        url = result;
                    }
                    window.location.href = url;
                }
            }
        });
        return false;
    });

    $(document).on('click', '#request_link', function (evt) {
        evt.preventDefault();
        $.ajax({
            url: "/Account/ForgotPassword",
            type: "POST",
            data: { "EmailAddress": $('#ForgotEmail').val() },
            success: function (result) {
                if (result.value == "Redirect"){
                    window.location = result.url;
                }
                if (result.value == "true") {
                    $('#popup6').fadeIn();
                    if ($(window).width() < limiter) {
                        $('.overflowcontainer').css({
                            'height': '100%',
                            'width': '100%',
                            'overflow': 'hidden',
                            'position': 'fixed'
                        });
                    };
                }
                else {
                    alert("not email");
                }
            }
        });
    });

    //selectbox style on contact page 
    $("#country").selectBoxIt({
        theme: "filter-white",
    });
    
    $("#Country").selectBoxIt({
        theme: "filter-white",
        autoWidth: false,
    });

    $("#country_id").selectBoxIt({
        theme: "filter-white",
        autoWidth: false,
    });

    $("#CountryID").selectBoxIt({
        theme: "filter-white",
        autoWidth: false,
    }); 
    
    $("#ServiceID").selectBoxIt({
        theme: "filter-white",
        autoWidth: false,
    });

    $("#survey").selectBoxIt({
        theme: "filter-white",
        autoWidth: false,
    });

    $("#gender").selectBoxIt({
        theme: "filter-white",
        autoWidth: false,
    });

    $("#Category").selectBoxIt({
        theme: "filter-white",
        autoWidth: false,
    });

    //$("select").selectBoxIt({ 'autoWidth': false });

    //Slide if sign in button is clicked
    $('.sign_in').click(function () {
        $('.forgot_div').hide();
        $('.sign_in_div').show();
        $('.sign_in_div_bg').slideToggle(function () {
            $('html, body').animate({
                scrollTop: $(".sign_in_div_bg").offset().top - 70
            }, 500);

        });
        $('.sign_up_div_bg').slideUp();
    });

    //Slide if sign up button is clicked
    $('.sign_up_email').click(function () {
        $('.sign_up_div_bg').slideToggle(function () {
            $('html, body').animate({
                scrollTop: $(".sign_up_div_bg").offset().top - 70
            }, 500);
        });
        $('.sign_in_div_bg').slideUp();

    });

    //slide sign in at surveys
    $('.profile_login').click(function () {
        $('.sign_in_div_bg').slideToggle();
    });

    //forgot password slide
    $(document).on('click', '.forgot', function (){
        $('.forgot_div').slideToggle();
        $('.sign_in_div').slideToggle();
    });

    $(document).on('click', '.forgot_activation', function () {
        window.location = $("#forgotpasswordtoken").val();
    })

    //hover navigation top
    $(".nav_item, .country, .country_mob").hover(
		function () {
		    $(this).addClass("nav_item_h");
		}, function () {
		    $(this).removeClass("nav_item_h");
		});

    //hover change buttons
    $(".change, .delete").hover(
		function () {
		    $(this).addClass("change_h");
		}, function () {
		    $(this).removeClass("change_h");
		});
    //hover payment history
    $(".history").hover(
		function () {
		    $(this).addClass("change_h");
		}, function () {
		    $(this).removeClass("change_h");
		});

    //hover save button
    $(".save1").hover(
      function () {
          $(this).addClass("save1_h");
      }, function () {
          $(this).removeClass("save1_h");
      });

    //hover Forgot password
    $(".forgot").hover(
		function () {
		    $(this).addClass("forgot_h");
		}, function () {
		    $(this).removeClass("forgot_h");
		});

    //hover button sign up via Facebook
    $(".sign_up_fb").hover(
		function () {
		    $(this).addClass("sign_up_fb_h");
		}, function () {
		    $(this).removeClass("sign_up_fb_h");
		});

    //hover button sign up via Email
    $(".sign_up_email").hover(
		function () {
		    $(this).addClass("sign_up_email_h");
		}, function () {
		    $(this).removeClass("sign_up_email_h");
		});

    $('.sign_up_now_btn').hover(function () {
        $(this).addClass("sign_up_email_h");
    }, function () {
        $(this).removeClass('sign_up_email_h');
    })

    //hover button sign in
    $(".sign_in").hover(
		function () {
		    $(this).addClass("sign_in_h");
		}, function () {
		    $(this).removeClass("sign_in_h");
		});

    //hover privacy in footer
    $(".privacy").hover(
		function () {
		    $(this).addClass("privacy_h");
		}, function () {
		    $(this).removeClass("privacy_h");
		});

    //hover terms in footer
    $(".terms").hover(
		function () {
		    $(this).addClass("terms_h");
		}, function () {
		    $(this).removeClass("terms_h");
		});

    //hover metrixlab in footer
    $(".metrixlab").hover(
		function () {
		    $(this).addClass("metrixlab_h");
		}, function () {
		    $(this).removeClass("metrixlab_h");
		});
    //hamburger navigation
    $('.hamburger_icon').click(function () {
        $('.navigation_mobile').slideToggle(500);
    });

    $('.nav_item_mob').click(function () {
        $(this).addClass('nav_item_mob_h');
    });

    //hover button Send
    $(".contact_send_btn").hover(
		function () {
		    $(this).addClass("contact_send_btn_h");
		}, function () {
		    $(this).removeClass("contact_send_btn_h");
		});

    //hover more about in footer
    $(".more_about").hover(
		function () {
		    $(this).addClass("more_about_h");
		}, function () {
		    $(this).removeClass("more_about_h");
		});

    //hover sign out button
    $(".logout").hover(
		function () {
		    $(this).addClass("logout_h");
		}, function () {
		    $(this).removeClass("logout_h");
		});
    //hover payment options
    $(".bank").hover(
		function () {
		    $(this).addClass("bank_h");
		}, function () {
		    $(this).removeClass("bank_h");
		});
    $(".charity").hover(
		function () {
		    $(this).addClass("charity_h");
		}, function () {
		    $(this).removeClass("charity_h");
		});
    $(".paypal").hover(
		function () {
		    $(this).addClass("paypal_h");
		}, function () {
		    $(this).removeClass("paypal_h");
		});

    //Popup modal   
    $(function popups() {

        var limiter = 767;
        $(document).on('click', 'div[data-modal-id]',function (e) {
            e.preventDefault();
            $(".modal-overlay").fadeTo(500, 0.7);
            var modalBox = $(this).attr('data-modal-id');
            if (modalBox == "popup1") {
                ProcessGetPaid(modalBox);
            }
            else {
                $('#' + modalBox).fadeIn($(this).data());
                if ($(window).width() < limiter) {
                    $('.overflowcontainer').css({
                        'height': '100%',
                        'width': '100%',
                        'overflow': 'hidden',
                        'position': 'fixed'
                    });
                };
            }          
        });


        $(".js-modal-close, .modal-overlay").click(function () {
            $(".modal-box, .modal-overlay").fadeOut(400, function () {
                $(".bankaccount").hide();
                $(".paypalaccount").hide();
                $(".donation").hide();
                $(".confirmation").hide();
                $(".confirmation2").hide();
                $(".modal_choice").fadeIn(1000);
                $('.overflowcontainer').css('position', 'relative');
            });

        });
        $(".js-modal-close-confirm").click(function () {
            location.reload();
        });

        //resize popup modal
        $(window).resize(function () {
            $(".modal-box").css({
                top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
                left: ($(window).width() - $(".modal-box").outerWidth()) / 2
            });
            $(".modal2").css({
                top: ($(window).height() - $(".modal2").outerHeight()) / 2,
                left: ($(window).width() - $(".modal2").outerWidth()) / 2,
            });
        });

        //keep popup in place
        $(window).scroll(function () {
            $(".modal-box").css({
                top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
                left: ($(window).width() - $(".modal-box").outerWidth()) / 2,

            });
            $(".modal2").css({
                top: ($(window).height() - $(".modal2").outerHeight()) / 2,
                left: ($(window).width() - $(".modal2").outerWidth()) / 2
            });
        });

        $(window).resize();

    });

    function ProcessGetPaid(modalBox) {
        //if the user doesn't have enough credit show error
        if ($(".subtitle").text().replace(/\D/g, '') < 1000) {
            $('#popup4').fadeIn();
        }
        else {
            //if the user has enough credit, but already has an unprocessed payment show that
            $.ajax({
                url: "../Account/CheckForUnprocessedPayment",
                type: "POST",
                success: function (result) {
                    if (result == "True") {
                        $('#popup5').fadeIn();
                    }
                    else {
                        $('#' + modalBox).fadeIn($(this).data());
                        if ($(window).width() < limiter) {
                            $('.overflowcontainer').css({
                                'height': '100%',
                                'width': '100%',
                                'overflow': 'hidden',
                                'position': 'fixed'
                            });
                        };
                    }
                }
            });
        }
    }


    //Active logo's charity
    $(".logo_char").click(
		function () {
		    $('div.logo_active').removeClass('logo_active');
		    $(this).addClass("logo_active");

		});

    $(".kruis").click(
		function () {
		    $('div.green_active, div.wnf_active, div.amnesty_active').removeClass('green_active wnf_active amnesty_active');
		    $(this).addClass("kruis_active");

		});

    $(".green").click(
		function () {
		    $('div.kruis_active, div.wnf_active, div.amnesty_active').removeClass('kruis_active wnf_active amnesty_active');
		    $(this).addClass("green_active");

		});

    $(".wnf").click(
		function () {
		    $('div.green_active, div.kruis_active, div.amnesty_active').removeClass('green_active kruis_active amnesty_active');
		    $(this).addClass("wnf_active");

		});

    $(".amnesty").click(
		function () {
		    $('div.kruis_active, div.wnf_active, div.green_active').removeClass('kruis_active wnf_active green_active');
		    $(this).addClass("amnesty_active");

		});


    //Change personal data
    $(document).on('click', '.change1', function () {
        $("span[data-valmsg-for='InvalidLogin']").hide();
        if (window.innerWidth < 479) {
            $("#country_idSelectBoxItContainer, #country_idSelectBoxIt, #country_idSelectBoxItOptions").width("151");
        }
        ChangePersonalData();
    });

    $(document).on('click', '.cancel_change', function () {
        $("#EditUserInput").hide();
        $("#EditUser").show();
        $('.change1').show();
        $('.hide, .save1, .delete').hide();
        $(".profile_text_wrapper").toggleClass("profile_text2");
    });

    $(document).on('submit', 'form.modify-form', function (evt) {
        evt.preventDefault();
        $("form").validate();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                $(".edit_profile_render").html(result.View);
                drawGraph3($("#ProfileBalance").val(), 10, 0);
                drawGraph4($("#ProfileCompletedPerc").val());
                $("#country_id").selectBoxIt({
                    theme: "filter-white",
                    autoWidth: false,
                });
                $("#datepicker").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true, yearRange: "-100:-18" });
                $("#datepicker").attr('readOnly', 'true');
                if (result.State == "invalid"){
                    ChangePersonalData();
                }
                else {
                    userEdited = true;
                }
            }
        });
        return false;
    });

    function ChangePersonalData() {   
        $("#EditUser").hide();
        $("#EditUserInput").show();
        $('.change1').hide();
        $(".profile_text_wrapper").toggleClass("profile_text2");
        //$(".column_profile").css("height", "600");
        $('.hide, .save1, .delete').show();
        $(window).resize();
    }

    $(document).on('click', '.open', function () {
        $.ajax({
            url: "StartSurvey",
            type: "POST",
            data: { "ServiceID":  $(this).attr('id') },
            success: function (result) {
                $('body').html(result);
            }
        });
    });

    //Subnavigation My Profile
    $('.subnav_surveys').click(function () {
        if (userEdited) {
            location.reload();
        }
        else {
            if ($(this).hasClass("subnav_inactive")) {
                $('.container').animate({ left: '-2px' });
                $(this).addClass('subnav_active');
                $(this).removeClass('subnav_inactive');
                $('.subnav_profile').removeClass('subnav_active');
                $('.subnav_profile').addClass('subnav_inactive');
            }
        } 
    });

    $('.subnav_profile').click(function () {
        drawGraph3($("#ProfileBalance").val(), 10, 0);
        drawGraph4($("#ProfileCompletedPerc").val());
        if ($(this).hasClass("subnav_inactive")) {
            $('.subnav_profile').addClass('subnav_active');
            $('.subnav_profile').removeClass('subnav_inactive');
            $('.subnav_surveys').removeClass('subnav_active');
            $('.subnav_surveys').addClass('subnav_inactive');
            $('.container').animate({ left: '-960px' });
            $('.profile_text').text($('.profile_text').text().replace('Survey name', $("#ProfileNextSurveyName").val()));
            $('#ProfileSurveyCompletenessID').attr("id", $("#ProfileNextSurveyName").attr('data-service-id'));
        }
    });

    //Filter surveys 
    $('.filter_btn').click(function () {
        if ($(this).hasClass("subnav_inactive")) {
            $('.filter_btn').removeClass('subnav_active');
            $('.filter_btn').addClass('subnav_inactive');
            $(this).removeClass("subnav_inactive");
            $(this).addClass('subnav_active');


        }
    });

    if ($("#available").hasClass("subnav_active")) {
        $(".available").show();
        $(".participated").hide();
    };

    $('#available').click(function () {
        $(".available").fadeIn(450);
        $(".participated").hide();

    });
    $('#history').click(function () {
        $(".available").hide();
        $(".participated").fadeIn(450);

    });

    //adjust height to content on profile page
    resize();

    function resize() {
        if ($(".subnav_profile").hasClass("subnav_active")) {
            var biggestHeight = "0";
            biggestHeight = $(".profile").height();
            $(".container").css("min-height", "423px").height(biggestHeight);
            $(".my_profile_wrapper").css("min-height", "423px").height(biggestHeight);
        } else {
            biggestHeight = $(".surveys").height();
            $(".container").css("min-height", "423px").height(biggestHeight);
            $(".my_profile_wrapper").css("min-height", "423px").height(biggestHeight);
        };

    };

    // Register for window resize
    $(window).resize(function () {

        // Do initial resize
        resize();
    });

    //resize when clicked
    $(".change").click(function () {
        setTimeout(
          function () {
              $(window).resize();
          }, 20);
    });
    $(".subnav_item").click(function () {
        resize();
    });

    //privacy articles slide down
    $(".privacy_head").click(function () {
        $(this).next().next().slideToggle();
        $(this).next().children().toggleClass("minus");
    });
    $(".icon_privacyfields").click(function () {
        $(this).parent().next().slideToggle();
        $(this).toggleClass("minus");
    });

    //Payment choices and flow   
    $(".choosebank").click(function () {
        $(".modal_choice").toggle();
        $(".bankaccount").toggle();
    });
    $(".choosepaypal").click(function () {
        $(".modal_choice").toggle();
        $(".paypalaccount").toggle();
    });
    $(".choosedonate").click(function () {
        $(".modal_choice").toggle();
        $(".donation").toggle();
    });

    $(".confirmbank").click(function () {
        var IBANValue = $("#inputiban").val();
        if (IBAN.isValid(IBANValue))
        {
            var City = $("#inputcity").val();
            var Name = $("#inputpayee").val();
            $.ajax({
                url: "../Account/SubmitPayment",
                type: "POST",
                data: { IBAN: IBANValue, City: City, Name: Name },
                success: function (result) {
                    if (result == "False") {
                        window.location = "/Home/Index";;
                    }
                    else {
                        $(".confirmation").toggle();
                        $(".bankaccount").toggle();
                    }   
                }
            });      
        }
        else {
            $("#IBAN_error").show();
        }
    });
    $(".confirmpaypal").click(function () {
        var PayPalEmail = $("#inputpp").val();
        if (isValidEmail(PayPalEmail)) {
            var City = $("#inputcity").val();
            var Name = $("#inputpayee").val();
            $.ajax({
                url: "../Account/SubmitPayment",
                type: "POST",
                data: { Email: PayPalEmail, City: City, Name: Name },
                success: function (result) {
                    $(".confirmation").toggle();
                    $(".paypalaccount").toggle();
                }
            });
        }
        else {
            $("#IBAN_error").show();
        }   
    });

    $(".confirmdonate").click(function () {
        $(".confirmation2").toggle();
        $(".donation").toggle();
    });

    $(".payment, .getpaid").click(function () {
        $(".modal-box").css({
            top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
            left: ($(window).width() - $(".modal-box").outerWidth()) / 2,


        });
    });

    //change location of sign in button for demo when get paid is clicked. 
    $(".paysurvey").click(function () {
        $('.forgot_div').hide();
        $(".sign_in_btn, .sign_up_now_btn").off("click");
        $(".sign_up_now_btn").removeClass("sign_in_btn");
        $(".sign_up_now_btn").addClass("getpaidlink");
        $('.sign_in_div').show();
        $('.sign_in_div_bg').slideToggle(function () {
            $('html, body').animate({
                scrollTop: $(".sign_in_div_bg").offset().top - 70
            }, 500);

        });
    });
    $(document).on("click", ".getpaidlink", function () {
        window.location = 'my_profile3.html';

    });

    //Payment history modal    
    $(".history").click(function () {
        $(".modal2").css({
            top: ($(window).height() - $(".modal2").outerHeight()) / 2,
            left: ($(window).width() - $(".modal2").outerWidth()) / 2,

        });
    });

    //Balance animation
    $(".plus").delay(700).fadeIn(300).animate({
        marginLeft: '+=105px'
    }, 500);

    //Footer alignment on surveys
    function footers() {
        var maxwidth = 767;
        if ($(window).width() > maxwidth) {
            $(".survey_footer").addClass("footer_surveys");
            $(".hr_hide").css('display', 'block');

        } else {
            $(".survey_footer").removeClass("footer_surveys");
            $(".hr_hide").css('display', 'none');

        };
    };
    function wrapperHeight() {
        var maxwidth = 767;
        if ($(window).width() > maxwidth) {
            $(".wrapper_start").css({
                height: ($(window).height() - 190)
            });
        } else {
            $(".wrapper_start").css({
                height: '100%'
            });
        };
    };
    // Register for window resize
    $(window).resize(function () {

        // Do initial resize
        footers();
        wrapperHeight();
    });
    $(".profile_login").click(function () {
        if ($(this).hasClass("visible_div")) {
            wrapperHeight();
            $(this).toggleClass("visible_div");
        }
        else {
            $('.wrapper_start').css({
                height: '100%'
            });
            $(this).toggleClass("visible_div");
        };
    });

    //change position of profile circle
    $(document).ready(function () {
        var limiter = 1000;

        if ($(window).width() < limiter) {
            $('#cellwrapper').appendTo("#insert_tablet");

        };
    });

    //change position of elements when opened on smaller devices
    $(document).ready(function () {
        var limiter = 767;

        if ($(window).width() < limiter) {
            $('.input_change').toggle();
            $('#cellwrapper').appendTo("#insert_cell");
        };
    });

    //Input field in focus when div opens
    $(".btn_banner").click(function () {
        $("input:visible:first").focus();
    });

    //Input field check when blurred
    $('input[type=email]').blur(function () {
        var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
        if (testEmail.test(this.value)) { $(this).removeClass("error"); }
        else {
            $(this).addClass("error")
        };
    });

    $('input[type=text]').blur(function () {

        if ((this.value) == "" && this.id != "streetname" && this.id != "streetnumber") { $(this).addClass("error"); }
        else {
            $(this).removeClass("error")
        };
    });

    $('input[type=password]').blur(function () {
        if ($(this).val().length < 6) {
            $(this).addClass("error");
        }
        else {
            $(this).removeClass("error")
        };
    });

    $("input[type=password], input[type=text], input[type=email]").focus(function () {
        $(this).removeClass("error");
    });

    $("#datepicker").blur(function () {
       $('#datepicker').removeClass("error");   
    });

    $(document).on("click", "#redirect_delete", function () {
        window.location = "/Account/Delete";
    });

    $(document).on("click", "#delete_user_account", function () {
        $.ajax({
            url: "/Account/DeleteAccount",
            type: "POST",
            success: function (result) {
                if (result) {
                    window.location = "/Home/Index";
                }
            }
        });
        return false;
    });
});

function isValidEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}



//Error message
//$(".error_message").text("The combination email address and password is not found. Please try again.").show();



