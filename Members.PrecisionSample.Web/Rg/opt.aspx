<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="opt.aspx.cs" Inherits="Members.PrecisionSample.Web.Registration.opt" %>

<asp:Content ContentPlaceHolderID="cphMain" ID="content2" runat="server">

    <script language="C#" runat="server">
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (hfShow1.Value == "0")
            {
                this.Form.Attributes.Add("onsubmit", "return oi_send();");
            }
            else if (hfShow1.Value == "1")
            {
                this.Form.Attributes.Remove("onsubmit");
            }
        }
        
    </script>

    <script type="text/javascript">
        function checkflag() {
            if ($get("<%=hfShow1.ClientID%>").value == 1) {
                return CheckForClick();
            }

        }</script>

    <center>
        <asp:Literal ID="litPixelScript" runat="server"></asp:Literal>
        <table style="width: 800px;">
            <tr>
                <td align="center" style="font-size: 12px; background-color: #FFFFFF; padding: 10px;
                    overflow: hidden; border: solid 1px #CACACA;">
                    <br />
                    <div id="divShow1" runat="server" style="color: Black; text-align: left;">
                        <img src="/Images/freesponoffers.jpg" />
                        <br />
                        <br />
                        Select "Yes" next to the offers you are interested in receiving more information
                        on.<br />
                        Then submit below to complete your registration and participate in your first survey.
                        <br />
                        <%--    Who doesn’t like something for free? Based on your profile you’ve qualified
                        to take advantage of the following freebies and opportunities from our sponsors.
                        To find out more simply select “Yes” next to those you’d like to receive additional
                        information about.--%>
                    </div>
                    <br />
                    <div id="divopt" runat="server" visible="false">
                        <input type="hidden" name="OI_FORM_IDENTIFIER" value="" />
                        <asp:Literal ID="litopt" runat="server"></asp:Literal>
                        <%-- <div id="divrid72" runat="server" visible="false" >
                            <script language="JavaScript" src="http://www.clear-request.com/oi/second/14377214.js"></script>
                        </div>
                        <div id="divridnot72" runat="server" visible="false" >
                            <script language="JavaScript" src="http://www.clear-request.com/oi/second/12409734.js"></script>
                        </div>--%>
                    </div>
                    <div id="divTurbo" runat="server" visible="false">
                        <asp:Literal ID="lit1" runat="server" Visible="false"  ></asp:Literal>
                        <%--  <script src="http://dm.tmginteractive.com/JSCoReg/surveydownline/SurveydownlineMultiOffers.aspx?id=547128&first_name=&last_name=&dob=&email=&phone=&zipcode=&country=&state=&gender="
                            type="text/javascript">
                        </script>--%>
                    </div>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 12px;" align="center">
                    <br />
                    <asp:ImageButton ID="img1" runat="server" ImageUrl="/Images/continue.jpg" OnClick="img1_Click"
                        OnClientClick="CheckForClick();" />
                    <asp:Literal ID="ltlAffiliateTracking" runat="server" />
                    <input type="hidden" id="hfShow1" runat="server" value="0" />
                    <input type="hidden" id="hfcountry" runat="server" />
                    &nbsp;
                </td>
            </tr>
        </table>
    </center>
</asp:Content>