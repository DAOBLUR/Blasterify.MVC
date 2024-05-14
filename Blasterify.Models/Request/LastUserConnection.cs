namespace Blasterify.Models.Request
{
    public class LastUserConnection
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsConnected { get; set; }
    }
}