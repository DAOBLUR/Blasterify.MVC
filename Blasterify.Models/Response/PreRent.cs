namespace Blasterify.Models.Response
{
    public class PreRent
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ClientUserId { get; set; }
        public List<PreRentItem>? PreRentItems { get; set; }
    }
}