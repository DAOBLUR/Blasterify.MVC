namespace Blasterify.Models.Yuno.Payment
{
    public class ProviderData
    {
        public string Id { get; set; }
        public string Transaction_Id { get; set; }
        public string Account_Id { get; set; }
        public string Status { get; set; }
        public string Sub_Status { get; set; }
        public string Status_Detail { get; set; }
        public string Response_Message { get; set; }
        public string Response_Code { get; set; }
        public RawResponse Raw_Response { get; set; }
        public string Third_Party_Transaction_Id { get; set; }
        public string Third_Party_Account_Id { get; set; }
    }
}