<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="np2.aspx.cs" Inherits="Members.PrecisionSample.Web.Registration.np2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="ctl00_Head1" runat="server">
    <title>Survey Downline </title>
    <link id="ctl00_lnkStylesheet" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" media="screen" href="http://www.surveydownline.com/css/reset.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="http://www.surveydownline.com/css/default.css" />

    <script type="text/javascript" src="http://www.surveydownline.com/Js/AC_RunActiveContent.js"></script>

    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="/css/ie7.css" type="text/css" rel="stylesheet" />
</head>
<body runat="server">
    <form name="aspnetForm" runat="server" method="post" onsubmit="javascript:return WebForm_OnSubmit();"
        id="aspnetForm">
        <div>
        </div>

        <script type="text/javascript">
            //<![CDATA[
            var theForm = document.forms['aspnetForm'];
            if (!theForm) {
                theForm = document.aspnetForm;
            }
            function __doPostBack(eventTarget, eventArgument) {
                if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                    theForm.__EVENTTARGET.value = eventTarget;
                    theForm.__EVENTARGUMENT.value = eventArgument;
                    theForm.submit();
                }
            }
            //]]>
        </script>



        <script type="text/javascript">
            //<![CDATA[
            if (typeof (Sys) === 'undefined') throw new Error('ASP.NET Ajax client-side framework failed to load.');
            //]]>
        </script>

        <script src="http://www.surveydownline.com/ScriptResource.axd?d=io6nc9dxo4U08vaRsbJtCpZvX393aW08TftbYaD-bAmH8t2wH0l3X6N3UD5uQOO84Ycasm1nXjuTb95CIsiB0Po4dr-m1CjeOgs-93x3J0mKJeGXQKTT_7vbGwR8w5AplmJalyYdcdvTFPViY1njoFc3_exp7sJ6tTtTVMbSrz76vLWO0&amp;t=22acfe2d"
            type="text/javascript"></script>

        <script type="text/javascript">
            //<![CDATA[
            function WebForm_OnSubmit() {
                if (typeof (ValidatorOnSubmit) == "function" && ValidatorOnSubmit() == false) return false;
                return true;
            }
            //]]>
        </script>

        <div>
        </div>

        <script type="text/javascript">
            //<![CDATA[
            Sys.WebForms.PageRequestManager._initialize('ctl00$scriptManager', document.getElementById('aspnetForm'));
            Sys.WebForms.PageRequestManager.getInstance()._updateControls([], [], [], 90);
            //]]>

        </script>

        <div id="container">
            <div id="topborder">
            </div>
            <a href="http://www.surveydownline.com/Mem/Hm.aspx" id="ctl00_ancHomeredirect" style="padding-left: 30px;">
                <img src="http://www.surveydownline.com/Images/-1.png" alt="paid surveys online" /></a>
            <div style="padding-left: 30px; margin-left: 30px;">
                <br />
                <br />
                <table style="width: 800px;">
                    <tr>
                        <td align="left" style="font-size: 12px; background-color: #FFFFFF; padding: 10px; overflow: hidden; border: solid 1px #CACACA;">
                            <!-- START: AdQuire Inline, Permission Data -->
                            <asp:Literal ID="litScript" runat="server"></asp:Literal>
                            <%-- <div id="PD_OuterContainer" style="">
                            </div>

                            <script id="PD_Script_Configs" type="text/javascript" src="//www.pdapi.com/cs/JSI/v1/site422/PDconfigs.js"></script>--%>

                            <!-- END: AdQuire Inline, Permission Data -->
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ImageButton ID="img1" CausesValidation="false" runat="server" ImageUrl="/Images/continue.jpg"
                                OnClick="img1_Click" OnClientClick="javascript:return TMG_CheckForClick();" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:Literal ID="ltlAffiliateTracking" runat="server" />
            <asp:Literal ID="litPixelScript" runat="server"></asp:Literal>

            <script type="text/javascript">
                function popup(url) {
                    var width = 800;
                    var height = 700;
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
                    if (window.focus) { newwin.focus() }
                    return false;
                }
            </script>

            <div id="footer">
                <p id="text">
                    &copy; Copyright 2009-2011 Survey Downline. | <a href="javascript: void(0)" id="ctl00_SiteFooter_ancsurveysonline"
                        onclick="popup('http://www.surveydownline.com/Misc/t.htm')" style="color: #b2b2b2;">Terms </a>| <a href="javascript: void(0)" id="ctl00_SiteFooter_ancsurveyspolicy"
                            onclick="popup('http://www.surveydownline.com/Misc/p.htm')" style="color: #b2b2b2;">Privacy</a> | <a href="javascript: void(0)" id="ctl00_SiteFooter_ancsurveycontact"
                                onclick="popup('http://www.surveydownline.com/Misc/Cu.aspx')" style="color: #b2b2b2;">Contact Us</a> | <a href="javascript: void(0)" id="ctl00_SiteFooter_ancsurveyAbout"
                                    onclick="popup('http://www.surveydownline.com/Misc/abt.htm')" style="color: #b2b2b2;">About Us</a> | <a href="javascript: void(0)" id="ctl00_SiteFooter_ancSurveySitemap"
                                        onclick="popup('http://www.surveydownline.com/Misc/faq.aspx')" style="color: #b2b2b2;">FAQ</a>| <a href='http://www.impactradius.com/agency-campaign-info/SmarterChaoscom.brand?c=239'
                                            target="_blank" style="color: #b2b2b2;">Affiliates</a>
                    <%--| <a href="javascript: void(0)" id="ctl00_SiteFooter_a1" onclick="popup('http://www.surveydownline.com/Misc/ipad.html')"
                                        style="color: #b2b2b2;">iPad Giveaway</a></p>--%>
            </div>
        </div>

        <script type="text/javascript">
            //<![CDATA[
            var Page_Validators = new Array(document.getElementById("ctl00_cphMain_rfvCountry"));
            //]]>
        </script>

        <script type="text/javascript">
            //<![CDATA[
            var ctl00_cphMain_rfvCountry = document.all ? document.all["ctl00_cphMain_rfvCountry"] : document.getElementById("ctl00_cphMain_rfvCountry");
            ctl00_cphMain_rfvCountry.controltovalidate = "ctl00_cphMain_ddlCountry";
            ctl00_cphMain_rfvCountry.errormessage = "Please select the Country";
            ctl00_cphMain_rfvCountry.display = "Dynamic";
            ctl00_cphMain_rfvCountry.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
            ctl00_cphMain_rfvCountry.initialvalue = "-1";
            //]]>
        </script>

        <script type="text/javascript">
            //<![CDATA[

            var Page_ValidationActive = false;
            if (typeof (ValidatorOnLoad) == "function") {
                ValidatorOnLoad();
            }

            function ValidatorOnSubmit() {
                if (Page_ValidationActive) {
                    return ValidatorCommonOnSubmit();
                }
                else {
                    return true;
                }
            }

            theForm.oldSubmit = theForm.submit;
            theForm.submit = WebForm_SaveScrollPositionSubmit;

            theForm.oldOnSubmit = theForm.onsubmit;
            theForm.onsubmit = WebForm_SaveScrollPositionOnSubmit;
            Sys.Application.initialize();

            document.getElementById('ctl00_cphMain_rfvCountry').dispose = function () {
                Array.remove(Page_Validators, document.getElementById('ctl00_cphMain_rfvCountry'));
            }
            //]]>
        </script>

    </form>
</body>
</html>
