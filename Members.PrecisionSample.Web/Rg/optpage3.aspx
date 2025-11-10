<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="optpage3.aspx.cs" Inherits="Members.PrecisionSample.Web.Registration.optpage3" %>

<asp:Content ContentPlaceHolderID="cphMain" ID="Contetn1" runat="server">

    <script src="https://cdn.ifficient.com/cdn/js/iffparent.js" type="text/javascript"></script>

    <script src="https://api.trustedform.com/parent.js" type="text/javascript"></script>

    <center>
        <asp:Literal ID="lit1" runat="server"></asp:Literal>
        <br />
        <div style="float: right; padding-right: 20px;">
            <asp:ImageButton ID="img1" runat="server" ImageUrl="/Images/noThanksContinue.jpg"
                OnClick="img2_Clcik" />
        </div>
    </center>
</asp:Content>
