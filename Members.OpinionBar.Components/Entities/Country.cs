using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    public class Country
    {
        #region Public variables
        public int CId { get; set; }
        public string CC { get; set; }
        public string CN { get; set; }
        public string LC { get; set; }
        public string ContinentName { get; set; }
        public string C { get; set; }
        public string CNP { get; set; }
        #endregion
    }
    public class Continent
    {
        #region Public variables
        public string ContinentName { get; set; }
        public List<Country> LstCountries { get; set; }
        #endregion
    }
}
