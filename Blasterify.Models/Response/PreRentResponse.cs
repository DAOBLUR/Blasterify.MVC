using System;
using System.Collections.Generic;

namespace Blasterify.Models.Response
{
    public class PreRentResponse
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public bool IsEnabled { get; set; }
        public int ClientUserId { get; set; }
        public int StatusId { get; set; }
        public List<PreRentItemResponse> PreRentItems { get; set; }
    }
}