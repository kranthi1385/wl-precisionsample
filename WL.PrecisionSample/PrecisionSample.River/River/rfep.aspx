<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rfep.aspx.cs" Inherits="Members.PrecisionSample.River.Web.River.rfep" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Opinionetwork Surveys</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />

    <script src="/Scripts/jquery-1.8.2.js" type="text/javascript"></script>

    <link id="lnkMainCss" rel="stylesheet" href="/css/river-main.css" type="text/css"
        media="screen" />
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />

    <script type="text/javascript">
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
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div class="cp-bg">
        </div>
        <div class="dvoverly">
        </div>
        <div class="container_12">
            <!-- /// begin page /// -->
            <!-- ///////////////////////////////////////// MASTHEAD ///////////////////////////////////////// -->
            <div id="masthead">
                <div id="logo">
                    <a href="/river/s1.htm">
                        <img src="/images/on_logocopy.PNG" />
                    </a>
                </div>
            </div>
            <div class="clear">
            </div>
            <!-- ///////////////////////////////////////// CONTENT ///////////////////////////////////////// -->
            <div id="dvRfep">
                <h3 class="surveyHeader">
                </h3>
                <div class="clear">
                </div>
                <div id="dvRepContainer">
                    <div>
                        <div>
                            <div style="width: 100%;">
                                <div>
                                    <div id="dvBookmark" style="font-family: Verdana; font-size: 17px;">
                                        <p class="pmessage" style="text-align: left; font-family: 'PT Sans Narrow' , sans-serif">
                                            Thank you for participating! That’s all the surveys we have for you today but you
                                            may return tomorrow and take 10 additional surveys.<br />
                                            <br />
                                            Create a bookmark by hitting "Ctrl + D". You will be credited for all completed
                                            surveys when you return through the bookmark.
                                        </p>
                                        <br />
                                        <br />
                                        <!-- <input type="button" id="btnClose" value="Close" style="width:100px;" />-->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <!-- ///////////////////////////////////////// FOOTER ///////////////////////////////////////// -->
    </center>
    </form>
</body>
</html>
