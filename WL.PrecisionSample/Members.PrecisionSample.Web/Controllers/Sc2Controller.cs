using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Members.PrecisionSample.Components.Business_Layer;
namespace Members.PrecisionSample.Web.Controllers
{
    public class Sc2Controller : Controller
    {
        // GET: Sc2

        public string start(string sg, string ug, string s, string project, string qg, string sub_id, string subid, string sub)
        {
            string _redirectUrl = string.Empty;
            string subID = (!string.IsNullOrEmpty(sub_id)) ? sub_id : ((!string.IsNullOrEmpty(subid)) ? subid : ((!string.IsNullOrEmpty(sub) ? sub : string.Empty)));
            PartnerManager objPartnerManager = new PartnerManager();

            if (string.IsNullOrEmpty(qg))
            {
                if (!string.IsNullOrEmpty(sg))
                {
                    // Get quota group guid from DB by using survey invitaion guid
                    //qg = Guid.NewGuid().ToString();
                    try
                    {
                        qg = objPartnerManager.GetQutoaGuid(sg);
                        _redirectUrl = ConfigurationManager.AppSettings["clickflowurl"] + "?qig=" + qg.Split(';')[0] + "&cid=" + qg.Split(';')[1] + "&ug=" + ug + "&project=" + project + "&s=" + s + "&sub_id=" + subID;
                    }
                    catch (Exception ex)
                    {
                        _redirectUrl = "";
                    }
                }
            }
            else
            {
                string cid = objPartnerManager.GetOrgId(ug);
                _redirectUrl = ConfigurationManager.AppSettings["clickflowurl"] + "?qig=" + qg + "&cid=" + cid + "&ug=" + ug + "&project=" + project + "&s=" + s + "&sub_id=" + subID ;
            }

            return _redirectUrl;

        }

    }
}