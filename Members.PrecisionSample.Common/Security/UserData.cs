using System;
using System.Collections.Generic;
using System.Text;

namespace Members.PrecisionSample.Common.Security
{
    /// <summary>
    /// Secutity.Userdata loads the current logged in user and his organisation data.
    /// </summary>
    public class UserData
    {
        #region Business Properties and Methods
        

        // declare members
        private string _userGuid;

        private string _userId;
        private string _userData;
        private string _password;
        private string _gender;
        private string _zipCode;
        private string _userLanguage;

        // user related
        private string _firstName;
        private string _lastName;

        private string _userEmailId;

        //private string _themename;

        private string _usertheme;

        //user Last login related
        private string _lastLogInDate;

        //User Is Allowed To Impersonate
        private bool _is_allowed_to_impersonate;

        // current organization related
        private Organization _current_org = null;

        //default organization related
        private Organization _default_org = null;

        #endregion

        #region Constructor
        /// <summary>
        /// On constuctor, the userData is parsed from the string passedin into the User-Data object.
        /// </summary>
        /// <param name="userData"></param>
        public UserData(string userData)
        {
            this._userData = userData;
            ParseData(this._userData);
        }
        #endregion

        #region User related published properties
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string UserLanguage
        {
            get
            {
                return _userLanguage;
            }
            set
            {
                _userLanguage = value;
            }
        }

        public string UserGuid
        {
            get { return _userGuid; }
            set { _userGuid = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        public string EmailAddress
        {
            get { return _userEmailId; }
            set { _firstName = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        public bool IsAllowedToImpersonate
        {
            get { return _is_allowed_to_impersonate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserThem
        {
            get { return _usertheme; }
            set { _usertheme = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;

            }

        }
        /// <summary>
        /// 
        /// </summary>

        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;

            }
        }
        /// <summary>
        /// 
        /// </summary>

        public string ZipCode
        {
            get
            {
                return _zipCode;
            }
            set
            {
                _zipCode = value;

            }
        }
        /// <summary>
        /// 
        /// </summary>


        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;

            }
        }

        #endregion

        #region User Last Login Date related published properties
        /// <summary>
        /// 
        /// </summary>
        public string LastLogInDate
        {
            get { return _lastLogInDate; }
        }

        #endregion

        #region Current Client related published properties
        /// <summary>
        /// 
        /// </summary>
        public Organization CurrentClient
        {
            get { return _current_org; }
        }
        #endregion Current Client related published properties

        #region Default Client related published properties
        /// <summary>
        /// 
        /// </summary>
        public Organization DefaultOrganization
        {
            get { return _default_org; }
        }
        #endregion Default Organization related published properties

        public override string ToString()
        {
            return this._userData;
        }

        #region  ParseIdentity Info

        /// <summary>
        /// This is used to parse the userData info from the database.
        /// </summary>
        /// <param name="completeInputData"></param>
        private void ParseData(string completeInputData)
        {
            string[] setOfEntities = completeInputData.Split('|');

            if (setOfEntities.Length > 1)
            {
                // Current Org Info 
                ParseCurrentOrgMembers(setOfEntities[0]);
                // Full Name of user
                parseCurrentUserData(setOfEntities[1]);
                
                // Default Org Info
               // ParseDefaultOrgMembers(setOfEntities[0]);
            }

        }

        /// <summary>
        /// parse Current User Info
        /// </summary>
        /// <param name="completeInputData"></param>
        private void parseCurrentUserData(string completeInputData)
        {
            string[] setOfEntities = completeInputData.Split(';');
          
            _userId = setOfEntities[0];
            _userGuid = setOfEntities[1];
            _firstName = setOfEntities[2];
            _lastName = setOfEntities[3];
            _zipCode = setOfEntities[4];
            _gender = setOfEntities[5];
            _password = setOfEntities[6];
            _userEmailId = setOfEntities[7];
            _userLanguage = setOfEntities[8];
           
        }

        /// <summary>
        /// Parse Current Org Info: Can be null, if read from the database.
        /// </summary>
        /// <param name="completeInputData"></param>
        private void ParseCurrentOrgMembers(string completeInputData)
        {
            if (completeInputData != string.Empty && completeInputData.Length > 0)
            {
                _current_org = new Organization(completeInputData);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="completeInputData"></param>
        private void ParseDefaultOrgMembers(string completeInputData)
        {
            if (completeInputData != string.Empty &&
                completeInputData.Length > 0)
            {
                _default_org = new Organization(completeInputData);
            }
        }

        #endregion

        #region Organization Class

        [Serializable()]
        public class Organization
        {
            #region Member Variables
            // declare members
            private string _client_name;
            private string _id;
            private string _nk;
            private string _guid;
            private string _client_logo;
            private string _client_theme;
            private string _client_url;
            private string _parent_org_id;          
            private string _masking_url;

            public string ClientName
            {
                get
                {
                    return _client_name;
                }
                set
                {
                    if (!_client_name.Equals(value))
                    {
                        _client_name = value;
                    }
                }
            }

            public string Nk
            {
                get
                {
                    return _nk;
                }
                set
                {
                    if (!_nk.Equals(value))
                    {
                        _nk = value;
                    }
                }
            }

            public string Id
            {
                get
                {
                    return _id;
                }
                set
                {
                    if (!_id.Equals(value))
                    {
                        _id = value;
                    }
                }
            }

            public string Guid
            {
                get
                {
                    return _guid;
                }
                set
                {
                    if (!_guid.Equals(value))
                    {
                        _guid = value;
                    }
                }
            }

            public string Logo
            {
                get
                {
                    return _client_logo;
                }
                set
                {
                    if (!_client_logo.Equals(value))
                    {
                        _client_logo = value;
                    }
                }
            }

            public string Theme
            {
                get
                {
                    return _client_theme;
                }
                set
                {
                    if (!_client_theme.Equals(value))
                    {
                        _client_theme = value;
                    }
                }
            }

            public string Url
            {
                get
                {
                    return _client_url;
                }
                set
                {
                    if (!_client_url.Equals(value))
                    {
                        _client_url = value;
                    }
                }
            }

            public string ParentOrgId
            {
                get
                {
                    return _parent_org_id;
                }
                set
                {
                    if (!_parent_org_id.Equals(value))
                    {
                        _parent_org_id = value;
                    }
                }
            }
            public string MaskingUrl
            {
                get
                {
                    return _masking_url;
                }
                set
                {
                    if (!_masking_url.Equals(value))
                    {
                        _masking_url = value;
                    }
                }
            }
           

            #endregion Member Variables

            /// <summary>
            /// 
            /// </summary>
            public Organization()
            {
                _client_name = string.Empty;
                _id = string.Empty; //org_id
                _nk = string.Empty;
                _guid = string.Empty;
                _client_logo = string.Empty;
                _client_theme = string.Empty;
                _client_url = string.Empty;
                _parent_org_id = string.Empty;
                _masking_url = string.Empty;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="completeApplicationData"></param>
            public Organization(string completeData)
            {
                ParseOrganizationMembers(completeData);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="completeInputData"></param>
            private void ParseOrganizationMembers(string completeInputData)
            {
                string[] setOfEntities = completeInputData.Split(';');
                _id = setOfEntities[0];
                _guid = setOfEntities[1];
                _client_name = setOfEntities[2];
                _client_url = setOfEntities[3];
                //if (setOfEntities[4] != "0")
                //    _parent_org_id = setOfEntities[4];
                //else
                //    _parent_org_id = string.Empty;
                
                //_masking_url = setOfEntities[5];
            }

        }

        #endregion Organization Class

    }
}
