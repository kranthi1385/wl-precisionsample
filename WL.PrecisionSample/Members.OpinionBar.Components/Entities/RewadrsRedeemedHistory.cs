using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    public class RewadrsRedeemedHistory
    {
        public int RedeemptionId { get; set; }
        public string RedemptionType { get; set; }
        public decimal RedemptionAmount { get; set; }
        public string CreateDt { get; set; }
        public string GiftcardName { get; set; }
        public string Descripion { get; set; }
        public string ApiResponse { get; set; }
    }

    public class Response
    {
        public string referenceOrderID { get; set; }
        public string externalRefID { get; set; }       
        public reward reward = new reward();
    }

    public class reward
    {        
        public string redemptionInstructions { get; set; }
    }

   

}
