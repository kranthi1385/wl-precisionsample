using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Security.Cryptography;

namespace Members.PrecisionSample.Common.Security
{
    /// <summary>
    /// Identity object representing a user
    /// </summary>
    [Serializable]
    public class Identity : IIdentity
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Identity Unathenticated = new Identity();
        /// <summary>
        /// 
        /// </summary>
        private string _authenticationType = "Forms";
        /// <summary>
        /// 
        /// </summary>
        private bool _isAuthenticated;
        /// <summary>
        /// 
        /// </summary>
        private string _name;
        /// <summary>
        /// 
        /// </summary>
        private Guid _userID;
        /// <summary>
        /// 
        /// </summary>
        private Members.PrecisionSample.Common.Security.UserDataProvider _userDataProvider;
        /// <summary>
        /// 
        /// </summary>
        private string _hostName;

        /// <summary>
        /// 
        /// </summary>
        public Identity() { }

        /// <summary>
        /// 
        /// </summary>
        public static Identity Current
        {
            get { return System.Web.HttpContext.Current.User.Identity.IsAuthenticated ? (Identity)System.Web.HttpContext.Current.User.Identity : null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="roles"></param>
        public Identity(string name, Guid userID, string hostName, Members.PrecisionSample.Common.Security.UserDataProvider userDataProvider)
        {
            this._name = name;
            this._userID = userID;
            this._isAuthenticated = true;
            this._userDataProvider = userDataProvider;
            this._hostName = hostName;
        }

        ///// <summary>
        ///// Gets the type of authentication used.
        ///// </summary>
        public string AuthenticationType
        {
            get { return this._authenticationType; }
        }

        /// <summary>
        /// Gets the type of authentication used
        /// </summary>
        public bool IsAuthenticated
        {
            get { return this._isAuthenticated; }
        }

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        public string Name
        {
            get { return this._name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid UserID
        {
            get { return this._userID; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HostName
        {
            get { return this._hostName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Members.PrecisionSample.Common.Security.UserData UserData
        {
            get { return this.UserDataProvider.Retrieve(this.UserID); }
        }

       

        /// <summary>
        /// 
        /// </summary>
        private Members.PrecisionSample.Common.Security.UserDataProvider UserDataProvider
        {
            get { return this._userDataProvider; }
        }
    }
}
