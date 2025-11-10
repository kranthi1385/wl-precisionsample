using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class reg
    {
        public string AnswerText { get; set; }
        public string LastName { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Questiontxt { get; set; }
        public int Questionid { get; set; }
        public int Questiontypeid { get; set; }
        public string Optiontxt { get; set; }
        public int Optionid { get; set; }
        public bool Hasoptions { get; set; }
        public List<Options> Optlst = new List<Options>();
    }
}
