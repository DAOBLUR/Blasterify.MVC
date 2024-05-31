using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Blasterify.Models.Yuno.Payment
{
    public class Payment
    {
        public string Id { get; set; }
        public string Account_Id { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string Sub_Status { get; set; }
        public string Merchant_Order_Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public AmountDetail Amount { get; set; }
        public Checkout Checkout { get; set; }
        public PaymentMethod Payment_Method { get; set; }
        public CustomerPayer Customer_Payer { get; set; }
        public object Additional_Data { get; set; }
        public object Taxes { get; set; }
        public Transaction Transactions { get; set; }
        public List<Transaction> Transactions_History { get; set; }
        public string Workflow { get; set; }
        public List<object> Metadata { get; set; }
        public object Fraud_Screening { get; set; }
        public string Payment_Link_Id { get; set; }
        public object Subscription_Code { get; set; }
        public RoutingRules Routing_Rules { get; set; }
    }
}