namespace Blasterify.Models.Yuno.Payment
{
    public class Checkout
    {
        public string Session { get; set; }
        public bool Sdk_Action_Required { get; set; }
    }
}