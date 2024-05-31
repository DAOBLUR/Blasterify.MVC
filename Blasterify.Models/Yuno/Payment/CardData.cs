using System;
using System.Collections.Generic;
using System.Text;

namespace Blasterify.Models.Yuno.Payment
{
    public class CardData
    {
        public string Holder_Name { get; set; }
        public string Iin { get; set; }
        public string Lfd { get; set; }
        public int Number_Length { get; set; }
        public int Security_Code_Length { get; set; }
        public string Brand { get; set; }
        public string Issuer_Name { get; set; }
        public object Issuer_Code { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public ThreeDSecure Three_D_Secure { get; set; }
    }
}
