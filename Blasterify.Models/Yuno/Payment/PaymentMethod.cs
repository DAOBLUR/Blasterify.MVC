namespace Blasterify.Models.Yuno.Payment
{
    public class PaymentMethod
    {
        public string Vaulted_Token { get; set; }
        public string Type { get; set; }
        public bool Vault_On_Success { get; set; }
        public string Token { get; set; }
        public PaymentMethodDetail Detail { get; set; }
    }
}