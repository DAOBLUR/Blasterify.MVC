namespace Blasterify.Client.Models
{
    public class LogIn
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}