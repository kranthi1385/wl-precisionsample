using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members.PrecisionSample.Components.Entities
{
    public class SurveyUrl
    {
        public string _projecturl = string.Empty;
        public int _firstClickCount = 0;
        public string _subId3 = string.Empty;
        public string ProjectUrl { get; set; }
        public int FirstClickCount { get; set; }
        public string SubId3 { get; set; }
        public int client_id { get; set; }
        public int projectId { get; set; }
        public double cpi { get; set; }
        public Guid projectGuid { get; set; }
        public string Payload { get; set; }
        public string RespondentJSON { get; set; }

    }
    public class GetInvite
    {
        public string SurveyId { get; set; }
        public string URL { get; set; }
        public string Result { get; set; }
        public int ResultCode { get; set; }
        public double PartnerAmount { get; set; }
        public double MemberAmount { get; set; }

    }

    public class TolunaUser
    {
        public Boolean IsTolunaMember { get; set; }
        public int CountryID { get; set; }
        public string DOB { get; set; }
        public string ZIPCode { get; set; }
        public string LanguageCode { get; set; }
        public List<TolunaQstOpt> LstQstOpt { get; set; }
        public TolunaUser()
        {
            LstQstOpt = new List<TolunaQstOpt>();
        }
    }

    public class TolunaQstOpt
    {
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
    }
    public class SentryEndURL
    {
        public string RedirectUrl { get; set; }
        public Guid InvitationGUID { get; set; }
    }
}