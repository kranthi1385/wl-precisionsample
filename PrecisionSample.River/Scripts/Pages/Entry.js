var Entry = function() {
    var that = {};
    var getUrlVars = function() {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

    /* Get The projectId and QuotaGroupId and UserGuid */
    var GetProjectDetails = function() {
        $.ajax({
            type: 'GET',
            url: '/services/RiverService.aspx?Mode=GetProjectDetails&ug=' + getUrlVars()["ug"] + '&rn=' + Math.random(),
            success: function(pagedata) {
                //debugger;
                $('#dvImageLoading').hide();
                window.location.href = pagedata;

            },
            error: function(error) {
                $('#dvImageLoading').hide();
            }
        });
    }

    /* End GetProject Details */

    that.init = function() {
        $('#dvImageLoading').show();
        GetProjectDetails();
    }
    return that;
} ();

$(document).ready(function() {
    Entry.init();
});
