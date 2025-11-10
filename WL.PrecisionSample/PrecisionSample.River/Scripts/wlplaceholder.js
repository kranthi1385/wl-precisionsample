jQuery(document).ready(function () {
    jQuery.ajax({
        url: '/partner/clientdetails.aspx',
        success: function (data) {
            jQuery(".orgname").text(data.OrgName);
            jQuery(".logo").attr("src", data.OrgLogo);
            jQuery(".memberurl").text(data.MemberUrl);
            jQuery(".emailaddress").text(data.Emailaddress);
            jQuery(".address").html(data.Address);
            jQuery(".mgloginpath").text(data.MgLoginPath);
            jQuery(document).attr('title', data.OrgName);
            jQuery(".password").text(data.Password);
            jQuery(".copyright").text(data.CopyrightYear);
            jQuery(".aboutustext").text(data.AboutusText);
            jQuery("styletheem").attr("href", data.StyleSheettheme);
        }
    });
});


