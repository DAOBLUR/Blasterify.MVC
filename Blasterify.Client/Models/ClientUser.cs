using System;

namespace Blasterify.Client.Models
{
    public class ClientUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string CardNumber { get; set; }

        public bool Status { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public DateTime SuscriptionDate { get; set; }

        public int SubscriptionId { get; set; }
    }
}