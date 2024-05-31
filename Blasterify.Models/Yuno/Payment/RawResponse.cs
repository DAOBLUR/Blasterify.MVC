namespace Blasterify.Models.Yuno.Payment
{
    public class RawResponse
    {
        public AmountRawResponse Amount { get; set; }
        public string Merchant_Account { get; set; }
        public string Payment_Psp_Reference { get; set; }
        public string Psp_Reference { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
    }
}