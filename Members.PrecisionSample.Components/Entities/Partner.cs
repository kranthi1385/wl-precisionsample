using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Partner
    {
        #region Private Variables

        private string _surveynName = string.Empty;
        private string _SurveyClickDate = string.Empty;
        private decimal _memberReward = 0;
        private int _surveyId = 0;
        private decimal _partnerReward = 0;
        private string _accessCode = string.Empty;
        private Guid _userGuid = Guid.Empty;
        private string _profileName = string.Empty;
        private int _profileId = 0;
        private string _surveyCompleteDate = string.Empty;
        private DateTime _prilmCompleteDate = DateTime.MinValue;
        private string _profileClickDate = string.Empty;
        private string _profileCompleteDate = string.Empty;
        private string _partnerRewardAmount = string.Empty;
        private int _profileCompleted = 0;
        private Guid _pendingProfileGuid = Guid.Empty;
        private string _subId = string.Empty;
        private int _projectId = 0;
        private decimal _partnerRevenueShare = 0;
        private int _preliminaryStatusId = 0;
        private bool _isPixelFired = false;
        private Int64 _user2ProjectId = 0;
        private int _orgId = 0;
        private DateTime _vindaleSurveyClickDate = DateTime.MinValue;
        private string _txId = string.Empty;
        private int _actualLoi = 0;
        private string _subId3 = string.Empty;
        private int _userId = 0;
        private string _rid = string.Empty;
        private string _partnerRedirectUrl = string.Empty;
        private bool _isShowEndPage = true;
        private string _source = string.Empty;
        private string _status = string.Empty;
        private bool _isS2SEndpage = false;
        private string _postbackUrl = string.Empty;
        private string _termUrl = string.Empty;
        private bool _isPopUp = false;
        private string _hashType = string.Empty;
        private string _hashkey = string.Empty;
        private string _hashParams = string.Empty;
        private int _orgTypeId = 0;
        private string _homePageUrl = string.Empty;
        private string _postbackBody = string.Empty;
        #endregion

        #region Public Properties

        public bool IsPopUp
        {
            get { return _isPopUp; }
            set { _isPopUp = value; }
        }

        public string TermUrl
        {
            get { return _termUrl; }
            set { _termUrl = value; }
        }

        public string PostbackUrl
        {
            get { return _postbackUrl; }
            set { _postbackUrl = value; }
        }

        public bool IsS2SEndpage
        {
            get { return _isS2SEndpage; }
            set { _isS2SEndpage = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }


        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public bool IsShowEndPage
        {
            get { return _isShowEndPage; }
            set { _isShowEndPage = value; }
        }


        public string PartnerRedirectUrl
        {
            get { return _partnerRedirectUrl; }
            set { _partnerRedirectUrl = value; }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string SubId3
        {
            get { return _subId3; }
            set { _subId3 = value; }
        }

        public string SurveynName
        {
            get { return _surveynName; }
            set { _surveynName = value; }
        }

        public string SurveyClickDate
        {
            get { return _SurveyClickDate; }
            set { _SurveyClickDate = value; }
        }

        public decimal MemberReward
        {
            get { return _memberReward; }
            set { _memberReward = value; }
        }

        public int SurveyId
        {
            get { return _surveyId; }
            set { _surveyId = value; }
        }

        public decimal PartnerReward
        {
            get { return _partnerReward; }
            set { _partnerReward = value; }
        }

        public string AccessCode
        {
            get { return _accessCode; }
            set { _accessCode = value; }
        }

        public Guid UserGuid
        {
            get { return _userGuid; }
            set { _userGuid = value; }
        }

        public string ProfileName
        {
            get { return _profileName; }
            set { _profileName = value; }
        }

        public int ProfileId
        {
            get { return _profileId; }
            set { _profileId = value; }
        }

        public string SurveyCompleteDate
        {
            get { return _surveyCompleteDate; }
            set { _surveyCompleteDate = value; }
        }

        public DateTime PrilmCompleteDate
        {
            get { return _prilmCompleteDate; }
            set { _prilmCompleteDate = value; }
        }

        public string ProfileClickDate
        {
            get { return _profileClickDate; }
            set { _profileClickDate = value; }
        }

        public string ProfileCompleteDate
        {
            get { return _profileCompleteDate; }
            set { _profileCompleteDate = value; }
        }

        public int ProfileCompleted
        {
            get { return _profileCompleted; }
            set { _profileCompleted = value; }
        }

        public Guid PendingProfileGuid
        {
            get { return _pendingProfileGuid; }
            set { _pendingProfileGuid = value; }
        }

        public string SubId
        {
            get { return _subId; }
            set { _subId = value; }
        }

        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        public decimal PartnerRevenueShare
        {
            get { return _partnerRevenueShare; }
            set { _partnerRevenueShare = value; }
        }

        public int PreliminaryStatusId
        {
            get { return _preliminaryStatusId; }
            set { _preliminaryStatusId = value; }
        }

        public bool IsPixelFired
        {
            get { return _isPixelFired; }
            set { _isPixelFired = value; }
        }

        public Int64 User2ProjectId
        {
            get { return _user2ProjectId; }
            set { _user2ProjectId = value; }
        }
        public int OrgId
        {
            get { return _orgId; }
            set { _orgId = value; }
        }
        public DateTime VindaleSurveyClickDate
        {
            get { return _vindaleSurveyClickDate; }
            set { _vindaleSurveyClickDate = value; }
        }
        public string TxId
        {
            get { return _txId; }
            set { _txId = value; }
        }
        public int ActualLoi
        {
            get { return _actualLoi; }
            set { _actualLoi = value; }
        }
        public string Rid
        {
            get { return _rid; }
            set { _rid = value; }
        }

        public string HashType
        {
            get
            {
                return _hashType;
            }

            set
            {
                _hashType = value;
            }
        }

        public string Hashkey
        {
            get
            {
                return _hashkey;
            }

            set
            {
                _hashkey = value;
            }
        }

        public string HashParams
        {
            get
            {
                return _hashParams;
            }

            set
            {
                _hashParams = value;
            }
        }

        public int OrgTypeId
        {
            get
            {
                return _orgTypeId;
            }

            set
            {
                _orgTypeId = value;
            }
        }

        public string HomePageUrl
        {
            get
            {
                return _homePageUrl;
            }

            set
            {
                _homePageUrl = value;
            }
        }

        public string PostbackBody
        {
            get
            {
                return _postbackBody;
            }

            set
            {
                _postbackBody = value;
            }
        }
        #endregion
    }
}
