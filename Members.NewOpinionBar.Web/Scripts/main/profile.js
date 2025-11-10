define(['app'], function (app) {
    app.register.controller('ProfileController', ['$rootScope', '$scope', '$location', '$window', 'translationsLoadingService', 'httpService', 'pagerService', 'getCurrentPageList', '$cookies',
        function ($rootScope, $scope, $location, $window, translationsLoadingService, httpService, pagerService, getCurrentPageList, $cookies) {
            translationsLoadingService.loadTranslatePagePath("hm");
            $scope.congrats = false;
            $scope.thankYou = false;
            $scope.weAreSorry = false;
            $scope.sorryQuota = false;
            $scope.youTerminated = false;
            $scope.alredyParticipated = false;
            $scope.thankParticipated = false;
            $scope.offersNotExists = false;
            $scope.showSurveys = false;
            $scope.available = true;
            $scope.showDetails = false;
            $scope.changeClick = false;
            $scope.closedProject = false;
            $scope.message = 0;
            var now = new $window.Date(),
            exp = new $window.Date(now.getFullYear(), now.getMonth() + 6, now.getDate());
            // to get URL params
            function getUrlVars() {
                var Url = window.location.href;
                var vars = {};
                var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }
            //Get query parmas value referrer id
            $scope.usg = getUrlVars()["usg"];
            if ($scope.usg == undefined) {
                $scope.usg = ''
            }
            $scope.pc = getUrlVars()["pc"];
            if ($scope.pc == undefined) {
                $scope.pc = $cookies.get('pc');
            }
            $scope.txid = getUrlVars()["txid"];
            if ($scope.txid == undefined) {
                $scope.txid = ''
            }
            $scope.leadid = getUrlVars()["leadid"];
            if ($scope.leadid == undefined) {
                $scope.leadid = $cookies.get('leadid');
            }
            debugger;
            if ($scope.pc != '' && $scope.pc != undefined && $scope.leadid != '') {
                httpService.getData('/Ms/GetScript?leadid=' + $scope.leadid).then(function (response) {
                    var scriptPixel = response;
                    for (i = 0 ; i < scriptPixel.length; i++) {
                        if (scriptPixel[i].pixeltype == 'J') {
                            var script = document.createElement("script");
                            script.type = "text/javascript";
                            script.async = true;// it is true by default, but we are jsut applying it.
                            if (scriptPixel[i].src != '') {
                                script.src = scriptPixel[i].src.replace("%%client_cpi%%", $scope.pc).replace("%%external_member_id%%", $scope.txid);
                            }
                            else if (scriptPixel[i].attributrValue != '') {
                                script.innerHTML = scriptPixel[i].attributrValue
                            }
                            document.body.appendChild(script);
                            $cookies.put('leadid', '', {
                                expires: exp,
                                path: '/'
                            });
                        }
                        else if (scriptPixel[i].pixeltype == 'I') {
                            var imgsfpixel = document.createElement('img');
                            imgsfpixel.height = "1px";
                            imgsfpixel.width = "1px";
                            imgsfpixel.alt = "Ipixel";
                            if (scriptPixel[i].src != '') {
                                imgsfpixel.src = scriptPixel[i].src.replace("%%client_cpi%%", $scope.pc).replace("%%external_member_id%%", $scope.txid);
                            }
                            else if (scriptPixel[i].attributrValue != '') {
                                imgsfpixel.innerHTML = scriptPixel[i].attributrValue
                            }
                            document.getElementsByTagName('head')[0].appendChild(imgsfpixel);
                            $cookies.put('leadid', '', {
                                expires: exp,
                                path: '/'
                            });
                        }

                    }
                }, function (err) {
                    //  $scope.errMsg = true;
                });
            }
            // new code
            if ($scope.leadid != '') {
                var imgsfpixel = document.createElement('img');
                imgsfpixel.height = "1px";
                imgsfpixel.width = "1px";
                imgsfpixel.alt = "Ipixel";
                imgsfpixel.src = "https://www.facebook.com/tr?id=4326893500713220&ev=PageView&noscript=1";
                document.getElementsByTagName('head')[0].appendChild(imgsfpixel);
            }
            if ($scope.pc != '' && $scope.pc != undefined && $scope.leadid != '') {
                httpService.getData('/Ms/PostbackScript?leadid=' + $scope.leadid).then(function (response) {
                    var scriptPixel = response;
                }, function (err) {
                    //  $scope.errMsg = true;
                });
            }

            $scope.cid = getUrlVars()["cid"];
            if ($scope.cid == undefined) {
                $scope.cid = ''
            }
            if ($scope.usg.toLowerCase() == "6ac169c6-df47-4cd1-8f4d-1311f5c5f163" || $scope.usg.toLowerCase() == "181cf682-614e-46ec-9716-816af9dfe43d" || $scope.usg.toLowerCase() == "167944ad-051f-48e2-b458-184a27c27ece" || $scope.usg.toLowerCase() == "e999a83c-f5c0-4cde-bee1-6557b6fa001f") {
                $scope.congrats = true;
            }
            else if ($scope.usg.toLowerCase() == "f6a6b754-4cf8-41bb-b9aa-5b97c412b1f4" || $scope.usg.toLowerCase() == "2b9038b6-db53-429a-8854-7bb83338b2d4") {
                if ($scope.cid == 385) {
                    $scope.prescreenerterm = true;
                }
                else {
                    $scope.thankYou = true;
                }
            }
            else if ($scope.usg.toLowerCase() == "C1F0F26D-FE64-4A7F-A2A3-69379A0FEA91") {
                if ($scope.cid == 38) {
                    $scope.psoqs = true;
                }
                else {
                    $scope.psoqthankYou = true;
                }
            }
            else if ($scope.usg.toLowerCase() == "ec9ad2bb-a92b-4781-87c1-5d3b505f6cd3") {
                $scope.weAreSorry = true;
            }
            else if ($scope.usg.toLowerCase() == "67b98bed-9c3f-42ae-bdd3-7e15f9c17f00") {
                if ($scope.cid == 38) {
                    $scope.QuotaFull = true;
                }
                else {
                    $scope.sorryQuota = true;
                }
            }
            else if ($scope.usg.toLowerCase() == "d5f04cf6-50ab-4617-9b0f-95b23a07488c" || $scope.usg.toLowerCase() == "b75a1590-2786-45f9-a5e3-656ae1c13724") {
                $scope.youTerminated = true;
            }
            else if ($scope.usg.toLowerCase() == "664b50cb-e1e7-40cc-b2eb-a94e1d54228f") {
                $scope.alredyParticipated = true;
            }
            else if ($scope.usg.toLowerCase() == "a24bc10d-1eeb-4a1a-83fd-8789180631ef") {
                $scope.thankParticipated = true;
            }
            else if ($scope.usg == "2BC664BA-94DD-41E8-B7E1-251A90105119") {
                $scope.dOIRecruit = true;
            }
            else if ($scope.usg == "8F69082C-5025-4F95-924E-F4BBE5C7BAE6") {
                $scope.closedProject = true;
            }
            else if ($scope.usg == null) {
                $scope.closedSurveyClick = true;
            }
            else if ($scope.usg != "") {
                $scope.offersNotExists = true;
            }
           
           
            $scope.surveys = []; //Get Surveys Data
            debugger;
            $scope.surveysList = function () {
                $scope.rewHistoryLst = '';
                $scope.showSurveys = false;
                httpService.getData('/Ms/GetSurveysList').then(function (response) {
                    $scope.available = true;
                    $scope.rewHistory = false;
                    if (response != '') {
                        if (response[0].ProjectId == 0) {
                            $scope.showSurveys = true;
                            $scope.available = false;
                        }
                        if ($rootScope.countryId == 572 || $rootScope.countryId == 494 || $rootScope.countryId == 497 || $rootScope.countryId == 502 || $rootScope.countryId == 503 || $rootScope.countryId == 505 || $rootScope.countryId == 506 || $rootScope.countryId == 511 || $rootScope.countryId == 512 || $rootScope.countryId == 1171 || $rootScope.countryId == 519 || $rootScope.countryId == 522 || $rootScope.countryId == 525 || $rootScope.countryId == 529 || $rootScope.countryId == 38 || $rootScope.countryId == 534 || $rootScope.countryId == 538 || $rootScope.countryId == 543 || $rootScope.countryId == 545 || $rootScope.countryId == 546 || $rootScope.countryId == 548 || $rootScope.countryId == 549 || $rootScope.countryId == 549 || $rootScope.countryId == 553 || $rootScope.countryId == 559 || $rootScope.countryId == 564 || $rootScope.countryId == 565 || $rootScope.countryId == 572 || $rootScope.countryId == 571 || $rootScope.countryId == 575 ||
                                                           $rootScope.countryId == 576 || $rootScope.countryId == 588 || $rootScope.countryId == 590 || $rootScope.countryId == 591 || $rootScope.countryId == 593 || $rootScope.countryId == 599 || $rootScope.countryId == 604 || $rootScope.countryId == 613 || $rootScope.countryId == 614 || $rootScope.countryId == 619 || $rootScope.countryId == 620 || $rootScope.countryId == 1180 || $rootScope.countryId == 1166 || $rootScope.countryId == 638 || $rootScope.countryId == 641 || $rootScope.countryId == 647 || $rootScope.countryId == 657 || $rootScope.countryId == 662 || $rootScope.countryId == 664 || $rootScope.countryId == 665 || $rootScope.countryId == 668 || $rootScope.countryId == 669 || $rootScope.countryId == 673 || $rootScope.countryId == 460 || $rootScope.countryId == 688 || $rootScope.countryId == 693 || $rootScope.countryId == 694 || $rootScope.countryId == 697 || $rootScope.countryId == 700 ||
                                                           $rootScope.countryId == 706 || $rootScope.countryId == 718 || $rootScope.countryId == 719 || $rootScope.countryId == 724 || $rootScope.countryId == 727 || $rootScope.countryId == 728 || $rootScope.countryId == 1164 || $rootScope.countryId == 731) {
                            angular.forEach(response, function (res, key) {
                                res.RewardText = res.RewardText.replace(".", ",");
                            });
                            $scope.surveys = response;
                        } else {
                            $scope.surveys = response;
                        }
                    }
                    else {
                        $scope.showSurveys = true;
                        $scope.available = false;
                        $scope.message = 1;
                    }
                }, function (err) {
                    $scope.errMgs = true;
                });
            }
            $scope.surveysList();
            //to show surveys popup
            $scope.popup = function (url) {
                var width = 900;
                var height = 550;
                var left = (screen.width - width) / 2;
                var top = (screen.height - height) / 2;
                var params = 'width=' + width + ', height=' + height;
                params += ', top=' + top + ', left=' + left;
                params += ', directories=no';
                params += ', location=no';
                params += ', menubar=no';
                params += ', resizable=no';
                params += ', scrollbars=yes';
                params += ', status=no';
                params += ', toolbar=no';
                newwin = window.open(url, 'windowname5', params);
                setTimeout(() => { this.surveysList(); }, 5000);
                if (window.focus) { newwin.focus() }
                return false;
            }

            //------------ Get member all avaliable reward history --------------------------------//
            $scope.surveyHistory = function () {
                httpService.postData('/Profile/GetRewardsHistory', "").then(function (response) {
                    $scope.userRewardsInfo = response;
                    //if (response == '') {
                    //    $scope.userRewardsInfo.TotalEarnings = 0;
                    //    $scope.userRewardsInfo.TotalRedemptions = 0;
                    //    $scope.userRewardsInfo.AccountBalance = 0;
                    //}
                    $scope.rewHistory = false;
                    $scope.available = false;
                    $scope.showSurveys = false;
                    if ($scope.userRewardsInfo == "") {
                        $scope.rewHistory = true;
                    }
                    if (response.LstRewardHistory != '' & response.LstRewardHistory != null) {
                        $scope.totalItems = response.LstRewardHistory.length;
                        if ($scope.totalItems > 0) {
                            $scope.pager = pagerService.getPager($scope.totalItems, 1);// set pager value and index 
                            //using service get current page list 
                            $scope.rewHistoryLst = getCurrentPageList.getCurrentPageRecords($scope.userRewardsInfo.LstRewardHistory, $scope.pager.startIndex, $scope.pager.endIndex);
                        }
                        else {
                            $scope.rewHistoryLst = response;
                        }
                    }
                }, function (err) {
                    $scope.errMsg = true;
                });
            }
            //Pagging click event
            $scope.setPage = function (page) { // page  click count  
                if (page < 1 || page > $scope.pager.totalPages) {  //current page  count lessthan 1 or more than page count  then retu
                    return;
                }
                // get pager object from service
                $scope.pager = pagerService.getPager($scope.totalItems, page, $scope.pager.startPage, $scope.pager.endPage); // set pager value and index 
                $scope.rewHistoryLst = [];
                //using service get current page list 
                $scope.rewHistoryLst = getCurrentPageList.getCurrentPageRecords($scope.userRewardsInfo.LstRewardHistory, $scope.pager.startIndex, $scope.pager.endIndex);
            }
            //Home Click
            //$scope.homeClick = function () {
            //    window.location.href = '/account/login';
            //}
            // profile Click




            //save details
            $scope.saveDetails = function (user) {
                $scope.changeClick = false;
                httpService.postData('/profile/saveUser', user).then(function (response) {
                    $scope.result = response;
                    if (response != "") {
                        $scope.submitMsg = true;
                        $scope.cancelMsg = false;
                        $scope.errMsg = false;
                    }
                }, function (err) {
                    $scope.errMsg = true;
                });
            }
            $(document).ajaxStart(function () {
                $("body").addClass("loading");
            });
            $(document).ajaxStop(function () {
                $("body").removeClass("loading");
            });

            var userEdited = false;

            $(document).ready(function () {

                //$("#datepicker").datepicker({
                //    dateFormat: 'dd/mm/yy', changeYear: true, yearRange: "-100:-18"
                //}
                //$("#datepicker").attr('readOnly', 'true');
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
                          $(".email").css("height", $(".register-form").height() - 30);
                      }, 20);
                });
                httpService.getData('/Common/GetCurrentDomainDetails').then(function (response) {
                    $scope.orgDetails = response;
                    //var orgLst = organizationService.getAllOrganization()
                    //angular.forEach(orgLst, function (org, key) {
                    //    if (org.orgId == response.ClientId) {
                    //        $scope.bgColor = org.bgcolor;
                    //    }
                    //})
                }, function (err) {
                });
                $scope.submit = function () {
                    httpService.getData('/Common/LogOut').then(function (response) {
                        window.location.href = $scope.orgDetails.MgLoginPath;
                    }, function (err) {
                        window.location.href = $scope.orgDetails.MgLoginPath;
                    });
                }


                $(document).on('click', '#request_link', function (evt) {
                    evt.preventDefault();
                    $.ajax({
                        url: "/Account/ForgotPassword",
                        type: "POST",
                        data: { "EmailAddress": $('#ForgotEmail').val() },
                        success: function (result) {
                            if (result.value == "Redirect") {
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
                //$("#country").selectBoxIt({
                //    theme: "filter-white",
                //});

                //$("#Country").selectBoxIt({
                //    theme: "filter-white",
                //    autoWidth: false,
                //});

                //$("#country_id").selectBoxIt({
                //    theme: "filter-white",
                //    autoWidth: false,
                //});

                //$("#CountryID").selectBoxIt({
                //    theme: "filter-white",
                //    autoWidth: false,
                //});

                //$("#ServiceID").selectBoxIt({
                //    theme: "filter-white",
                //    autoWidth: false,
                //});

                //$("#survey").selectBoxIt({
                //    theme: "filter-white",
                //    autoWidth: false,
                //});

                //$("#gender").selectBoxIt({
                //    theme: "filter-white",
                //    autoWidth: false,
                //});

                //$("#Category").selectBoxIt({
                //    theme: "filter-white",
                //    autoWidth: false,
                //});

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
                $(document).on('click', '.forgot', function () {
                    $('.forgot_div').slideToggle();
                    $('.sign_in_div').slideToggle();
                });

                $(document).on('click', '.forgot_activation', function () {
                    window.location = $("#forgotpasswordtoken").val();
                })

                ////hover navigation top
                //$(".nav_item, .country, .country_mob").hover(
                //    function () {
                //        $(this).addClass("nav_item_h");
                //    }, function () {
                //        $(this).removeClass("nav_item_h");
                //    });

                ////hover change buttons
                //$(".change, .delete").hover(
                //    function () {
                //        $(this).addClass("change_h");
                //    }, function () {
                //        $(this).removeClass("change_h");
                //    });
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
                //$('.hamburger_icon').click(function () {
                //    $('.navigation_mobile').slideToggle(500);
                //});

                //$('.nav_item_mob').click(function () {
                //    $(this).addClass('nav_item_mob_h');
                //});

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
                //$(".charity").hover(
                //    function () {
                //        $(this).addClass("charity_h");
                //    }, function () {
                //        $(this).removeClass("charity_h");
                //    });
                $(".charity").click(function () {
                    $(".modal2").css({
                        top: ($(window).height() - $(".modal2").outerHeight()) / 2,
                        left: ($(window).width() - $(".modal2").outerWidth()) / 2,

                    });
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
                    $(document).on('click', 'div[data-modal-id]', function (e) {
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
                        //$.ajax({
                        //    url: "../Account/CheckForUnprocessedPayment",
                        //    type: "POST",
                        //    success: function (result) {
                        //        if (result == "True") {
                        //            $('#popup5').fadeIn();
                        //        }
                        //else {
                        $('#' + modalBox).fadeIn($(this).data());
                        if ($(window).width() < limiter) {
                            $('.overflowcontainer').css({
                                'height': '100%',
                                'width': '100%',
                                'overflow': 'hidden',
                                'position': 'fixed'
                            });
                        };
                        //}
                        //}
                        //});
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
                        data: { "ServiceID": $(this).attr('id') },
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
                    //drawGraph3($("#ProfileBalance").val(), 10, 0);
                    //drawGraph4($("#ProfileCompletedPerc").val());
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
                    if (IBAN.isValid(IBANValue)) {
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

                //$("#datepicker").blur(function () {
                //    $('#datepicker').removeClass("error");
                //});

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
        }]);
});