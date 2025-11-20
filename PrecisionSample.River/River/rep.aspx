<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rep.aspx.cs" Inherits="Members.PrecisionSample.River.Web.River.rep" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link rel="stylesheet" href="/css/reset.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/css/960.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/css/main.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/css/slide.css" type="text/css" media="screen" />
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />

    <script type="text/javascript" src=" https://code.jquery.com/jquery-1.7.1.min.js"></script>

    <style type="text/css">
        #border
        {
            border: solid 1px #CACACA; /*overflow: hidden;*/
            max-width: 570px;
            height: 320px;
            position: relative;
            font-family: Verdana;
            font-size: 15px;
            padding: 10px;
        }
        #btnYesSuccess, #btnNoSuccess
        {
            background: rgb(219,200,254);
            background: -moz-linear-gradient(top, rgba(219,200,254,1) 0%, rgba(212,181,253,1) 100%);
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(219,200,254,1)), color-stop(100%,rgba(212,181,253,1)));
            background: -webkit-linear-gradient(top, rgba(219,200,254,1) 0%,rgba(212,181,253,1) 100%);
            background: -o-linear-gradient(top, rgba(219,200,254,1) 0%,rgba(212,181,253,1) 100%);
            background: -ms-linear-gradient(top, rgba(219,200,254,1) 0%,rgba(212,181,253,1) 100%);
            background: linear-gradient(to bottom, rgba(219,200,254,1) 0%,rgba(212,181,253,1) 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#dbc8fe', endColorstr='#d4b5fd',GradientType=0 );
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            -khtml-border-radius: 3px;
            padding: 4px 15px;
            border: 1px solid #999999;
            font-size: 18px;
            font-weight: bold;
            color: #52085E;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container_12">
        <!-- /// begin page /// -->
        <!-- ///////////////////////////////////////// MASTHEAD ///////////////////////////////////////// -->
        <div id="masthead">
            <h1 id="logo">
                <a href="/river/s1.htm">Opinionetwork</a></h1>
            <!--  <h1 id="logo2">
                <a href="index.html">sample partner</a></h1>-->
        </div>
        <div class="clear" style="margin-bottom: -52px;">
        </div>
        <!-- ///////////////////////////////////////// CONTENT ///////////////////////////////////////// -->
        <div id="survey">
            <h3 class="surveyHeader">
            </h3>
            <div class="clear">
            </div>
            <div id="surveyQuestionContainer">
                <div id="surveyQuestion">
                </div>
                <div>
                    <div id="dvmessage" style="font-family: Verdana; font-size: 15px;">
                        <div id="dvSuccessMessage" style="margin-top: 15px;" runat="server" visible="false">
                            <p style="color: Green;">
                                Success! You’ve completed this survey and will receive your reward shortly.
                                <br />
                                Would you like to take another survey?
                            </p>
                            <br />
                        </div>
                        <div id="dvFailureMessage" style="margin-top: 15px;" runat="server" visible="false">
                            <p>
                                Unfortunately your profile does not meet our Client’s requirements for this study.<br />
                                Would you like to take another survey?
                            </p>
                            <br />
                        </div>
                        <div style="margin-top: 15px;">
                            <asp:Button ID="btnYesSuccess" Text="Yes, take me there!" runat="server" Width="200px"
                                OnClick="btnYes_click" />
                            <asp:HiddenField ID="button" runat="server" Value="Yes" />
                            <asp:Button ID="btnNoSuccess" Text="Not at this time" runat="server" Width="170px"
                                OnClick="btnNo_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <!-- ///////////////////////////////////////// FOOTER ///////////////////////////////////////// -->
    <div id="footer">
        <p>
        </p>
    </div>
    </form>
</body>
</html>
