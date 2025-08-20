//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using PrecisionSample.Services.Components.Entites;
//using System.Text;

//namespace PrecisionSample.Services.Controllers
//{
//    public class LegacyController : ApiController
//    {
//        [HttpPost]
//        [Route("Create")]
//        public string Create(int RID, string TxId, string ExtMemberId, string Country, string State, string FirstName, string LastName, string EmailAddress, string Zip, string Gender, string Dob, string Address1, string Address2, string Ethnicity, string City)
//        {
//            Member objMember = new Member();
//            objMember.Rid = RID;
//            objMember.TxId = TxId;
//            objMember.ExtMemberId = ExtMemberId;
//            objMember.Country = Country;
//            objMember.State = State;
//            objMember.FirstName = FirstName;
//            objMember.LastName = LastName;
//            objMember.EmailAddress = EmailAddress;
//            objMember.Zip = Zip;
//            objMember.Gender = Gender;
//            objMember.Dob = Dob;
//            objMember.Address1 = Address1;
//            objMember.Address2 = Address2;
//            objMember.Ethnicity = Ethnicity;
//            objMember.City = City;
//            if (ModelState.IsValid)
//            {
//                MemberController core = new MemberController();
//                return core.Create(objMember);
//            }
//            else
//            {
//                Output objoutput = new Output();
//                objoutput.ErroMessage = "Required fields are missing or invalid parameters";
//                objoutput.StatusCode = 0;
//                return objoutput.ToString();
//            }
//        }
//        [HttpPost]
//        [Route("Update")]
//        public string Update(Guid UserGuid, int RID, string TxId, string ExtMemberId, string Country, string State, string FirstName, string LastName, string EmailAddress, string Zip, string Gender, string Dob, string Address1, string Address2, string Ethnicity, string City)
//        {
//            UpdateMember objMember = new UpdateMember();
//            objMember.UserGuid = UserGuid;
//            objMember.Rid = RID;
//            objMember.TxId = TxId;
//            objMember.ExtMemberId = ExtMemberId;
//            objMember.Country = Country;
//            objMember.State = State;
//            objMember.FirstName = FirstName;
//            objMember.LastName = LastName;
//            objMember.EmailAddress = EmailAddress;
//            objMember.Zip = Zip;
//            objMember.Gender = Gender;
//            objMember.Dob = Dob;
//            objMember.Address1 = Address1;
//            objMember.Address2 = Address2;
//            objMember.Ethnicity = Ethnicity;
//            objMember.City = City;
//            if (ModelState.IsValid)
//            {
//                MemberController core = new MemberController();
//                return core.Update(objMember);
//            }
//            else
//            {
//                Output objoutput = new Output();
//                objoutput.ErroMessage = "Required fields are missing or invalid parameters";
//                objoutput.StatusCode = 0;
//                return objoutput.ToString();
//            }
//        }
//        [HttpPost]
//        [Route("UnSubscribe")]
//        public string UnSubscribe(int Rid, string UserName)
//        {
//            Unsubscribe objUnsubscribe = new Unsubscribe();
//            objUnsubscribe.Rid = Rid;
//            objUnsubscribe.UserName = UserName;
//            if (ModelState.IsValid)
//            {
//                MemberController core = new MemberController();
//                return core.Unsubscribe(Rid, UserName);
//            }
//            else
//            {
//                Output objoutput = new Output();
//                objoutput.ErroMessage = "Required fields are missing";
//                objoutput.StatusCode = 0;
//                return objoutput.ToString();
//            }
//        }
//        [HttpPost]
//        [Route("ReSubscribe")]
//        public string ReSubscribe(Guid UserGuid)
//        {
//            if (UserGuid != Guid.Empty)
//            {
//                MemberController core = new MemberController();
//                return core.Resubscribe(UserGuid);
//            }
//            else
//            {
//                Output objoutput = new Output();
//                objoutput.ErroMessage = "UserGuid Required";
//                objoutput.StatusCode = 0;
//                return objoutput.ToString();
//            }

//        }
//        //[HttpPost]
//        //[Route("GetProfile")]
//        //public string GetProfile(string UserGuid)
//        //{
//        //    if (string.IsNullOrEmpty(UserGuid))
//        //    {
//        //        MemberController core = new MemberController();
//        //        return core.GetProfile(UserGuid);
//        //    }
//        //    else
//        //    {
//        //        Output objoutput = new Output();
//        //        objoutput.ErroMessage = "UserGuid Required";
//        //        objoutput.StatusCode = 0;
//        //        return objoutput.ToString();
//        //    }

//        //}
//        [HttpPost]
//        [Route("GetSurveys")]
//        public HttpResponseMessage GetSurveys(string UserGuid)
//        {
//            string message = string.Empty;
//            List<Surveys> lstSurveys = new List<Surveys>();
//            if (string.IsNullOrEmpty(UserGuid))
//            {
//                MemberController core = new MemberController();
//                return core.GetSurveys(UserGuid);
//            }

//            else
//            {
//                //Output objoutput = new Output();
//                //objoutput.ErroMessage = "UserGuid Required";
//                //objoutput.StatusCode = 0;
//                //return objoutput.ToString();
//                message = "UserGuid is Required";
//                return Request.CreateResponse(HttpStatusCode.OK, message);
//            }
//        }
//        [HttpPost]
//        [Route("GetSurveysHistory")]
//        public HttpResponseMessage GetSurveysHistory(string UserGuid)
//        {
//            string message = string.Empty;
//            List<SurveyHistory> lstSurveyHistory = new List<SurveyHistory>();
//            if (string.IsNullOrEmpty(UserGuid))
//            {
//                MemberController core = new MemberController();
//                return core.GetSurveysHistory(UserGuid);
//            }
//            else
//            {
//                //Output objoutput = new Output();
//                //objoutput.ErroMessage = "UserGuid Required";
//                //objoutput.StatusCode = 0;
//                //return objoutput.ToString();
//                message = "UserGuid is Required";
//                return Request.CreateResponse(HttpStatusCode.OK, message);
//            }
//        }
//        [HttpPost]
//        [Route("UpdateProfile")]
//        public string UpdateProfile(string xml)
//        {
//            if (string.IsNullOrEmpty(xml))
//            {
//                MemberController core = new MemberController();
//                return core.UpdateProfile(xml);
//            }
//            else
//            {
//                Output objoutput = new Output();
//                objoutput.ErroMessage = "Response XML is Required";
//                objoutput.StatusCode = 0;
//                return objoutput.ToString();
//            }

//        }

//    }
//}
