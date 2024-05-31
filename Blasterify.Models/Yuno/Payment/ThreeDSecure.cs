namespace Blasterify.Models.Yuno.Payment
{
    public class ThreeDSecure
    {
        public object Version { get; set; }
        public object Electronic_Commerce_Indicator { get; set; }
        public object Cryptogram { get; set; }
        public object Transaction_Id { get; set; }
        public object Directory_Server_Transaction_Id { get; set; }
        public object Pares_Status { get; set; }
        public object Acs_Id { get; set; }
    }
}