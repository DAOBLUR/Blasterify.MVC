namespace Blasterify.Models.Yuno.Payment
{
    public class Card
    {
        public bool Verify { get; set; }
        public bool Capture { get; set; }
        public int Installments { get; set; }
        public object Installments_Plan_Id { get; set; }
        public int First_Installment_Deferral { get; set; }
        public string Installments_Type { get; set; }
        public object Installment_Amount { get; set; }
        public string Soft_Descriptor { get; set; }
        public string Authorization_Code { get; set; }
        public string Retrieval_Reference_Number { get; set; }
        public object Voucher { get; set; }
        public CardData Card_Data { get; set; }
        public StoredCredentials Stored_Credentials { get; set; }
    }
}