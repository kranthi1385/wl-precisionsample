<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="offers.aspx.cs" Inherits="Members.PrecisionSample.Web.Registration.offers" %>

<asp:Content ContentPlaceHolderID="cphMain" ID="content2" runat="server">

    <script language="javascript" type="text/javascript">

        function openNewWin(url) {

            var x = window.open(url, 'mynewwin', 'width=800,height=600,toolbar=1,resizable=yes,scrollbars=yes');

            x.focus();

        }

    </script>

    <center>
        <br />
        <br />
        <div id="div3" style="display: block; width: 830px;">
            <table style="padding-left: 20px;">
                <tr>
                    <td align="left" style="font-size: 12px; background-color: #F5F5F5; padding: 10px;
                        overflow: hidden; border: solid 1px #CACACA;">
                        Welcome to SurveyDownline
                        <asp:Label ID="lblFirstName" runat="server" ForeColor="#1E91CB"></asp:Label>!<br />
                        <br />
                        Based on your profile, our partners listed below invite you to join their panels.
                        Invitations are only extended when you’ve already qualified for a large number of
                        surveys so joining will definitely be worth your time!
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" width="80%" cellspacing="0">
                <tr>
                    <td align="center" style="font-size: 12px; padding-left:25px;                       
                        height: 120px;">
                         <div style="background-color: #F5F5F5; border: solid 1px #CACACA;">
                        <asp:ImageButton ID="imgOffer1" AlternateText="Offer" runat="server" OnClick="btnOffer1_Click"
                            Width="150px" Height="120px" /></div>
                    </td>
                    <td style="width: 110px;">
                    </td>
                    <td align="center" style="font-size: 12px; height: 120px;padding-left: 25px; ">
                        <div style="background-color: #F5F5F5; border: solid 1px #CACACA;">
                            <asp:ImageButton ID="imgOffer2" AlternateText="Offer" runat="server" OnClick="btnOffer2_Click"
                                Width="150px" Height="120px" /></div>
                    </td>
                    <td style="width: 110px;">
                    </td>
                    <td align="center" style="font-size: 12px; padding-left:25px;
                        height: 120px;">
                         <div style="background-color: #F5F5F5; border: solid 1px #CACACA;">
                        <asp:ImageButton ID="imgOffer3" AlternateText="Offer" runat="server" OnClick="btnOffer3_Click"
                            Width="150px" Height="120px" />
                            </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" width="90%" cellspacing="0">
                <tr>
                    <td style="text-align: left; font-size: 12px; padding-left: 28px;">
                        <asp:ImageButton ID="imgbtnOffer1" runat="server" ImageUrl="/Images/joinnowforoffers.jpg"
                            CausesValidation="false" OnClick="imgbtnOffer1_Click" />
                    </td>
                    <td style="text-align: left; font-size: 12px; padding-left: 28px;">
                        <asp:ImageButton ID="imgbtnOffer2" runat="server" ImageUrl="/Images/joinnowforoffers.jpg"
                            CausesValidation="false" OnClick="imgbtnOffer2_Click" />
                    </td>
                    <td style="text-align: left; font-size: 12px; padding-left: 32px;">
                        <asp:ImageButton ID="imgbtnOffer3" runat="server" ImageUrl="/Images/joinnowforoffers.jpg"
                            CausesValidation="false" OnClick="imgbtnOffer3_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <br />
           
            <table>
                <tr>
                    <td style="text-align: left; font-size: 12px;">
                        <asp:ImageButton ID="btnContinue" runat="server" ImageUrl="/Images/continue.jpg"
                            CausesValidation="false" OnClick="btnContinue_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
</asp:Content>
