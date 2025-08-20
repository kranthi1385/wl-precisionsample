using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Torreward
    {
        #region private varables
        private string _pID = string.Empty;
        private string _cOMID = string.Empty;
        private string _rWD = "0";
        private string _tRANSID = string.Empty;
        private string _status = string.Empty;
        private string _cPROJID = string.Empty;
        private int _isCurrency = 0;

        public int IsCurrency
        {
            get { return _isCurrency; }
            set { _isCurrency = value; }
        }

        #endregion

        #region Public varables

        public string PID
        {
            get { return _pID; }
            set { _pID = value; }
        }

        public string COMID
        {
            get { return _cOMID; }
            set { _cOMID = value; }
        }
        public string RWD
        {
            get { return _rWD; }
            set { _rWD = value; }
        }
        public string TRANSID
        {
            get { return _tRANSID; }
            set { _tRANSID = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string CPROJID
        {
            get { return _cPROJID; }
            set { _cPROJID = value; }
        }
        #endregion
    }
}
