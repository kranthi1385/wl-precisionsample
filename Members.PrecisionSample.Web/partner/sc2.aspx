<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sc2.aspx.cs" Inherits="Members.PrecisionSample.Web.partner.sc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.12.4.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            var sub_id = getUrlVars()['sub_id'];
            if (sub_id == undefined) {
                sub_id = '';
            }
            var subid = getUrlVars()['subid'];
            if (subid == undefined) {
                subid = '';
            }
            var sub = getUrlVars()['sub'];
            if (sub == undefined) {
                sub = '';
            }
            var sid = getUrlVars()['sid']
            if (sid == undefined) {
                sid = '';
            }
            var qid = getUrlVars()['qid']
            if (qid == undefined) {
                qid = '';
            }
            var s = getUrlVars()['s']
            if (s == undefined) {
                s = '';
            }
            var project = getUrlVars()['project']
            if (project == undefined) {
                project = '';
            }
            var ug = getUrlVars()['ug']
            if (ug == undefined) {
                ug = '';
            }

            $.ajax({
                //url: "/ControllerName/ActionName",
                url: '/sc2/start?sg=' + sid + '&ug=' + ug + '&s=' + s + '&project=' + project
                + '&qg=' + qid + '&sub_id=' + sub_id + '&subid=' + subid + '&sub=' + sub,
                success: function (data) {
                    if (data != null || data != undefined) {
                        window.location.href = data;
                    }
                }
            });
            function getUrlVars() {
                var Url = window.location.href.toLowerCase();
                var vars = {};
                var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
                });
                return vars;
            }

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
